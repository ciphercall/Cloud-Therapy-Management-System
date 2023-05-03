using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class ReportController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();


        public ReportController()
        {
            ViewData["HighLight_Menu_BillingReport"] = "Heigh Light Menu";
        }




        //Therapy Report (Item Lists)
        public ActionResult GetItemList()
        {
            return View();
        }








        //Therapy Report (Item Ledger)
        public ActionResult ItemLedger()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ItemLedger(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("GetItemLedger");
        }

        public ActionResult GetItemLedger()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }








        //Therapy Report (Closing Stock Details)
        public ActionResult ClosingStock_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClosingStock_Details(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_ClosingStock_Details");
        }

        public ActionResult Get_ClosingStock_Details()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }










        //Therapy Report (Refer Statement Details)
        public ActionResult ReferStatement_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReferStatement_Details(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_ReferStatement_Details");
        }

        public ActionResult Get_ReferStatement_Details()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }












        //Therapy Report (Refer Statement Summarized)
        public ActionResult ReferStatement_Summarized()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReferStatement_Summarized(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_ReferStatement_Summarized");
        }

        public ActionResult Get_ReferStatement_Summarized()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }










        //Therapy Report (Patient Therapy History)
        public ActionResult patient_Therapy_History()
        {
            return View();
        }

        [HttpPost]
        public ActionResult patient_Therapy_History(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_patient_Therapy_History");
        }

        public ActionResult Get_patient_Therapy_History()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }





        //AutoComplete  (Patient Therapy History)
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPatientInformation(Int64 changedtxt)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String patientNm = "", address = "", mobileNo1 = "", mobileNo2 = "", emailId = "", remarks = "", inserttime = "", insertIpno = "", insltude = "", Gender = "", referName = "";
            Int64 Pst_Patient_Id = 0, companyID = 0, insertUserId = 0, ReferID = 0, patientID = 0, age = 0, patientYear = 0;


            var rt = db.PST_PatientDbSet.Where(n => n.PATIENTIDM == changedtxt &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             n.pst_Patient_Id,
                                                             n.COMPID,
                                                             n.PATIENTID,
                                                             n.PATIENTYY,
                                                             n.PATIENTNM,
                                                             n.ADDRESS,
                                                             n.GENDER,
                                                             n.AGE,
                                                             n.MOBNO1,
                                                             n.MOBNO2,
                                                             n.EMAILID,
                                                             n.REFERID,
                                                             n.REMARKS

                                                         });
            foreach (var n in rt)
            {
                patientID = Convert.ToInt64(n.PATIENTID);
                patientNm = n.PATIENTNM;              
            }
            
            var result = new
            {
                PATIENTID = patientID,
                PATIENTNM = patientNm,             
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }








        //AutoComplete (Patient Therapy History)
        public JsonResult TagSearch(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.PST_PatientDbSet
                       where p.COMPID == compid
                       select new { p.PATIENTIDM };
            foreach (var tag in tags)
            {
                result.Add(tag.PATIENTIDM.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }




        //AutoComplete  (Patient Therapy History)
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> patientIDM_List = new List<string>();
            var tags = from p in db.PST_PatientDbSet
                       where p.COMPID == compid
                       select new { p.PATIENTIDM, p.COMPID };
            foreach (var tag in tags)
            {
                patientIDM_List.Add(tag.PATIENTIDM.ToString());
            }

            var rt = patientIDM_List.Where(t => t.StartsWith(changedText)).ToList();

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
                        string ss = Convert.ToString(n);
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

            var result = new { PATIENTIDM = itemId };
            return Json(result, JsonRequestBehavior.AllowGet);

        }



















        //Therapy Report (Sale Statement Details)
        public ActionResult SaleStatement_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaleStatement_Details(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_SaleStatement_Details");
        }

        public ActionResult Get_SaleStatement_Details()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }





        //Therapy Report (Sale Statement Summarized)
        public ActionResult SaleStatement_Summarized()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaleStatement_Summarized(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_SaleStatement_Summarized");
        }

        public ActionResult Get_SaleStatement_Summarized()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }








        //Therapy Report (Sale Purchase Details)
        public ActionResult Sale_Purchase_DETAILS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale_Purchase_DETAILS(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_Sale_Purchase_DETAILS");
        }

        public ActionResult Get_Sale_Purchase_DETAILS()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }









        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Trans_TypeChanged(string txtType)
        {
            List<SelectListItem> getHeadName = new List<SelectListItem>();
            Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var findPartyID = (from n in db.GlAcchartDbSet where n.COMPID == compid select n).ToList();
            Int64 headCD = 0,headCD2=0;
            string HeadCD_Substring = "";
            foreach (var glAcchart in findPartyID)
            {
                headCD = Convert.ToInt64(glAcchart.HEADCD.ToString().Substring(3, 3));

                HeadCD_Substring = Convert.ToString(headCD);
                headCD2 = Convert.ToInt64(HeadCD_Substring.Substring(1, 1));

                if (txtType == "BUY")
                {
                    if (headCD == 203)
                    {
                        getHeadName.Add(new SelectListItem { Text = glAcchart.ACCOUNTNM, Value = glAcchart.ACCOUNTCD.ToString() });
                    }
                }
                else if (txtType == "SALE")
                {
                    if (headCD >= 115 && headCD2 == 1)
                    {
                        getHeadName.Add(new SelectListItem { Text = glAcchart.ACCOUNTNM, Value = glAcchart.ACCOUNTCD.ToString() });
                    }
                }               
            }

            return Json(getHeadName, JsonRequestBehavior.AllowGet);
        }













        //Therapy Report (Sale/Purchase Summary)
        public ActionResult Sale_Purchase_SUMMARY()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale_Purchase_SUMMARY(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_Sale_Purchase_SUMMARY");
        }

        public ActionResult Get_Sale_Purchase_SUMMARY()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }









        //Therapy Report (Sale Purchase Summary-All Head)
        public ActionResult Sale_Purchase_SUMMARY_All_Head()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale_Purchase_SUMMARY_All_Head(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_Sale_Purchase_SUMMARY_All_Head");
        }

        public ActionResult Get_Sale_Purchase_SUMMARY_All_Head()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }








        //Therapy Report (Sale Purchase Summary-All Item)
        public ActionResult Sale_Purchase_SUMMARY_All_Item()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale_Purchase_SUMMARY_All_Item(PageModel model)
        {
            TempData["model"] = model;
            return RedirectToAction("Get_Sale_Purchase_SUMMARY_All_Item");
        }

        public ActionResult Get_Sale_Purchase_SUMMARY_All_Item()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
