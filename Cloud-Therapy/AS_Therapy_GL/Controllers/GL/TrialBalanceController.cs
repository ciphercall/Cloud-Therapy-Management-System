using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class TrialBalanceController : AppController
    {
        private TimeZoneInfo timeZoneInfo;
        DateTime currentDateTime;

        public TrialBalanceController()
        {
            ViewData["HighLight_Menu_AccountReports"] = "High Light Menu";
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        // GET: /TrialBalance/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PageModel model,string command)
        {
            if (command == "Show")
            {
                string date = Convert.ToString(model.AGlMaster.TRANSDT);
                DateTime myDateTime = DateTime.Parse(date);
                string converttoString = myDateTime.ToString("dd-MMM-yyyy");
                TempData["Trial_Balance_Date"] = converttoString;
                TempData["Trial Balance"] = model;
                return RedirectToAction("Index");
            }
            else //if (command == "Print")
            {
                TempData["Trial Balance"] = model;
                return RedirectToAction("TrialBalanceReport");
            }
        }

        public ActionResult TrialBalanceReport()
        {
            PageModel model = (PageModel)TempData["Trial Balance"];
            return View(model);
        }




        public ActionResult RowWiseLinkPost(Int64 COMPID, Int64 DEBITCD, string ACCOUNTNM)
        {
            GL_ACCHART model = new GL_ACCHART();
            model.COMPID = COMPID;
            model.ACCOUNTCD = DEBITCD;
            model.ACCOUNTNM = ACCOUNTNM;
            TempData["rowWiseLinkPost_Model"] = model;
            return RedirectToAction("GetDrilDownReport");
        }


        public ActionResult GetDrilDownReport()
        {
            GL_ACCHART model = (GL_ACCHART)TempData["rowWiseLinkPost_Model"];
            return View(model);
        }






        //...................................................................................................................
        // Get Data for click event call . This process showing from RandD_javascriptWise.cshtml
        public JsonResult getData(string compid, string DebitCD)
        {
            //var jsonData = '[{"rank":"9","content":"Alon","UID":"5"},{"rank":"6","content":"Tala","UID":"6"}]';
            Int64 companyID = Convert.ToInt64(compid);
            Int64 debitcd = Convert.ToInt64(DebitCD);

            var currentTime = currentDateTime.ToString("dd-MMM-yyyy");
            var CurrentYear = currentDateTime.ToString("yyyy");

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Therapy_GL_DbContext"].ToString());
            string query = string.Format(
                @"SELECT DISTINCT B.COMPID,B.DEBITCD, LASTDT, UPPER(CONVERT(varchar(3),LASTDT,7))+'-'+CONVERT(varchar(2),LASTDT,2) LASTMY,
A.DEBITAMT DRAMT, A.CREDITAMT CRAMT, SUM(B.DEBITAMT)-SUM(B.CREDITAMT) AMT
FROM (
SELECT DISTINCT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,TRANSDT)+1,0)) LASTDT,COMPID,DEBITCD, SUM(DEBITAMT) DEBITAMT, SUM(CREDITAMT) CREDITAMT
FROM GL_MASTER WHERE COMPID = '{0}' AND DEBITCD='{1}' AND right(CONVERT(varchar(11),TRANSDT),4) = '{3}' AND TRANSDT <= '{2}'
GROUP BY DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,TRANSDT)+1,0)),COMPID,DEBITCD
) A, GL_MASTER AS B
WHERE B.TRANSDT <= A.LASTDT AND B.DEBITCD = '{1}' AND B.TRANSDT <= '{2}' AND B.COMPID = '{0}' AND A.COMPID = B.COMPID
GROUP BY   B.COMPID,B.DEBITCD,LASTDT, UPPER(CONVERT(varchar(3),LASTDT,7))+'-'+CONVERT(varchar(2),LASTDT,2), A.DEBITAMT, A.CREDITAMT",
                companyID, debitcd, currentTime, CurrentYear);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            var list1 = ds.AsEnumerable().Select(x => x.ItemArray.ToList()).ToList();

            return this.Json(list1, JsonRequestBehavior.AllowGet);
        }


    }
}
