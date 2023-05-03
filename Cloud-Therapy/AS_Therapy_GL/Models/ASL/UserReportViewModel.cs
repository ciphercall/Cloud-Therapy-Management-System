using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Models
{
    public class UserReportViewModel
    {
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public AslUserco AslUserco { get; set; }

        //..........For Use only (GetUserLogPdfResult) passing List Model, before add this model(create list) from AslLOG table. 
        public Int64? USERID { get; set; }
        public Int64? COMPID { get; set; }
        public string LOGTYPE { get; set; }
        public string LOGDATE { get; set; }
        public string LOGTIME { get; set; }
        public string LOGDATA { get; set; }
    }
}