using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models.ASL
{
    [Table("ASL_SchedularCalendar")]
    public class ASL_SchedularCalendar
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //id, text, start_date and end_date properties are mandatory
        public Int64 Id { get; set; }

        [Key, Column(Order = 1)]
        public Int64 COMPID { get; set; }

        [Key, Column(Order = 2)]
        public Int64 USERID { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public String Status { get; set; }
    }
}