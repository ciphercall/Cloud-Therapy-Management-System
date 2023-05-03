using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Controllers;
using AS_Therapy_GL.Models;
using AS_Therapy_GL.Models.ASL;
using AS_Therapy_GL.Models.DTO;
using iTextSharp.text;
using Microsoft.SqlServer.Server;

namespace AS_Therapy_GL.Controllers.ASL
{
    public class SmsController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public SmsController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            ViewData["HighLight_Menu_PromotionForm"] = "High Light Menu";
        }






        // GET: /SMS/
        public ActionResult Index()
        {
            WebClient chcksmsqtySndSms = new WebClient();
            //string userName = "asl-sms";
            //string usPss = "asl.123SMS@3412";

            Int64 LoggedCompId = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
            var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                     where m.COMPID == LoggedCompId
                                     select new { m.SMSIDP, m.SMSPWP };
            string userName = "", usPss = "";
            foreach (var VARIABLE in getSMSUserNamePass)
            {
                userName = VARIABLE.SMSIDP;
                usPss = VARIABLE.SMSPWP;
            }

            string qty = "";
            if (userName == null || usPss == null)
            {
                qty = "0";
            }
            else
            {
                qty = chcksmsqtySndSms.DownloadString("http://app.itsolutionbd.net/api/command?username=" + userName + "&password=" + usPss + "&cmd=Credits");
            }
            TempData["CheckSMSQuantity"] = qty;
            return View();
        }



        [HttpPost]
        public ActionResult Index(MailDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.GROUPID == null && model.MobileNo == null)
                {
                    ViewBag.SMSMessage = "Select group name or Mobile number filed first!";
                    return View("Index");
                }


                WebClient chcksmsqtySndSms = new WebClient();
                string senderName = "ALCHEMY-BD";
                //string userName = "asl-sms";
                //string usPss = "asl.123SMS@3412";

                var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                         where m.COMPID == model.COMPID
                                         select new { m.SMSIDP, m.SMSPWP };
                string userName = "", usPss = "";
                foreach (var VARIABLE in getSMSUserNamePass)
                {
                    userName = VARIABLE.SMSIDP;
                    usPss = VARIABLE.SMSPWP;
                }

                if (userName != "" && usPss != "")
                {
                    try
                    {
                        string qty = chcksmsqtySndSms.DownloadString("http://app.itsolutionbd.net/api/command?username=" + userName + "&password=" + usPss + "&cmd=Credits");
                        TempData["CheckSMSQuantity"] = qty;

                        WebClient cardsms = new WebClient();
                        string cashStatus = "";

                        //Current group contact list add in ASL_PEMAIL table. 
                        if (model.GROUPID != null)
                        {
                            var getUploadContactList = (from m in db.UploadContactDbSet where m.COMPID == model.COMPID && m.GROUPID == model.GROUPID select new{m.MOBNO1,m.MOBNO2}).ToList();
                            foreach (var addList in getUploadContactList)
                            {
                                if (BdNumberValidate(addList.MOBNO1))
                                {
                                    string mobileNo = addList.MOBNO1;
                                    Insert_ASL_PSMS_Table(model,mobileNo);
                                }
                                if (BdNumberValidate(addList.MOBNO2))
                                {
                                    string mobileNo = addList.MOBNO2;
                                    Insert_ASL_PSMS_Table(model,mobileNo);
                                }
                            }
                        }


                        //model.ToEmail list add in ASL_PEMAIL Table.
                        if (model.MobileNo != null)
                        {
                            string[] multi = model.MobileNo.Split(',');
                            foreach (var multiemailId in multi)
                            {
                                if (multiemailId != "" && BdNumberValidate(multiemailId))
                                {
                                    string mobileNo = multiemailId;
                                    Insert_ASL_PSMS_Table(model,mobileNo);
                                }
                            }
                        }



                        string currentDate = td.ToString("yyyy-MM-dd");
                        DateTime transdate = Convert.ToDateTime(currentDate.Substring(0, 10));
                        Int64 year = Convert.ToInt64(td.ToString("yyyy"));
                        var find_ASLPSMS = (from m in db.SendLogSMSDbSet where m.COMPID == model.COMPID && m.TRANSDT == transdate && m.TRANSYY == year && m.STATUS == "PENDING" select m).ToList();
                        Int64 countSms = 0;
                        foreach (var x in find_ASLPSMS)
                        {
                            if (model.Language == "ENG")
                                cashStatus = cardsms.DownloadString("http://app.itsolutionbd.net/api/sendsms/plain?user=" + userName +
                                                   "&password=" + usPss + "&sender=" + senderName + "&SMSText=" + model.Body + "&GSM=" +
                                                   x.MOBNO);
                            else
                                cashStatus = cardsms.DownloadString("http://app.itsolutionbd.net/api/sendsms/plain?user=" + userName +
                                                   "&password=" + usPss + "&sender=" + senderName + "&SMSText=" + model.Body + "&GSM=" +
                                                    x.MOBNO + "&type=longSMS&datacoding=8");

                            if (GetApiResponseCodeMeaning(cashStatus) == "Request was successful")
                            {
                                countSms++;
                                Update_ASL_PSMS_Table_SendingEmail(x, model);
                            }
                        }



                        //  if ((cashStatus == "-1") || (cashStatus == "-2") || (cashStatus == "-3") || (cashStatus == "-5") ||
                        //(cashStatus == "-6") || (cashStatus == "-10") || (cashStatus == "-11") ||
                        //(cashStatus == "-13") || (cashStatus == "-22") || (cashStatus == "-23") || (cashStatus == "-26") ||
                        //(cashStatus == "-27") || (cashStatus == "-28") || (cashStatus == "-29") || (cashStatus == "-30") ||
                        //(cashStatus == "-33") || (cashStatus == "-34") || (cashStatus == "-99"))
                        //  {
                        //      ViewBag.SMSMessage = "Sms not sent.";
                        //      return View();
                        //  }
                        //  else
                        //  {
                        //      TempData["SMSMessage"] = "Sms sent.";
                        //      return RedirectToAction("Index");
                        //  }


                        if (countSms != 0)
                        {
                            TempData["SMSMessage"] = "Sending Done in " + countSms + " sms.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.SMSMessage = "Sms not sent.";
                            return View("Index");
                        }

                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.SMSMessage = "Sending Failed!!";
                        return View();
                    }

                }
                else
                {
                    ViewBag.SMSMessage = "Your SMS id not registered!!";
                    return View("Index");
                }

            }
            return View();
        }






        private bool BdNumberValidate(string number)
        {
            try
            {
                if (number.Length > 13 || number.Length < 13)
                {
                    return false;
                }
                else
                {
                    string operatorCode = number.Substring(0, 5);
                    switch (operatorCode)
                    {
                        case "88018":
                        case "88017":
                        case "88019":
                        case "88016":
                        case "88011":
                        case "88015":
                            return true; //all of the operator in case is return true
                            break;
                        default:
                            return false; //other operator code return false
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private string GetApiResponseCodeMeaning(string responseCode)
        {

            if (responseCode.Length > 5)
            {
                return "Request was successful";
            }
            switch (responseCode)
            {

                case "-1":
                    return "Error in processing the request";
                    break;
                case "-2":
                    return "Not enough credits on a specific account";
                    break;
                case "-3":
                    return "Targeted network is not covered on specific account";
                    break;
                case "-5":
                    return "Username or password is invalid";
                    break;
                case "-6":
                    return "Destination address is missing in the request ";
                    break;
                case "10":
                    return "Username is missing in the request";
                    break;
                case "11":
                    return "Password is missing in the request";
                    break;
                case "13":
                    return "Number is not recognized by Infobip platform";
                    break;
                case "22":
                    return "Incorrect XML format, caused by syntax error";
                    break;
                case "23":
                    return "General error, reasons may vary";
                    break;
                case "26":
                    return "General API error, reasons may vary";
                    break;
                case "27":
                    return "Invalid scheduling parametar";
                    break;
                case "28":
                    return "Invalid PushURL in the request";
                    break;
                case "29":
                    return "Incorrect Json format, caused by syntax error";
                    break;
                case "30":
                    return "Invalid APPID in the request";
                    break;
                case "33":
                    return "Duplicated MessagelD in the request";
                    break;
                case "34":
                    return "Sender name is not allowed";
                    break;
                case "99":
                    return "Error in processing request, reasons may vary";
                    break;
                default:
                    return "Unkown response code";
                    break;
            }
        }









        public void Insert_ASL_PSMS_Table(MailDTO currentMailBody,string mobileNo)
        {
            ASL_PSMS smsLogData = new ASL_PSMS();
            smsLogData.COMPID = Convert.ToInt64(currentMailBody.COMPID);
            string currentDate = td.ToString("dd-MMM-yyyy");
            smsLogData.TRANSDT = Convert.ToDateTime(currentDate);
            smsLogData.TRANSYY = Convert.ToInt64(td.ToString("yyyy"));

            string year = td.ToString("yyyy");
            string last2Digit_inYEAR = year.Substring(2, 2);
            Int64 max = Convert.ToInt64(currentMailBody.COMPID + last2Digit_inYEAR + "9999");
            try
            {
                Int64 maxTransNO = Convert.ToInt64((from n in db.SendLogSMSDbSet where n.COMPID == currentMailBody.COMPID && n.TRANSYY == smsLogData.TRANSYY select n.TRANSNO).Max());
                if (maxTransNO == 0)
                {
                    smsLogData.TRANSNO = Convert.ToInt64(currentMailBody.COMPID + last2Digit_inYEAR + "0001");
                }
                else if (maxTransNO <= max)
                {
                    smsLogData.TRANSNO = maxTransNO + 1;
                }
            }
            catch
            {
                smsLogData.TRANSNO = Convert.ToInt64(currentMailBody.COMPID + last2Digit_inYEAR + "0001");
            }

            if(mobileNo!=null)
            {
                smsLogData.MOBNO = mobileNo;
            }

            smsLogData.LANGUAGE = currentMailBody.Language;
            smsLogData.MESSAGE = currentMailBody.Body;
            smsLogData.STATUS = "PENDING";
            //smsLogData.SENTTM = null;

            smsLogData.USERPC = strHostName;
            smsLogData.INSIPNO = ipAddress.ToString();
            smsLogData.INSTIME = Convert.ToDateTime(td);
            smsLogData.INSUSERID = currentMailBody.INSUSERID;
            smsLogData.INSLTUDE = Convert.ToString(currentMailBody.INSLTUDE);
            db.SendLogSMSDbSet.Add(smsLogData);
            db.SaveChanges();
        }


        public void Update_ASL_PSMS_Table_SendingEmail(ASL_PSMS model, MailDTO currentMailBody)
        {
            var updateTable =
                (from m in db.SendLogSMSDbSet where m.ID == model.ID && m.COMPID == model.COMPID select m).ToList();
            if (updateTable.Count == 1)
            {
                foreach (ASL_PSMS smsLogData in updateTable)
                {
                    smsLogData.STATUS = "SENT";
                    smsLogData.SENTTM = Convert.ToDateTime(td);
                    smsLogData.USERPC = strHostName;
                    smsLogData.UPDIPNO = ipAddress.ToString();
                    smsLogData.UPDTIME = Convert.ToDateTime(td);
                    smsLogData.UPDUSERID = currentMailBody.INSUSERID;
                    smsLogData.UPDLTUDE = Convert.ToString(currentMailBody.INSLTUDE);
                }
                db.SaveChanges();
            }
        }










        // GET: /Mail/
        public ActionResult getPendingSMS()
        {
            WebClient chcksmsqtySndSms = new WebClient();
            //string userName = "asl-sms";
            //string usPss = "asl.123SMS@3412";

            Int64 LoggedCompId = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
            var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                     where m.COMPID == LoggedCompId
                                     select new { m.SMSIDP, m.SMSPWP };
            string userName = "", usPss = "";
            foreach (var VARIABLE in getSMSUserNamePass)
            {
                userName = VARIABLE.SMSIDP;
                usPss = VARIABLE.SMSPWP;
            }

            string qty = "";
            if (userName == null || usPss == null)
            {
                qty = "0";
            }
            else
            {
                qty = chcksmsqtySndSms.DownloadString("http://app.itsolutionbd.net/api/command?username=" + userName + "&password=" + usPss + "&cmd=Credits");
            }
            TempData["CheckSMSQuantity"] = qty;
            return View();
        }



        [HttpPost]
        public ActionResult getPendingSMS(PendingMailSmsDTO model, string command)
        {
            if (ModelState.IsValid)
            {
                DateTime transDate = Convert.ToDateTime(model.TRANSDT);
                if (command == "Search")
                {
                    List<ASL_PSMS> pendingList = new List<ASL_PSMS>();
                    var findPendingList =
                        (from m in db.SendLogSMSDbSet
                         where m.COMPID == model.COMPID && m.TRANSDT == transDate && m.STATUS == "PENDING"
                         select m).ToList();
                    foreach (var x in findPendingList)
                    {
                        pendingList.Add(new ASL_PSMS { MOBNO = x.MOBNO.ToString(), MESSAGE = x.MESSAGE.ToString(), STATUS = x.STATUS.ToString() });
                        ViewData["PendingList_SMS"] = pendingList;
                    }
                    return View();
                }
                else if (command == "Send")
                {
                    try
                    {
                        WebClient chcksmsqtySndSms = new WebClient();
                        string senderName = "ALCHEMY-BD";
                        //string userName = "asl-sms";
                        //string usPss = "asl.123SMS@3412";

                        var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                                 where m.COMPID == model.COMPID
                                                 select new { m.EMAILIDP, m.EMAILPWP };
                        string userName = "", usPss = "";
                        foreach (var VARIABLE in getSMSUserNamePass)
                        {
                            userName = VARIABLE.EMAILIDP;
                            usPss = VARIABLE.EMAILPWP;
                        }


                        if (userName == null || usPss == null)
                        {
                            ViewBag.PendingSMSMessage = "Your Email id not registered!!";
                            return View();
                        }

                        string qty = chcksmsqtySndSms.DownloadString("http://app.itsolutionbd.net/api/command?username=" + userName + "&password=" + usPss + "&cmd=Credits");
                        TempData["CheckSMSQuantity"] = qty;

                        WebClient cardsms = new WebClient();
                        string cashStatus = "";
                        var findPendingList = (from m in db.SendLogSMSDbSet where m.COMPID == model.COMPID && m.TRANSDT == transDate && m.STATUS == "PENDING" select m).ToList();
                        Int64 count = 0;
                        foreach (var x in findPendingList)
                        {
                            count++;
                            if (x.LANGUAGE == "ENG")
                                cashStatus = cardsms.DownloadString("http://app.itsolutionbd.net/api/sendsms/plain?user=" + userName +
                                                   "&password=" + usPss + "&sender=" + senderName + "&SMSText=" + x.MESSAGE + "&GSM=" +
                                                   x.MOBNO);
                            else // if (x.LANGUAGE == "BNG")
                                cashStatus = cardsms.DownloadString("http://app.itsolutionbd.net/api/sendsms/plain?user=" + userName +
                                                   "&password=" + usPss + "&sender=" + senderName + "&SMSText=" + x.MESSAGE + "&GSM=" +
                                                    x.MOBNO + "&type=longSMS&datacoding=8");

                            if (GetApiResponseCodeMeaning(cashStatus) == "Request was successful")
                            {
                                count++;
                                MailDTO pendingSMSBody = new MailDTO();
                                pendingSMSBody.INSUSERID = model.UPDUSERID;
                                pendingSMSBody.INSLTUDE = model.UPDLTUDE;
                                Update_ASL_PSMS_Table_SendingEmail(x, pendingSMSBody);
                            }

                        }
                        ViewBag.PendingSMSMessage = "Send Succesfully " + count + " pending sms.";
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.PendingSMSMessage = "Sending Failed!!";
                        return View();
                    }
                }
            }
            return View("getPendingSMS");
        }






        //tag-it autocomplete
        public JsonResult TagSearch_tagit(string term, string compid)
        {
            var companyid = Convert.ToInt16(compid);
            var mobileNO1 = (from p in db.UploadContactDbSet
                        where p.COMPID == companyid && p.MOBNO1 != null
                        select p.MOBNO1).ToList();
            var mobileNO2 = (from p in db.UploadContactDbSet
                             where p.COMPID == companyid && p.MOBNO2 != null
                             select p.MOBNO2).ToList();
            var tags = mobileNO1.Union(mobileNO2).Distinct().ToList();
            
            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);

        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
