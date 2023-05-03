using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.Web.Mvc.CompareAttribute;

namespace AS_Therapy_GL.Models
{
    public class Password
    {
        [Required(ErrorMessage = "Enter Old Password!")]
        [Remote("Check_Password", "Password", ErrorMessage = "Your old password does not match!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password!")]
        [DataType(DataType.Password)]
         public string NewPassword { get; set; }

        [Required(ErrorMessage = "Enter Confirmed Password!")]
        [Compare("NewPassword", ErrorMessage = "The New password and confirmation password do not match!")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
        
        public AslUserco AslUsercoPasswordCheckModel { get; set; }
    }
}