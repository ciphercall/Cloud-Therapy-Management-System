using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;
using iTextSharp.text.pdf;

namespace AS_Therapy_GL.Controllers
{
    public class BuyController : AppController
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

        public BuyController()
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

            string PartyName = "";
            var findPartyID = (from n in db.GlAcchartDbSet where n.COMPID == compid && n.ACCOUNTCD == model.RSID select n).ToList();
            foreach (var y in findPartyID)
            {
                PartyName = y.ACCOUNTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Transaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nSupplier:" + PartyName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nRemarks: " + model.REMARKS + ".");
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

            string PartyName = "";
            var findPartyID = (from n in db.GlAcchartDbSet where n.COMPID == compid && n.ACCOUNTCD == model.RSID select n).ToList();
            foreach (var y in findPartyID)
            {
                PartyName = y.ACCOUNTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }
            aslLog.LOGDATA = Convert.ToString("Transaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nSupplier:" + PartyName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nRemarks: " + model.REMARKS + ".");
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

            string PartyName = "";
            var findPartyID = (from n in db.GlAcchartDbSet where n.COMPID == compid && n.ACCOUNTCD == model.RSID select n).ToList();
            foreach (var y in findPartyID)
            {
                PartyName = y.ACCOUNTNM.ToString();
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }
            aslLog.LOGDATA = Convert.ToString("Delete also item list data! " + "\nTransaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nSupplier:" + PartyName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nRemarks: " + model.REMARKS + ".");
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

            string PartyName = "";
            var findPartyID = (from n in db.GlAcchartDbSet where n.COMPID == compid && n.ACCOUNTCD == model.RSID select n).ToList();
            foreach (var y in findPartyID)
            {
                PartyName = y.ACCOUNTNM.ToString();
            }
            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }
            AslDelete.DELDATA = Convert.ToString("Delete also item list data! " + "\nTransaction Date:" + transDate + ",\nYear:" + model.TRANSYY + ",\nTransaction type:" + model.TRANSTP + ",\nMemo NO:" + model.TRANSNO + ",\nSupplier:" + PartyName + ",\nTotal Amount:" + model.TOTAMT + ",\nDiscount Amount: " + model.DISCOUNT + ",\nTotal Net: " + model.TOTNET + ",\nCash Amount: " + model.AMTCASH + ",\nCredit Amount: " + model.AMTCREDIT + ",\nRemarks: " + model.REMARKS + ".");
            AslDelete.USERPC = strHostName;
            db.AslDeleteDbSet.Add(AslDelete);
        }









        // GET: /Transaction/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            var dt = (PageModel)TempData["data"];
            return View(dt);
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
                                      where role.USERID == loggedUserID && role.CONTROLLERNAME == "BUY" && role.ACTIONNAME == "Index"
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



                if (model.AGL_acchart.ACCOUNTCD == null)
                {
                    ViewBag.errorParty = "Please select a party name!";
                    return View("Index");
                }


                //Validation Check
                if ((model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0) && model.pst_Trans.QTY == null)
                {
                    //ViewBag.TableNO = "Table name required!";
                    ViewBag.errorItemid = "Please select a valid item name!";
                    ViewBag.errorQty = "Please select a valid quantity !";
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["data"] = model;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.ITEMID == null || model.pst_Trans.ITEMID == 0)
                {
                    ViewBag.errorItemid = "Please select a valid item name!";
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["data"] = model;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }
                else if (model.pst_Trans.QTY == null)
                {
                    ViewBag.errorQty = "Please select a valid quantity !";
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                    TempData["transdate"] = model.pst_Trans.TRANSDT;
                    TempData["data"] = model;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    return View("Index");
                }

                model.pst_Trans.USERPC = strHostName;
                model.pst_Trans.INSIPNO = ipAddress.ToString();
                model.pst_Trans.INSTIME = td;
                model.pst_Trans.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.pst_Trans.TRANSTP = "BUY";
                model.pst_Trans.RSID = model.AGL_acchart.ACCOUNTCD;


                var res = db.PST_TransDbSet.Where(a => a.TRANSNO == model.pst_Trans.TRANSNO && a.TRANSTP == "BUY" && a.TRANSYY == model.pst_Trans.TRANSYY && a.COMPID == compid).Count(a => a.ITEMID == model.pst_Trans.ITEMID) == 0;

                if (res == true)
                {
                    var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == model.pst_Trans.TRANSNO && x.TRANSTP == "BUY" && x.TRANSYY == model.pst_Trans.TRANSYY && x.COMPID == compid)
                                  .Max(o => o.ITEMSL);
                    var transno_Max = db.PST_TransDbSet.Where(x => x.TRANSYY == model.pst_Trans.TRANSYY && x.TRANSTP == "BUY" && x.COMPID == compid)
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
                        model.pst_Trans.REFPCNT = 0;
                        model.pst_Trans.REFAMT = 0;

                        model.PST_Item.ITEMNM = "";
                        TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                        model.pst_Trans.REFPCNT = 0;
                        model.pst_Trans.REFAMT = 0;

                        model.PST_Item.ITEMNM = "";
                        TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                        TempData["transdate"] = model.pst_Trans.TRANSDT;
                        TempData["transYear"] = model.pst_Trans.TRANSYY;
                        TempData["data"] = model;
                        return RedirectToAction("Index");
                    }

                }

                else
                {
                    var result = (from n in db.PST_TransDbSet
                                  where n.TRANSNO == model.pst_Trans.TRANSNO &&
                                        n.COMPID == compid &&
                                        n.ITEMID == model.pst_Trans.ITEMID &&
                                        n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "BUY"
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

                    db.Entry(model.pst_Trans).State = EntityState.Modified;
                    db.SaveChanges();

                    model.pst_Trans.ITEMSL = 0;
                    model.pst_Trans.ITEMID = 0;
                    model.pst_Trans.QTY = 0;
                    model.pst_Trans.RATE = 0;
                    model.pst_Trans.AMOUNT = 0;
                    model.pst_Trans.REFPCNT = 0;
                    model.pst_Trans.REFAMT = 0;

                    model.PST_Item.ITEMNM = "";
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                model.Pst_Transmst.TRANSTP = "BUY";
                model.Pst_Transmst.RSID = model.AGL_acchart.ACCOUNTCD;

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
                          n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "BUY"
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
                    model.Pst_Transmst.TRANSTP = "BUY";
                    model.Pst_Transmst.RSID = model.AGL_acchart.ACCOUNTCD;

                    Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                    db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                    db.SaveChanges();
                }


                PageModel aPageModel = new PageModel();
                aPageModel.pst_Trans.TRANSNO = model.pst_Trans.TRANSNO;
                aPageModel.pst_Trans.TRANSDT = model.pst_Trans.TRANSDT;
                aPageModel.pst_Trans.COMPID = model.pst_Trans.COMPID;
                aPageModel.Pst_Transmst.TRANSTP = "BUY";
                aPageModel.Pst_Transmst.TRANSYY = model.pst_Trans.TRANSYY;
                TempData["Sale_Command"] = command;
                TempData["pageModel"] = aPageModel;
                return RedirectToAction("BuyMemo", "BillingReport");
            }

            else if (command == "POS")
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
                          n.TRANSYY == model.pst_Trans.TRANSYY && n.TRANSTP == "BUY"
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
                    model.Pst_Transmst.TRANSTP = "BUY";
                    model.Pst_Transmst.RSID = model.AGL_acchart.ACCOUNTCD;

                    Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                    db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                    db.SaveChanges();
                }


                PageModel aPageModel = new PageModel();
                aPageModel.pst_Trans.TRANSNO = model.pst_Trans.TRANSNO;
                aPageModel.pst_Trans.TRANSDT = model.pst_Trans.TRANSDT;
                aPageModel.pst_Trans.COMPID = model.pst_Trans.COMPID;
                aPageModel.Pst_Transmst.TRANSTP = "BUY";
                aPageModel.Pst_Transmst.TRANSYY = model.pst_Trans.TRANSYY;
                TempData["Sale_Command"] = command;
                TempData["pageModel"] = aPageModel;
                return RedirectToAction("BuyMemo", "BillingReport");
            }

            else
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult OrderDelete(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, PageModel model)
        {
            PST_TRANS pstTrans = db.PST_TransDbSet.Find(tid);

            db.PST_TransDbSet.Remove(pstTrans);
            db.SaveChanges();

            var result = (from t in db.PST_TransDbSet
                          where t.COMPID == compid && t.TRANSNO == orderNo && t.TRANSYY == Year && t.TRANSTP == "BUY"
                          select new { t.TRANSYY, t.PATIENTID, t.RSID, t.TRANSDT }
             ).Distinct().ToList();


            foreach (var n in result)
            {
                model.pst_Trans.TRANSYY = Convert.ToInt64(n.TRANSYY);
                model.AGL_acchart.ACCOUNTCD = Convert.ToInt64(n.RSID);
                TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == orderNo && x.TRANSYY == Year && x.TRANSTP == "BUY" && x.COMPID == compid)
                            .Max(o => o.ITEMSL);
                model.pst_Trans.TRANSNO = orderNo;
                model.pst_Trans.ITEMSL = sid;
                model.pst_Trans.TRANSDT = Date;
            }



            TempData["data"] = model;
            TempData["transno"] = orderNo;
            return RedirectToAction("Index");
        }




        public ActionResult OrderUpdate(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, Int64 itemId, PageModel model)
        {
            model.pst_Trans = db.PST_TransDbSet.Find(tid);
            model.AGL_acchart.ACCOUNTCD = model.pst_Trans.RSID;

            var item = from r in db.PST_ItemDbSet where r.ITEMID == itemId select r.ITEMNM;
            foreach (var it in item)
            {
                model.PST_Item.ITEMNM = it.ToString();
            }

            model.pst_Trans.TRANSDT = Date;
            model.pst_Trans.TRANSYY = Year;
            TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                                  where role.USERID == loggedUserID && role.CONTROLLERNAME == "Buy" && role.ACTIONNAME == "Index"
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
                var FIND_TRANSDT_TRANSYY = (from m in db.PST_TransDbSet
                                           where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "BUY"
                                           select new { m.TRANSDT, m.TRANSYY }).Distinct().ToList();
                foreach (var VARIABLE in FIND_TRANSDT_TRANSYY)
                {
                    model.pst_Trans.TRANSDT = VARIABLE.TRANSDT;
                    model.pst_Trans.TRANSYY = VARIABLE.TRANSYY;
                }

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
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                    return View("EditOrder");
                }
                else if (model.pst_Trans.QTY == null)
                {
                    ViewBag.errorQty = "Please select a valid quantity !";

                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    TempData["transdate"] = currentDate;
                    TempData["transYear"] = model.pst_Trans.TRANSYY;
                    TempData["data"] = model;
                    TempData["transno"] = model.pst_Trans.TRANSNO;
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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


                var res = db.PST_TransDbSet.Where(a => a.TRANSDT == model.pst_Trans.TRANSDT && a.TRANSNO == model.pst_Trans.TRANSNO && a.TRANSTP == "BUY" && a.COMPID == compid).Count(a => a.ITEMID == model.pst_Trans.ITEMID) == 0;
                if (res == true)
                {

                    //Permission Check
                    Int64 loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    var checkPermission = from role in db.AslRoleDbSet
                                          where role.USERID == loggedUserID && role.CONTROLLERNAME == "BUY" && role.ACTIONNAME == "Index"
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
                                               where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "BUY"
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
                    model.pst_Trans.TRANSTP = "BUY";
                    model.pst_Trans.RSID = model.AGL_acchart.ACCOUNTCD;

                    var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == model.pst_Trans.TRANSNO && x.TRANSTP == "BUY" && x.COMPID == compid)
                               .Max(o => o.ITEMSL);
                    var transno_Max = db.PST_TransDbSet.Where(x => x.TRANSYY == model.pst_Trans.TRANSYY && x.TRANSTP == "BUY" && x.COMPID == compid)
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
                        model.pst_Trans.REFPCNT = 0;
                        model.pst_Trans.REFAMT = 0;

                        model.PST_Item.ITEMNM = "";
                        TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                                        where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "BUY"
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
                        model.pst_Trans.REFPCNT = 0;
                        model.pst_Trans.REFAMT = 0;

                        model.PST_Item.ITEMNM = "";

                        string date = model.pst_Trans.TRANSDT.ToString();
                        DateTime MyDateTime = DateTime.Parse(date);
                        string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                        TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                        TempData["transdate"] = currentDate;
                        TempData["transYear"] = model.pst_Trans.TRANSYY;
                        TempData["data"] = model;
                        return RedirectToAction("EditOrder");
                    }

                }

                else
                {
                    var result = (from n in db.PST_TransDbSet
                                  where n.TRANSNO == model.pst_Trans.TRANSNO &&
                                        n.COMPID == compid &&
                                        n.ITEMID == model.pst_Trans.ITEMID &&
                                        n.TRANSDT == model.pst_Trans.TRANSDT && n.TRANSTP == "BUY"
                                  select new
                                  {
                                      transPID = n.PST_TRANS_ID,
                                      sl = n.ITEMSL,
                                      year = n.TRANSYY,
                                      type = n.TRANSTP,
                                      InsertUserId = n.INSUSERID,
                                      InsertTime = n.INSTIME,
                                      InsertIpNo = n.INSIPNO,
                                  }
                           );

                    foreach (var item in result)
                    {
                        model.pst_Trans.PST_TRANS_ID = item.transPID;
                        model.pst_Trans.ITEMSL = item.sl;
                        model.pst_Trans.TRANSYY = item.year;
                        model.pst_Trans.TRANSTP = item.type;

                        model.pst_Trans.USERPC = strHostName;
                        model.pst_Trans.INSUSERID = item.InsertUserId;
                        model.pst_Trans.INSTIME = item.InsertTime;
                        model.pst_Trans.INSIPNO = item.InsertIpNo;
                        model.pst_Trans.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        model.pst_Trans.UPDLTUDE = model.pst_Trans.INSLTUDE;
                        model.pst_Trans.UPDIPNO = ipAddress.ToString();
                        model.pst_Trans.UPDTIME = td;

                        model.pst_Trans.RSID = model.AGL_acchart.ACCOUNTCD;
                    }

                    db.Entry(model.pst_Trans).State = EntityState.Modified;
                    db.SaveChanges();


                    //Discount rate, Vat rate, Service charge pass the value.
                    var transMst = (from m in db.PST_TransMstDbSet
                                    where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "BUY"
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
                    model.pst_Trans.REFPCNT = 0;
                    model.pst_Trans.REFAMT = 0;

                    model.PST_Item.ITEMNM = "";

                    string date = model.pst_Trans.TRANSDT.ToString();
                    DateTime MyDateTime = DateTime.Parse(date);
                    string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                    TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                                          where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "BUY"
                                          select new { m.TRANSDT, m.TRANSYY };
                foreach (var VARIABLE in get_TRANSDT_TRANSYY)
                {
                    model.Pst_Transmst.TRANSDT = VARIABLE.TRANSDT;
                    model.Pst_Transmst.TRANSYY = VARIABLE.TRANSYY;
                }

                var findTransNO = (from n in db.PST_TransMstDbSet
                                   where n.TRANSNO == model.pst_Trans.TRANSNO && n.COMPID == model.pst_Trans.COMPID &&
                          n.TRANSYY == model.Pst_Transmst.TRANSYY && n.TRANSTP == "BUY"
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
                        pstTransmst.TRANSTP = "BUY";

                        pstTransmst.RSID = model.AGL_acchart.ACCOUNTCD;

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
                model.Pst_Transmst.TRANSTP = "BUY";
                model.Pst_Transmst.RSID = model.AGL_acchart.ACCOUNTCD;

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
                model.pst_Trans.REFPCNT = 0;
                model.pst_Trans.REFAMT = 0;

                model.PST_Item.ITEMNM = "";

                var transMst = (from m in db.PST_TransMstDbSet
                                where m.TRANSDT == model.pst_Trans.TRANSDT && m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "BUY"
                                select new { m.PST_TRANSMST_ID, m.TOTAMT, m.DISCOUNT, m.TOTNET, m.AMTCASH, m.AMTCREDIT, m.TOTREF, m.REMARKS, m.PATIENTID, m.RSID }).ToList();

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

                        model.AGL_acchart.ACCOUNTCD = a.RSID;

                    }

                }

                var stk_Trans = (from m in db.PST_TransDbSet
                                 where m.TRANSDT == model.pst_Trans.TRANSDT && m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSTP == "BUY"
                                 select new { m.PATIENTID, m.RSID, m.TRANSYY }).ToList();
                foreach (var s in stk_Trans)
                {
                    model.AGL_acchart.ACCOUNTCD = s.RSID;
                    model.pst_Trans.TRANSYY = s.TRANSYY;
                    break;
                }

                string date = model.pst_Trans.TRANSDT.ToString();
                DateTime MyDateTime = DateTime.Parse(date);
                string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
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
                                          where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "BUY"
                                          select new { m.TRANSDT, m.TRANSYY };
                foreach (var VARIABLE in get_TRANSDT_TRANSYY)
                {
                    model.Pst_Transmst.TRANSDT = VARIABLE.TRANSDT;
                    model.Pst_Transmst.TRANSYY = VARIABLE.TRANSYY;
                }

                var findTransNO = (from n in db.PST_TransMstDbSet
                                   where n.TRANSNO == model.pst_Trans.TRANSNO && n.COMPID == model.pst_Trans.COMPID &&
                          n.TRANSYY == model.Pst_Transmst.TRANSYY && n.TRANSTP == "BUY"
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
                        pstTransmst.TRANSTP = "BUY";

                        pstTransmst.RSID = model.AGL_acchart.ACCOUNTCD;

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
                    model.Pst_Transmst.TRANSTP = "BUY";

                    model.Pst_Transmst.RSID = model.AGL_acchart.ACCOUNTCD;

                    Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                    db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                    db.SaveChanges();
                }

                PageModel aPageModel = new PageModel();
                aPageModel.pst_Trans.TRANSNO = model.pst_Trans.TRANSNO;
                aPageModel.pst_Trans.TRANSDT = model.Pst_Transmst.TRANSDT;
                aPageModel.pst_Trans.COMPID = model.pst_Trans.COMPID;
                aPageModel.Pst_Transmst.TRANSTP = "BUY";
                aPageModel.Pst_Transmst.TRANSYY = model.Pst_Transmst.TRANSYY;
                TempData["Sale_Command"] = command;
                TempData["pageModel"] = aPageModel;
                return RedirectToAction("BuyMemo", "BillingReport");
            }

            else if (command == "POS")
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
                                          where m.COMPID == compid && m.TRANSNO == model.pst_Trans.TRANSNO && m.TRANSDT == model.pst_Trans.TRANSDT && m.TRANSTP == "BUY"
                                          select new { m.TRANSDT, m.TRANSYY };
                foreach (var VARIABLE in get_TRANSDT_TRANSYY)
                {
                    model.Pst_Transmst.TRANSDT = VARIABLE.TRANSDT;
                    model.Pst_Transmst.TRANSYY = VARIABLE.TRANSYY;
                }

                var findTransNO = (from n in db.PST_TransMstDbSet
                                   where n.TRANSNO == model.pst_Trans.TRANSNO && n.COMPID == model.pst_Trans.COMPID &&
                          n.TRANSYY == model.Pst_Transmst.TRANSYY && n.TRANSTP == "BUY"
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
                        pstTransmst.TRANSTP = "BUY";

                        pstTransmst.RSID = model.AGL_acchart.ACCOUNTCD;

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
                    model.Pst_Transmst.TRANSTP = "BUY";

                    model.Pst_Transmst.RSID = model.AGL_acchart.ACCOUNTCD;

                    Insert_PST_TRANSMST_LogData(model.Pst_Transmst);
                    db.PST_TransMstDbSet.Add(model.Pst_Transmst);
                    db.SaveChanges();
                }

                PageModel aPageModel = new PageModel();
                aPageModel.pst_Trans.TRANSNO = model.pst_Trans.TRANSNO;
                aPageModel.pst_Trans.TRANSDT = model.Pst_Transmst.TRANSDT;
                aPageModel.pst_Trans.COMPID = model.pst_Trans.COMPID;
                aPageModel.Pst_Transmst.TRANSTP = "BUY";
                aPageModel.Pst_Transmst.TRANSYY = model.Pst_Transmst.TRANSYY;
                TempData["Sale_Command"] = command;
                TempData["pageModel"] = aPageModel;
                return RedirectToAction("BuyMemo", "BillingReport");
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






        public ActionResult EditOrderDelete(Int64 tid, Int64 orderNo, DateTime Date, Int64 Year, Int64 itemsl, PageModel model)
        {
            //Permission Check
            Int64 loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            var checkPermission = from role in db.AslRoleDbSet
                                  where role.USERID == loggedUserID && role.CONTROLLERNAME == "BUY" && role.ACTIONNAME == "Index"
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
                          where t.COMPID == compid && t.TRANSNO == orderNo && t.TRANSDT == Date && t.TRANSYY == Year && t.TRANSTP == "BUY"
                          select t).ToList();
            //Minimum RmsTrans Table MemoNO delete then RmsTransMst tabel data delete(key-> memoNO, compid)
            if (result.Count == 0)
            {
                var searchRmsTransMst = (from n in db.PST_TransMstDbSet where n.COMPID == compid && n.TRANSNO == orderNo && n.TRANSDT == Date && n.TRANSYY == Year && n.TRANSTP == "BUY" select n).ToList();
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

            //Discount rate, Vat rate, Service charge pass the value.
            var transMst = (from m in db.PST_TransMstDbSet
                            where m.COMPID == compid && m.TRANSNO == orderNo && m.TRANSDT == Date && m.TRANSYY == Year && m.TRANSTP == "BUY"
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


            var stk_Trans = (from m in db.PST_TransDbSet
                             where m.COMPID == compid && m.TRANSNO == orderNo && m.TRANSDT == Date && m.TRANSYY == Year && m.TRANSTP == "BUY"
                             select new { m.PATIENTID, m.RSID }).ToList();
            foreach (var s in stk_Trans)
            {
                model.AGL_acchart.ACCOUNTCD = s.RSID;
                TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
                break;
            }


            var sid = db.PST_TransDbSet.Where(x => x.TRANSNO == model.pst_Trans.TRANSNO && x.TRANSDT == Date && x.COMPID == compid && x.TRANSYY == Year && x.TRANSTP == "BUY").Max(o => o.ITEMSL);
            model.pst_Trans.TRANSNO = orderNo;
            model.pst_Trans.ITEMSL = sid;
            model.pst_Trans.TRANSDT = Date;
            model.pst_Trans.TRANSYY = Year;

            if (model.pst_Trans.TRANSDT != null)
            {
                string date = model.pst_Trans.TRANSDT.ToString();
                DateTime MyDateTime = DateTime.Parse(date);
                string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
                TempData["transdate"] = currentDate;
                TempData["transYear"] = model.pst_Trans.TRANSYY;
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

            //Discount rate, Vat rate, Service charge pass the value.
            var transMst = (from m in db.PST_TransMstDbSet
                            where m.COMPID == compid && m.TRANSNO == orderNo && m.TRANSDT == Date && m.TRANSYY == Year && m.TRANSTP == "BUY"
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

            model.AGL_acchart.ACCOUNTCD = model.pst_Trans.RSID;

            string date = model.pst_Trans.TRANSDT.ToString();
            DateTime MyDateTime = DateTime.Parse(date);
            string currentDate = MyDateTime.ToString("dd-MMM-yyyy");
            TempData["ACCOUNTCD"] = model.AGL_acchart.ACCOUNTCD;
            TempData["transdate"] = currentDate;
            TempData["transYear"] = Year;
            TempData["data"] = model;
            TempData["transno"] = model.pst_Trans.TRANSNO;

            return RedirectToAction("EditOrder");

        }














        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DateChanged_getYear(DateTime changedtxt)
        {
            string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(7, 4);
            Int64 year = Convert.ToInt64(getYear);
            var result = new { YEAR = year };
            return Json(result, JsonRequestBehavior.AllowGet);
        }





        public JsonResult DateChanged(string theDate)
        {

            DateTime dt = Convert.ToDateTime(theDate);
            DateTime dd = DateTime.Parse(theDate, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime datetm = Convert.ToDateTime(dd);

            List<SelectListItem> trans = new List<SelectListItem>();
            var transres = (from n in db.PST_TransDbSet
                            where n.TRANSDT == dd && n.COMPID == compid && n.TRANSTP == "BUY"
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
            string tableNumber = "", transType = "";
            DateTime trandate;

            Int64 transno = Convert.ToInt64(changedDropDown);

            TempData["transno"] = transno;

            var rt = db.PST_TransDbSet.Where(n => n.TRANSNO == transno && n.COMPID == compid).Select(n => new
            {
                transtype = n.TRANSTP
            });

            foreach (var n in rt)
            {
                transType = n.transtype;
            }

            var result = new { TRANSTP = transType, TRANSNO = transno };
            return Json(result, JsonRequestBehavior.AllowGet);

        }







        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText, string changeditemType)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);
            string itemId = "";
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



            //if (changeditemType == "THERAPY")
            //{
            //    string substring_itemID = itemId.Substring(3, 2);
            //    string substring_itemID_ForLast4Digit = itemId.Substring(5, 4);
            //    posNid = Convert.ToString(compid + "01" + substring_itemID_ForLast4Digit);

            //}
            //else if (changeditemType == "ACCESSORIES")
            //{
            //    posNid = "";
            //}


            var result = new { itemid = itemId, Rate = rate, qty = 1 };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        //AutoComplete
        public JsonResult TagSearch(string term, string changedDropDown)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);
            Int64 catid_1 = Convert.ToInt64(compid + "01");
            Int64 catid_2 = Convert.ToInt64(compid + "02");

            //(changedDropDown == "ACCESSORIES")
            var tags = from p in db.PST_ItemDbSet
                       where p.COMPID == compid && p.CATID != catid_1 && p.CATID != catid_2
                       select p.ITEMNM;

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
