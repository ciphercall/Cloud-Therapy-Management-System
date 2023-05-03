using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            Session["HomePage"] = "Show home page";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            Session["HomePage"] = "Show home page";
            return View();
        }


        public ActionResult Faq()
        {
            ViewBag.Message = "Your app description page.";
            Session["HomePage"] = "Show home page";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            Session["HomePage"] = "Show home page";
            return View();
        }


        [HttpPost]
        public ActionResult Contact(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                //Send user email to Alchemy Software email address.
                MailMessage mail = new MailMessage();
                mail.To.Add("info@alchemy-bd.com");
                mail.From = new MailAddress("admin@alchemy-bd.com");
                mail.Subject = "Complain/suggestion/Inquery";
                mail.Body = "Name: " + model.Name + Environment.NewLine + "\nEmail: " + model.Email + Environment.NewLine + "\nPhone:" + model.Phone + Environment.NewLine + "\n\n" + Environment.NewLine + model.Message;
                mail.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient();
                client.Host = "mail.alchemy-bd.com";
                client.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                client.EnableSsl = false;
                client.Send(mail);

                return RedirectToAction("Contact");
            }

            Session["HomePage"] = "Show home page";
            return View("Contact");
        }




    }
}
