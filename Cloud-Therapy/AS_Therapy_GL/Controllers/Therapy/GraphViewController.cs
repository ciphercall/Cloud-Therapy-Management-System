using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class GraphViewController : AppController
    {
        public GraphViewController()
        {
            ViewData["HighLight_Menu_DashBoard"] = "High Light DashBoard";
        }

        // GET: /GraphView/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexPost(Int64 cid, Int64 patientID, Int64 patient_idm)
        {
            PageModel model= new PageModel();
            model.Pst_Patient.COMPID = cid;
            model.Pst_Patient.PATIENTID = patientID;
            model.Pst_Patient.PATIENTIDM = patient_idm;
            TempData["model"] = model;
            return RedirectToAction("Get_patient_Therapy_History", "Report", TempData["model"]);
        }

    }
}
