using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AS_Therapy_GL.Models;
using Microsoft.Ajax.Utilities;

namespace AS_Therapy_GL.Controllers
{
    public class PST_PatientController : AppController
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

        public PST_PatientController()
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





        public ASL_LOG aslLog = new ASL_LOG();

        // SAVE ALL INFORMATION from PST_PATIENT TO Asl_lOG Database Table.
        public void Insert_Refer_LogData(PST_PATIENT pstPatient)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pstPatient.COMPID && n.USERID == pstPatient.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(pstPatient.COMPID);
            aslLog.USERID = pstPatient.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pstPatient.INSIPNO;
            aslLog.LOGLTUDE = pstPatient.INSLTUDE;
            aslLog.TABLEID = "PST_PATIENT";

            string referName = "";
            var findReferName = (from m in db.PST_ReferDbSet where m.COMPID == pstPatient.COMPID && m.REFERID == pstPatient.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + pstPatient.PATIENTIDM + ",\nPatient Name: " + pstPatient.PATIENTNM + ",\nAddress: " + pstPatient.ADDRESS + ",\nGender: " + pstPatient.GENDER + ",\nAge: " + pstPatient.AGE + ",\nMobile No: " + pstPatient.MOBNO1 + ",\nOther Number: " + pstPatient.MOBNO2 + ",\nEmail ID: " + pstPatient.EMAILID + ",\nRefer name: " + referName + ",\nRemarks: " + pstPatient.REMARKS + ".");
            aslLog.USERPC = pstPatient.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }

        // Edit ALL INFORMATION from PST_PATIENT TO Asl_lOG Database Table.
        public void Update_Refer_LogData(PST_PATIENT pstPatient)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pstPatient.COMPID && n.USERID == pstPatient.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(pstPatient.COMPID);
            aslLog.USERID = pstPatient.UPDUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pstPatient.UPDIPNO;
            aslLog.LOGLTUDE = pstPatient.UPDLTUDE;
            aslLog.TABLEID = "PST_PATIENT";

            string referName = "";
            var findReferName = (from m in db.PST_ReferDbSet where m.COMPID == pstPatient.COMPID && m.REFERID == pstPatient.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + pstPatient.PATIENTIDM + ",\nPatient Name: " + pstPatient.PATIENTNM + ",\nAddress: " + pstPatient.ADDRESS + ",\nGender: " + pstPatient.GENDER + ",\nAge: " + pstPatient.AGE + ",\nMobile No: " + pstPatient.MOBNO1 + ",\nOther Number: " + pstPatient.MOBNO2 + ",\nEmail ID: " + pstPatient.EMAILID + ",\nRefer name: " + referName + ",\nRemarks: " + pstPatient.REMARKS + ".");
            aslLog.USERPC = pstPatient.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        // Delete ALL INFORMATION from PST_PATIENT TO Asl_lOG Database Table.
        public void Delete_Refer_LogData(PST_PATIENT pstPatient)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == pstPatient.COMPID && n.USERID == pstPatient.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(pstPatient.COMPID);
            aslLog.USERID = pstPatient.UPDUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = pstPatient.UPDIPNO;
            aslLog.LOGLTUDE = pstPatient.UPDLTUDE;
            aslLog.TABLEID = "PST_PATIENT";
            string referName = "";
            var findReferName = (from m in db.PST_ReferDbSet where m.COMPID == pstPatient.COMPID && m.REFERID == pstPatient.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + pstPatient.PATIENTIDM + ",\nPatient Name: " + pstPatient.PATIENTNM + ",\nAddress: " + pstPatient.ADDRESS + ",\nGender: " + pstPatient.GENDER + ",\nAge: " + pstPatient.AGE + ",\nMobile No: " + pstPatient.MOBNO1 + ",\nOther Number: " + pstPatient.MOBNO2 + ",\nEmail ID: " + pstPatient.EMAILID + ",\nRefer name: " + referName + ",\nRemarks: " + pstPatient.REMARKS + ".");
            aslLog.USERPC = pstPatient.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        public ASL_DELETE AslDelete = new ASL_DELETE();

        // Delete ALL INFORMATION from PST_PATIENT TO ASL_DELETE Database Table.
        public void Delete_Refer_DELETE(PST_PATIENT pstPatient)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("HH:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == pstPatient.COMPID && n.USERID == pstPatient.UPDUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(pstPatient.COMPID);
            AslDelete.USERID = pstPatient.UPDUSERID;
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = pstPatient.UPDIPNO;
            AslDelete.DELLTUDE = pstPatient.UPDLTUDE;
            AslDelete.TABLEID = "PST_PATIENT";

            string referName = "";
            var findReferName = (from m in db.PST_ReferDbSet where m.COMPID == pstPatient.COMPID && m.REFERID == pstPatient.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            AslDelete.DELDATA = Convert.ToString("Patient ID: " + pstPatient.PATIENTIDM + ",\nPatient Name: " + pstPatient.PATIENTNM + ",\nAddress: " + pstPatient.ADDRESS + ",\nGender: " + pstPatient.GENDER + ",\nAge: " + pstPatient.AGE + ",\nMobile No: " + pstPatient.MOBNO1 + ",\nOther Number: " + pstPatient.MOBNO2 + ",\nEmail ID: " + pstPatient.EMAILID + ",\nRefer name: " + referName + ",\nRemarks: " + pstPatient.REMARKS + ".");
            AslDelete.USERPC = pstPatient.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);
        }







        //Get 
        public ActionResult Create()
        {
            return View();
        }



        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PST_PATIENT model, string command)
        {
            if (ModelState.IsValid)
            {
                DateTime patientDate = Convert.ToDateTime(model.USERPC);
                model.PATIENTDT = patientDate;

                model.USERPC = strHostName;
                model.INSIPNO = ipAddress.ToString();
                model.INSTIME = Convert.ToDateTime(td);
                model.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);


                string patientYear = Convert.ToString(model.PATIENTYY);
                string Year = Convert.ToString(patientYear.Substring(2, 2));
                Int64 R = Convert.ToInt64(Year + "9999");

                var check_Again_SerialID = (from m in db.PST_PatientDbSet
                                            where
                                                m.COMPID == model.COMPID && m.PATIENTYY == model.PATIENTYY && m.PATIENTID == model.PATIENTID &&
                                                m.PATIENTIDM == model.PATIENTIDM
                                            select m).ToList();

                if (check_Again_SerialID.Count == 0 && model.PATIENTIDM <= R)
                {

                }
                else if (model.PATIENTIDM <= R && check_Again_SerialID.Count != 0)
                {
                    model.PATIENTIDM = model.PATIENTIDM + 1;
                    model.PATIENTID = Convert.ToInt64(model.COMPID + "1" + model.PATIENTIDM);
                }
                else
                {
                    TempData["PatientCreationMessage"] = "Not possible entry! ";
                    return RedirectToAction("Create");
                }


                Insert_Refer_LogData(model);
                db.PST_PatientDbSet.Add(model);
                db.SaveChanges();


                string patientID = model.PATIENTID.ToString();
                Int64 substring_PatientId = Convert.ToInt64(patientID.Substring(0, 6));
                var check_GLAcchartMst = (from m in db.GlAccharmstDbSet
                                          where m.COMPID == model.COMPID && m.HEADTP == 1 && m.HEADCD == substring_PatientId
                                          select m).ToList();
                if (check_GLAcchartMst.Count == 0)
                {
                    GL_ACCHARMST aGlAccharmst = new GL_ACCHARMST();
                    aGlAccharmst.COMPID = model.COMPID;
                    aGlAccharmst.HEADTP = 1;
                    aGlAccharmst.HEADCD = Convert.ToInt64(substring_PatientId);
                    aGlAccharmst.HEADNM = Convert.ToString("Patient-" + model.PATIENTYY);
                    aGlAccharmst.USERPC = strHostName;
                    aGlAccharmst.INSIPNO = ipAddress.ToString();
                    aGlAccharmst.INSTIME = Convert.ToDateTime(td);
                    aGlAccharmst.INSLTUDE = model.INSLTUDE;
                    aGlAccharmst.INSUSERID = model.INSUSERID;

                    db.GlAccharmstDbSet.Add(aGlAccharmst);
                    db.SaveChanges();
                }


                GL_ACCHART glAcchart = new GL_ACCHART();
                glAcchart.COMPID = model.COMPID;
                glAcchart.HEADTP = 1;
                glAcchart.HEADCD = Convert.ToInt64(substring_PatientId);
                glAcchart.ACCOUNTCD = model.PATIENTID;
                glAcchart.ACCOUNTNM = Convert.ToString(model.PATIENTIDM + "-" + model.PATIENTNM);
                glAcchart.USERPC = strHostName;
                glAcchart.INSIPNO = ipAddress.ToString();
                glAcchart.INSTIME = Convert.ToDateTime(td);
                glAcchart.INSLTUDE = model.INSLTUDE;
                glAcchart.INSUSERID = model.INSUSERID;

                db.GlAcchartDbSet.Add(glAcchart);
                db.SaveChanges();

                if (command == "Save")
                {
                    TempData["PatientCreationMessage"] = "Patient Name: '" + model.PATIENTNM + "' successfully registered! ";
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["Patient_To_Sale"] = model;
                    //return RedirectToAction("Index", "Sale", TempData["Patient_To_Sale"]);
                    return RedirectToAction("Index", "Sale");
                }


            }
            return RedirectToAction("Create");
        }








        //Get
        public ActionResult Update()
        {
            return View();
        }


        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PST_PATIENT model)
        {
            var result_Find = (from n in db.PST_PatientDbSet
                               where model.COMPID == n.COMPID && n.PATIENTYY == model.PATIENTYY && n.PATIENTIDM == model.PATIENTIDM
                               select n).Count();

            if (result_Find != 0)
            {
                db.Entry(model).State = EntityState.Modified;

                model.USERPC = strHostName;
                model.UPDIPNO = ipAddress.ToString();
                model.UPDTIME = Convert.ToDateTime(td);
                model.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                Update_Refer_LogData(model);
                db.SaveChanges();


                //Gl_AcChart field Update
                var searchGL_AcChart = (from m in db.GlAcchartDbSet where m.COMPID == model.COMPID && m.HEADTP == 1 && m.ACCOUNTCD == model.PATIENTID select m).ToList();
                if (searchGL_AcChart.Count != 0)
                {
                    foreach (GL_ACCHART glAcchart in searchGL_AcChart)
                    {
                        glAcchart.ACCOUNTNM = Convert.ToString(model.PATIENTIDM + "-" + model.PATIENTNM);
                        glAcchart.USERPC = strHostName;
                        glAcchart.UPDIPNO = ipAddress.ToString();
                        glAcchart.UPDTIME = Convert.ToDateTime(td);
                        glAcchart.UPDLTUDE = model.UPDLTUDE;
                        glAcchart.UPDUSERID = model.UPDUSERID;

                    }
                    db.SaveChanges();
                }



                TempData["PatientUpdateMessage"] = "Patient Name: '" + model.PATIENTNM + "' successfully updated!";
                return RedirectToAction("Update");
            }
            else
            {
                TempData["selectPatientID"] = "Please select valid Patient ID first !";
                return RedirectToAction("Update");
            }
        }



        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPatientInformation(Int64 changedtxt)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String patientDate = "", patientNm = "", address = "", mobileNo1 = "", mobileNo2 = "", emailId = "", remarks = "", inserttime = "", insertIpno = "", insltude = "", Gender = "", referName = "";
            Int64 Pst_Patient_Id = 0, companyID = 0, insertUserId = 0, ReferID = 0, patientID = 0, age = 0, patientYear = 0;


            var rt = db.PST_PatientDbSet.Where(n => n.PATIENTIDM == changedtxt &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             n.pst_Patient_Id,
                                                             n.COMPID,
                                                             n.PATIENTID,
                                                             n.PATIENTDT,
                                                             n.PATIENTYY,
                                                             n.PATIENTNM,
                                                             n.ADDRESS,
                                                             n.GENDER,
                                                             n.AGE,
                                                             n.MOBNO1,
                                                             n.MOBNO2,
                                                             n.EMAILID,
                                                             n.REFERID,
                                                             n.REMARKS,
                                                             insuserid = n.INSUSERID,
                                                             instime = n.INSTIME,
                                                             insipno = n.INSIPNO,
                                                             insltude = n.INSLTUDE

                                                         });
            foreach (var n in rt)
            {
                Pst_Patient_Id = n.pst_Patient_Id;
                companyID = Convert.ToInt64(n.COMPID);
                patientID = Convert.ToInt64(n.PATIENTID);
                patientDate = Convert.ToString(n.PATIENTDT);
                patientYear = Convert.ToInt64(n.PATIENTYY);
                patientNm = n.PATIENTNM;
                address = n.ADDRESS;
                Gender = n.GENDER;
                age = n.AGE;
                mobileNo1 = n.MOBNO1;
                mobileNo2 = n.MOBNO2;
                emailId = n.EMAILID;
                ReferID = Convert.ToInt64(n.REFERID);
                remarks = n.REMARKS;

                insertUserId = n.insuserid;
                inserttime = Convert.ToString(n.instime);
                insertIpno = Convert.ToString(n.insipno);
                insltude = Convert.ToString(n.insltude);
            }


            var find_Refername = db.PST_ReferDbSet.Where(n => n.REFERID == ReferID &&
                                                                n.COMPID == compid).Select(n => new { n.REFERNM });
            foreach (var m in find_Refername)
            {
                referName = m.REFERNM;
            }

            var result = new
            {
                pst_Patient_Id = Pst_Patient_Id,
                COMPID = companyID,
                PATIENTID = patientID,
                PATIENTDT = patientDate,
                PATIENTYY = patientYear,
                PATIENTNM = patientNm,
                ADDRESS = address,
                GENDER = Gender,
                AGE = age,
                MOBNO1 = mobileNo1,
                MOBNO2 = mobileNo2,
                EMAILID = emailId,
                REFERID = ReferID,
                REFERNM = referName,
                REMARKS = remarks,
                INSUSERID = insertUserId,
                INSTIME = inserttime,
                INSIPNO = insertIpno,
                INSLTUDE = insltude
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }








        //AutoComplete
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




        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
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

















        //Get
        public ActionResult Delete()
        {
            return View();
        }



        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PST_PATIENT model)
        {
            var result_Find = (from n in db.PST_PatientDbSet
                               where model.COMPID == n.COMPID && n.pst_Patient_Id == model.pst_Patient_Id
                               select n).Count();
            if (result_Find != 0)
            {
                PST_PATIENT deleteModel = db.PST_PatientDbSet.Find(model.pst_Patient_Id);

                // Check patient ID in PST_Trans Table
                var check_PST_Trans =
                    (from m in db.PST_TransDbSet
                     where m.COMPID == deleteModel.COMPID && m.PATIENTID == deleteModel.PATIENTID
                     select m).ToList();
                if (check_PST_Trans.Count == 0)
                {
                    deleteModel.USERPC = strHostName;
                    deleteModel.UPDIPNO = ipAddress.ToString();
                    deleteModel.UPDTIME = Convert.ToDateTime(td);
                    deleteModel.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    deleteModel.UPDLTUDE = model.UPDLTUDE;

                    Delete_Refer_DELETE(deleteModel);
                    Delete_Refer_LogData(deleteModel);

                    db.PST_PatientDbSet.Remove(deleteModel);
                    db.SaveChanges();



                    //Gl_AcChart field Update
                    var searchGL_AcChart =
                        (from m in db.GlAcchartDbSet
                         where m.COMPID == model.COMPID && m.HEADTP == 1 && m.ACCOUNTCD == model.PATIENTID
                         select m).ToList();
                    if (searchGL_AcChart.Count != 0)
                    {
                        foreach (GL_ACCHART glAcchart in searchGL_AcChart)
                        {
                            db.GlAcchartDbSet.Remove(glAcchart);
                        }
                        db.SaveChanges();
                    }

                    TempData["PatientDeleteMessage"] = "Patient Name: '" + model.PATIENTNM + "' successfully deleted!";
                    return RedirectToAction("Delete");
                }
                else
                {
                    TempData["PatientDeleteMessage"] = "This Patient also inputed in billing form. Delete not successfully !";
                    return RedirectToAction("Delete");
                }

            }
            else
            {
                TempData["selectPatientID"] = "Please select valid Patient ID first !";
                return RedirectToAction("Delete");

            }
        }





        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}
