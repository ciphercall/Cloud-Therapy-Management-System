using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class BalanceSheetController : AppController
    {


        public BalanceSheetController()
        {
            ViewData["HighLight_Menu_AccountReports"] = "High Light Menu";
        }



        // GET: /BalanceSheet/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PageModel model, string command)
        {
            if (command == "Show")
            {
                string date = Convert.ToString(model.ToDate);
                DateTime myDateTime = DateTime.Parse(date);
                string converttoString = myDateTime.ToString("dd-MMM-yyyy");
                TempData["BalanceSheet_Date"] = converttoString;
                TempData["BalanceSheet_Model"] = model;
                return RedirectToAction("Index");
            }
            else //if (command == "Print")
            {
                TempData["BalanceSheet"] = model;
                return RedirectToAction("BalanceSheetReport");
            }
        }

        public ActionResult BalanceSheetReport()
        {

            PageModel model = (PageModel)TempData["BalanceSheet"];
            return View(model);
        }

    }
}
