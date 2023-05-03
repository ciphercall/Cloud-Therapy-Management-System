using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models
{
    public class ASL_DELETE
    {
        [Key]
        public Int64 Asl_DeleteID { get; set; }
       
        public Int64? COMPID { get; set; }
        public Int64? USERID { get; set; }
        public Int64? DELSLNO { get; set; }
        public string DELDATE { get; set; }
        public string DELTIME { get; set; }
        public string DELIPNO { get; set; }
        public string DELLTUDE { get; set; }
        public string TABLEID { get; set; }
        public string DELDATA { get; set; }
        public string USERPC { get; set; }
    }
}