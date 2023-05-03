using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Controllers;
using AS_Therapy_GL.Models;
using RazorPDF;

namespace AS_Therapy_GL.Controllers
{
    public class ListReportController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();

       
        public ActionResult Get_HeadOfAccounts_List()
        {
            //var pdf = new PdfResult(null, "Get_HeadOfAccounts_List");
            //return pdf;
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
