using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class PST_ItemController : AppController
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

        public PST_ItemController()
        {
            //if (System.Web.HttpContext.Current.Request.Cookies["UI"] != null)
            //{

            //}
            //else
            //{
            //    Index();
            //}
            //HttpCookie decodedCookie1 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CI"]);
            //HttpCookie decodedCookie2 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UI"]);
            //HttpCookie decodedCookie3 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UT"]);
            //HttpCookie decodedCookie4 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UN"]);
            //HttpCookie decodedCookie5 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["US"]);
            //HttpCookie decodedCookie6 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CS"]);

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            ViewData["HighLight_Menu_BillingForm"] = "Heigh Light Menu";
        }




        // Create ASL_LOG object and it used to this Insert_PSTItemMst_LogData, Update_PST_ItemMst_LogData, Delete_PSt_ItemMst_LogData (STK_ITEMMST posItemmst).
        public ASL_LOG aslLog = new ASL_LOG();

        // SAVE ALL INFORMATION from CategoryItemModel TO Asl_lOG Database Table.
        public void Insert_PSTItemMst_LogData(PageModel model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.PST_Itemmst.COMPID && n.USERID == model.PST_Itemmst.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.PST_Itemmst.COMPID);
            aslLog.USERID = model.PST_Itemmst.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.PST_Itemmst.INSIPNO;
            aslLog.LOGLTUDE = model.PST_Itemmst.INSLTUDE;
            aslLog.TABLEID = "PST_ITEMMST";
            aslLog.LOGDATA = Convert.ToString("Category Name: " + model.PST_Itemmst.CATNM + ",\nRemarks: " + model.PST_Itemmst.REMARKS + ".");
            aslLog.USERPC = model.PST_Itemmst.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }




        // Edit ALL INFORMATION from STK_ITEMMST TO Asl_lOG Database Table.
        public void Update_PST_ItemMst_LogData(PST_ITEMMST pst_Itemmst)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));


            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pst_Itemmst.COMPID && n.USERID == pst_Itemmst.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(pst_Itemmst.COMPID);
            aslLog.USERID = pst_Itemmst.UPDUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pst_Itemmst.UPDIPNO;
            aslLog.LOGLTUDE = pst_Itemmst.UPDLTUDE;
            aslLog.TABLEID = "PST_ITEMMST";
            aslLog.LOGDATA = Convert.ToString("Category Name: " + pst_Itemmst.CATNM + ",\nRemarks: " + pst_Itemmst.REMARKS + ".");
            aslLog.USERPC = pst_Itemmst.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }





        // Delete ALL INFORMATION from STK_ITEMMST TO Asl_lOG Database Table.
        public void Delete_PSt_ItemMst_LogData(PST_ITEMMST pst_Itemmst)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pst_Itemmst.COMPID && n.USERID == pst_Itemmst.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(pst_Itemmst.COMPID);
            aslLog.USERID = pst_Itemmst.UPDUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pst_Itemmst.UPDIPNO;
            aslLog.LOGLTUDE = pst_Itemmst.UPDLTUDE;
            aslLog.TABLEID = "PST_ITEMMST";
            aslLog.LOGDATA = Convert.ToString("Category Name: " + pst_Itemmst.CATNM + ",\nRemarks: " + pst_Itemmst.REMARKS + ".");
            aslLog.USERPC = pst_Itemmst.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }






        // SAVE ALL INFORMATION from PST_ITEM TO Asl_lOG Database Table.
        public void Insert_pstItem_LogData(PST_ITEM pstItem)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pstItem.COMPID && n.USERID == pstItem.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(pstItem.COMPID);
            aslLog.USERID = pstItem.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pstItem.INSIPNO;
            aslLog.LOGLTUDE = pstItem.INSLTUDE;
            aslLog.TABLEID = "PST_ITEM";
            aslLog.LOGDATA = Convert.ToString("Item Name: " + pstItem.ITEMNM + ",\nUnit: " + pstItem.UNIT + ",\nRate: " + pstItem.RATE + ",\nRemarks: " + pstItem.REMARKS + ".");
            aslLog.USERPC = pstItem.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }






        // Edit ALL INFORMATION from PST_ITEM TO Asl_lOG Database Table.
        public void Update_pstItem_LogData(ASL_LOG aslLOGRef)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == aslLOGRef.COMPID && n.USERID == aslLOGRef.USERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(aslLOGRef.COMPID);
            aslLog.USERID = aslLOGRef.USERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = aslLOGRef.LOGIPNO;
            aslLog.LOGLTUDE = aslLOGRef.LOGLTUDE;
            aslLog.TABLEID = "PST_ITEM";
            aslLog.LOGDATA = aslLOGRef.LOGDATA;
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
        }




        // Delete ALL INFORMATION from PST_ITEM TO Asl_lOG Database Table.
        public void Delete_pstITEM_LogData(PST_ITEM pstItem)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));


            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pstItem.COMPID && n.USERID == pstItem.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(pstItem.COMPID);
            //aslLog.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
            aslLog.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pstItem.UPDIPNO;
            aslLog.LOGLTUDE = pstItem.UPDLTUDE;
            aslLog.TABLEID = "PST_ITEM";
            aslLog.LOGDATA = Convert.ToString("Item Name: " + pstItem.ITEMNM + ",\nUnit: " + pstItem.UNIT + ",\nRate: " + pstItem.RATE + ",\nRemarks: " + pstItem.REMARKS + ".");
            aslLog.USERPC = pstItem.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }




        // Create ASL_DELETE object and it used to this Delete_ASL_DELETE (AslUserco aslUsercos).
        public ASL_DELETE AslDelete = new ASL_DELETE();

        // Delete ALL INFORMATION from STK_ITEMMST TO ASL_DELETE Database Table.
        public void Delete_ASL_DELETE_pstItemMst(PST_ITEMMST pstItemmst)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == pstItemmst.COMPID && n.USERID == pstItemmst.UPDUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(pstItemmst.COMPID);
            //AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
            AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = pstItemmst.UPDIPNO;
            AslDelete.DELLTUDE = pstItemmst.UPDLTUDE;
            AslDelete.TABLEID = "PST_ITEMMST";
            AslDelete.DELDATA = Convert.ToString("Category Name: " + pstItemmst.CATNM + ",\nRemarks: " + pstItemmst.REMARKS + ".");
            AslDelete.USERPC = pstItemmst.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);
        }





        // Delete ALL INFORMATION from PST_ITEM TO ASL_DELETE Database Table.
        public void Delete_ASL_DELETE_pstITEM(PST_ITEM pstItem)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));


            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == pstItem.COMPID && n.USERID == pstItem.UPDUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(pstItem.COMPID);
            //AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
            AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = pstItem.UPDIPNO;
            AslDelete.DELLTUDE = pstItem.UPDLTUDE;
            AslDelete.TABLEID = "PST_ITEM";
            AslDelete.DELDATA = Convert.ToString("Item Name: " + pstItem.ITEMNM + ",\nUnit: " + pstItem.UNIT + ",\nRate: " + pstItem.RATE + ",\nRemarks: " + pstItem.REMARKS + ".");
            AslDelete.USERPC = pstItem.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);
        }





        // GET: /CategoryItem/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            var dt = (PageModel)TempData["category"];
            return View(dt);
        }



        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel pstItemModel, string command)
        {
            if (command == "Add")
            {
                //.........................................................Create Permission Check
                //var LoggedCompId = System.Web.HttpContext.Current.Request.Cookies["CI"].Value;
                //var loggedUserID = System.Web.HttpContext.Current.Request.Cookies["UI"].Value;

                var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"];
                var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"];

                var createStatus = "";

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
                string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='PST_Item' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
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
                    TempData["ShowAddButton"] = "Show Add Button";
                    TempData["category"] = pstItemModel;
                    TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                    TempData["message"] = "Permission not granted!";
                    return RedirectToAction("Index");
                }
                //...............................................................................................

                pstItemModel.PST_Item.COMPID = pstItemModel.PST_Itemmst.COMPID;
                pstItemModel.PST_Item.CATID = pstItemModel.PST_Itemmst.CATID;
                if (pstItemModel.PST_Item.CATID == null)
                {
                    TempData["message"] = "Enter Category First";
                    return View("Index");
                }
                if (pstItemModel.PST_Item.ITEMNM == null)
                {
                    TempData["ShowAddButton"] = "Show Add Button";
                    TempData["Null_Item_Name"] = "Item Name required!";
                    TempData["category"] = pstItemModel;
                    TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                    return View("Index");
                }
                pstItemModel.PST_Item.USERPC = strHostName;
                pstItemModel.PST_Item.INSIPNO = ipAddress.ToString();
                pstItemModel.PST_Item.INSTIME = td;
                //pstItemModel.PST_Item.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                pstItemModel.PST_Item.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                try
                {

                    PST_ITEMMST stk_itemmst_CompID = db.PST_ItemmstDbSet.FirstOrDefault(r => (r.COMPID == pstItemModel.PST_Itemmst.COMPID));
                    Int64 catagoryID = Convert.ToInt64(pstItemModel.PST_Itemmst.CATID);
                    PST_ITEMMST stk_ItemMst_CATID = db.PST_ItemmstDbSet.FirstOrDefault(r => (r.CATID == catagoryID));

                    if (stk_itemmst_CompID == null && stk_ItemMst_CATID == null)
                    {
                        TempData["ShowAddButton"] = "Show Add Button";
                        TempData["message"] = "Catagory ID not found ";
                        return View("Index");
                    }
                    else
                    {
                        Int64 maxData = Convert.ToInt64((from n in db.PST_ItemDbSet where n.COMPID == pstItemModel.PST_Itemmst.COMPID && n.CATID == pstItemModel.PST_Itemmst.CATID select n.ITEMID).Max());

                        Int64 R = Convert.ToInt64(catagoryID + "9999");

                        if (maxData == 0)
                        {
                            pstItemModel.PST_Item.ITEMID = Convert.ToInt64(catagoryID + "0001");
                            Insert_pstItem_LogData(pstItemModel.PST_Item);

                            db.PST_ItemDbSet.Add(pstItemModel.PST_Item);
                            if (db.SaveChanges() > 0)
                            {
                                TempData["message"] = "Item Successfully Saved";
                                pstItemModel.PST_Item.ITEMNM = "";
                                pstItemModel.PST_Item.UNIT = "";
                                pstItemModel.PST_Item.RATE = 0;
                                pstItemModel.PST_Item.REMARKS = "";



                            }

                        }
                        else if (maxData <= R)
                        {

                            pstItemModel.PST_Item.ITEMID = maxData + 1;
                            Insert_pstItem_LogData(pstItemModel.PST_Item);

                            db.PST_ItemDbSet.Add(pstItemModel.PST_Item);
                            db.SaveChanges();
                            TempData["message"] = "Item Successfully Saved";
                            pstItemModel.PST_Item.ITEMNM = "";
                            pstItemModel.PST_Item.UNIT = "";
                            pstItemModel.PST_Item.RATE = 0;
                            pstItemModel.PST_Item.REMARKS = "";


                        }
                        else
                        {
                            TempData["message"] = "Item entry not possible";
                            TempData["ShowAddButton"] = "Show Add Button";

                        }
                    }

                }
                catch (Exception ex)
                {

                }
                TempData["ShowAddButton"] = "Show Add Button";
                TempData["category"] = pstItemModel;
                TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                return RedirectToAction("Index");
            }


            if (command == "Submit")
            {

                if (pstItemModel.PST_Itemmst.CATNM != null)
                {
                    //Get Ip ADDRESS,Time & user PC Name
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];


                    pstItemModel.PST_Itemmst.USERPC = strHostName;
                    pstItemModel.PST_Itemmst.INSIPNO = ipAddress.ToString();
                    pstItemModel.PST_Itemmst.INSTIME = Convert.ToDateTime(td);
                    TempData["latitute_CategoryList"] = pstItemModel.PST_Itemmst.INSLTUDE;
                    //Insert User ID save STK_ITEMMST table attribute (INSUSERID) field
                    //pstItemModel.PST_Itemmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                    pstItemModel.PST_Itemmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    Int64 companyID = Convert.ToInt64(pstItemModel.PST_Itemmst.COMPID);


                    Int64 minCategoryId = Convert.ToInt64((from n in db.PST_ItemmstDbSet where n.COMPID == companyID select n.CATID).Min());
                    //if (aCategoryItemModel.PST_Itemmst.CATID == null)
                    //{
                    //    aCategoryItemModel.PST_Itemmst.CATID = minCategoryId;
                    //}


                    var result = db.PST_ItemmstDbSet.Count(d => d.CATNM == pstItemModel.PST_Itemmst.CATNM
                                                              && d.COMPID == companyID);
                    if (result == 0)
                    {

                        //.........................................................Create Permission Check
                        //var LoggedCompId = System.Web.HttpContext.Current.Request.Cookies["CI"].Value;
                        //var loggedUserID = System.Web.HttpContext.Current.Request.Cookies["UI"].Value;
                        var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"];
                        var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"];

                        var createStatus = "";

                        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
                        string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='PST_Item' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
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
                            TempData["category"] = pstItemModel;
                            TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                            TempData["ShowAddButton"] = "Show Add Button";
                            TempData["message"] = "Permission not granted!";
                            return RedirectToAction("Index");
                        }
                        //...............................................................................................


                        AslUserco aslUserco = db.AslUsercoDbSet.FirstOrDefault(r => (r.COMPID == companyID));
                        if (aslUserco == null)
                        {
                            TempData["message"] = " User ID not found ";
                            TempData["ShowAddButton"] = "Show Add Button";
                        }
                        else
                        {
                            Int64 maxData = Convert.ToInt64((from n in db.PST_ItemmstDbSet where n.COMPID == pstItemModel.PST_Itemmst.COMPID select n.CATID).Max());

                            Int64 R = Convert.ToInt64(pstItemModel.PST_Itemmst.COMPID + "99");

                            if (maxData == 0)
                            {
                                pstItemModel.PST_Itemmst.CATID = Convert.ToInt64(pstItemModel.PST_Itemmst.COMPID + "01");

                                Insert_PSTItemMst_LogData(pstItemModel);

                                db.PST_ItemmstDbSet.Add(pstItemModel.PST_Itemmst);
                                db.SaveChanges();

                                TempData["message"] = "Category Name: '" + pstItemModel.PST_Itemmst.CATNM + "' successfully saved.\n Please Create the item List.";
                                TempData["ShowAddButton"] = "Show Add Button";
                                TempData["category"] = pstItemModel;
                                TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                                pstItemModel.PST_Item.ITEMNM = "";
                                pstItemModel.PST_Item.UNIT = "";
                                pstItemModel.PST_Item.RATE = 0;
                                pstItemModel.PST_Item.REMARKS = "";
                                return RedirectToAction("Index");
                            }
                            else if (maxData <= R)
                            {
                                pstItemModel.PST_Itemmst.CATID = maxData + 1;

                                Insert_PSTItemMst_LogData(pstItemModel);

                                db.PST_ItemmstDbSet.Add(pstItemModel.PST_Itemmst);
                                db.SaveChanges();

                                TempData["message"] = "Category Name: '" + pstItemModel.PST_Itemmst.CATNM + "' successfully saved.\n Please Create the item List. ";
                                TempData["ShowAddButton"] = "Show Add Button";
                                TempData["category"] = pstItemModel;
                                TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                                pstItemModel.PST_Item.ITEMNM = "";
                                pstItemModel.PST_Item.UNIT = "";
                                pstItemModel.PST_Item.RATE = 0;
                                pstItemModel.PST_Item.REMARKS = "";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["ShowAddButton"] = "Show Add Button";
                                TempData["message"] = "Not possible entry ";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    else if (result > 0)
                    {
                        var findCategoryID = (from m in db.PST_ItemmstDbSet
                                              where m.COMPID == companyID && m.CATNM == pstItemModel.PST_Itemmst.CATNM
                                              select new { m.CATID }).Distinct().ToList();
                        foreach (var l in findCategoryID)
                        {
                            pstItemModel.PST_Itemmst.CATID = l.CATID;
                        }



                        //TempData["message"] = "Get the Item List";
                        TempData["ShowAddButton"] = "Show Add Button";
                        TempData["category"] = pstItemModel;
                        TempData["categoryId"] = pstItemModel.PST_Itemmst.CATID;
                        TempData["latitute_CategoryList"] = pstItemModel.PST_Itemmst.INSLTUDE;
                        pstItemModel.PST_Item.ITEMNM = "";
                        pstItemModel.PST_Item.UNIT = "";
                        pstItemModel.PST_Item.RATE = 0;
                        pstItemModel.PST_Item.REMARKS = "";
                        return RedirectToAction("Index");
                    }
                }

                else if (pstItemModel.PST_Itemmst.CATNM == null && pstItemModel.PST_Itemmst.REMARKS != null)
                {
                    TempData["CategoryMsg"] = "Please Enter Category Name.";
                    return View("Index");
                }
                else if (pstItemModel.PST_Itemmst.CATNM == null)
                {
                    TempData["CategoryMsg"] = "Please Enter Category Name!";
                    return RedirectToAction("Index");
                }


            }

            if (command == "Update")
            {
                var query =
                    from a in db.PST_ItemDbSet
                    where (a.ITEMID == pstItemModel.PST_Item.ITEMID && a.COMPID == pstItemModel.PST_Itemmst.COMPID && a.CATID == pstItemModel.PST_Itemmst.CATID)
                    select a;
                pstItemModel.PST_Item.COMPID = pstItemModel.PST_Itemmst.COMPID;
                pstItemModel.PST_Item.CATID = pstItemModel.PST_Itemmst.CATID;


                foreach (PST_ITEM a in query)
                {
                    // Insert any additional changes to column values.
                    a.ITEMNM = pstItemModel.PST_Item.ITEMNM;
                    a.UNIT = pstItemModel.PST_Item.UNIT;
                    a.RATE = pstItemModel.PST_Item.RATE;
                    a.REMARKS = pstItemModel.PST_Item.REMARKS;
                    a.UPDIPNO = ipAddress.ToString();
                    a.UPDTIME = td;
                    //a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                    a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    a.UPDLTUDE = pstItemModel.PST_Itemmst.INSLTUDE;
                    TempData["StkIemLogData"] = Convert.ToString("Item Name: " + a.ITEMNM + ",\nUnit: " + a.UNIT + ",\nRate: " + a.RATE + ",\nRemarks: " + a.REMARKS + ".");

                }

                ASL_LOG aslLogref = new ASL_LOG();

                aslLogref.LOGIPNO = ipAddress.ToString();
                aslLogref.COMPID = pstItemModel.PST_Item.COMPID;
                aslLogref.LOGLTUDE = pstItemModel.PST_Item.INSLTUDE;

                //Update User ID save ASL_ROLE table attribute UPDUSERID
                //aslLogref.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                aslLogref.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                aslLogref.LOGDATA = TempData["StkIemLogData"].ToString();
                Update_pstItem_LogData(aslLogref);

                db.SaveChanges();

                TempData["category"] = pstItemModel;
                TempData["categoryId"] = pstItemModel.PST_Item.CATID;
                TempData["ShowAddButton"] = "Show Add Button";
                pstItemModel.PST_Item.ITEMNM = "";
                pstItemModel.PST_Item.UNIT = "";
                pstItemModel.PST_Item.RATE = 0;
                pstItemModel.PST_Item.REMARKS = "";
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }






        public ActionResult EditItemUpdate(Int64 id, Int64 cid, Int64 catid, Int64 itemId, string itemname, PageModel model)
        {
            //.........................................................Create Permission Check
            //var LoggedCompId = System.Web.HttpContext.Current.Request.Cookies["CI"].Value;
            //var loggedUserID = System.Web.HttpContext.Current.Request.Cookies["UI"].Value;
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"];
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"];

            var updateStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
            string query1 = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='PST_Item' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
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
            var categoryName_Remarks = from m in db.PST_ItemmstDbSet where m.CATID == catid && m.COMPID == cid select m;
            foreach (var categoryResult in categoryName_Remarks)
            {
                model.PST_Itemmst.COMPID = cid;
                model.PST_Itemmst.CATID = catid;
                model.PST_Itemmst.CATNM = categoryResult.CATNM;
                model.PST_Itemmst.REMARKS = categoryResult.REMARKS;
            }
            TempData["category"] = model;
            TempData["categoryId"] = catid;
            TempData["ShowAddButton"] = null;
            if (updateStatus == 'I'.ToString())
            {
                TempData["message"] = "Permission not granted!";
                return RedirectToAction("Index");
            }
            //...............................................................................................

            model.PST_Item = db.PST_ItemDbSet.Find(id);

            var item = from r in db.PST_ItemDbSet where r.PST_ITEM_ID == id select r.ITEMNM;
            foreach (var it in item)
            {
                model.PST_Item.ITEMNM = it.ToString();
            }

            return RedirectToAction("Index");

        }




        public ActionResult ItemDelete(Int64 id, Int64 cid, Int64 catid, Int64 itemId, string itemname, PageModel model)
        {
            //.........................................................Create Permission Check
            //var LoggedCompId = System.Web.HttpContext.Current.Request.Cookies["CI"].Value;
            //var loggedUserID = System.Web.HttpContext.Current.Request.Cookies["UI"].Value;
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"];
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"];

            var deleteStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
            string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='PST_Item' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
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
            var categoryName_Remarks = from m in db.PST_ItemmstDbSet where m.CATID == catid && m.COMPID == cid select m;
            foreach (var categoryResult in categoryName_Remarks)
            {
                model.PST_Itemmst.COMPID = cid;
                model.PST_Itemmst.CATID = catid;
                model.PST_Itemmst.CATNM = categoryResult.CATNM;
                model.PST_Itemmst.REMARKS = categoryResult.REMARKS;
            }
            TempData["category"] = model;
            TempData["categoryId"] = catid;
            TempData["ShowAddButton"] = "Show Add Button";
            if (deleteStatus == 'I'.ToString())
            {
                TempData["message"] = "Permission not granted!";
                return RedirectToAction("Index");

            }
            //...............................................................................................

            PST_ITEM pstItem = db.PST_ItemDbSet.Find(id);
            //Get Ip ADDRESS,Time & user PC Name
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            pstItem.USERPC = strHostName;
            pstItem.UPDIPNO = ipAddress.ToString();
            pstItem.UPDTIME = Convert.ToDateTime(td);
            //Delete User ID save STK_ITEMMST table attribute (UPDUSERID) field
            //stkitem.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
            pstItem.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            if (TempData["latitute_CategoryList"] != null)
            {
                //Get current LOGLTUDE data 
                pstItem.UPDLTUDE = TempData["latitute_CategoryList"].ToString();
            }

            Delete_pstITEM_LogData(pstItem);
            Delete_ASL_DELETE_pstITEM(pstItem);
            db.PST_ItemDbSet.Remove(pstItem);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        //
        // GET: /CategoryItemModel/
        public ActionResult ShowCategoryList()
        {
            ViewData["HighLight_Menu_Settings"] = "Heigh Light Menu";
            ViewData["HighLight_Menu_BillingForm"] = null;
            //Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["CI"].Value);
            Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
            var result = (from n in db.PST_ItemmstDbSet
                          where n.COMPID == compid
                          select n
                     );
            return View(result);
        }

        //
        // GET: /CategoryItemModel

        public ActionResult EditCategoryList(int id = 0)
        {
            ViewData["HighLight_Menu_Settings"] = "Heigh Light Menu";
            ViewData["HighLight_Menu_BillingForm"] = null;
            PST_ITEMMST pstItemmst = db.PST_ItemmstDbSet.Find(id);
            if (pstItemmst == null)
            {
                return HttpNotFound();
            }
            return View(pstItemmst);
        }


        // POST: /CategoryItemModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategoryList(PST_ITEMMST pstItemmst, string Command)
        {
            if (Command == "Update")
            {
                if (ModelState.IsValid)
                {
                    //Get Ip ADDRESS,Time & user PC Name
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];


                    pstItemmst.UPDIPNO = ipAddress.ToString();
                    pstItemmst.UPDTIME = Convert.ToDateTime(td);

                    //Insert User ID save ASL_MENUMST table attribute INSUSERID
                    //stkItemmst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                    pstItemmst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    Update_PST_ItemMst_LogData(pstItemmst);

                    db.Entry(pstItemmst).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["UpdateCategoryInfo"] = "Category Name: '" + pstItemmst.CATNM + "' update successfully!";
                    return RedirectToAction("ShowCategoryList");
                }
                return View(pstItemmst);
            }
            else
            {
                return RedirectToAction("ShowCategoryList");
            }
        }




        //
        // GET: /CategoryItemModel

        public ActionResult DeleteCategory(int id = 0)
        {
            ViewData["HighLight_Menu_Settings"] = "Heigh Light Menu";
            ViewData["HighLight_Menu_BillingForm"] = null;
            PST_ITEMMST pstItemmst = db.PST_ItemmstDbSet.Find(id);
            if (pstItemmst == null)
            {
                return HttpNotFound();
            }
            return View(pstItemmst);
        }

        //
        // POST: /CategoryItemModel

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirmed(int id, PST_ITEMMST pst_Itemmst_Delete, String Command)
        {
            if (Command == "Yes")
            {
                PST_ITEMMST pstItemmst = db.PST_ItemmstDbSet.Find(id);
                //Get Ip ADDRESS,Time & user PC Name 
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                pstItemmst.USERPC = strHostName;
                pstItemmst.UPDIPNO = ipAddress.ToString();
                pstItemmst.UPDTIME = Convert.ToDateTime(td);
                //Delete User ID save STK_ITEMMST table attribute (UPDUSERID) field
                //pstItemmst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                pstItemmst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                //Get current LOGLTUDE data 
                pstItemmst.UPDLTUDE = pst_Itemmst_Delete.UPDLTUDE;

                //Search all information from Menu Table,when it match to the Module ID
                var menuList = ((from sub in db.PST_ItemDbSet select sub)
                    .Where(sub => sub.CATID == pstItemmst.CATID)).ToList();

                var date = Convert.ToString(td.ToString("dd-MMM-yyyy"));
                var time = Convert.ToString(td.ToString("hh:mm:ss tt"));

                Int64 maxSerialNoDelete =
                    Convert.ToInt64(
                        (from n in db.AslDeleteDbSet
                         where n.COMPID == pstItemmst.COMPID && n.USERID == pstItemmst.UPDUSERID
                         select n.DELSLNO).Max());
                if (maxSerialNoDelete == 0)
                {
                    AslDelete.DELSLNO = Convert.ToInt64("1");
                }
                else
                {
                    AslDelete.DELSLNO = maxSerialNoDelete + 1;
                }
                // Delete ALL INFORMATION from PST_ITEM TO Asl_Delete Database Table.
                AslDelete.COMPID = Convert.ToInt64(pstItemmst.COMPID);
                AslDelete.USERID = Convert.ToInt64(pstItemmst.UPDUSERID);
                AslDelete.DELSLNO = AslDelete.DELSLNO;
                AslDelete.DELDATE = Convert.ToString(date);
                AslDelete.DELTIME = Convert.ToString(time);
                AslDelete.DELIPNO = pstItemmst.UPDIPNO;
                AslDelete.DELLTUDE = pstItemmst.UPDLTUDE;
                AslDelete.TABLEID = "PST_ITEM";
                AslDelete.USERPC = pstItemmst.USERPC;
                AslDelete.DELDATA = " ";


                Int64 maxSerialNoLog =
                    Convert.ToInt64(
                        (from n in db.AslLogDbSet
                         where n.COMPID == pstItemmst.COMPID && n.USERID == pstItemmst.UPDUSERID
                         select n.LOGSLNO).Max());
                if (maxSerialNoLog == 0)
                {
                    aslLog.LOGSLNO = Convert.ToInt64("1");
                }
                else
                {
                    aslLog.LOGSLNO = maxSerialNoLog + 1;
                }
                // Delete ALL INFORMATION from PST_ITEM TO Asl_lOG Database Table.
                aslLog.COMPID = Convert.ToInt64(pstItemmst.COMPID);
                aslLog.USERID = Convert.ToInt64(pstItemmst.UPDUSERID);
                aslLog.LOGTYPE = "DELETE";
                aslLog.LOGSLNO = aslLog.LOGSLNO;
                aslLog.LOGDATE = Convert.ToDateTime(date);
                aslLog.LOGTIME = Convert.ToString(time);
                aslLog.LOGIPNO = pstItemmst.UPDIPNO;
                aslLog.LOGLTUDE = pstItemmst.UPDLTUDE;
                aslLog.TABLEID = "PST_ITEM";
                aslLog.USERPC = pstItemmst.USERPC;
                aslLog.LOGDATA = "";

                Int64 serial = 1;
                if (menuList.Count == 0)
                {
                    AslDelete.DELDATA = "Category wise item data empty !";
                    aslLog.LOGDATA = "Category wise item data empty !";
                }


                foreach (var n in menuList)
                {
                    AslDelete.DELDATA = AslDelete.DELDATA +
                                        Convert.ToString("(" + serial + ")"+"Item Name: " + n.ITEMNM + ",\nUnit: " + n.UNIT + ",\n Rate: " + n.RATE +
                                                         ",\nRemarks: " + n.REMARKS +
                                                         " .\n..................\n");
                    aslLog.LOGDATA = aslLog.LOGDATA +
                                    Convert.ToString("(" + serial + ")" + "Item Name: " + n.ITEMNM + ",\nUnit: " + n.UNIT + ",\n Rate: " + n.RATE +
                                                         ",\nRemarks: " + n.REMARKS +
                                                         " .\n..................\n");

                    serial += 1;
                    db.PST_ItemDbSet.Remove(n);

                }
                db.AslDeleteDbSet.Add(AslDelete);
                db.AslLogDbSet.Add(aslLog);
                db.SaveChanges();

                Delete_PSt_ItemMst_LogData(pstItemmst);
                Delete_ASL_DELETE_pstItemMst(pstItemmst);

                db.PST_ItemmstDbSet.Remove(pstItemmst);
                db.SaveChanges();

                TempData["DeleteCategoryInfo"] = "Category Name: '" + pstItemmst.CATNM + "' delete successfully!";
                return RedirectToAction("ShowCategoryList");
            }
            else
            {
                return RedirectToAction("ShowCategoryList");
            }
        }


        //// Get All Item Information
        //public ActionResult ShowItemList()
        //{
        //    Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
        //    var result = (from n in db.StkItemDbSet
        //                  where n.COMPID == compid
        //                  select n
        //             );
        //    return View(result);
        //}





        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText)
        {
            //var compid = Convert.ToInt16(System.Web.HttpContext.Current.Request.Cookies["CI"].Value);
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);
            String itemId = "";
            string remarks = "";
            var rt = db.PST_ItemmstDbSet.Where(n => n.CATNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             catid = n.CATID,
                                                             rmks = n.REMARKS
                                                         });
            foreach (var n in rt)
            {
                itemId = Convert.ToString(n.catid);
                remarks = Convert.ToString(n.rmks);
            }

            var result = new { catid = itemId, rmrks = remarks };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        //AutoComplete
        public JsonResult TagSearch(string term)
        {
            //var compid = Convert.ToInt16(System.Web.HttpContext.Current.Request.Cookies["CI"].Value);
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"]);


            var tags = from p in db.PST_ItemmstDbSet
                       where p.COMPID == compid
                       select p.CATNM;

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
