using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models.DTO
{
    public class MailDTO
    {
        public Int64? COMPID { get; set; }
        public Int64? GROUPID { get; set; }
        public string ToEmail { get; set; }
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Subject required!")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body required!")]
        public string Body { get; set; }

        public string Color { get; set; }
        public string Language { get; set; }
        public string CompanyName { get; set; }






        public string USERPC { get; set; }
        public Int64 INSUSERID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64 UPDUSERID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }
    }
}