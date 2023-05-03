using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class RegisterController : AppController
    {

        public RegisterController()
        {
            ViewData["HighLight_Menu_AccountReports"] = "High Light Menu";
        }




        // GET: /Register/
        public ActionResult ChequeRegisterIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChequeRegisterIndex(PageModel model)
        {


            TempData["ChequeRegister"] = model;
            return RedirectToAction("ChequeRegisterReport");
        }
        public ActionResult ChequeRegisterReport()
        {
            PageModel model = (PageModel)TempData["ChequeRegister"];
            return View(model);
        }



        public ActionResult JournalRegisterIndex()
        {
            return View();
        }

    }
}
