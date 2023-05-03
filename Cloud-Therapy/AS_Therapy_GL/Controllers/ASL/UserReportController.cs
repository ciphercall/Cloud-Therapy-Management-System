using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Ajax.Utilities;
using AS_Therapy_GL.Models;
using Newtonsoft.Json;
using RazorPDF;
using WebGrease.Css.Ast;

namespace AS_Therapy_GL.Controllers
{
    public class UserReportController : AppController
    {

        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();


        public UserReportController()
        {
            ViewData["HighLight_Menu_UserReport"] = "High Light Menu";
        }




        // GET: /UserReport/
        public ActionResult GetCompanyUserLogData()
        {
            return View();
        }

        ////Search GetCompanyUserLogData Table, this Log table works partial
        //public PartialViewResult UserLogData(Int64 userID)
        //{
        //    List<ASL_LOG> aslLOG = new List<ASL_LOG>();

        //    aslLOG = db.AslLogDbSet.Where(e => e.USERID == userID).ToList();
        //    return PartialView("~/Views/UserReport/_UserLogData.cshtml", aslLOG);

        //}


        //Post User Report
        [HttpPost]
        public ActionResult GetCompanyUserLogData(UserReportViewModel userReportViewModel)
        {
            //var getUserLogData = new List<ASL_LOG>();
            //getUserLogData = db.AslLogDbSet.Where(e => (e.USERID == userReportViewModel.AslUserco.USERID) && (e.LOGDATE >= userReportViewModel.FromDate && e.LOGDATE <= userReportViewModel.ToDate)).ToList();
            List<UserReportViewModel> getUserLogData = new List<UserReportViewModel>();
            var result = (db.AslLogDbSet.Where(n => n.USERID == userReportViewModel.AslUserco.USERID && n.COMPID == userReportViewModel.AslUserco.COMPID &&
                                                    n.LOGDATE >= userReportViewModel.FromDate &&
                                                    n.LOGDATE <= userReportViewModel.ToDate)
                .Select(n => new { userId = n.USERID, logDate = n.LOGDATE, n.LOGTIME, n.LOGDATA, n.LOGTYPE })).ToList();
            foreach (var VARIABLE in result)
            {
                getUserLogData.Add(new UserReportViewModel() { USERID = VARIABLE.userId, LOGTYPE = VARIABLE.LOGTYPE, LOGDATE = VARIABLE.logDate.Value.ToString("dd-MMM-yyyy"), LOGTIME = VARIABLE.LOGTIME, LOGDATA = VARIABLE.LOGDATA });
            }

            if (userReportViewModel.FromDate > userReportViewModel.ToDate)
            {
                TempData["ErrorFromDateMessage"] = "Plese select correct 'From Date' field!";
                TempData["ErrorToDateMessage"] = "Plese select correct 'To Date' field!";
                return RedirectToAction("GetCompanyUserLogData");
            }
            else if (userReportViewModel.AslUserco.USERNM == null && userReportViewModel.FromDate == null && userReportViewModel.ToDate == null)
            {
                TempData["ErrorFromDateMessage"] = "Plese select correct 'From Date' field!";
                TempData["ErrorToDateMessage"] = "Plese select correct 'To Date' field!";
                TempData["ErrorMessageUserId"] = "Please select the user Id dropdown field!";
                return RedirectToAction("GetCompanyUserLogData");
            }
            else if (userReportViewModel.AslUserco.USERNM == null)
            {
                TempData["ErrorMessageUserId"] = "Please select the user Id dropdown field!";
                return RedirectToAction("GetCompanyUserLogData");
            }

            if (getUserLogData.Count == 0)
            {
                TempData["ErrorMessagetUserLogData"] = "This user Log data empty!";
                return RedirectToAction("GetCompanyUserLogData");
            }
            //var pdf = new PdfResult(getUserLogData, "GetUserLogPdfResult");
            TempData["GetUserLogPdfResult_list"] = getUserLogData;

            TempData["userReportViewModel"] = userReportViewModel;
            return RedirectToAction("GetUserLogPdfResult");
            //return pdf;
        }



        public ActionResult GetUserLogPdfResult()
        {
            var list = (List<UserReportViewModel>)TempData["GetUserLogPdfResult_list"];
            var model = (UserReportViewModel)TempData["userReportViewModel"];

            //From Date
            string FromDate = model.FromDate.ToString();
            DateTime datefrom = DateTime.Parse(FromDate);
            string fromDate = datefrom.ToString("dd-MMM-yyyy");
            //To Date
            string ToDate = model.ToDate.ToString();
            DateTime dateto = DateTime.Parse(ToDate);
            string toDate = dateto.ToString("dd-MMM-yyyy");

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return View(list);

        }



        //Get User Name when Username Dropdownlist Changed.
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UserNameChanged(Int64 changedDropDown)
        {
            Int64 userid = 0;
            string userName = "", operationType = "";
            var rt = db.AslUsercoDbSet.Where(n => n.USERID == changedDropDown).Select(n => new
            {
                UserID = n.USERID,
                UserName = n.USERNM,
                opTp = n.OPTP

            });

            foreach (var n in rt)
            {
                userid = Convert.ToInt64(n.UserID);
                userName = n.UserName;
                operationType = n.opTp;
            }

            var result = new { USERID = userid, USERNM = userName, OPTP = operationType };
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
