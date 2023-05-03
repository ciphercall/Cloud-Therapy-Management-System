using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class LoginController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        public LoginController()
        {
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }



        // Create ASL_LOG object and it used to this SaveLogin_LogData (LoginModel loginModel).
        public ASL_LOG aslLog = new ASL_LOG();

        // Edit ALL INFORMATION from AslUserCo TO Asl_lOG Database Table.
        public void SaveLogin_LogData(LoginModel loginModel)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == loginModel.COMPID && n.USERID == loginModel.USERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(loginModel.COMPID);
            aslLog.USERID = Convert.ToInt64(loginModel.USERID);
            aslLog.LOGTYPE = "LOG IN";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date); 
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = loginModel.LOGIPNO;
            aslLog.LOGLTUDE = loginModel.LOGLTUDE;
            aslLog.LOGDATA = loginModel.LOGDATA;
            aslLog.TABLEID = "AslUsercoes";
            aslLog.USERPC = loginModel.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }



        public ActionResult Index()
        {
            Session["HomePage"] = "Show home page";
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (DataAccess.LoginDAL.AdminIsValid(model.LOGINID, model.LOGINPW))
                {
                    FormsAuthentication.SetAuthCookie(model.LOGINID, true);
                    var result = (from n in db.AslUsercoDbSet
                                  where n.LOGINID == model.LOGINID &&
                                        n.LOGINPW == model.LOGINPW
                                  select new
                                  {
                                      companyid = n.COMPID,
                                      userid = n.USERID,
                                      usertype = n.OPTP,
                                      username = n.USERNM,
                                      status=n.STATUS,
                                      timeFor=n.TIMEFR,
                                      timeTo=n.TIMETO
                                  }
                        );


                    string timeFor = "", timeTo = "", userStatus = "", userType = "";
                    Int64? companyId = 0;
                    //LogIn Session create for User.
                    foreach (var n in result)
                    {
                        Session["loggedCompID"] = n.companyid;
                        Session["loggedUserID"] = n.userid;
                        Session["LoggedUserType"] = n.usertype;
                        Session["LoggedUserName"] = n.username;
                        Session["LoggedUserStatus"] = n.status;
                        timeFor = n.timeFor;
                        timeTo = n.timeTo;
                        userStatus = n.status;
                        companyId = n.companyid;
                        userType = n.usertype;
                    }

                    //Check Company Status
                    var checkCompanyStatus = (from n in db.AslCompanyDbSet
                        where n.COMPID == companyId
                        select new
                        {
                            checkStatus= n.STATUS
                        });

                    String companyStatus = "";
                    foreach (var check in checkCompanyStatus)
                    {
                        Session["LoggedCompanyStatus"] = check.checkStatus;
                        companyStatus = check.checkStatus;
                    }


                    //Check TimeFor & TimeTo for current datetime login
                    model.LOGTIME = Convert.ToString(td.ToString("HH:mm:ss"));
                    TimeSpan logTimeSpan = TimeSpan.Parse(model.LOGTIME);
                    TimeSpan timeForSpan = TimeSpan.Parse(timeFor);
                    TimeSpan timeToSpan = TimeSpan.Parse(timeTo);
                    if (timeForSpan <= logTimeSpan && logTimeSpan <= timeToSpan && userStatus=="A")
                    {

                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }

                    //Check User Satatus
                    if (companyStatus == "A" || companyStatus == "" )
                    {
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }


                    var savelogData = (from n in db.AslUsercoDbSet
                                       where n.LOGINID == model.LOGINID &&
                                             n.LOGINPW == model.LOGINPW
                                       select new
                                       {
                                           n.COMPID,
                                           n.USERID,
                                           n.USERNM,
                                           n.DEPTNM,
                                           n.OPTP,
                                           n.ADDRESS,
                                           n.MOBNO,
                                           n.EMAILID,
                                           n.LOGINBY,
                                           n.LOGINID,
                                           n.LOGINPW,
                                           n.TIMEFR,
                                           n.TIMETO,
                                           n.STATUS
                                       }
                        );

                    foreach (var n in savelogData)
                    {
                        model.COMPID = n.COMPID;
                        model.USERID = n.USERID;
                        model.LOGDATA = Convert.ToString("User Name: " + n.USERNM + ",\nDepartment Name: " + n.DEPTNM + ",\nOperation Type: " + n.OPTP + ",\nAddress: " + n.ADDRESS + ",\nMobile No: " + n.MOBNO + ",\nEmail ID: " + n.EMAILID + ",\nLogin BY: " + n.LOGINBY + ",\nLogin ID: " + n.LOGINID + ",\nTime From: " + n.TIMEFR + ",\nTime To: " + n.TIMETO + ",\nStatus: " + n.STATUS + ".");
                    }


                    //Get Ip ADDRESS,Time & user PC Name
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];

                    model.USERPC = strHostName;
                    model.LOGIPNO = ipAddress.ToString();
                    //model.LOGTIME = DateTime.Parse(td.ToString(), dateformat, System.Globalization.DateTimeStyles.AssumeLocal);

                    SaveLogin_LogData(model);
                    db.SaveChanges();

                    if (userType == "AslSuperadmin")
                    {
                        return RedirectToAction("Index", "AslCompany");
                    }
                    else //if (userType == "CompanyAdmin")
                    {
                        return RedirectToAction("Index", "GraphView");
                    }
                    
                }
                else
                {
                    ViewData["ErrorMessage"] = "The login ID or password you entered is incorrect. ";
                    Session["HomePage"] = "Show home page";
                    return View("Index");
                }
            }
            Session["HomePage"] = "Show home page";
            return View("Index");
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
