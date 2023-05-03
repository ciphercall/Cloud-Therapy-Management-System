using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AS_Therapy_GL.Models
{
    public class AslCompany
    {
        [Key]
        public Int64 AslCompanyId { get; set; }

        [Display(Name = "ID")]
        public Int64? COMPID { get; set; }

        [Required(ErrorMessage = "Company Name can not be empty!")]
        [Remote("Check_Compnm", "AslCompany", ErrorMessage = "Company Name already exists")]
        [Display(Name = "Company Name")]
        public string COMPNM { get; set; }

        [Required(ErrorMessage = "Company ADDRESS can not be empty!")]
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }

        [Display(Name = "Address2")]
        public string ADDRESS2 { get; set; }



        [Required(ErrorMessage = "Contact No can not be empty!")]
        [Remote("Check_ContactNo", "AslCompany", ErrorMessage = "Company Contact No already exists")]
        [Display(Name = "Contact No")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone number like 8801711001100")]
        public string CONTACTNO { get; set; }

        [Required(ErrorMessage = "Email address can not be empty!")]
        [Remote("Check_EmailId", "AslCompany", ErrorMessage = "Company EMail ID already exists")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email ADDRESS")]
        public string EMAILID { get; set; }

        [Display(Name = "Webpage ID")]
        public string WEBID { get; set; }

        [Required(ErrorMessage = "STATUS can not be empty!")]
        public string STATUS { get; set; }





        [Remote("Check_EmailId_Promotional", "AslCompany", ErrorMessage = "Company EMail ID already exists")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email ADDRESS")]
        public string EMAILIDP { get; set; }

        [DataType(DataType.Password)]
        public string EMAILPWP { get; set; }
        public string SMSIDP { get; set; }

        [DataType(DataType.Password)]
        public string SMSPWP { get; set; }





        [Display(Name = "User PC")]
        public string USERPC { get; set; }
        public Int64? INSUSERID { get; set; }

        [Display(Name = "Insert Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }

        [Display(Name = "Inesrt IP ADDRESS")]
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64? UPDUSERID { get; set; }

        [Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }

        [Display(Name = "Update IP ADDRESS")]
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }


    }
}