using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class ClosingBalanceController : AppController
    {
        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        Therapy_GL_DbContext db = new Therapy_GL_DbContext();
        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;


        public ClosingBalanceController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            ViewData["HighLight_Menu_AccountForm"] = "highlight menu";
        }






        public ASL_LOG aslLog = new ASL_LOG();

        public void Insert_ClosingBalance_LogData(PageModel model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.AGlMaster.COMPID && n.USERID == model.AGlMaster.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.AGlMaster.COMPID);
            aslLog.USERID = model.AGlMaster.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.AGlMaster.INSIPNO;
            aslLog.LOGLTUDE = model.AGlMaster.INSLTUDE;
            aslLog.TABLEID = "GL_MASTER";

            string accountname = "";
            var find_account =
                (from n in db.GlAcchartDbSet
                 where n.COMPID == model.AGlMaster.COMPID && n.ACCOUNTCD == model.AGlMaster.DEBITCD
                 select n).ToList();
            foreach (var name in find_account)
            {
                accountname = name.ACCOUNTNM;
            }

            string transDate = "";
            if (model.AGlMaster.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.AGlMaster.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Closing: " + "Closing Balance" + ",\nDate: " + transDate + ",\nAccountName: " + accountname + ",\nDebitamount :" + model.AGlMaster.DEBITAMT + ",\nCreditAmount" + model.AGlMaster.CREDITAMT + ".");
            aslLog.USERPC = model.AGlMaster.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }




        public void Update_ClosingBalance_LogData(PageModel model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));


            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.AGlMaster.COMPID && n.USERID == model.AGlMaster.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.AGlMaster.COMPID);
            aslLog.USERID = model.AGlMaster.UPDUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.AGlMaster.UPDIPNO;
            aslLog.LOGLTUDE = model.AGlMaster.UPDLTUDE;
            aslLog.TABLEID = "GL_MASTER";

            string accountname = "";
            var find_account =
                (from n in db.GlAcchartDbSet
                 where n.COMPID == model.AGlMaster.COMPID && n.ACCOUNTCD == model.AGlMaster.DEBITCD
                 select n).ToList();
            foreach (var name in find_account)
            {
                accountname = name.ACCOUNTNM;
            }


            string transDate = "";
            if (model.AGlMaster.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.AGlMaster.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Closing: " + "Closing Balance" + ",\nDate: " + transDate + ",\nAccountName: " + accountname + ",\nDebitamount :" + model.AGlMaster.DEBITAMT + ",\nCreditAmount" + model.AGlMaster.CREDITAMT + ".");
            aslLog.USERPC = model.AGlMaster.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }





        public void Delete_ClosingBalance_LogData(GL_MASTER model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = model.UPDUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.UPDIPNO;
            aslLog.LOGLTUDE = model.UPDLTUDE;
            aslLog.TABLEID = "GL_MASTER";

            string accountname = "";
            var find_account =
                (from n in db.GlAcchartDbSet
                 where n.COMPID == model.COMPID && n.ACCOUNTCD == model.DEBITCD
                 select n).ToList();
            foreach (var name in find_account)
            {
                accountname = name.ACCOUNTNM;
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            aslLog.LOGDATA = Convert.ToString("Closing: " + "Closing Balance" + ",\nDate: " + transDate + ",\nAccountName: " + accountname + ",\nDebitamount :" + model.DEBITAMT + ",\nCreditAmount" + model.CREDITAMT + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }








        public ASL_DELETE AslDelete = new ASL_DELETE();
        public void Delete_closingBalance(GL_MASTER model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(model.COMPID);
            AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = model.UPDIPNO;
            AslDelete.DELLTUDE = model.UPDLTUDE;
            AslDelete.TABLEID = "GL_MASTER";

            string accountname = "";
            var find_account =
                (from n in db.GlAcchartDbSet
                 where n.COMPID == model.COMPID && n.ACCOUNTCD == model.DEBITCD
                 select n).ToList();
            foreach (var name in find_account)
            {
                accountname = name.ACCOUNTNM;
            }

            string transDate = "";
            if (model.TRANSDT != null)
            {
                string convert_Date = Convert.ToString(model.TRANSDT);
                DateTime MyDateTime = DateTime.Parse(convert_Date);
                transDate = MyDateTime.ToString("dd-MMM-yyyy");
            }

            AslDelete.DELDATA = Convert.ToString("Closing: " + "Closing Balance" + ",\nDate: " + transDate + ",\nAccountName: " + accountname + ",\nDebitamount :" + model.DEBITAMT + ",\nCreditAmount" + model.CREDITAMT + ".");
            AslDelete.USERPC = model.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);
        }















        // GET: /ClosingBalance/
        public ActionResult Index()
        {
            var dt = (PageModel)TempData["costpool"];
            return View(dt);
        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel model, string command)
        {

            if (command == "Submit")
            {
                if (model.AGlMaster.TRANSDT != null)
                {
                    Int64 companyID = Convert.ToInt64(model.AGlMaster.COMPID);
                    DateTime changedtxt = Convert.ToDateTime(model.AGlMaster.TRANSDT);
                    string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));

                    TempData["latitute_deleteAccount"] = model.AGlMaster.INSLTUDE;

                    var result = db.GlMasterDbSet.Count(d => d.TRANSTP == "OPEN" && d.COMPID == companyID);

                    if (result == 0)
                    {
                        TempData["message"] = "Continue";
                        TempData["costpool"] = model;
                        TempData["TRANSDT"] = converttoString;
                        TempData["ShowAddButton"] = "Show Add Button";

                        model.AGlMaster.DEBITCD = 0;
                        model.AGlMaster.DEBITAMT = 0;
                        model.AGlMaster.CREDITAMT = 0;
                    }
                    else
                    {
                        var check_date = db.GlMasterDbSet.Count(d => model.AGlMaster.TRANSDT == d.TRANSDT && d.COMPID == companyID);
                        

                        if (check_date == 0)
                        {
                            TempData["Not_Possible"] = "not possible";
                            TempData["costpool"] = model;
                            TempData["TRANSDT"] = converttoString;
                            TempData["ShowAddButton"] = "Show Add Button";

                            model.AGlMaster.DEBITCD = 0;
                            model.AGlMaster.DEBITAMT = 0;
                            model.AGlMaster.CREDITAMT = 0;
                        }
                        else
                        {
                            TempData["message"] = "Continue";
                            TempData["costpool"] = model;
                            TempData["TRANSDT"] = converttoString;

                            TempData["ShowAddButton"] = "Show Add Button";

                            model.AGlMaster.DEBITCD = 0;
                            model.AGlMaster.DEBITAMT = 0;
                            model.AGlMaster.CREDITAMT = 0;

                        }
                    }
                }
                return RedirectToAction("Index");
            }




            if (command == "Add")
            {
                if (model.AGlMaster.TRANSDT != null)
                {
                    //Get Ip ADDRESS,Time & user PC Name
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];

                    model.AGlMaster.USERPC = strHostName;
                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                    model.AGlMaster.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                    Int64 companyID = Convert.ToInt64(model.AGlMaster.COMPID);
                    DateTime changedtxt = Convert.ToDateTime(model.AGlMaster.TRANSDT);



                    //.........................................................Create Permission Check
                    var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
                    var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

                    var createStatus = "";

                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
                    string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='ClosingBalance' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable ds = new DataTable();
                    da.Fill(ds);

                    foreach (DataRow row in ds.Rows)
                    {
                        createStatus = row["INSERTR"].ToString();
                    }

                    conn.Close();

                    if (createStatus == 'I'.ToString())
                    {
                        TempData["message"] = "Permission not granted!";
                        return RedirectToAction("Index");
                    }
                    //...............................................................................................


                    model.AGlMaster.TRANSTP = "OPEN";



                    string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));

                    string getYear = converttoString.Substring(9, 2);
                    string getMonth = converttoString.Substring(3, 3);
                    string Month = getMonth + "-" + getYear;

                    model.AGlMaster.TRANSMY = Convert.ToString(Month);



                    string converttoString1 = Convert.ToString(changedtxt.ToString("dd-MM-yyyy"));

                    string getYear1 = converttoString1.Substring(6, 4);
                    string getmonth1 = converttoString1.Substring(3, 2);
                    string fultransno = getYear1 + getmonth1 + "0001";

                    model.AGlMaster.TRANSNO = Convert.ToInt64(fultransno);


                    
                    //Insert_COSTPOOLMST_LogData(model);


                    Int64 maxtranssl = Convert.ToInt64((from n in db.GlMasterDbSet where n.COMPID == companyID && n.TRANSDT == model.AGlMaster.TRANSDT && n.TRANSTP == model.AGlMaster.TRANSTP select n.TRANSSL).Max());
                    if (maxtranssl == 0)
                    {
                        string getTransSL = "50001";
                        model.AGlMaster.TRANSSL = Convert.ToInt64(getTransSL);
                    }
                    else
                    {
                        model.AGlMaster.TRANSSL = maxtranssl + 1;

                    }


                    model.AGlMaster.DEBITCD = model.AGlMaster.DEBITCD;
                    model.AGlMaster.DEBITAMT = model.AGlMaster.DEBITAMT;
                    model.AGlMaster.CREDITAMT = model.AGlMaster.CREDITAMT;

                    db.GlMasterDbSet.Add(model.AGlMaster);
                    Insert_ClosingBalance_LogData(model);
                    db.SaveChanges();

                    TempData["message"] = "Succesfully Saved";
                    TempData["ShowAddButton"] = "Show Add Button";
                    TempData["costpool"] = model;

                    TempData["TRANSDT"] = converttoString;

                    model.AGlMaster.DEBITCD = 0;
                    model.AGlMaster.DEBITAMT = 0;
                    model.AGlMaster.CREDITAMT = 0;
                    return RedirectToAction("Index");

                }
            }


            if (command == "Update")
            {
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];



                Int64 companyID = Convert.ToInt64(model.AGlMaster.COMPID);

                DateTime changedtxt = Convert.ToDateTime(model.AGlMaster.TRANSDT);

                string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));


                var query =
                   from a in db.GlMasterDbSet
                   where (a.GL_MasterID == model.AGlMaster.GL_MasterID && a.COMPID == model.AGlMaster.COMPID)
                   select a;

                foreach (var n in query)
                {
                    n.GL_MasterID = model.AGlMaster.GL_MasterID;
                    n.TRANSDT = model.AGlMaster.TRANSDT;
                    n.TRANSMY = model.AGlMaster.TRANSMY;
                    n.TRANSNO = model.AGlMaster.TRANSNO;
                    n.TRANSSL = model.AGlMaster.TRANSSL;
                    n.DEBITCD = model.AGlMaster.DEBITCD;
                    n.DEBITAMT = model.AGlMaster.DEBITAMT;
                    n.CREDITAMT = model.AGlMaster.CREDITAMT;

                    n.USERPC = strHostName;
                    n.UPDIPNO = ipAddress.ToString();
                    n.UPDTIME = Convert.ToDateTime(td);
                    n.UPDLTUDE = model.AGlMaster.INSLTUDE;
                    n.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                }

                model.AGlMaster.UPDLTUDE = model.AGlMaster.INSLTUDE;
                model.AGlMaster.UPDIPNO = ipAddress.ToString();
                model.AGlMaster.UPDTIME = Convert.ToDateTime(td);
                model.AGlMaster.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.AGlMaster.USERPC = strHostName;

                Update_ClosingBalance_LogData(model);
                db.SaveChanges();
                
                TempData["message"] = "Update Successfully";
                TempData["ShowAddButton"] = "Show Add Button";
                TempData["costpool"] = model;
                TempData["TRANSDT"] = converttoString;

                model.AGlMaster.DEBITCD = 0;
                model.AGlMaster.DEBITAMT = 0;
                model.AGlMaster.CREDITAMT = 0;
                return RedirectToAction("Index");
            }




            return RedirectToAction("Index");


        }

        public ActionResult EditAccountUpdate(Int64 id, Int64 cid, string type, DateTime date, Int64 serial, PageModel model)
        {
            //.........................................................Update Permission Check
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

            var updateStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
            string query1 = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='ClosingBalance' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query1, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                updateStatus = row["UPDATER"].ToString();
            }
            conn.Close();

            //add the data from database to model
            var result = from m in db.GlMasterDbSet where m.GL_MasterID == id && m.COMPID == cid select m;

            foreach (var categoryResult in result)
            {
                model.AGlMaster.COMPID = cid;
                model.AGlMaster.GL_MasterID = id;
                model.AGlMaster.TRANSDT = categoryResult.TRANSDT;
                model.AGlMaster.TRANSTP = categoryResult.TRANSTP;
                model.AGlMaster.TRANSNO = categoryResult.TRANSNO;
                model.AGlMaster.TRANSMY = categoryResult.TRANSMY;
                model.AGlMaster.TRANSSL = categoryResult.TRANSSL;
                model.AGlMaster.DEBITCD = categoryResult.DEBITCD;
                model.AGlMaster.DEBITAMT = categoryResult.DEBITAMT;
                model.AGlMaster.CREDITAMT = categoryResult.CREDITAMT;


            }

            DateTime changedtxt = Convert.ToDateTime(model.AGlMaster.TRANSDT);

            string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));

            

            if (updateStatus == 'I'.ToString())
            {
                TempData["TRANSDT"] = converttoString;
                TempData["debitcd"] = model.AGlMaster.DEBITCD;
                TempData["message"] = "Permission not granted!";
                TempData["ShowAddButton"] = "show add button";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["costpool"] = model;
                TempData["TRANSDT"] = converttoString;
                TempData["debitcd"] = model.AGlMaster.DEBITCD;
                TempData["message"] = "editmode";
                TempData["ShowAddButton"] = null;
                return RedirectToAction("Index");
            }

        }


        public ActionResult AccountDelete(Int64 id, Int64 cid, string type, DateTime date, Int64 serial, PageModel model)
        {
            //.........................................................Create Permission Check
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

            var deleteStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
            string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='ClosingBalance' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                deleteStatus = row["DELETER"].ToString();
            }
            conn.Close();

        

            //add the data from database to model
            var rowid = from m in db.GlMasterDbSet where m.GL_MasterID == id && m.COMPID == cid select m;
            foreach (var categoryResult in rowid)
            {
                model.AGlMaster.COMPID = cid;
                model.AGlMaster.GL_MasterID = id;
                model.AGlMaster.TRANSDT = categoryResult.TRANSDT;

            }

            DateTime changedtxt = Convert.ToDateTime(model.AGlMaster.TRANSDT);
            string converttoString = Convert.ToString(changedtxt.ToString("dd-MMM-yyyy"));

            if (deleteStatus == "I")
            {
                TempData["costpool"] = model;
                TempData["TRANSDT"] = converttoString;
                TempData["message"] = "Permission not granted!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["costpool"] = model;
                TempData["TRANSDT"] = converttoString;
                TempData["message"] = "Delete Successfully";
                TempData["ShowAddButton"] = "Show Add Button";
            }
          
            

            GL_MASTER Accountitem = db.GlMasterDbSet.Find(id);

            //Get Ip ADDRESS,Time & user PC Name
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            Accountitem.USERPC = strHostName;
            Accountitem.UPDIPNO = ipAddress.ToString();
            Accountitem.UPDTIME = Convert.ToDateTime(td);
            //Delete User ID save POS_ITEMMST table attribute (UPDUSERID) field
            Accountitem.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            if (TempData["latitute_deleteAccount"] != null)
            {
                //Get current LOGLTUDE data 
                Accountitem.UPDLTUDE = TempData["latitute_deleteAccount"].ToString();
            }
            
            db.GlMasterDbSet.Remove(Accountitem);
            Delete_ClosingBalance_LogData(Accountitem);
            Delete_closingBalance(Accountitem);
            db.SaveChanges();



            model.AGlMaster.DEBITCD = 0;
            model.AGlMaster.DEBITAMT = 0;
            model.AGlMaster.CREDITAMT = 0;

            return RedirectToAction("Index");
        }




        //AutoComplete
        public JsonResult TagSearch(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            
            string cash = Convert.ToString(compid) + "101";
            string bank = Convert.ToString(compid) + "102";

            var tags = from p in db.GlAcchartDbSet
                       where p.COMPID == compid 
                       select p.ACCOUNTNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);




        }

        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());


            String itemId = "";

            var rt = db.GlAcchartDbSet.Where(n => n.ACCOUNTNM.StartsWith(changedText) &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             accountname = n.ACCOUNTNM

                                                         }).ToList();
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
                        string ss = Convert.ToString(n.accountname);
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
            

            String itemId2 = "";

            var rt2 = db.GlAcchartDbSet.Where(n => n.ACCOUNTNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             debitcd = n.ACCOUNTCD,

                                                         });
            foreach (var n in rt2)
            {
                itemId2 = Convert.ToString(n.debitcd);

            }

            var result = new {Accountname=itemId, debitcd = itemId2 };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
