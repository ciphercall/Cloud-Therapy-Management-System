using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class StatementController : AppController
    {

        public StatementController()
        {
            ViewData["HighLight_Menu_AccountReports"] = "High Light Menu";
        }




        // GET: /Statement/
        public ActionResult RPDetailsIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RPDetailsIndex(PageModel model)
        {
            TempData["Details"] = model;
            return RedirectToAction("StatementDetailsReport");
        }

        public ActionResult StatementDetailsReport()
        {
            PageModel model = (PageModel)TempData["Details"];
            return View(model);
        }



        public ActionResult RPSummaryIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RPSummaryIndex(PageModel model)
        {
            TempData["Summary"] = model;
            return RedirectToAction("StatementSummaryReport");
        }

        public ActionResult StatementSummaryReport()
        {
            PageModel model = (PageModel)TempData["Summary"];
            return View(model);
        }

    }
}
