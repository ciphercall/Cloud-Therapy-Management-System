using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.DataAccess;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers.Calendar
{
    public class SchedulerCalendarController : AppController
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
        Int64 loggedUserid;
        Int64 loggedCompId;

        public SchedulerCalendarController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            ViewData["HighLight_Menu_SchedularCalendar"] = "High Light DashBoard";
            try
            {
                loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                loggedCompId = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            }
            catch (Exception)
            {
                RedirectToAction("Index", "Logout");
            }
          
        }



        public ActionResult Index()
        {
            return View();
        }

        public void UpdateEvent(Int64 id, string NewEventStart, string NewEventEnd)
        {
            DiaryEvent.UpdateDiaryEvent(id, NewEventStart, NewEventEnd,loggedUserid, loggedCompId);
        }


        public bool SaveEvent(string userid, string Title, string Text, string status, string startDate,string startTime, string endDate,string endTime)
        {
            if (userid != "0" && userid != null && userid != "")
            {
                loggedUserid = Convert.ToInt64(userid);
            }
            return DiaryEvent.CreateNewEvent(Title, Text, status, startDate, startTime, endDate, endTime, loggedUserid, loggedCompId);
        }

        public bool Update(Int64 id, string Title, string Text, string status ,string startDate, string startTime, string endDate, string endTime)
        {
            return DiaryEvent.UpdateEvent(id,Title, Text, status, startDate, startTime, endDate, endTime, loggedUserid, loggedCompId);
        }

        public bool Delete(Int64 id, string Title, string Text, string status, string startDate, string startTime, string endDate, string endTime)
        {
            return DiaryEvent.DeleteEvent(id, Title, Text, status, startDate, startTime, endDate, endTime, loggedUserid, loggedCompId);
        }



        public JsonResult GetDiarySummary(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAppointmentSummaryInDateRange(start, end, loggedUserid);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                someKey = e.SomeImportantKeyID,
                                allDay = false,

                                text = e.Text,
                                startDate = e.StartDate,
                                endDate = e.EndDate,
                                status = e.Status,
                                startTime = e.StartTime,
                                endTime = e.EndTime,
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiaryEvents(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAllAppointmentsInDateRange(start, end, loggedUserid);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                color = e.StatusColor,
                                className = e.ClassName,
                                someKey = e.SomeImportantKeyID,
                                allDay = false,

                                text = e.Text,
                                status = e.Status,
                                startDate = e.StartDate,
                                endDate = e.EndDate,
                                startTime = e.StartTime,
                                endTime =e.EndTime,
                              
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
