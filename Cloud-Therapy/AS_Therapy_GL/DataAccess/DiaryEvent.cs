using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AS_Therapy_GL.Models;
using AS_Therapy_GL.Models.ASL;

namespace AS_Therapy_GL.DataAccess
{
    public class DiaryEvent
    {
        public Int64 ID;
        public string Title;
        public int SomeImportantKeyID;
        public string StartDateString;
        public string EndDateString;
        public string StatusString;
        public string StatusColor;
        public string ClassName;

        public string Text;
        public string StartDate;
        public string StartTime;
        public string EndDate;
        public string EndTime;
        public string Status;

        public static List<DiaryEvent> LoadAllAppointmentsInDateRange(double start, double end, Int64 loggedUserid)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
            using (Therapy_GL_DbContext db = new Therapy_GL_DbContext())
            {
                List<DiaryEvent> result = new List<DiaryEvent>();

                var rslt = (from m in db.SchedularCalendarDbSet
                            where m.USERID == loggedUserid && m.StartDate >= fromDate && m.EndDate <= toDate
                            select m).ToList();


                int statusEnum = 0, id = 1;
                int SomeImportantKeyID = 0;
                //string rgDate = "";
                foreach (var item in rslt)
                {
                    DiaryEvent rec = new DiaryEvent();
                    rec.ID = item.Id;
                    rec.SomeImportantKeyID = SomeImportantKeyID;
                    SomeImportantKeyID++;
                    id++;

                    rec.StartDateString = string.Format("{0:yyyy-MM-ddThh:mm}", item.StartDate);
                    // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                    rec.EndDateString = string.Format("{0:yyyy-MM-ddThh:mm}", item.EndDate);
                    // field AppointmentLength is in minutes

                    rec.Title = item.Title;
                    rec.Text = item.Text;
                    rec.Status = item.Status;
                    
                    rec.EndDate = string.Format("{0:dd/MM/yyyy}", item.EndDate);
                    DateTime endDate = Convert.ToDateTime(item.EndDate);
                    String convertTimeFromEndDate = endDate.ToString("HH:mm");
                    rec.EndTime = convertTimeFromEndDate;


                    rec.StartDate = string.Format("{0:dd/MM/yyyy}", item.StartDate); 
                    DateTime startDate = Convert.ToDateTime(item.StartDate);
                    String convertTimeFromStartDate = startDate.ToString("HH:mm");
                    rec.StartTime = convertTimeFromStartDate;

                    if (item.Status == "Active")
                    {
                        statusEnum = 0;
                    }
                    else if (item.Status == "Inactive")
                    {
                        statusEnum = 2;
                    }
                    else if (item.Status == "Completed")
                    {
                        statusEnum = 1;
                    }

                    rec.StatusString =
                        SharedCalander.Enums.GetName<SharedCalander.AppointmentStatus>(
                            (SharedCalander.AppointmentStatus)statusEnum);
                    rec.StatusColor =
                        SharedCalander.Enums.GetEnumDescription<SharedCalander.AppointmentStatus>(rec.StatusString);
                    string ColorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":"));
                    rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":") + 1,
                        rec.StatusColor.Length - ColorCode.Length - 1);
                    rec.StatusColor = ColorCode;
                    result.Add(rec);
                }

                return result;
            }

        }


        public static List<DiaryEvent> LoadAppointmentSummaryInDateRange(double start, double end, Int64 loggedUserid)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
            using (Therapy_GL_DbContext db = new Therapy_GL_DbContext())
            {
                List<DiaryEvent> res = new List<DiaryEvent>();

                var rslt = (from reg in db.SchedularCalendarDbSet
                            where reg.USERID == loggedUserid && reg.StartDate >= fromDate && reg.EndDate <= toDate
                            select reg).ToList();
                int i = 0;
                string rgDate = "";
                foreach (var item in rslt)
                {
                    DiaryEvent rec = new DiaryEvent();
                    rec.ID = item.Id; //we dont link this back to anything as its a group summary but the fullcalendar needs unique IDs for each event item (unless its a repeating event)
                    rec.SomeImportantKeyID = -1;
                    string StartDate = string.Format("{0:yyyy-MM-dd}", item.StartDate);
                    rec.StartDateString = StartDate + "T00:00:00"; //ISO 8601 format
                    string EndDate = string.Format("{0:yyyy-MM-dd}", item.EndDate);
                    rec.EndDateString = EndDate + "T23:59:59";
                    //rec.Title = "Booked";

                    rec.Title = item.Title;
                    rec.Text = item.Text;
                    rec.Status = item.Status;

                    rec.EndDate = string.Format("{0:dd/MM/yyyy}", item.EndDate);
                    DateTime endDate = Convert.ToDateTime(item.EndDate);
                    String convertTimeFromEndDate = endDate.ToString("HH:mm");
                    rec.EndTime = convertTimeFromEndDate;


                    rec.StartDate = string.Format("{0:dd/MM/yyyy}", item.StartDate);
                    DateTime startDate = Convert.ToDateTime(item.StartDate);
                    String convertTimeFromStartDate = startDate.ToString("HH:mm");
                    rec.StartTime = convertTimeFromStartDate;

                    res.Add(rec);
                    i++;
                }

                return res;
            }

        }

        public static void UpdateDiaryEvent(Int64 id, string NewEventStart, string NewEventEnd, Int64 loggedUserid, Int64 loggedCompId)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            using (Therapy_GL_DbContext db = new Therapy_GL_DbContext())
            {
                var rec = db.SchedularCalendarDbSet.FirstOrDefault(s => s.Id == id && s.USERID == loggedUserid && s.COMPID == loggedCompId);
                if (rec != null)
                {
                    DateTime DateTimeStart = DateTime.Parse(NewEventStart, null, DateTimeStyles.RoundtripKind).ToLocalTime(); // and convert offset to localtime
                    rec.StartDate = DateTimeStart;
                    if (!String.IsNullOrEmpty(NewEventEnd))
                    {
                        //TimeSpan span = DateTime.Parse(NewEventEnd, null, DateTimeStyles.RoundtripKind).ToLocalTime() - DateTimeStart;
                        //rec.AppointmentLength = Convert.ToInt32(span.TotalMinutes);
                        DateTime DateTimeEnd = DateTime.Parse(NewEventEnd, null, DateTimeStyles.RoundtripKind).ToLocalTime();
                        rec.EndDate = DateTimeEnd;
                    }
                    else
                    {
                        String endDate = DateTimeStart.ToString("dd/MM/yyyy");
                        String endTime = "23:59";
                        rec.EndDate = DateTime.ParseExact(endDate + " " + endTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);                        
                    }
                    db.SaveChanges();
                }
            }

        }



        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }



        public static bool CreateNewEvent(string Title, string Text, string status, string startDate, string startTime, string endDate, string endTime, Int64 loggedUserid, Int64 loggedCompId)
        {
            if (Title == null || Title == "")
            {
                return false;
            }
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            try
            {
                Therapy_GL_DbContext db = new Therapy_GL_DbContext();
                ASL_SchedularCalendar rec = new ASL_SchedularCalendar();
                rec.COMPID = loggedCompId;
                rec.USERID = loggedUserid;
                rec.Title = Title;
                rec.Text = Text;
                if (startTime == null)
                {
                    startTime = "00:01";
                }
                if (endTime == null)
                {
                    endTime = "23:59";
                }
                if (startDate != null)
                {
                    rec.StartDate = DateTime.ParseExact(startDate + " " + startTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                else
                {
                    rec.StartDate = currentDateTime;
                }
                if (endDate != null)
                {
                    rec.EndDate = DateTime.ParseExact(endDate + " " + endTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture); 
                }
                else
                {
                    rec.EndDate = currentDateTime;
                }

                rec.Status = status;
                //rec.DateTimeScheduled = DateTime.ParseExact(NewEventDate + " " + NewEventTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                //rec.AppointmentLength = Int32.Parse(NewEventDuration);
                db.SchedularCalendarDbSet.Add(rec);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }



        public static bool UpdateEvent(Int64 id, string Title, string Text, string status, string startDate, string startTime, string endDate, string endTime, Int64 loggedUserid, Int64 loggedCompId)
        {
            try
            {
                Therapy_GL_DbContext db = new Therapy_GL_DbContext();
                var rec = db.SchedularCalendarDbSet.FirstOrDefault(s => s.Id == id && s.USERID == loggedUserid && s.COMPID == loggedCompId);
                if (rec != null)
                {
                    rec.Title = Title;
                    rec.Text = Text;
                    rec.Status = status;
                    if (startTime == null)
                    {
                        startTime = "00:01";
                    }
                    if (endTime == null)
                    {
                        endTime = "00:01";
                    }
                    if (startDate != null)
                    {
                        rec.StartDate = DateTime.ParseExact(startDate + " " + startTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    if (endDate != null)
                    {
                        rec.EndDate = DateTime.ParseExact(endDate + " " + endTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture); 
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }



        public static bool DeleteEvent(Int64 id, string Title, string Text, string status, string startDate, string startTime, string endDate, string endTime, Int64 loggedUserid, Int64 loggedCompId)
        {
            try
            {
                Therapy_GL_DbContext db = new Therapy_GL_DbContext();
                var rec = db.SchedularCalendarDbSet.FirstOrDefault(s => s.Id == id && s.USERID == loggedUserid && s.COMPID == loggedCompId);
                if (rec != null)
                {
                    db.SchedularCalendarDbSet.Remove(rec);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        

    }

}