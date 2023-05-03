using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AS_Therapy_GL.Models
{
    public class AslUserco
    {
        [Key]
        public Int64 AslUsercoId { get; set; }

        //AslUsercoId
        [Display(Name = "Company ID")]
        public Int64? COMPID { get; set; }

        [Display(Name = "User Reg.No")]
        public Int64? USERID { get; set; }

        [Required(ErrorMessage = "User Name can not be empty!")]
        [Display(Name = "User")]
        public string USERNM { get; set; }

        [Display(Name = "Department")]
        public string DEPTNM { get; set; }

        [Required(ErrorMessage = "Operation Type field can not be empty!")]
        [Display(Name = "Op. Type")]
        public string OPTP { get; set; }


        [Required(ErrorMessage = "User ADDRESS can not be empty!")]
        [Display(Name = "User Address")]
        public string ADDRESS { get; set; }


        [Required(ErrorMessage = "Mobile Number field can not be empty!")]
        [Remote("Check_PhoneNumber", "AslUserCO", ErrorMessage = "User Mobile number already exists!")]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string MOBNO { get; set; }


        //[Required(ErrorMessage = "Email address can not be empty!")]
        [Remote("Check_UserEmail", "AslUserCO", ErrorMessage = "User Email address already exists!")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string EMAILID { get; set; }

        [Required(ErrorMessage = "Login By Email/MobileNO field can not be empty!")]
        [Display(Name = "Login By")]
        public string LOGINBY { get; set; }

        [Display(Name = "Login ID")]
        [Required(ErrorMessage = "Login ID can not be empty!")]
        [Remote("Check_UserLogIn", "AslUserCO", ErrorMessage = "User Login ID already exists!")]
        public string LOGINID { get; set; }

        [Display(Name = "Login Password")]
        [Required(ErrorMessage = "Login Password Field can not be empty!")]
        [DataType(DataType.Password)]
        public string LOGINPW { get; set; }

        [Display(Name = "Start")]
        [Required(ErrorMessage = "Starting Time can not be empty!")]
        public string TIMEFR { get; set; }

        [Display(Name = "End")]
        [Required(ErrorMessage = "Ending Time can not be empty!")]
        public string TIMETO { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "STATUS can not be empty!")]
        public string STATUS { get; set; }




        [Display(Name = "User PC")]
        public string USERPC { get; set; }
        public Int64 INSUSERID { get; set; }

        [Display(Name = "Insert Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }

        [Display(Name = "Inesrt IP ADDRESS")]
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64 UPDUSERID { get; set; }

        [Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }

        [Display(Name = "Update IP ADDRESS")]
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }



    }
}