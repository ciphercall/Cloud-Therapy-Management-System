using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models.DTO
{
    public class PendingMailSmsDTO
    {
        public Int64? COMPID { get; set; }

        [Required(ErrorMessage = "Body required!")]
        public string TRANSDT { get; set; }
        public string Color { get; set; }

        public Int64 UPDUSERID { get; set; }
        public string UPDLTUDE { get; set; }
    }
}