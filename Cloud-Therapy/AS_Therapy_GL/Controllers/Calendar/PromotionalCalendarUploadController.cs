using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;
using AS_Therapy_GL.Models.ASL;

namespace AS_Therapy_GL.Controllers.Calendar
{
    public class PromotionalCalendarUploadController : AppController
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

        public PromotionalCalendarUploadController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, ASL_PCalendarImage model)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/P_Calendar_images/"), model.Year + model.Month + "_" + fileName);

                    model.FilePath = "/P_Calendar_images/" + model.Year + model.Month + "_" + fileName;
                    var findPreviousImages =
                        (from m in db.CalendarImageDbSet where m.Year == model.Year && m.Month == model.Month select m)
                            .ToList();
                    if (findPreviousImages.Count != 0)
                    {
                        String oldPath = "";
                        foreach (var getPath in findPreviousImages)
                        {
                            oldPath = getPath.FilePath;
                            getPath.FilePath = model.FilePath;
                        }
                        db.SaveChanges();

                        String oldFullPath = Request.MapPath("~" + oldPath);
                        if (System.IO.File.Exists(oldFullPath))
                        {
                            System.IO.File.Delete(oldFullPath);
                        }
                    }
                    else
                    {
                        db.CalendarImageDbSet.Add(model);
                        db.SaveChanges();
                    }

                    file.SaveAs(path);
                    ViewBag.UploadMessage = "Upload successfully done! ";
                }
            }
            catch
            {
                ViewBag.UploadMessage = "Image file is not in correct Format.";
            }


            return View();
        }



        //Get Data From ASL_PCalendarImage Table
        public PartialViewResult CalendarInfo(Int64 year)
        {
            List<ASL_PCalendarImage> calendarImages = new List<ASL_PCalendarImage>();

            calendarImages = db.CalendarImageDbSet.Where(e => e.Year == year).OrderBy(e=>e.Month).ToList();
            return PartialView("~/Views/PromotionalCalendarUpload/_CalendarImageInfo.cshtml", calendarImages);
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
