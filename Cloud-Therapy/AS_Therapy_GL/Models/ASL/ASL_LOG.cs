using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models
{
    public class ASL_LOG
    {
        [Key]
        public Int64 Asl_LOGid { get; set; }
        public Int64? COMPID { get; set; }

        public Int64? USERID { get; set; }
        public string LOGTYPE { get; set; }
        public Int64? LOGSLNO { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LOGDATE { get; set; }
        public string LOGTIME { get; set; }
        public string LOGIPNO { get; set; }
        public string LOGLTUDE { get; set; }
        public string TABLEID { get; set; }
        public string LOGDATA { get; set; }
        public string USERPC { get; set; }
  
    }
}