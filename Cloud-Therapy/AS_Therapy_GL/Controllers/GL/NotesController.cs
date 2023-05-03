using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class NotesController : AppController
    {
        Therapy_GL_DbContext db = new Therapy_GL_DbContext();

        public NotesController()
        {
            ViewData["HighLight_Menu_AccountReports"] = "High Light Menu";
        }






        // GET: /Notes/
        public ActionResult Notes1Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Notes1Index(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["liabilities"] = model;
            return RedirectToAction("Liabilities1Report");
        }
        public ActionResult Liabilities1Report()
        {
            PageModel model = (PageModel)TempData["liabilities"];
            return View(model);
        }





        public ActionResult Notes2Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Notes2Index(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["liabilities2"] = model;
            return RedirectToAction("Liabilities2Report");
        }
        public ActionResult Liabilities2Report()
        {
            PageModel model = (PageModel)TempData["liabilities2"];
            return View(model);
        }






        public JsonResult TagSearch(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.GlAccharmstDbSet
                       where p.COMPID == compid
                       select p.HEADNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);

        }




        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());


            String itemId = "";

            var rt = db.GlAccharmstDbSet.Where(n => n.HEADNM.StartsWith(changedText) &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             headname = n.HEADNM

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
                        string ss = Convert.ToString(n.headname);
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

            var rt2 = db.GlAccharmstDbSet.Where(n => n.HEADNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             headcd = n.HEADCD,

                                                         });
            foreach (var n in rt2)
            {
                itemId2 = Convert.ToString(n.headcd);

            }

            var result = new { headname = itemId, headcd = itemId2 };
            return Json(result, JsonRequestBehavior.AllowGet);

        }





        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
