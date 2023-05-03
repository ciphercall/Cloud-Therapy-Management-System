using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class AslUserCOController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();


        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        public AslUserCOController()
        {
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        // Create ASL_LOG object and it used to this Insert_Asl_LogData, Update_Asl_LogData, Delete_Asl_LogData (AslUserco aslUsercos).
        public ASL_LOG aslLog = new ASL_LOG();

        // SAVE ALL INFORMATION from AslUserCo TO Asl_lOG Database Table.
        public void Insert_Asl_LogData(AslUserco aslUsercos)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == aslUsercos.COMPID && n.USERID == aslUsercos.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(aslUsercos.COMPID);
            aslLog.USERID = aslUsercos.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = aslUsercos.INSIPNO;
            aslLog.LOGLTUDE = aslUsercos.INSLTUDE;
            aslLog.TABLEID = "AslUsercoes";
            aslLog.LOGDATA = Convert.ToString("User Name: " + aslUsercos.USERNM + ",\nDepartment Name: " + aslUsercos.DEPTNM + ",\nOperation Type: " + aslUsercos.OPTP + ",\nAddress: " + aslUsercos.ADDRESS + ",\nMobile No: " + aslUsercos.MOBNO + ",\nEmail ID: " + aslUsercos.EMAILID + ",\nLogin BY: " + aslUsercos.LOGINBY + ",\nLogin ID: " + aslUsercos.LOGINID  + ",\nTime From: " + aslUsercos.TIMEFR + ",\nTime To: " + aslUsercos.TIMETO + ",\nStatus: " + aslUsercos.STATUS + ".");
            aslLog.USERPC = aslUsercos.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }

        // Edit ALL INFORMATION from AslUserCo TO Asl_lOG Database Table.
        public void Update_Asl_LogData(AslUserco aslUsercos)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == aslUsercos.COMPID && n.USERID == aslUsercos.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(aslUsercos.COMPID);
            aslLog.USERID = aslUsercos.UPDUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = aslUsercos.UPDIPNO;
            aslLog.LOGLTUDE = aslUsercos.UPDLTUDE;
            aslLog.TABLEID = "AslUsercoes";
            aslLog.LOGDATA = Convert.ToString("User Name: " + aslUsercos.USERNM + ",\nDepartment Name: " + aslUsercos.DEPTNM + ",\nOperation Type: " + aslUsercos.OPTP + ",\nAddress: " + aslUsercos.ADDRESS + ",\nMobile No: " + aslUsercos.MOBNO + ",\nEmail ID: " + aslUsercos.EMAILID + ",\nLogin BY: " + aslUsercos.LOGINBY + ",\nLogin ID: " + aslUsercos.LOGINID + ",\nTime From: " + aslUsercos.TIMEFR + ",\nTime To: " + aslUsercos.TIMETO + ",\nStatus: " + aslUsercos.STATUS + ".");
            aslLog.USERPC = aslUsercos.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        // Delete ALL INFORMATION from AslUserCo TO Asl_lOG Database Table.
        public void Delete_Asl_LogData(AslUserco aslUsercos)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == aslUsercos.COMPID && n.USERID == aslUsercos.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(aslUsercos.COMPID);
            aslLog.USERID = aslUsercos.UPDUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = aslUsercos.UPDIPNO;
            aslLog.LOGLTUDE = aslUsercos.UPDLTUDE;
            aslLog.TABLEID = "AslUsercoes";
            aslLog.LOGDATA = Convert.ToString("User Name: " + aslUsercos.USERNM + ",\nDepartment Name: " + aslUsercos.DEPTNM + ",\nOperation Type: " + aslUsercos.OPTP + ",\nAddress: " + aslUsercos.ADDRESS + ",\nMobile No: " + aslUsercos.MOBNO + ",\nEmail ID: " + aslUsercos.EMAILID + ",\nLogin BY: " + aslUsercos.LOGINBY + ",\nLogin ID: " + aslUsercos.LOGINID + ",\nTime From: " + aslUsercos.TIMEFR + ",\nTime To: " + aslUsercos.TIMETO + ",\nStatus: " + aslUsercos.STATUS + ".");
            aslLog.USERPC = aslUsercos.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }



        // Create ASL_DELETE object and it used to this Delete_ASL_DELETE (AslUserco aslUsercos).
        public ASL_DELETE AslDelete = new ASL_DELETE();

        // Delete ALL INFORMATION from AslUserCo TO ASL_DELETE Database Table.
        public void Delete_ASL_DELETE(AslUserco aslUsercos)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("HH:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == aslUsercos.COMPID && n.USERID == aslUsercos.UPDUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(aslUsercos.COMPID);
            AslDelete.USERID = aslUsercos.UPDUSERID;
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = aslUsercos.UPDIPNO;
            AslDelete.DELLTUDE = aslUsercos.UPDLTUDE;
            AslDelete.TABLEID = "AslUsercoes";
            AslDelete.DELDATA = Convert.ToString("User Name: " + aslUsercos.USERNM + ",\nDepartment Name: " + aslUsercos.DEPTNM + ",\nOperation Type: " + aslUsercos.OPTP + ",\nAddress: " + aslUsercos.ADDRESS + ",\nMobile No: " + aslUsercos.MOBNO + ",\nEmail ID: " + aslUsercos.EMAILID + ",\nLogin BY: " + aslUsercos.LOGINBY + ",\nLogin ID: " + aslUsercos.LOGINID + ",\nTime From: " + aslUsercos.TIMEFR + ",\nTime To: " + aslUsercos.TIMETO + ",\nStatus: " + aslUsercos.STATUS + ".");
            AslDelete.USERPC = aslUsercos.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);
        }

        //
        // GET: /AslUserCO/

        public ActionResult Index()
        {
            //Exception exception;
            //HandleErrorInfo errorInfo;
            //try
            //{
            //    Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
            //    var optp = System.Web.HttpContext.Current.Session["LoggedUserType"].ToString();

            //    if (optp == "AslSuperadmin")
            //    {
            //        return View(db.AslUsercoDbSet);
            //    }
            //    else
            //    {
            //        List<AslUserco> aslUsercos;

            //        var result = (from n in db.AslUsercoDbSet
            //                      where n.COMPID == compid
            //                      select n
            //            );

            //        return View(result);
            //    }

            //}

            //catch
            //{
            //    ViewBag.Message = "Unable to recognize the Client!";
            //    exception = new Exception("Unable to recognize the Client!");
            //    errorInfo = new HandleErrorInfo(exception, "Home", "Index");
            //    return View("Error", errorInfo);
            //}

            return View();

        }


        //
        // GET: /AslUserCO/Details/5

        public ActionResult Details(int id = 0)
        {
            AslUserco asluserco = db.AslUsercoDbSet.Find(id);
            if (asluserco == null)
            {
                return HttpNotFound();
            }
            return View(asluserco);
        }

       
        // GET: /AslUserCO/Create
        public ActionResult Create()
        {
            ViewData["HighLight_Menu_UserForm"] = "High Light Menu";
            var dt = (AslUserco)TempData["User"];

            ////<br/> brack removed;
            //if (System.Web.HttpContext.Current.Session["LoggedUserType"] != null)
            //{
            //    var LoggedUserTp = System.Web.HttpContext.Current.Session["LoggedUserType"].ToString();
            //    if (LoggedUserTp == "CompanyAdmin" || LoggedUserTp == "UserAdmin" || LoggedUserTp == "User")
            //    {
            //        TempData["BrackFieldDropFromLayout_AslUserCOController"] = " <br/> brack removed";
            //    }
            //}
            
            return View(dt);

        }

        //
        // POST: /AslUserCO/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AslUserco asluserco)
        {
            if (ModelState.IsValid)
            {
                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];


                asluserco.USERPC = strHostName;
                asluserco.INSIPNO = ipAddress.ToString();
                asluserco.INSTIME = Convert.ToDateTime(td);


                Int64 cid = Convert.ToInt64(asluserco.COMPID);

                if (cid == 0)
                {
                    TempData["UserCreationMessage"] = "Company ID not found! ";
                    return RedirectToAction("Create");
                }
                else
                {
                    Int64 maxData = Convert.ToInt64((from n in db.AslUsercoDbSet where asluserco.COMPID == n.COMPID select n.USERID).Max());

                    Int64 R = Convert.ToInt64(asluserco.COMPID + "99");

                    if (maxData == 0)
                    {
                        asluserco.OPTP = "CompanyAdmin";
                        asluserco.USERID = Convert.ToInt64(asluserco.COMPID + "01");
                        //Insert User ID save AslUSerco table attribute INSUSERID
                        asluserco.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        Insert_Asl_LogData(asluserco);
                        db.AslUsercoDbSet.Add(asluserco);
                        db.SaveChanges();

                        TempData["UserCreationMessage"] = "User Name: '" + asluserco.USERNM + "' successfully registered! ";

                        //Role database list add
                        var qrM = (from a in db.AslMenuDbSet select a).OrderBy(x=>x.SERIAL);
                        foreach (ASL_MENU a in qrM)
                        {
                            ASL_ROLE role = new ASL_ROLE()
                            {

                                COMPID = Convert.ToInt64(asluserco.COMPID),
                                USERID = Convert.ToInt64(asluserco.COMPID + "01"),
                                MODULEID = a.MODULEID,
                                SERIAL = a.SERIAL,
                                MENUID = a.MENUID,
                                MENUTP = a.MENUTP,
                                INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                INSIPNO = ipAddress.ToString(),
                                INSTIME = td,
                                USERPC = strHostName,
                                STATUS = "A",
                                INSERTR = "A",
                                UPDATER = "A",
                                DELETER = "A",
                                MENUNAME = a.MENUNM,
                                ACTIONNAME = a.ACTIONNAME,
                                CONTROLLERNAME = a.CONTROLLERNAME
                            };
                            db.AslRoleDbSet.Add(role);
                        }
                        db.SaveChanges();

                      

                        if (asluserco.EMAILID != null)
                        {
                            try
                            {
                                //Send UserName & Password to User Mail Address
                                String companyName1st = "";
                                var searchCompanyName1st = db.AslCompanyDbSet.Where(n => n.COMPID == asluserco.COMPID).Select(n => new
                                {
                                    compNm = n.COMPNM

                                });
                                foreach (var n in searchCompanyName1st)
                                {
                                    companyName1st = n.compNm;
                                }
                                MailMessage mailCompany = new MailMessage();
                                mailCompany.To.Add(asluserco.EMAILID);
                                mailCompany.From = new MailAddress("admin@alchemy-bd.com");
                                mailCompany.Subject = "Mail Confirmation";
                                mailCompany.Body = "Alchemy Restaurant Management Online Registration System.\n\n" + Environment.NewLine + "Hi, " + asluserco.USERNM + Environment.NewLine
                                           + "Your Company Name: " + companyName1st + Environment.NewLine + "Your Operation Type: " + asluserco.OPTP + Environment.NewLine + "Your Login ID: " + asluserco.LOGINID + Environment.NewLine
                                           + "Your Password: " + asluserco.LOGINPW + Environment.NewLine + "\nStay with us," + Environment.NewLine + "Alchemy Software";
                                mailCompany.Priority = MailPriority.Normal;

                                SmtpClient clientComapny = new SmtpClient();
                                clientComapny.Host = "mail.alchemy-bd.com";
                                clientComapny.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                                clientComapny.EnableSsl = false;
                                clientComapny.Send(mailCompany);
                            }
                            catch (Exception ex)
                            {
                                TempData["UserCreationMessage"] = "User Name: '" + asluserco.USERNM + "' successfully registered! This User Password not send to her email address.";
                            }
                            
                        }
                       
                        return RedirectToAction("SearchCompanyList");


                    }
                    else if (maxData <= R)
                    {
                        asluserco.USERID = maxData + 1;
                        //Insert User ID save AslUSerco table attribute INSUSERID
                        asluserco.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        Insert_Asl_LogData(asluserco);
                        db.AslUsercoDbSet.Add(asluserco);
                        db.SaveChanges();

                        TempData["UserCreationMessage"] = "User Name: '" + asluserco.USERNM + "' successfully registered! ";

                        //Role database list add
                        var qrM = (from a in db.AslMenuDbSet select a).OrderBy(x=>x.SERIAL);
                        foreach (ASL_MENU a in qrM)
                        {
                            ASL_ROLE role = new ASL_ROLE()
                            {

                                COMPID = Convert.ToInt64(asluserco.COMPID),
                                USERID = maxData + 1,
                                MODULEID = a.MODULEID,
                                SERIAL=a.SERIAL,
                                MENUID = a.MENUID,
                                MENUTP = a.MENUTP,
                                INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                INSIPNO = ipAddress.ToString(),
                                INSTIME = td,
                                USERPC = strHostName,
                                STATUS = "I",
                                INSERTR = "I",
                                UPDATER = "I",
                                DELETER = "I",
                                MENUNAME = a.MENUNM,
                                ACTIONNAME = a.ACTIONNAME,
                                CONTROLLERNAME = a.CONTROLLERNAME
                            };
                            db.AslRoleDbSet.Add(role);

                        }

                        db.SaveChanges();


                    }
                    else
                    {
                        TempData["UserCreationMessage"] = "Not possible entry! ";
                        return RedirectToAction("Create");
                    }


                    if (asluserco.EMAILID!=null)
                    {
                        try
                        {
                            //Send UserName & Password to User Mail Address
                            String companyName = "";
                            var searchCompanyName = db.AslCompanyDbSet.Where(n => n.COMPID == asluserco.COMPID).Select(n => new
                            {
                                compNm = n.COMPNM

                            });
                            foreach (var n in searchCompanyName)
                            {
                                companyName = n.compNm;
                            }

                            MailMessage mail = new MailMessage();
                            mail.To.Add(asluserco.EMAILID);
                            mail.From = new MailAddress("admin@alchemy-bd.com");
                            mail.Subject = "Mail Confirmation";
                            mail.Body = "Alchemy Restaurant Management Online Registration System.\n\n" + Environment.NewLine + "Hi, " + asluserco.USERNM + Environment.NewLine
                                       + "Your Company Name: " + companyName + Environment.NewLine + "Your Operation Type: " + asluserco.OPTP + Environment.NewLine + "Your Login ID: " + asluserco.LOGINID + Environment.NewLine
                                       + "Your Password: " + asluserco.LOGINPW + Environment.NewLine + "\nStay with us," + Environment.NewLine + "Alchemy Software";
                            mail.Priority = MailPriority.Normal;

                            SmtpClient client = new SmtpClient();
                            client.Host = "mail.alchemy-bd.com";
                            client.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                            client.EnableSsl = false;
                            client.Send(mail);

                            return RedirectToAction("Create");
                        }
                        catch (Exception ex)
                        {
                            TempData["UserCreationMessage"] = "User Name: '" + asluserco.USERNM + "' successfully registered! This User Password not send to her email address.";
                        }                      
                    }
                }
            }
            return RedirectToAction("Create");

        }



        public ActionResult Edit(int id = 0)
        {
            AslUserco asluserco = db.AslUsercoDbSet.Find(id);
            if (asluserco == null)
            {
                return HttpNotFound();
            }
            return View(asluserco);
        }

        //
        // POST: /AslUserCO/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AslUserco asluserco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asluserco).State = EntityState.Modified;

                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                asluserco.USERPC = strHostName;
                asluserco.UPDIPNO = ipAddress.ToString();
                asluserco.UPDTIME = Convert.ToDateTime(td);
                //Update User ID save AslUSerco table attribute INSUSERID
                asluserco.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                Update_Asl_LogData(asluserco);


                db.SaveChanges();
                //TempData["UserUpdateMessage"] = "'" + asluserco.USERNM + "' successfully updated! ";
                ViewBag.Message = "'" + asluserco.USERNM + "' successfully updated! ";
                return RedirectToAction("Edit");
            }
            return View(asluserco);
        }

        //
        // GET: /AslUserCO/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AslUserco asluserco = db.AslUsercoDbSet.Find(id);
            if (asluserco == null)
            {
                return HttpNotFound();
            }
            return View(asluserco);
        }

        //
        // POST: /AslUserCO/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, AslUserco aslUsercoDelete)
        {
            AslUserco asluserco = db.AslUsercoDbSet.Find(id);

            //Get Ip ADDRESS,Time & user PC Name 
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            asluserco.USERPC = strHostName;
            asluserco.UPDIPNO = ipAddress.ToString();
            asluserco.UPDTIME = DateTime.Parse(td.ToString(), dateformat, System.Globalization.DateTimeStyles.AssumeLocal);

            //Delete User ID save AslUSerco table attribute INSUSERID
            asluserco.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            //Get current LOGLTUDE data 
            asluserco.UPDLTUDE = aslUsercoDelete.UPDLTUDE;

            Delete_Asl_LogData(asluserco);
            Delete_ASL_DELETE(asluserco);

            db.AslUsercoDbSet.Remove(asluserco);
            db.SaveChanges();



            //Role database list Delete
            var roleList = (from sub in db.AslRoleDbSet select sub)
             .Where(sub => sub.USERID == asluserco.USERID);
            foreach (var n in roleList)
            {
                db.AslRoleDbSet.Remove(n);
            }
            db.SaveChanges();


            return RedirectToAction("Index");
        }


        //Get: /AslUserco
        public ActionResult UpdateForm()
        {
            ////<br/> brack removed;
            //if (System.Web.HttpContext.Current.Session["LoggedUserType"] != null)
            //{
            //    var LoggedUserTp = System.Web.HttpContext.Current.Session["LoggedUserType"].ToString();
            //    if (LoggedUserTp == "CompanyAdmin" || LoggedUserTp == "UserAdmin" || LoggedUserTp == "User")
            //    {
            //        TempData["BrackFieldDropFromLayout_AslUserCOController"] = " <br/> brack removed";
            //    }
            //}
            ViewData["HighLight_Menu_UserForm"] = "High Light Menu";
            return View();
        }


        //
        // POST: /AslUserCO/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForm(AslUserco asluserco, String command)
        {
            if (command == "Update")
            {
                if (asluserco.USERNM != null && asluserco.OPTP != null && asluserco.ADDRESS != null && asluserco.MOBNO != null  && asluserco.LOGINBY != null && asluserco.LOGINID != null && asluserco.LOGINPW != null && asluserco.STATUS != null)
                {
                    string mobileNo = "", emailid = "", loginID = "";
                    mobileNo = asluserco.MOBNO.ToString();
                    if (asluserco.EMAILID != null)
                    {
                        emailid = asluserco.EMAILID.ToString();
                    }
                    loginID = asluserco.LOGINID.ToString();
                    var Mobile_Exists = db.AslUsercoDbSet.Count(d => d.MOBNO == mobileNo);
                    var EmailID_Exists = db.AslUsercoDbSet.Count(d => d.EMAILID == emailid);
                    var LoginID_Exists = db.AslUsercoDbSet.Count(d => d.LOGINID == loginID);

                    var findData = (from m in db.AslUsercoDbSet
                                    where m.COMPID == asluserco.COMPID && m.AslUsercoId == asluserco.AslUsercoId
                                    select new { m.MOBNO, m.EMAILID, m.LOGINID }).ToList();
                    foreach (var get in findData)
                    {
                        if (Mobile_Exists != 0)
                        {
                            asluserco.MOBNO = get.MOBNO;
                        }
                        if (EmailID_Exists != 0)
                        {
                            asluserco.EMAILID = get.EMAILID;
                        }
                        if (LoginID_Exists != 0)
                        {
                            asluserco.LOGINID = get.LOGINID;
                        }
                    }

                    db.Entry(asluserco).State = EntityState.Modified;

                    //Get Ip ADDRESS,Time & user PC Name
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];

                    asluserco.USERPC = strHostName;
                    asluserco.UPDIPNO = ipAddress.ToString();
                    asluserco.UPDTIME = Convert.ToDateTime(td);
                    //asluserco.UPDTIME = DateTime.Parse(td.ToString(), dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    //Update User ID save AslUSerco table attribute INSUSERID
                    asluserco.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                    Update_Asl_LogData(asluserco);

                    db.SaveChanges();
                    TempData["UserUpdateMessage"] = "'" + asluserco.USERNM + "' successfully updated!";
                    return RedirectToAction("UpdateForm");
                }
                else
                {
                    return View("UpdateForm");
                }
            }

            else
            {
                return RedirectToAction("Create");
            }



        }



        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetUserInformation(Int64 changedDropDown)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String userName = "", departmentName = "", operationType = "", address = "", mobileNo = "", emailId = "", loginBy = "", loginId = "", loginPassword = "", timerFor = "", timerto = "", status = "", inserttime = "", insertIpno = "", insltude = "", userpc = "";
            Int64 aslUserCoId = 0, companyID = 0, insertUserId = 0;
            var rt = db.AslUsercoDbSet.Where(n => n.USERID == changedDropDown &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             aslUserCoID = n.AslUsercoId,
                                                             companyID = n.COMPID,
                                                             userName = n.USERNM,
                                                             departmentName = n.DEPTNM,
                                                             operationType = n.OPTP,
                                                             address = n.ADDRESS,
                                                             mobileNo = n.MOBNO,
                                                             emailID = n.EMAILID,
                                                             loginBy = n.LOGINBY,
                                                             loginID = n.LOGINID,
                                                             loginPassword = n.LOGINPW,
                                                             timerFor = n.TIMEFR,
                                                             timerTo = n.TIMETO,
                                                             status = n.STATUS,
                                                             insuserid = n.INSUSERID,
                                                             instime = n.INSTIME,
                                                             insipno = n.INSIPNO,
                                                             insltude = n.INSLTUDE

                                                         });
            foreach (var n in rt)
            {
                aslUserCoId = n.aslUserCoID;
                companyID = Convert.ToInt64(n.companyID);
                userName = Convert.ToString(n.userName);
                departmentName = Convert.ToString(n.departmentName);
                operationType = Convert.ToString(n.operationType);
                address = Convert.ToString(n.address);
                mobileNo = Convert.ToString(n.mobileNo);
                emailId = Convert.ToString(n.emailID);
                loginBy = Convert.ToString(n.loginBy);
                loginId = Convert.ToString(n.loginID);
                loginPassword = Convert.ToString(n.loginPassword);
                timerFor = Convert.ToString(n.timerFor);
                timerto = Convert.ToString(n.timerTo);
                status = Convert.ToString(n.status);
                insertUserId = n.insuserid;
                inserttime = Convert.ToString(n.instime);
                insertIpno = Convert.ToString(n.insipno);
                insltude = Convert.ToString(n.insltude);
            }

            var result = new
            {
                ASLUSERCOID = aslUserCoId,
                COMPID = companyID,
                USERNAME = userName,
                DEPTNM = departmentName,
                OPTP = operationType,
                ADDRESS = address,
                MOBNO = mobileNo,
                EMAILID = emailId,
                LOGINBY = loginBy,
                LOGINID = loginId,
                LOGINPASSWORD = loginPassword,
                TIMERFOR = timerFor,
                TIMERTO = timerto,
                STATUS = status,
                INSUSERID = insertUserId,
                INSTIME = inserttime,
                INSIPNO = insertIpno,
                INSLTUDE = insltude
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        //Get: /AslUserco
        public ActionResult DeleteForm()
        {
            ////<br/> brack removed;
            //if (System.Web.HttpContext.Current.Session["LoggedUserType"] != null)
            //{
            //    var LoggedUserTp = System.Web.HttpContext.Current.Session["LoggedUserType"].ToString();
            //    if (LoggedUserTp == "CompanyAdmin" || LoggedUserTp == "UserAdmin" || LoggedUserTp == "User")
            //    {
            //        TempData["BrackFieldDropFromLayout_AslUserCOController"] = " <br/> brack removed";
            //    }
            //}
            ViewData["HighLight_Menu_UserForm"] = "High Light Menu";
            return View();
        }

        //
        // POST: /AslUserCO/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(AslUserco asluserco, String command)
        {
            if (command == "Delete")
            {

                if (asluserco.USERNM != null && asluserco.OPTP != null && asluserco.ADDRESS != null &&
                    asluserco.MOBNO != null && asluserco.LOGINBY != null &&
                    asluserco.LOGINID != null && asluserco.LOGINPW != null && asluserco.STATUS != null)
                {
                    AslUserco aslUsercoDelete = db.AslUsercoDbSet.Find(asluserco.AslUsercoId);

                    //Get Ip ADDRESS,Time & user PC Name 
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];

                    aslUsercoDelete.USERPC = strHostName;
                    aslUsercoDelete.UPDIPNO = ipAddress.ToString();
                    aslUsercoDelete.UPDTIME = Convert.ToDateTime(td);

                    //Delete User ID save AslUSerco table attribute INSUSERID
                    aslUsercoDelete.UPDUSERID =
                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    //Get current LOGLTUDE data 
                    aslUsercoDelete.UPDLTUDE = asluserco.UPDLTUDE;

                    Delete_Asl_LogData(aslUsercoDelete);
                    Delete_ASL_DELETE(aslUsercoDelete);

                    db.AslUsercoDbSet.Remove(aslUsercoDelete);
                    db.SaveChanges();

                    //Role database list Delete
                    var roleList = (from sub in db.AslRoleDbSet select sub)
                        .Where(sub => sub.USERID == aslUsercoDelete.USERID);
                    foreach (var n in roleList)
                    {
                        db.AslRoleDbSet.Remove(n);
                    }
                    db.SaveChanges();

                    TempData["UserDeleteMessage"] = "'" + aslUsercoDelete.USERNM + "' successfully Deleted!";
                    return RedirectToAction("DeleteForm");
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            else
            {
                return RedirectToAction("Create");
            }
        }


        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetUserInformationDelete(Int64 changedDropDown)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String userName = "", departmentName = "", operationType = "", address = "", mobileNo = "", emailId = "", loginBy = "", loginId = "", loginPassword = "", timerFor = "", timerto = "", status = "";
            Int64 aslUserCoId = 0, companyID = 0;
            var rt = db.AslUsercoDbSet.Where(n => n.USERID == changedDropDown &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             aslUserCoID = n.AslUsercoId,
                                                             companyID = n.COMPID,
                                                             userName = n.USERNM,
                                                             departmentName = n.DEPTNM,
                                                             operationType = n.OPTP,
                                                             address = n.ADDRESS,
                                                             mobileNo = n.MOBNO,
                                                             emailID = n.EMAILID,
                                                             loginBy = n.LOGINBY,
                                                             loginID = n.LOGINID,
                                                             loginPassword = n.LOGINPW,
                                                             timerFor = n.TIMEFR,
                                                             timerTo = n.TIMETO,
                                                             status = n.STATUS
                                                         });
            foreach (var n in rt)
            {
                aslUserCoId = n.aslUserCoID;
                companyID = Convert.ToInt64(n.companyID);
                userName = Convert.ToString(n.userName);
                departmentName = Convert.ToString(n.departmentName);
                operationType = Convert.ToString(n.operationType);
                address = Convert.ToString(n.address);
                mobileNo = Convert.ToString(n.mobileNo);
                emailId = Convert.ToString(n.emailID);
                loginBy = Convert.ToString(n.loginBy);
                loginId = Convert.ToString(n.loginID);
                loginPassword = Convert.ToString(n.loginPassword);
                timerFor = Convert.ToString(n.timerFor);
                timerto = Convert.ToString(n.timerTo);
                status = Convert.ToString(n.status);
            }

            var result = new { ASLUSERCOID = aslUserCoId, COMPID = companyID, USERNAME = userName, DEPTNM = departmentName, OPTP = operationType, ADDRESS = address, MOBNO = mobileNo, EMAILID = emailId, LOGINBY = loginBy, LOGINID = loginId, LOGINPASSWORD = loginPassword, TIMERFOR = timerFor, TIMERTO = timerto, STATUS = status };
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        //public JsonResult TagSearch(string term)
        //{
        //    // Get Tags from database
        //    var tags = from p in db.AslCompanyDbSet
        //               select p.COMPNM;

        //    return this.Json(tags.Where(t => t.StartsWith(term)),
        //                    JsonRequestBehavior.AllowGet);
        //}



        // GET: /ASLuserco/SearchUserList/5
        public ActionResult SearchUserList()
        {
            ViewData["HighLight_Menu_Settings"] = "High Light Menu";
            return View();
        }


        //SearchUserList Table, this view table works partial
        public PartialViewResult UserInfo(Int64 userID, Int64 companyID)
        {
            List<AslUserco> aslUsercos = new List<AslUserco>();

            if (userID != 0 && companyID != 0)
            {
                aslUsercos = db.AslUsercoDbSet.Where(e => e.COMPID == companyID && e.USERID == userID).ToList();

                return PartialView("~/Views/AslUserCO/_UserInfo.cshtml", aslUsercos);
            }
            else
            {
                aslUsercos = db.AslUsercoDbSet.Where(e => e.COMPID == companyID).ToList();
                return PartialView("~/Views/AslUserCO/_UserInfo.cshtml", aslUsercos);
            }

        }



        public JsonResult TagSearch(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.AslUsercoDbSet
                       where p.COMPID == compid
                       select p.USERNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                            JsonRequestBehavior.AllowGet);
        }

        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UserNameChanged(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 itemId = 0;
            Int64 companyId = 0;
            var rt = db.AslUsercoDbSet.Where(n => n.USERNM == changedText && n.COMPID == compid).Select(n => new
            {
                userID = n.USERID,
                companyID = n.COMPID
            });

            foreach (var n in rt)
            {
                itemId = Convert.ToInt64(n.userID);
                companyId = Convert.ToInt64(n.companyID);
            }
            var result = new { ITEMID = itemId, COMPID = companyId };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        // GET: /ASLuserco/SearchCompanyList/5
        public ActionResult SearchCompanyList()
        {
            ViewData["HighLight_Menu_InformationForm"] = "High Light Menu";
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchCompanyList(CompanyModel model)
        {
            TempData["SearchCompanyList_PartialView"] = "Show Partial view";
            return RedirectToAction("SearchCompanyList");
        }




        //Get Comapany Name when Comapny Name Dropdownlist Changed.
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ComapnyNameChanged(Int64 changedDropDown)
        {
            // var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 compid = 0;
            var rt = db.AslCompanyDbSet.Where(n => n.COMPID == changedDropDown).Select(n => new
            {
                companyId = n.COMPID

            });

            foreach (var n in rt)
            {
                compid = Convert.ToInt64(n.companyId);
            }

            return Json(compid, JsonRequestBehavior.AllowGet);

        }

        //SearchCompanyList Table, this view table works partial
        public PartialViewResult CompanyInfo(Int64 companyID)
        {
            List<AslUserco> aslUsercos = new List<AslUserco>();

            if (companyID != 0)
            {
                aslUsercos = db.AslUsercoDbSet.Where(e => e.COMPID == companyID).ToList();
                return PartialView("~/Views/AslUserCO/_CompanyInfo.cshtml", aslUsercos);
            }
            else
            {
                aslUsercos = db.AslUsercoDbSet.Where(e => e.OPTP != "AslSuperadmin").ToList();
                return PartialView("~/Views/AslUserCO/_CompanyInfo.cshtml", aslUsercos);
            }

        }




        public JsonResult Check_PhoneNumber(string mobNo)
        {
            var result = db.AslUsercoDbSet.Count(d => d.MOBNO == mobNo) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_UserEmail(string emailId)
        {
            var result1 = db.AslUsercoDbSet.Count(a => a.EMAILID == emailId) == 0;
            return Json(result1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_UserLogIn(string loginid)
        {
            var result1 = db.AslUsercoDbSet.Count(a => a.LOGINID == loginid) == 0;
            return Json(result1, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}