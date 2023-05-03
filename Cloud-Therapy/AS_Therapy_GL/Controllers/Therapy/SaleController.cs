using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;
using iTextSharp.text;

namespace AS_Therapy_GL.Controllers
{
    public class SaleController : AppController
    {
        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;
        public string transdate;

        Therapy_GL_DbContext db = new Therapy_GL_DbContext();
        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        private Int64 compid;

        public SaleController()
        {
            //HttpCookie decodedCookie1 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CI"]);
            //HttpCookie decodedCookie2 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UI"]);
            //HttpCookie decodedCookie3 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UT"]);
            //HttpCookie decodedCookie4 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UN"]);
            //HttpCookie decodedCookie5 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["US"]);
            //HttpCookie decodedCookie6 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CS"]);

            //if (System.Web.HttpContext.Current.Request.Cookies["UI"] != null)
            //{
            //    compid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["CI"].Value);
            //}
            //else
            //{
            //    Index();
            //}
            compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            transdate = Convert.ToString(td.ToString("d"));

            ViewData["HighLight_Menu_BillingForm"] = "Heigh Light Menu";
        }



        // Create ASL_LOG object and it used to this Insert_Asl_LogData (RMS_TRANSMST rmsTransmst).
        public ASL_LOG aslLog = new ASL_LOG();

        // SAVE ALL INFORMATION from RmsTransMst TO Asl_lOG Database Table.
        public void Insert_PST_TRANSMST_LogData(PST_TRANSMST model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == compid && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(compid);
            aslLog.USERID = model.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.INSIPNO;
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.TABLEID = "PST_TRANSMST";

            string PatientName = "";
            Int64 patientIDM = 0;
            var findStoreName = (from n in db.PST_PatientDbSet where n.COMPID == compid && n.PATIENTID == model.PATIENTID select n).ToList();
            foreach (var x in findStoreName)
            {
                patientIDM = Convert.ToInt64(x.PATIENTIDM);
                PatientName = x.PATIENTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Transaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nPatient ID: " + patientIDM + ",\nPatient Name:" + PatientName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nTOTREF: " + model.TOTREF + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        public void Update_PST_TRANSMST_LogData(PST_TRANSMST model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == compid && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(compid);
            aslLog.USERID = model.INSUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.INSIPNO;
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.TABLEID = "PST_TRANSMST";

            string PatientName = "";
            Int64 patientIDM = 0;
            var findStoreName = (from n in db.PST_PatientDbSet where n.COMPID == compid && n.PATIENTID == model.PATIENTID select n).ToList();
            foreach (var x in findStoreName)
            {
                patientIDM = Convert.ToInt64(x.PATIENTIDM);
                PatientName = x.PATIENTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Transaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nPatient ID: " + patientIDM + ",\nPatient Name:" + PatientName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nTOTREF: " + model.TOTREF + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }





        public void Delete_PST_TRANSMST_LogData(PST_TRANSMST model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 userID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == compid && n.USERID == userID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(compid);
            aslLog.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.TABLEID = "PST_TRANSMST";

            string PatientName = "";
            Int64 patientIDM = 0;
            var findStoreName = (from n in db.PST_PatientDbSet where n.COMPID == compid && n.PATIENTID == model.PATIENTID select n).ToList();
            foreach (var x in findStoreName)
            {
                patientIDM = Convert.ToInt64(x.PATIENTIDM);
                PatientName = x.PATIENTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Delete also item list data! " + "Transaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nPatient ID: " + patientIDM + ",\nPatient Name:" + PatientName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nTOTREF: " + model.TOTREF + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
        }





        public ASL_DELETE AslDelete = new ASL_DELETE();
        public void Delete_PST_TRANSMST_DELETE(PST_TRANSMST model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("HH:mm:ss tt"));

            Int64 userID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == model.COMPID && n.USERID == userID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(model.COMPID);
            AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = ipAddress.ToString();
            AslDelete.DELLTUDE = model.UPDLTUDE;
            AslDelete.TABLEID = "PST_TRANSMST";

            string PatientName = "";
            Int64 patientIDM = 0;
            var findStoreName = (from n in db.PST_PatientDbSet where n.COMPID == compid && n.PATIENTID == model.PATIENTID select n).ToList();
            foreach (var x in findStoreName)
            {
                patientIDM = Convert.ToInt64(x.PATIENTIDM);
                PatientName = x.PATIENTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            AslDelete.DELDATA = Convert.ToString("Delete also item list Data! " + "Transaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nPatient ID: " + patientIDM + ",\nPatient Name:" + PatientName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nTOTREF: " + model.TOTREF + ",\nRemarks: " + model.REMARKS + ".");
            AslDelete.USERPC = strHostName;
            db.AslDeleteDbSet.Add(AslDelete);
        }









        // GET: /Transaction/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            //if (TempData["Patient_To_Sale"] != null)
            //{
            //    var dt = (PageModel)TempData["Patient_To_Sale"];
            //    return View(dt);
            //}
            //else
            //{
            var dt = (PageModel)TempData["data"];
            return View(dt);
            // }

        }






        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel model, string command)
        {
            if (command == "Add")
            {
                //Permission Check
                Int64 loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                var checkPermission = from role in db.AslRoleDbSet
                                      where role.USERID == loggedUserID && role.CONTROLLERNAME == "Sale" && role.ACTIONNAME == "Index"
                                      select new { role.INSERTR };
                string Insert = "";
                foreach (var VARIABLE in checkPermission)
                {
                    Insert = VARIABLE.INSERTR;
                }

                if (Insert == "I")
                {
                    ViewBag.InsertPermission = "Permission Denied !";
                    return View("Index");
                }


                if (model.pst_Trans.TRANSDT == null)
                {
                    ViewBag.errorDate = "Please select date!";
                    return View("Index");
                }


                var PatientName = (from n in db.PST_PatientDbSet where n.COMPID == compid && n.PATIENTIDM == model.Pst_Patient.PATIENTIDM select n).ToList();
                foreach (var Name in PatientName)
                {
                    model.pst_Trans.PATIENTID = Convert.ToInt64(Name.PATIENTID);
                }
                if (PatientName.Count == 0)
                {
                    ViewBag.errorPatient = "Please select a valid Patient Id!";
                    return View("Index");
                }



                //Validation Check
                if ((model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.POSNID == null && model.pst_Trans.ITEMTP == "THERAPY")
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    ViewBag.errorPositionName = "Please select a valid Position Name!";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0)
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if ((model.pst_Trans.POSNID == null || model.pst_Trans.POSNID == 0) && model.pst_Trans.ITEMTP == "THERAPY")
                {
                    ViewBag.errorPositionName = "Please select a valid Position Name!";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if ((model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.QTY == null)
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    ViewBag.errorQty = "Please select a valid quantity !";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.QTY == null && (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0))
                {
                    ViewBag.errorQty = "Please select a valid quantity !";
                    ViewBag.errorItemid = "Please Select a Valid Item Name !";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.QTY == null)
                {
                    ViewBag.errorQty = "Please select a valid quantity !";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }




                var patient_Info = from m in db.PST_PatientDbSet
                                   from n in db.PST_ReferDbSet
                                   where m.COMPID == compid && m.COMPID == n.COMPID
                                         && m.PATIENTID == model.pst_Trans.PATIENTID && m.REFERID == n.REFERID
                                   select new { m.PATIENTNM, n.REFERNM, n.REFPCNT };
                foreach (var x in patient_Info)
                {
                    model.Pst_Patient.PATIENTNM = x.PATIENTNM;
                    model.Pst_Refer.REFERNM = x.REFERNM;
                    model.pst_Trans.REFPCNT = x.REFPCNT;
                }



                model.pst_Trans.USERPC = strHostName;
                model.pst_Trans.INSIPNO = ipAddress.ToString();
                model.pst_Trans.INSTIME = td;
                model.pst_Trans.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.pst_Trans.TRANSTP = "SALE";
                model.pst_Trans.REFAMT = Convert.ToDecimal((model.pst_Trans.AMOUNT * model.pst_Trans.REFPCNT) / 100);

                var searchRow = true;
                if (model.pst_Trans.ITEMTP == "ACCESSORIES")
                {
                    model.pst_Trans.POSNID = null;
                    var result = db.PST_TransDbSet.Where(a => a.TRANSNO == model.pst_Trans.TRANSNO && a.TRANSTP == "SALE" && a.TRANSYY == model.pst_Trans.TRANSYY && a.COMPID == compid).Count(a => a.ITEMID == model.pst_Trans.ITEMID) == 0;
                    searchRow = result;
                }
                else
                {
                    var result = db.PST_TransDbSet.Where(a => a.TRANSNO == model.pst_Trans.TRANSNO && a.TRANSTP == "SALE" && a.TRANSYY == model.pst_Trans.TRANSYY && a.COMPID == compid).Count(a => a.ITEMID == model.pst_Trans.ITEMID && a.POSNID == model.pst_Trans.POSNID) == 0;
                    searchRow = result;
                }


                var res = searchRow;

                if (res == true)
                {
                    var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == model.pst_Trans.TRANSNO && x.TRANSTP == "SALE" && x.TRANSYY == model.pst_Trans.TRANSYY && x.COMPID == compid)
                                  .Max(o => o.ITEMSL);
                    var transno_Max = db.PST_TransDbSet.Where(x => x.TRANSYY == model.pst_Trans.TRANSYY && x.TRANSTP == "SALE" && x.COMPID == compid)
                        .Max(s => s.TRANSNO);
                    string transno = Convert.ToString(transno_Max);
                    if (model.pst_Trans.TRANSNO == null)
                    {
                        if (transno == "")
                        {
                            transno = Convert.ToString(model.pst_Trans.TRANSYY + "0001");
                            model.pst_Trans.TRANSNO = Convert.ToInt64(transno);
                            TempData["transno"] = transno;
                        }
                        else
                        {
                            Int64 convertTransNO = Convert.ToInt64(transno.Substring(4, 4));
                            Int64 transNO_Int = convertTransNO + 1;
                            if (transNO_Int < 10)
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + "000" + transNO_Int.ToString());
                            }
                            else if ((10 <= transNO_Int) && (transNO_Int <= 99))
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + "00" + transNO_Int.ToString());
                            }
                            else if ((100 <= transNO_Int) && (transNO_Int <= 999))
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + "0" + transNO_Int.ToString());
                            }
                            else
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + transNO_Int.ToString());
                            }

                            TempData["transno"] = model.pst_Trans.TRANSNO;
                        }

                        if (sid == null)
                        {
                            model.pst_Trans.ITEMSL = 1;
                        }
                        else
                        {
                            model.pst_Trans.ITEMSL = sid + 1;
                        }


                        db.PST_TransDbSet.Add(model.pst_Trans);
                        db.SaveChanges();

                        model.pst_Trans.ITEMSL = 0;
                        model.pst_Trans.ITEMID = 0;
                        model.pst_Trans.QTY = 0;
                        model.pst_Trans.RATE = 0;
                        model.pst_Trans.AMOUNT = 0;


                        model.PST_Item.ITEMNM = "";
                        model.pst_Trans.POSNID = 0;
                        model.Empty = "";

                        TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                        //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                        TempData["transdate"] = model.pst_Trans.TRANSDT;
                        TempData["transYear"] = model.pst_Trans.TRANSYY;
                        TempData["data"] = model;
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        TempData["transno"] = model.pst_Trans.TRANSNO;
                        model.pst_Trans.ITEMSL = Convert.ToInt64(sid) + 1;

                        db.PST_TransDbSet.Add(model.pst_Trans);
                        db.SaveChanges();

                        model.pst_Trans.ITEMSL = 0;
                        model.pst_Trans.ITEMID = 0;
                        model.pst_Trans.QTY = 0;
                        model.pst_Trans.RATE = 0;
                        model.pst_Trans.AMOUNT = 0;

                        model.PST_Item.ITEMNM = "";
                        model.pst_Trans.POSNID = 0;
                        model.Empty = "";

                        TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                        //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                        TempData["transdate"] = model.pst_Trans.TRANSDT;
                        TempData["transYear"] = model.pst_Trans.TRANSYY;
                        TempData["data"] = model;
                        return RedirectToAction("Index");
                    }

                }

                else
                {
                    if (model.pst_Trans.ITEMTP == "ACCESSORIES")
                    {
                        var result = (from n in db.PST_TransDbSet
                                      where n.TRANSNO == model.pst_Trans.TRANSNO &&
                                            n.COMPID == compid &&
                                            n.ITEMID == model.pst_Trans.ITEMID &&
                                            n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "SALE"
                                      select new
                                      {
                                          n.PST_TRANS_ID,
                                          n.ITEMSL
                                      }
                            );

                        foreach (var item in result)
                        {
                            model.pst_Trans.PST_TRANS_ID = item.PST_TRANS_ID;
                            model.pst_Trans.ITEMSL = item.ITEMSL;
                        }
                    }
                    else
                    {
                        var result = (from n in db.PST_TransDbSet
                                      where n.TRANSNO == model.pst_Trans.TRANSNO &&
                                            n.COMPID == compid &&
                                            n.ITEMID == model.pst_Trans.ITEMID && n.POSNID == model.pst_Trans.POSNID &&
                                            n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "SALE"
                                      select new
                                      {
                                          n.PST_TRANS_ID,
                                          n.ITEMSL
                                      }
                         );

                        foreach (var item in result)
                        {
                            model.pst_Trans.PST_TRANS_ID = item.PST_TRANS_ID;
                            model.pst_Trans.ITEMSL = item.ITEMSL;
                        }
                    }


                    db.Entry(model.pst_Trans).State = EntityState.Modified;
                    db.SaveChanges();

                    model.pst_Trans.ITEMSL = 0;
                    model.pst_Trans.ITEMID = 0;
                    model.pst_Trans.QTY = 0;
                    model.pst_Trans.RATE = 0;
                    model.pst_Trans.AMOUNT = 0;

                    model.PST_Item.ITEMNM = "";
                    model.pst_Trans.POSNID = 0;
                    model.Empty = "";

                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    TempData["data"] = model;

                    return RedirectToAction("Index");

                }
                //}

            }

            else if (command == "Save")
            {
                if (model.Pst_Transmst.TOTNET == 0)
                {
                    ViewBag.addItemList = "Please Add item list!";
                    return View("Index");
                }
                else if (model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.addItemList = "Please Add item list!";
                    return View("Index");
                }


                model.Pst_Transmst.USERPC = strHostName;
                model.Pst_Transmst.INSIPNO = ipAddress.ToString();
                model.Pst_Transmst.INSTIME = td;
                model.Pst_Transmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.Pst_Transmst.TRANSDT = model.pst_Trans.TRANSDT;
                model.Pst_Transmst.TRANSYY = model.pst_Trans.TRANSYY;
                model.Pst_Transmst.COMPID = model.pst_Trans.COMPID;
                model.Pst_Transmst.TRANSNO = Convert.ToInt64(model.pst_Trans.TRANSNO);
                model.Pst_Transmst.TRANSTP = "SALE";
                model.Pst_Transmst.PATIENTID = model.pst_Trans.PATIENTID;

                Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            else if (command == "A4")
            {
                if (model.Pst_Transmst.TOTNET == 0)
                {
                    ViewBag.addItemList = "Please Add item list!";
                    return View("Index");
                }
                else if (model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.addItemList = "Please Add item list!";
                    return View("Index");
                }


                var findTransNO = (from n in db.PST_TransMstDbSet
                                   where n.TRANSNO == model.pst_Trans.TRANSNO && n.COMPID == model.pst_Trans.COMPID &&
                          n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "SALE"
                                   select n).ToList();
                if (findTransNO.Count != 0)
                {

                }
                else
                {
                    model.Pst_Transmst.USERPC = strHostName;
                    model.Pst_Transmst.INSIPNO = ipAddress.ToString();
                    model.Pst_Transmst.INSTIME = td;
                    model.Pst_Transmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    model.Pst_Transmst.TRANSDT = model.pst_Trans.TRANSDT;
                    model.Pst_Transmst.TRANSYY = model.pst_Trans.TRANSYY;
                    model.Pst_Transmst.COMPID = model.pst_Trans.COMPID;
                    model.Pst_Transmst.TRANSNO = Convert.ToInt64(model.pst_Trans.TRANSNO);
                    model.Pst_Transmst.TRANSTP = "SALE";
                    model.Pst_Transmst.PATIENTID = model.pst_Trans.PATIENTID;

                    Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                    db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                    db.SaveChanges();
                }


                PageModel aPageModel = new PageModel();
                aPageModel.pst_Trans.TRANSNO = model.pst_Trans.TRANSNO;
                aPageModel.pst_Trans.TRANSDT = model.pst_Trans.TRANSDT;
                aPageModel.pst_Trans.COMPID = model.pst_Trans.COMPID;
                aPageModel.Pst_Transmst.TRANSTP = "SALE";
                aPageModel.Pst_Transmst.TRANSYY = model.pst_Trans.TRANSYY;
                aPageModel.Pst_Patient.PATIENTIDM = model.Pst_Patient.PATIENTIDM;
                TempData["Sale_Command"] = command;
                TempData["pageModel"] = aPageModel;
                return RedirectToAction("SaleMemo", "BillingReport");
            }


            else
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult OrderDelete(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, Int64 patientid, PageModel model)
        {
            PST_TRANS pstTrans = db.PST_TransDbSet.Find(tid);

            db.PST_TransDbSet.Remove(pstTrans);
            db.SaveChanges();

            var result = (from t in db.PST_TransDbSet
                          where t.COMPID == compid && t.TRANSNO == orderNo && t.TRANSDT == Date && t.TRANSYY == Year && t.TRANSTP == "SALE"
                          select new { t.TRANSYY, t.PATIENTID, t.RSID, t.TRANSDT }
             ).Distinct().ToList();


            foreach (var n in result)
            {
                model.pst_Trans.TRANSYY = Convert.ToInt64(n.TRANSYY);
                model.AGL_acchart.ACCOUNTCD = Convert.ToInt64(n.RSID);
                TempData["PATIENTID"] = n.PATIENTID;
                //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                TempData["transdate"] = n.TRANSDT;
                TempData["transYear"] = n.TRANSYY;
            }

            if (result.Count == 0)
            {
                TempData["transdate"] = null;
                TempData["transYear"] = null;
                model.pst_Trans.TRANSYY = null;
                model.pst_Trans.TRANSDT = null;
                orderNo = 0;
            }
            else
            {
                var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == orderNo && x.TRANSDT == Date && x.TRANSYY == Year && x.TRANSTP == "SALE" && x.COMPID == compid)
                            .Max(o => o.ITEMSL);
                model.pst_Trans.TRANSNO = orderNo;
                model.pst_Trans.ITEMSL = sid;
                model.pst_Trans.TRANSDT = Date;
            }

            var patient_Info = from m in db.PST_PatientDbSet
                               from n in db.PST_ReferDbSet
                               where m.COMPID == compid && m.COMPID == n.COMPID
                                     && m.PATIENTID == patientid && m.REFERID == n.REFERID
                               select new { m.PATIENTNM, n.REFERNM, n.REFPCNT };
            foreach (var x in patient_Info)
            {
                model.Pst_Patient.PATIENTNM = x.PATIENTNM;
                model.Pst_Refer.REFERNM = x.REFERNM;
                model.pst_Trans.REFPCNT = x.REFPCNT;
            }

            TempData["data"] = model;
            TempData["transno"] = orderNo;
            return RedirectToAction("Index");
        }




        public ActionResult OrderUpdate(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, Int64 itemId, PageModel model)
        {
            model.pst_Trans = db.PST_TransDbSet.Find(tid);

            var item = from r in db.PST_ItemDbSet where r.ITEMID == itemId select r.ITEMNM;
            foreach (var it in item)
            {
                model.PST_Item.ITEMNM = it.ToString();
            }

            model.pst_Trans.TRANSDT = Date;
            model.pst_Trans.TRANSYY = Year;


            var patient_Info = from m in db.PST_PatientDbSet
                               from n in db.PST_ReferDbSet
                               where m.COMPID == compid && m.COMPID == n.COMPID
                                     && m.PATIENTID == model.pst_Trans.PATIENTID && m.REFERID == n.REFERID
                               select new { m.PATIENTNM, n.REFERNM, n.REFPCNT };
            foreach (var x in patient_Info)
            {
                model.Pst_Patient.PATIENTNM = x.PATIENTNM;
                model.Pst_Refer.REFERNM = x.REFERNM;
                model.pst_Trans.REFPCNT = x.REFPCNT;
            }


            var pos_item = from r in db.PST_ItemDbSet where r.ITEMID == model.pst_Trans.POSNID && r.COMPID == model.pst_Trans.COMPID select r.ITEMNM;
            foreach (var it in pos_item)
            {
                TempData["positionName"] = it.ToString();
            }

            if (model.pst_Trans.ITEMTP == "ACCESSORIES")
            {
                model.pst_Trans.POSNID = null;
                TempData["Hide_positionName"] = "Hide Position Name";
            }

            TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
            //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
            TempData["transdate"] = model.pst_Trans.TRANSDT;
            TempData["transYear"] = model.pst_Trans.TRANSYY;
            TempData["data"] = model;
            TempData["transno"] = model.pst_Trans.TRANSNO;
            return RedirectToAction("Index");

        }




        //[AcceptVerbs("POST")]
        //public ActionResult OrderComplete(PageModel model)
        //{
        //    return RedirectToAction("Index");
        //}










        [AcceptVerbs("GET")]
        [ActionName("EditOrder")]
        public ActionResult EditOrder()
        {
            //Permission Check
            Int64 loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            var checkPermission = from role in db.AslRoleDbSet
                                  where role.USERID == loggedUserID && role.CONTROLLERNAME == "Sale" && role.ACTIONNAME == "Index"
                                  select new { role.UPDATER };
            string Update = "";
            foreach (var VARIABLE in checkPermission)
            {
                Update = VARIABLE.UPDATER;
            }

            if (Update == "I")
            {
                TempData["UpdatePermission"] = "Update Permission Denied !";
                return RedirectToAction("Index");
            }
            var dt = (PageModel)TempData["data"];
            return View(dt);
        }





        [AcceptVerbs("POST")]
        [ActionName("EditOrder")]
        public ActionResult EditOrder(PageModel model, string command)
        {
            if (command == "Add")
            {
                //Validation Check
                if (model.pst_Trans.TRANSDT == null && model.pst_Trans.QTY == null && (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.TransactionDate = "Transaction date required!";
                    ViewBag.errorQty = "Please select a valid quantity !";
                    ViewBag.errorItemid = "Please Select a Valid Item Name !";
                    ViewBag.MemoNO = "Please select a Memo NO!";
                    return View("EditOrder");
                }
                else if (model.pst_Trans.TRANSDT == null && model.pst_Trans.QTY == null && (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0))
                {
                    ViewBag.TransactionDate = "Transaction date required!";
                    ViewBag.errorQty = "Please select a valid quantity !";
                    ViewBag.errorItemid = "Please Select a Valid Item Name !";
                    TempData["data"] = model;
                    return View("EditOrder");
                }
                else if (model.pst_Trans.QTY == null && (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.errorQty = "Please select a valid quantity !";
                    ViewBag.errorItemid = "Please Select a Valid Item Name !";
                    ViewBag.MemoNO = "Please select a Memo NO!";
                    ViewBag.MemoNO = "Please select a Memo NO!";
                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    TempData["transdate"] = currentDate;
                    TempData["data"] = model;
                    return View("EditOrder");
                }
                else if (model.pst_Trans.TRANSDT == null)
                {
                    ViewBag.TransactionDate = "Transaction date required!";
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("EditOrder");
                }
                else if (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0)
                {
                    ViewBag.errorItemid = "Please Select a Valid Item Name !";

                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    TempData["transdate"] = currentDate;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("EditOrder");
                }
                else if (model.pst_Trans.QTY == null)
                {
                    ViewBag.errorQty = "Please select a valid quantity !";

                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transdate"] = currentDate;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("EditOrder");
                }
                else if (model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.MemoNO = "Please select a Memo NO!";

                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    TempData["transdate"] = currentDate;
                    TempData["data"] = model;
                    return View("EditOrder");
                }


                var PatientName = (from n in db.PST_PatientDbSet where n.COMPID == compid && n.PATIENTIDM == model.Pst_Patient.PATIENTIDM select n).ToList();
                foreach (var Name in PatientName)
                {
                    model.pst_Trans.PATIENTID = Convert.ToInt64(Name.PATIENTID);
                }
                if (PatientName.Count == 0)
                {
                    ViewBag.errorPatient = "Please select a valid Patient Id!";
                    return View("Index");
                }


                //Validation Check
                if ((model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.POSNID == null && model.pst_Trans.ITEMTP == "THERAPY")
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    ViewBag.errorPositionName = "Please select a valid Position Name!";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0)
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if ((model.pst_Trans.POSNID == null || model.pst_Trans.POSNID == 0) && model.pst_Trans.ITEMTP == "THERAPY")
                {
                    ViewBag.errorPositionName = "Please select a valid Position Name!";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if ((model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.QTY == null)
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    ViewBag.errorQty = "Please select a valid quantity !";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.QTY == null && (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0))
                {
                    ViewBag.errorQty = "Please select a valid quantity !";
                    ViewBag.errorItemid = "Please Select a Valid Item Name !";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.QTY == null)
                {
                    ViewBag.errorQty = "Please select a valid quantity !";
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }



                var searchRow = true;
                if (model.pst_Trans.ITEMTP == "ACCESSORIES")
                {
                    model.pst_Trans.POSNID = null;
                    var result = db.PST_TransDbSet.Where(a => a.TRANSNO == model.pst_Trans.TRANSNO && a.TRANSTP == "SALE" && a.TRANSYY == model.pst_Trans.TRANSYY && a.COMPID == compid).Count(a => a.ITEMID == model.pst_Trans.ITEMID) == 0;
                    searchRow = result;
                }
                else
                {
                    var result = db.PST_TransDbSet.Where(a => a.TRANSNO == model.pst_Trans.TRANSNO && a.TRANSTP == "SALE" && a.TRANSYY == model.pst_Trans.TRANSYY && a.COMPID == compid).Count(a => a.ITEMID == model.pst_Trans.ITEMID && a.POSNID == model.pst_Trans.POSNID) == 0;
                    searchRow = result;
                }


                var res = searchRow;
                if (res == true)
                {

                    //Permission Check
                    Int64 loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    var checkPermission = from role in db.AslRoleDbSet
                                          where role.USERID == loggedUserID && role.CONTROLLERNAME == "Sale" && role.ACTIONNAME == "Index"
                                          select new { role.INSERTR };
                    string Insert = "";
                    foreach (var VARIABLE in checkPermission)
                    {
                        Insert = VARIABLE.INSERTR;
                    }

                    if (Insert == "I")
                    {
                        ViewBag.InsertPermission = "Permission Denied !";
                        return View("EditOrder");
                    }



                    var get_TRANSDT_TRANSYY = (from m in db.PST_TransDbSet
                                               where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "SALE"
                                               select new { m.TRANSDT, m.TRANSYY }).Distinct().ToList();
                    foreach (var VARIABLE in get_TRANSDT_TRANSYY)
                    {
                        model.pst_Trans.TRANSDT = VARIABLE.TRANSDT;
                        model.pst_Trans.TRANSYY = VARIABLE.TRANSYY;
                    }

                    model.pst_Trans.USERPC = strHostName;
                    model.pst_Trans.INSIPNO = ipAddress.ToString();
                    model.pst_Trans.INSTIME = td;
                    model.pst_Trans.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    model.pst_Trans.TRANSTP = "SALE";
                    model.pst_Trans.REFAMT = Convert.ToDecimal((model.pst_Trans.AMOUNT * model.pst_Trans.REFPCNT) / 100);

                    var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == model.pst_Trans.TRANSNO && x.TRANSTP == "SALE" && x.TRANSYY == model.pst_Trans.TRANSYY && x.COMPID == compid)
                               .Max(o => o.ITEMSL);
                    var transno_Max = db.PST_TransDbSet.Where(x => x.TRANSYY == model.pst_Trans.TRANSYY && x.TRANSTP == "SALE" && x.COMPID == compid)
                        .Max(s => s.TRANSNO);

                    string transno = Convert.ToString(transno_Max);
                    if (model.pst_Trans.TRANSNO == null)
                    {
                        if (transno == "")
                        {
                            transno = Convert.ToString(model.pst_Trans.TRANSYY + "0001");
                            model.pst_Trans.TRANSNO = Convert.ToInt64(transno);
                            TempData["transno"] = transno;
                        }
                        else
                        {
                            Int64 convertTransNO = Convert.ToInt64(transno.Substring(4, 4));
                            Int64 transNO_Int = convertTransNO + 1;
                            if (transNO_Int < 10)
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + "000" + transNO_Int.ToString());
                            }
                            else if ((10 <= transNO_Int) && (transNO_Int <= 99))
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + "00" + transNO_Int.ToString());
                            }
                            else if ((100 <= transNO_Int) && (transNO_Int <= 999))
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + "0" + transNO_Int.ToString());
                            }
                            else
                            {
                                model.pst_Trans.TRANSNO = Convert.ToInt64(transno.Substring(0, 4) + transNO_Int.ToString());
                            }

                            TempData["transno"] = model.pst_Trans.TRANSNO;
                        }

                        if (sid == null)
                        {
                            model.pst_Trans.ITEMSL = 1;
                        }
                        else
                        {
                            model.pst_Trans.ITEMSL = sid + 1;
                        }

                        db.PST_TransDbSet.Add(model.pst_Trans);
                        db.SaveChanges();

                        model.pst_Trans.ITEMSL = 0;
                        model.pst_Trans.ITEMID = 0;
                        model.pst_Trans.QTY = 0;
                        model.pst_Trans.RATE = 0;
                        model.pst_Trans.AMOUNT = 0;

                        model.PST_Item.ITEMNM = "";
                        model.pst_Trans.POSNID = 0;
                        model.Empty = "";

                        TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                        //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                        TempData["transdate"] = model.pst_Trans.TRANSDT;
                        TempData["transYear"] = model.pst_Trans.TRANSYY;
                        TempData["data"] = model;
                        return RedirectToAction("EditOrder");
                    }

                    else
                    {
                        TempData["transno"] = model.pst_Trans.TRANSNO;
                        model.pst_Trans.ITEMSL = Convert.ToInt64(sid) + 1;

                        db.PST_TransDbSet.Add(model.pst_Trans);
                        db.SaveChanges();

                        //Discount rate, Vat rate, Service charge pass the value.
                        var transMst = (from m in db.PST_TransMstDbSet
                                        where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "SALE"
                                        select new { m.PST_TRANSMST_ID, m.TOTAMT, m.DISCOUNT, m.TOTNET, m.AMTCASH, m.AMTCREDIT, m.TOTREF, m.REMARKS }).ToList();

                        if (transMst.Count != 0)
                        {
                            foreach (var a in transMst)
                            {
                                TempData["HidendiscountRate"] = a.DISCOUNT;
                                //TempData["HidenVatRate"] = a.VATRT;
                                //TempData["HidenServiceCharge"] = a.OTCAMT;
                                model.Pst_Transmst.REMARKS = a.REMARKS;
                            }

                        }

                        model.pst_Trans.ITEMSL = 0;
                        model.pst_Trans.ITEMID = 0;
                        model.pst_Trans.QTY = 0;
                        model.pst_Trans.RATE = 0;
                        model.pst_Trans.AMOUNT = 0;

                        model.PST_Item.ITEMNM = "";
                        model.pst_Trans.POSNID = 0;
                        model.Empty = "";


                        string date = model.pst_Trans.TRANSDT.ToString();
                        DateTime MyDateTime = DateTime.Parse(date);
                        string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                        TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                        //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                        TempData["transdate"] = currentDate;
                        TempData["transYear"] = model.pst_Trans.TRANSYY;
                        TempData["data"] = model;
                        return RedirectToAction("EditOrder");
                    }

                }

                else
                {

                    if (model.pst_Trans.ITEMTP == "ACCESSORIES")
                    {
                        var result = (from n in db.PST_TransDbSet
                                      where n.TRANSNO == model.pst_Trans.TRANSNO &&
                                            n.COMPID == compid &&
                                            n.ITEMID == model.pst_Trans.ITEMID &&
                                            n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "SALE"
                                      select new
                                      {
                                          n.PST_TRANS_ID,
                                          n.ITEMSL,
                                          n.TRANSYY,
                                          n.TRANSTP,
                                          n.INSUSERID,
                                          n.INSTIME,
                                          n.INSIPNO,
                                      }
                            );

                        foreach (var item in result)
                        {
                            model.pst_Trans.PST_TRANS_ID = item.PST_TRANS_ID;
                            model.pst_Trans.ITEMSL = item.ITEMSL;

                            model.pst_Trans.TRANSYY = item.TRANSYY;
                            model.pst_Trans.TRANSTP = item.TRANSTP;

                            model.pst_Trans.USERPC = strHostName;
                            model.pst_Trans.INSUSERID = item.INSUSERID;
                            model.pst_Trans.INSTIME = item.INSTIME;
                            model.pst_Trans.INSIPNO = item.INSIPNO;
                            model.pst_Trans.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.pst_Trans.UPDLTUDE = model.pst_Trans.INSLTUDE;
                            model.pst_Trans.UPDIPNO = ipAddress.ToString();
                            model.pst_Trans.UPDTIME = td;
                        }
                    }
                    else
                    {
                        var result = (from n in db.PST_TransDbSet
                                      where n.TRANSNO == model.pst_Trans.TRANSNO &&
                                            n.COMPID == compid &&
                                            n.ITEMID == model.pst_Trans.ITEMID && n.POSNID == model.pst_Trans.POSNID &&
                                            n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "SALE"
                                      select new
                                      {
                                          n.PST_TRANS_ID,
                                          n.ITEMSL,
                                          n.TRANSYY,
                                          n.TRANSTP,
                                          n.INSUSERID,
                                          n.INSTIME,
                                          n.INSIPNO,
                                      }
                         );

                        foreach (var item in result)
                        {
                            model.pst_Trans.PST_TRANS_ID = item.PST_TRANS_ID;
                            model.pst_Trans.ITEMSL = item.ITEMSL;

                            model.pst_Trans.TRANSYY = item.TRANSYY;
                            model.pst_Trans.TRANSTP = item.TRANSTP;

                            model.pst_Trans.USERPC = strHostName;
                            model.pst_Trans.INSUSERID = item.INSUSERID;
                            model.pst_Trans.INSTIME = item.INSTIME;
                            model.pst_Trans.INSIPNO = item.INSIPNO;
                            model.pst_Trans.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.pst_Trans.UPDLTUDE = model.pst_Trans.INSLTUDE;
                            model.pst_Trans.UPDIPNO = ipAddress.ToString();
                            model.pst_Trans.UPDTIME = td;
                        }
                    }

                    model.pst_Trans.REFAMT = Convert.ToDecimal((model.pst_Trans.AMOUNT * model.pst_Trans.REFPCNT) / 100);
                    db.Entry(model.pst_Trans).State = EntityState.Modified;
                    db.SaveChanges();


                    //Discount rate, Vat rate, Service charge pass the value.
                    var transMst = (from m in db.PST_TransMstDbSet
                                    where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "SALE"
                                    select new { m.PST_TRANSMST_ID, m.TOTAMT, m.DISCOUNT, m.TOTNET, m.AMTCASH, m.AMTCREDIT, m.TOTREF, m.REMARKS }).ToList();

                    if (transMst.Count != 0)
                    {
                        foreach (var a in transMst)
                        {
                            TempData["HidendiscountRate"] = a.DISCOUNT;
                            //TempData["HidenVatRate"] = a.VATRT;
                            //TempData["HidenServiceCharge"] = a.OTCAMT;
                            model.Pst_Transmst.REMARKS = a.REMARKS;
                        }

                    }

                    model.pst_Trans.ITEMSL = 0;
                    model.pst_Trans.ITEMID = 0;
                    model.pst_Trans.QTY = 0;
                    model.pst_Trans.RATE = 0;
                    model.pst_Trans.AMOUNT = 0;

                    model.PST_Item.ITEMNM = "";
                    model.pst_Trans.POSNID = 0;
                    model.Empty = "";

                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                    TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                    TempData["transdate"] = currentDate;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    TempData["data"] = model;

                    return RedirectToAction("EditOrder");

                }
                //}

            }


            else if (command == "Save")
            {
                //Validation Check
                if (model.pst_Trans.TRANSDT == null && model.pst_Trans.TRANSNO == null && model.Pst_Transmst.TOTNET == 0)
                {
                    ViewBag.TransactionDate = "Transaction date required!";
                    return View("EditOrder");
                }
                else if (model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.addItemList = "Please Add item list!";
                    return View("EditOrder");
                }

                //update StkTransMaster table
                var get_TRANSDT_TRANSYY = from m in db.PST_TransDbSet
                                          where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "SALE"
                                          select new { m.TRANSDT, m.TRANSYY };
                foreach (var VARIABLE in get_TRANSDT_TRANSYY)
                {
                    model.Pst_Transmst.TRANSDT = VARIABLE.TRANSDT;
                    model.Pst_Transmst.TRANSYY = VARIABLE.TRANSYY;
                }

                var findTransNO = (from n in db.PST_TransMstDbSet
                                   where n.TRANSNO == model.pst_Trans.TRANSNO && n.COMPID == model.pst_Trans.COMPID &&
                          n.TRANSYY == model.Pst_Transmst.TRANSYY && n.TRANSTP == "SALE"
                                   select n).ToList();
                if (findTransNO.Count != 0)
                {
                    foreach (PST_TRANSMST pstTransmst in findTransNO)
                    {
                        pstTransmst.USERPC = strHostName;
                        pstTransmst.UPDLTUDE = pstTransmst.INSLTUDE;
                        pstTransmst.UPDIPNO = ipAddress.ToString();
                        pstTransmst.UPDTIME = td;
                        pstTransmst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        pstTransmst.TRANSDT = pstTransmst.TRANSDT;
                        pstTransmst.TRANSYY = pstTransmst.TRANSYY;

                        pstTransmst.COMPID = model.pst_Trans.COMPID;
                        pstTransmst.TRANSNO = Convert.ToInt64(model.pst_Trans.TRANSNO);
                        pstTransmst.TRANSTP = "SALE";

                        pstTransmst.PATIENTID = model.pst_Trans.PATIENTID;

                        pstTransmst.TOTAMT = model.Pst_Transmst.TOTAMT;
                        pstTransmst.DISCOUNT = model.Pst_Transmst.DISCOUNT;
                        pstTransmst.TOTNET = model.Pst_Transmst.TOTNET;
                        pstTransmst.AMTCASH = model.Pst_Transmst.AMTCASH;
                        pstTransmst.AMTCREDIT = model.Pst_Transmst.AMTCREDIT;
                        pstTransmst.TOTREF = model.Pst_Transmst.TOTREF;
                        pstTransmst.REMARKS = model.Pst_Transmst.REMARKS;


                        Update_PST_TRANSMST_LogData(pstTransmst);
                    }

                    db.SaveChanges();
                    return RedirectToAction("EditOrder");
                }


                model.Pst_Transmst.USERPC = strHostName;
                model.Pst_Transmst.INSIPNO = ipAddress.ToString();
                model.Pst_Transmst.INSTIME = td;
                model.Pst_Transmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                model.Pst_Transmst.COMPID = model.pst_Trans.COMPID;
                model.Pst_Transmst.TRANSNO = Convert.ToInt64(model.pst_Trans.TRANSNO);
                model.Pst_Transmst.TRANSTP = "SALE";
                model.Pst_Transmst.PATIENTID = model.pst_Trans.PATIENTID;

                Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                db.SaveChanges();
                return RedirectToAction("EditOrder");
            }

            else if (command == "search")
            {
                model.pst_Trans.ITEMSL = 0;
                model.pst_Trans.ITEMID = 0;
                model.pst_Trans.QTY = 0;
                model.pst_Trans.RATE = 0;
                model.pst_Trans.AMOUNT = 0;

                model.PST_Item.ITEMNM = "";

                var transMst = (from m in db.PST_TransMstDbSet
                                where m.TRANSDT == model.pst_Trans.TRANSDT && m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "SALE"
                                select new { m.PST_TRANSMST_ID, m.TOTAMT, m.DISCOUNT, m.TOTNET, m.AMTCASH, m.AMTCREDIT, m.TOTREF, m.REMARKS, m.PATIENTID }).ToList();

                if (transMst != null)
                {
                    foreach (var a in transMst)
                    {
                        TempData["totalAmount"] = a.TOTAMT;
                        TempData["discountRate"] = a.DISCOUNT;
                        TempData["NetAmount"] = a.TOTNET;
                        TempData["CashAmount"] = a.AMTCASH;
                        TempData["CreditAmount"] = a.AMTCREDIT;
                        TempData["Remarks"] = a.REMARKS;
                        model.Pst_Transmst.REMARKS = a.REMARKS;


                    }

                }

                var stk_Trans = (from m in db.PST_TransDbSet
                                 where m.TRANSDT == model.pst_Trans.TRANSDT && m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "SALE"
                                 select new { m.PATIENTID, m.RSID, m.TRANSYY }).ToList();
                foreach (var s in stk_Trans)
                {
                    model.pst_Trans.TRANSYY = s.TRANSYY;
                    model.pst_Trans.PATIENTID = s.PATIENTID;
                    break;
                }

                string date = model.pst_Trans.TRANSDT.ToString();
                DateTime MyDateTime = DateTime.Parse(date);
                string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
                TempData["transdate"] = currentDate;
                TempData["transYear"] = model.pst_Trans.TRANSYY;
                TempData["transno"] = model.pst_Trans.TRANSNO;
                TempData["data"] = model;
                return RedirectToAction("EditOrder");
            }

            else if (command == "A4")
            {
                //Validation Check
                if (model.pst_Trans.TRANSDT == null && model.pst_Trans.TRANSNO == null && model.Pst_Transmst.TOTNET == 0)
                {
                    ViewBag.TransactionDate = "Transaction date required!";
                    return View("EditOrder");
                }
                else if (model.pst_Trans.TRANSNO == null)
                {
                    ViewBag.addItemList = "Please Add item list!";
                    return View("EditOrder");
                }


                //update RmstransMaster table
                var get_TRANSDT_TRANSYY = from m in db.PST_TransDbSet
                                          where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "SALE"
                                          select new { m.TRANSDT, m.TRANSYY };
                foreach (var VARIABLE in get_TRANSDT_TRANSYY)
                {
                    model.Pst_Transmst.TRANSDT = VARIABLE.TRANSDT;
                    model.Pst_Transmst.TRANSYY = VARIABLE.TRANSYY;
                }

                var findTransNO = (from n in db.PST_TransMstDbSet
                                   where n.TRANSNO == model.pst_Trans.TRANSNO && n.COMPID == model.pst_Trans.COMPID &&
                          n.TRANSYY == model.Pst_Transmst.TRANSYY && n.TRANSTP == "SALE"
                                   select n).ToList();
                if (findTransNO.Count != 0)
                {
                    foreach (PST_TRANSMST pstTransmst in findTransNO)
                    {
                        pstTransmst.USERPC = strHostName;
                        pstTransmst.UPDLTUDE = pstTransmst.INSLTUDE;
                        pstTransmst.UPDIPNO = ipAddress.ToString();
                        pstTransmst.UPDTIME = td;
                        pstTransmst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        pstTransmst.TRANSDT = pstTransmst.TRANSDT;
                        pstTransmst.TRANSYY = pstTransmst.TRANSYY;

                        pstTransmst.COMPID = model.pst_Trans.COMPID;
                        pstTransmst.TRANSNO = Convert.ToInt64(model.pst_Trans.TRANSNO);
                        pstTransmst.TRANSTP = "SALE";

                        pstTransmst.PATIENTID = model.pst_Trans.PATIENTID;

                        pstTransmst.TOTAMT = model.Pst_Transmst.TOTAMT;
                        pstTransmst.DISCOUNT = model.Pst_Transmst.DISCOUNT;
                        pstTransmst.TOTNET = model.Pst_Transmst.TOTNET;
                        pstTransmst.AMTCASH = model.Pst_Transmst.AMTCASH;
                        pstTransmst.AMTCREDIT = model.Pst_Transmst.AMTCREDIT;
                        pstTransmst.TOTREF = model.Pst_Transmst.TOTREF;
                        pstTransmst.REMARKS = model.Pst_Transmst.REMARKS;
                        Update_PST_TRANSMST_LogData(pstTransmst);
                    }
                    db.SaveChanges();

                }
                else
                {
                    model.Pst_Transmst.USERPC = strHostName;
                    model.Pst_Transmst.INSIPNO = ipAddress.ToString();
                    model.Pst_Transmst.INSTIME = td;
                    model.Pst_Transmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                    model.Pst_Transmst.COMPID = model.pst_Trans.COMPID;
                    model.Pst_Transmst.TRANSNO = Convert.ToInt64(model.pst_Trans.TRANSNO);
                    model.Pst_Transmst.TRANSTP = "SALE";

                    model.Pst_Transmst.PATIENTID = model.pst_Trans.PATIENTID;

                    Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                    db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                    db.SaveChanges();
                }

                PageModel aPageModel = new PageModel();
                aPageModel.pst_Trans.TRANSNO = model.pst_Trans.TRANSNO;
                aPageModel.pst_Trans.TRANSDT = model.pst_Trans.TRANSDT;
                aPageModel.pst_Trans.COMPID = model.pst_Trans.COMPID;
                aPageModel.Pst_Transmst.TRANSTP = "SALE";
                aPageModel.Pst_Transmst.TRANSYY = model.pst_Trans.TRANSYY;
                aPageModel.Pst_Patient.PATIENTIDM = model.Pst_Patient.PATIENTIDM;
                TempData["Sale_Command"] = command;
                TempData["pageModel"] = aPageModel;
                return RedirectToAction("SaleMemo", "BillingReport");
            }


            else if (command == "New")
            {
                return RedirectToAction("Index");
            }

            else
            {
                return RedirectToAction("EditOrder");
            }

        }






        public ActionResult EditOrderDelete(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, Int64 patientid, PageModel model)
        {
            //Permission Check
            Int64 loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            var checkPermission = from role in db.AslRoleDbSet
                                  where role.USERID == loggedUserID && role.CONTROLLERNAME == "Sale" && role.ACTIONNAME == "Index"
                                  select new { role.DELETER };
            string Delete = "";
            foreach (var VARIABLE in checkPermission)
            {
                Delete = VARIABLE.DELETER;
            }
            if (Delete == "I")
            {
                TempData["DeletePermission"] = "Delete Permission Denied !";
                return RedirectToAction("EditOrder");
            }

            PST_TRANS pstTrans = db.PST_TransDbSet.Find(tid);
            db.PST_TransDbSet.Remove(pstTrans);
            db.SaveChanges();


            var result = (from t in db.PST_TransDbSet
                          where t.COMPID == compid && t.TRANSNO == orderNo && t.TRANSDT == Date && t.TRANSYY == Year && t.TRANSTP == "SALE"
                          select t).ToList();

            foreach (var n in result)
            {
                model.pst_Trans.TRANSYY = Convert.ToInt64(n.TRANSYY);
                model.AGL_acchart.ACCOUNTCD = Convert.ToInt64(n.RSID);
                TempData["PATIENTID"] = n.PATIENTID;
                //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                TempData["transdate"] = n.TRANSDT;
                TempData["transYear"] = n.TRANSYY;
            }
            //Minimum RmsTrans Table MemoNO delete then RmsTransMst tabel data delete(key-> memoNO, compid)
            if (result.Count == 0)
            {
                var searchRmsTransMst = (from n in db.PST_TransMstDbSet where n.COMPID == compid && n.TRANSNO == orderNo && n.TRANSDT == Date && n.TRANSYY == Year && n.TRANSTP == "SALE" select n).ToList();
                PST_TRANSMST aPstTransmst = new PST_TRANSMST();
                foreach (var m in searchRmsTransMst)
                {
                    aPstTransmst = m;

                }
                if (aPstTransmst != null && searchRmsTransMst.Count != 0)
                {
                    Delete_PST_TRANSMST_LogData(aPstTransmst);
                    Delete_PST_TRANSMST_DELETE(aPstTransmst);
                    db.PST_TransMstDbSet.Remove(aPstTransmst);
                    db.SaveChanges();
                }

            }


            if (result.Count == 0)
            {
                TempData["transdate"] = null;
                TempData["transYear"] = null;
                model.pst_Trans.TRANSYY = null;
                model.pst_Trans.TRANSDT = null;
                orderNo = 0;
            }
            else
            {
                var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == orderNo && x.TRANSDT == Date && x.TRANSYY == Year && x.TRANSTP == "SALE" && x.COMPID == compid)
                            .Max(o => o.ITEMSL);
                model.pst_Trans.TRANSNO = orderNo;
                model.pst_Trans.ITEMSL = sid;
                model.pst_Trans.TRANSDT = Date;
            }

            if (model.pst_Trans.TRANSDT != null)
            {
                string date = model.pst_Trans.TRANSDT.ToString();
                DateTime MyDateTime = DateTime.Parse(date);
                string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                TempData["transdate"] = currentDate;
                TempData["transYear"] = model.pst_Trans.TRANSYY;
            }


            var patient_Info = from m in db.PST_PatientDbSet
                               from n in db.PST_ReferDbSet
                               where m.COMPID == compid && m.COMPID == n.COMPID
                                     && m.PATIENTID == patientid && m.REFERID == n.REFERID
                               select new { m.PATIENTNM, n.REFERNM, n.REFPCNT };
            foreach (var x in patient_Info)
            {
                model.Pst_Patient.PATIENTNM = x.PATIENTNM;
                model.Pst_Refer.REFERNM = x.REFERNM;
                model.pst_Trans.REFPCNT = x.REFPCNT;
            }

            //Discount rate, Vat rate, Service charge pass the value.
            var transMst = (from m in db.PST_TransMstDbSet
                            where m.COMPID == compid && m.TRANSNO == orderNo && m.TRANSDT == Date && m.TRANSYY == Year && m.TRANSTP == "SALE"
                            select new { m.PST_TRANSMST_ID, m.TOTAMT, m.DISCOUNT, m.TOTNET, m.AMTCASH, m.AMTCREDIT, m.TOTREF, m.REMARKS }).ToList();
            if (transMst.Count != 0)
            {
                foreach (var a in transMst)
                {
                    TempData["HidendiscountRate"] = a.DISCOUNT;
                    //TempData["HidenVatRate"] = a.VATRT;
                    //TempData["HidenServiceCharge"] = a.OTCAMT;
                    TempData["Remarks"] = a.REMARKS;
                }
            }


            TempData["data"] = model;
            TempData["transno"] = orderNo;
            return RedirectToAction("EditOrder");

        }






        public ActionResult EditOrderUpdate(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, Int64 itemId, PageModel model)
        {
            model.pst_Trans = db.PST_TransDbSet.Find(tid);

            var item = from r in db.PST_ItemDbSet where r.ITEMID == itemId select r.ITEMNM;
            foreach (var it in item)
            {
                model.PST_Item.ITEMNM = it.ToString();
            }

            model.pst_Trans.TRANSDT = Date;
            model.pst_Trans.TRANSYY = Year;


            var patient_Info = from m in db.PST_PatientDbSet
                               from n in db.PST_ReferDbSet
                               where m.COMPID == compid && m.COMPID == n.COMPID
                                     && m.PATIENTID == model.pst_Trans.PATIENTID && m.REFERID == n.REFERID
                               select new { m.PATIENTNM, n.REFERNM, n.REFPCNT };
            foreach (var x in patient_Info)
            {
                model.Pst_Patient.PATIENTNM = x.PATIENTNM;
                model.Pst_Refer.REFERNM = x.REFERNM;
                model.pst_Trans.REFPCNT = x.REFPCNT;
            }


            //Discount rate, Vat rate, Service charge pass the value.
            var transMst = (from m in db.PST_TransMstDbSet
                            where m.COMPID == compid && m.TRANSNO == orderNo && m.TRANSDT == Date && m.TRANSYY == Year && m.TRANSTP == "SALE"
                            select new { m.PST_TRANSMST_ID, m.TOTAMT, m.DISCOUNT, m.TOTNET, m.AMTCASH, m.AMTCREDIT, m.TOTREF, m.REMARKS }).ToList();

            if (transMst.Count != 0)
            {
                foreach (var a in transMst)
                {
                    TempData["HidendiscountRate"] = a.DISCOUNT;
                    //TempData["HidenVatRate"] = a.VATRT;
                    //TempData["HidenServiceCharge"] = a.OTCAMT;
                    model.Pst_Transmst.REMARKS = a.REMARKS;
                }

            }


            var pos_item = from r in db.PST_ItemDbSet where r.ITEMID == model.pst_Trans.POSNID && r.COMPID == model.pst_Trans.COMPID select r.ITEMNM;
            foreach (var it in pos_item)
            {
                TempData["positionName"] = it.ToString();
            }

            if (model.pst_Trans.ITEMTP == "ACCESSORIES")
            {
                model.pst_Trans.POSNID = null;
                TempData["Hide_positionName"] = "Hide Position Name";
            }

            TempData["PATIENTID"] = model.pst_Trans.PATIENTID;
            //TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
            TempData["transdate"] = model.pst_Trans.TRANSDT;
            TempData["transYear"] = model.pst_Trans.TRANSYY;
            TempData["data"] = model;
            TempData["transno"] = model.pst_Trans.TRANSNO;
            return RedirectToAction("EditOrder");
        }














        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult DateChanged_getYear(DateTime changedtxt)
        //{
        //    string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));
        //    string getYear = converttoString.Substring(7, 4);
        //    Int64 year = Convert.ToInt64(getYear);
        //    var result = new { YEAR = year };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}





        public JsonResult DateChanged(string theDate)
        {

            DateTime dt = Convert.ToDateTime(theDate);
            DateTime dd = DateTime.Parse(theDate, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime datetm = Convert.ToDateTime(dd);

            List<SelectListItem> trans = new List<SelectListItem>();
            var transres = (from n in db.PST_TransDbSet
                            where n.TRANSDT == dd && n.COMPID == compid && n.TRANSTP == "SALE"
                            select new
                            {
                                n.TRANSNO
                            }
                            )
                            .Distinct()
                            .ToList();
            string transNO = null;
            foreach (var f in transres)
            {
                if (transNO == null)
                {
                    transNO = Convert.ToString(f.TRANSNO);
                }
                trans.Add(new SelectListItem { Text = f.TRANSNO.ToString(), Value = f.TRANSNO.ToString() });
            }

            var FirsttransNO = new { TransNO = transNO };
            return Json(new SelectList(trans, "Value", "Text"));
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult TransNoChanged(Int64 changedDropDown)
        {
            Int64 transno = Convert.ToInt64(changedDropDown);

            TempData["transno"] = transno;

            var rt = (from trans in db.PST_TransDbSet
                      where trans.COMPID == compid
                      join patient in db.PST_PatientDbSet on compid equals patient.COMPID
                      join refer in db.PST_ReferDbSet on compid equals refer.COMPID
                      where
                          trans.COMPID == refer.COMPID && refer.COMPID == patient.COMPID && trans.TRANSNO == changedDropDown &&
                          patient.PATIENTID == trans.PATIENTID && refer.REFERID == patient.REFERID
                      select new { patient.PATIENTIDM, patient.PATIENTNM, refer.REFERNM, refer.REFPCNT }).Distinct().ToList();

            string patientIDM = "", patientname = "", referName = "", referPercent = "";
            foreach (var n in rt)
            {
                patientIDM = n.PATIENTIDM.ToString();
                patientname = n.PATIENTNM;
                referName = n.REFERNM;
                referPercent = n.REFPCNT.ToString();
            }

            var result = new { TRANSNO = transno, PATIENTIDM = patientIDM, PATIENTNM = patientname, REFERNM = referName, REFPCNT = referPercent };
            return Json(result, JsonRequestBehavior.AllowGet);

        }







        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText, string changeditemType)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);
            string itemId = "", posNid = "";
            string rate = "";
            decimal qty = 0;
            var rt = db.PST_ItemDbSet.Where(n => n.ITEMNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             itemid = n.ITEMID,
                                                             rate = n.RATE,
                                                         });
            foreach (var n in rt)
            {
                itemId = Convert.ToString(n.itemid);
                rate = Convert.ToString(n.rate);
            }

            var result = new { itemid = itemId, Rate = rate, qty = 1 };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        //AutoComplete
        public JsonResult TagSearch(string term, string changedDropDown)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);
            Int64 catId_1 = Convert.ToInt64(compid + "01");
            Int64 catId_2 = Convert.ToInt64(compid + "02");

            if (changedDropDown == "ACCESSORIES")
            {
                var tags = from p in db.PST_ItemDbSet
                           where p.COMPID == compid && p.CATID != catId_1 && p.CATID != catId_2
                           select p.ITEMNM;
                return this.Json(tags.Where(t => t.StartsWith(term)),
                    JsonRequestBehavior.AllowGet);
            }
            else //if (changedDropDown == "THERAPY")
            {
                var tags = from p in db.PST_ItemDbSet
                           where p.COMPID == compid && p.CATID == catId_2
                           select p.ITEMNM;
                return this.Json(tags.Where(t => t.StartsWith(term)),
                     JsonRequestBehavior.AllowGet);

            }
        }



        //AutoComplete
        public JsonResult TagSearch_PositionName(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);
            Int64 catId = Convert.ToInt64(compid + "01");

            var tags = from p in db.PST_ItemDbSet
                       where p.COMPID == compid && p.CATID == catId
                       select p.ITEMNM;
            return this.Json(tags.Where(t => t.StartsWith(term)),
                JsonRequestBehavior.AllowGet);

        }






        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult PositionNameChanged(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            string Position_Id = "";
            var rt = db.PST_ItemDbSet.Where(n => n.ITEMNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
            {
                n.ITEMID
            });

            foreach (var n in rt)
            {
                Position_Id = Convert.ToString(n.ITEMID);
            }

            return Json(Position_Id, JsonRequestBehavior.AllowGet);

        }









        //AutoComplete
        public JsonResult TagSearch_PST_Patient(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.PST_PatientDbSet
                       where p.COMPID == compid
                       select new { p.PATIENTIDM };
            foreach (var tag in tags)
            {
                result.Add(tag.PATIENTIDM.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }




        //AutoComplete 
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> patientIDM_List = new List<string>();
            var tags = from p in db.PST_PatientDbSet
                       where p.COMPID == compid
                       select new { p.PATIENTIDM, p.COMPID };
            foreach (var tag in tags)
            {
                patientIDM_List.Add(tag.PATIENTIDM.ToString());
            }

            var rt = patientIDM_List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }

                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }

                    }

                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }

                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            var result = new { PATIENTIDM = itemId };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        //AutoComplete 
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPatientInformation(Int64 changedtxt)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String patientNm = "", referName = "";
            Int64 ReferID = 0;
            Decimal Refpcnt = 0;

            var rt = db.PST_PatientDbSet.Where(n => n.PATIENTIDM == changedtxt &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             n.pst_Patient_Id,
                                                             n.PATIENTNM,
                                                             n.REFERID,
                                                         });
            foreach (var n in rt)
            {
                patientNm = n.PATIENTNM;
                ReferID = Convert.ToInt64(n.REFERID);
            }


            var find_Refername = db.PST_ReferDbSet.Where(n => n.REFERID == ReferID &&
                                                                n.COMPID == compid).Select(n => new { n.REFERNM, n.REFPCNT });
            foreach (var m in find_Refername)
            {
                Refpcnt = Convert.ToDecimal(m.REFPCNT);
                referName = m.REFERNM;
            }

            var result = new
            {
                PATIENTNM = patientNm,
                REFERNM = referName,
                REFPCNT = Refpcnt,
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
