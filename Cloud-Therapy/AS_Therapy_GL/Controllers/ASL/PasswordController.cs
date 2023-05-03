using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class PasswordController : AppController
    {

        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();

        public ActionResult PasswordChangedForm()
        {
            return View();
        }

        // GET: /Password/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordChangedForm(Password password)
        {

            var aslUserco = from p in db.AslUsercoDbSet
                            where p.COMPID == password.AslUsercoPasswordCheckModel.COMPID
                                  && p.USERID == password.AslUsercoPasswordCheckModel.USERID
                            select p;
           
            foreach (var a in aslUserco)
            {
                //a.COMPID = password.AslUsercoPasswordCheckModel.COMPID;
                a.LOGINPW = password.NewPassword;
                db.Entry(a).State = EntityState.Modified;

                if (a.EMAILID != null)
                {
                    //Send NEW Password to User Mail Address
                    MailMessage mail = new MailMessage();
                    mail.To.Add(a.EMAILID);
                    mail.From = new MailAddress("admin@alchemy-bd.com");
                    mail.Subject = "Mail Confirmation";
                    mail.Body = "Alchemy Restaurant Management System.\n\n" + Environment.NewLine + "Hi, " + a.USERNM + Environment.NewLine
                               + "Your Login ID: " + a.LOGINID + Environment.NewLine + "Your Operation Type: " + a.OPTP + Environment.NewLine
                               + "Your New Password: " + a.LOGINPW + Environment.NewLine + "Your Status: " + a.STATUS + Environment.NewLine
                               + "\nStay with us," + Environment.NewLine + "Alchemy Software";
                    mail.Priority = MailPriority.Normal;

                    SmtpClient client = new SmtpClient();
                    client.Host = "mail.alchemy-bd.com";
                    client.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                    client.EnableSsl = false;
                    client.Send(mail);
                }
               
            }
            db.SaveChanges();
            ViewBag.NewPassword = "Your password successfully updated!";
            return View();

        }


        //Check Old Password 
        public JsonResult Check_Password(string oldpassword)
        {
            Int64 compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 userid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());

            var result = db.AslUsercoDbSet.Count(d => d.LOGINPW == oldpassword && d.COMPID == compid && d.USERID == userid) != 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
