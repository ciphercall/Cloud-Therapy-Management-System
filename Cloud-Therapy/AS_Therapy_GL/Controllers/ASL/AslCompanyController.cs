using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class AslCompanyController : Controller
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();


        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        public AslCompanyController()
        {
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        //
        // GET: /AslCompany/

        public ActionResult Index()
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            return View(db.AslCompanyDbSet.ToList());
        }

       
        // GET: /AslCompany/Details/5
        public ActionResult Details(short id = 0)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            AslCompany aslcompany = db.AslCompanyDbSet.Find(id);
            if (aslcompany == null)
            {
                return HttpNotFound();
            }
            return View(aslcompany);
        }

        

        // GET: /AslCompany/Create
        public ActionResult Create()
        {
            ViewData["HighLight_Menu_InputForm"] = "Heigh Light Menu";
            return View();
        }


        
        // POST: /AslCompany/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AslCompany aslcompany)
        {
            if (ModelState.IsValid)
            {
                AslUserco aslUserco = new AslUserco();
                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];


                aslcompany.USERPC = strHostName;
                aslcompany.INSIPNO = ipAddress.ToString();
                aslcompany.INSTIME = Convert.ToDateTime(td);

                var id = Convert.ToString(db.AslCompanyDbSet.Max(s => s.COMPID));

                if (id == "")
                {
                    aslcompany.COMPID = 101;

                    aslUserco.COMPID = aslcompany.COMPID;
                    aslUserco.ADDRESS = aslcompany.ADDRESS;
                    aslUserco.MOBNO = aslcompany.CONTACTNO;
                    aslUserco.EMAILID = aslcompany.EMAILID;
                    aslUserco.LOGINPW = aslcompany.COMPNM.Substring(0, 2) + "asl" + "123%";
                    TempData["User"] = aslUserco;//pass the object(reference) to ' TepmData["User"] ' method and also pass TepmData["User"] value to "AslUserco" Create.Cshtml.(this is working same as session)
                    TempData["companyName"] = aslcompany.COMPNM.ToString();// pass this  TempData["COMPNM"] value to "AslUserco" Create.Cshtml.
                    aslcompany.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);//Log in Admin(Super Admin) userID save AslCompany attribute (INSUSERID) filed

                    db.AslCompanyDbSet.Add(aslcompany);
                    if (db.SaveChanges() > 0)
                    {
                        ViewBag.Message = "'" + aslcompany.COMPNM +
                                          "' successfully registered ";
                    }


                    // Auto Insert in GL_ACCHARMST table
                    GL_ACCHARMST aGlAccharmst = new GL_ACCHARMST();
                    for (int count = 1; count < 4; count++)
                    {
                        aGlAccharmst.COMPID = aslcompany.COMPID;
                        aGlAccharmst.HEADTP = 1;
                        if (count == 1)
                        {
                            aGlAccharmst.HEADCD = Convert.ToInt64(aslcompany.COMPID + "10" + "1");
                            aGlAccharmst.HEADNM = "CASH";
                        }
                        else if (count == 2)
                        {
                            aGlAccharmst.HEADCD = Convert.ToInt64(aslcompany.COMPID + "10" + "2");
                            aGlAccharmst.HEADNM = "BANK";
                        }
                        else if (count == 3)
                        {
                            aGlAccharmst.HEADCD = Convert.ToInt64(aslcompany.COMPID + "10" + "3");
                            aGlAccharmst.HEADNM = "PARTY";
                        }
                        aGlAccharmst.USERPC = strHostName;
                        aGlAccharmst.INSIPNO = ipAddress.ToString();
                        aGlAccharmst.INSTIME = Convert.ToDateTime(td);
                        aGlAccharmst.INSLTUDE = aslcompany.INSLTUDE;
                        aGlAccharmst.INSUSERID = aslcompany.INSUSERID;
                        db.GlAccharmstDbSet.Add(aGlAccharmst);
                        db.SaveChanges();
                    }

                }
                else if (id != null)
                {
                    Int64 nid = Int64.Parse(id);
                    if (nid < 200)
                    {
                        aslcompany.COMPID = nid + 1;

                        aslUserco.COMPID = aslcompany.COMPID;
                        aslUserco.ADDRESS = aslcompany.ADDRESS;
                        aslUserco.MOBNO = aslcompany.CONTACTNO;
                        aslUserco.EMAILID = aslcompany.EMAILID;
                        aslUserco.LOGINPW = aslcompany.COMPNM.Substring(0, 3) + "asl" + "123%";
                        TempData["User"] = aslUserco;//pass the object(reference) to ' TepmData["User"] ' method and also pass TepmData["User"] value to "AslUserco" Create.Cshtml.(this is working same as session)

                        TempData["companyName"] = aslcompany.COMPNM.ToString();  // pass this  TempData["COMPNM"] value to "AslUserco" Create.Cshtml.

                        //Log in Admin(Super Admin) userID save AslCompany attribute (INSUSERID) filed
                        aslcompany.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                        db.AslCompanyDbSet.Add(aslcompany);
                        if (db.SaveChanges() > 0)
                        {
                            ViewBag.Message = "'" + aslcompany.COMPNM +
                                              "' successfully registered! ";
                        }


                        // Auto Insert in GL_ACCHARMST table
                        GL_ACCHARMST aGlAccharmst = new GL_ACCHARMST();
                        for (int count = 1; count < 4; count++)
                        {
                            aGlAccharmst.COMPID = aslcompany.COMPID;
                            aGlAccharmst.HEADTP = 1;
                            if (count == 1)
                            {
                                aGlAccharmst.HEADCD = Convert.ToInt64(aslcompany.COMPID + "10" + "1");
                                aGlAccharmst.HEADNM = "CASH";
                            }
                            else if (count == 2)
                            {
                                aGlAccharmst.HEADCD = Convert.ToInt64(aslcompany.COMPID + "10" + "2");
                                aGlAccharmst.HEADNM = "BANK";
                            }
                            else if (count == 3)
                            {
                                aGlAccharmst.HEADCD = Convert.ToInt64(aslcompany.COMPID + "10" + "3");
                                aGlAccharmst.HEADNM = "PARTY";
                            }
                            aGlAccharmst.USERPC = strHostName;
                            aGlAccharmst.INSIPNO = ipAddress.ToString();
                            aGlAccharmst.INSTIME = Convert.ToDateTime(td);
                            aGlAccharmst.INSLTUDE = aslcompany.INSLTUDE;
                            aGlAccharmst.INSUSERID = aslcompany.INSUSERID;
                            db.GlAccharmstDbSet.Add(aGlAccharmst);
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        ViewBag.Message = " Company entry not possible!";
                        return View(aslcompany);
                    }
                }

                //Send Company Information to Company Admin Mail Address
                MailMessage mail = new MailMessage();
                mail.To.Add(aslcompany.EMAILID);
                mail.From = new MailAddress("admin@alchemy-bd.com");
                mail.Subject = "Mail Confirmation";
                mail.Body = "Alchemy Restaurant Management Online Registration System.\n\n" + Environment.NewLine + "Hi, " + aslcompany.COMPNM + Environment.NewLine + "This Company successfully created our management system. " + Environment.NewLine
                            + "Company Address: " + aslcompany.ADDRESS + Environment.NewLine + "Company Contact No: " + aslcompany.CONTACTNO + Environment.NewLine + "Company EmailID: " + aslcompany.EMAILID + Environment.NewLine
                            + "Company Web ID: " + aslcompany.WEBID + Environment.NewLine + "\nStay with us," + Environment.NewLine + "Alchemy Software";
                mail.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient();
                client.Host = "mail.alchemy-bd.com";
                client.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                client.EnableSsl = false;
                client.Send(mail);

                //return RedirectToAction("Create", "AslUserCo", TempData["User"]);
                return RedirectToAction("Create", "AslUserCo");
            }

            return View(aslcompany);
        }


        //
        // GET: /AslCompany/Edit/5

        public ActionResult Edit(short id = 0)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            AslCompany aslcompany = db.AslCompanyDbSet.Find(id);
            if (aslcompany == null)
            {
                return HttpNotFound();
            }
            return View(aslcompany);
        }

        //
        // POST: /AslCompany/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AslCompany aslcompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aslcompany).State = EntityState.Modified;

                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                aslcompany.USERPC = strHostName;
                aslcompany.UPDIPNO = ipAddress.ToString();
                aslcompany.UPDTIME = Convert.ToDateTime(td);
                aslcompany.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                db.SaveChanges();
                ViewBag.Message = "'" + aslcompany.COMPNM + "' successfully updated ";
                ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
                return View(aslcompany);
            }
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            return View(aslcompany);
        }

        //
        // GET: /AslCompany/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            AslCompany aslcompany = db.AslCompanyDbSet.Find(id);
            if (aslcompany == null)
            {
                return HttpNotFound();
            }
            return View(aslcompany);
        }

        //
        // POST: /AslCompany/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AslCompany aslcompany = db.AslCompanyDbSet.Find(id);

            ////User database list Delete
            //var companAllInfo = (from user in db.AslUsercoDbSet
            //                     from role in db.AslRoleDbSet
            //                     from catagoryName in db.PosItemMstDbSet
            //                     from catagoryItem in db.RmsItemDbSet
            //                     select new { user, role, catagoryName, catagoryItem })
            //    .Where(e => e.user.COMPID == aslcompany.COMPID && e.role.COMPID == aslcompany.COMPID && e.catagoryName.COMPID == aslcompany.COMPID && e.catagoryItem.COMPID == aslcompany.COMPID);



            //foreach (var remove in companAllInfo)
            //{
            //    db.AslUsercoDbSet.Remove(remove.user);
            //    db.AslRoleDbSet.Remove(remove.role);
            //    db.PosItemMstDbSet.Remove(remove.catagoryName);
            //    db.RmsItemDbSet.Remove(remove.catagoryItem);
            //}
            //db.SaveChanges();

            //db.AslCompanyDbSet.Remove(aslcompany);
            //db.SaveChanges();

            return RedirectToAction("Index");
        }


        public JsonResult Check_Compnm(string compnm)
        {
            var result = db.AslCompanyDbSet.Count(d => d.COMPNM == compnm) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Check_ContactNo(string contactNo)
        {
            var result = db.AslCompanyDbSet.Count(d => d.CONTACTNO == contactNo) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_EmailId(string emailId)
        {
            var result = db.AslCompanyDbSet.Count(d => d.EMAILID == emailId) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Check_EmailId_Promotional(string emailIdP)
        {
            var result = db.AslCompanyDbSet.Count(d => d.EMAILIDP == emailIdP) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}