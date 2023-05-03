using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models.DTO
{
    public class SchedulerCalendarDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String Status { get; set; }
    }
}