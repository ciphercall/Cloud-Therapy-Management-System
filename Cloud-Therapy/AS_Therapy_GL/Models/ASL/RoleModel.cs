using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models
{
    public class RoleModel
    {
        public RoleModel()
        {
            this.AslMenu = new ASL_MENU();
            this.AslMenumst = new ASL_MENUMST();
            this.AslRole = new ASL_ROLE();
            this.AslUserco = new AslUserco();
            this.AslCompany = new AslCompany();
        }

        public AslCompany AslCompany { get; set; }
        public AslUserco AslUserco { get; set; }
        public ASL_MENUMST AslMenumst { get; set; }
        public ASL_MENU AslMenu { get; set; }
        public ASL_ROLE AslRole { get; set; }



        // Home page Contact Page Model
        public string Name { get; set; }

        [Required(ErrorMessage = "Email address can not be empty!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email ADDRESS")]
        public string Email { get; set; }

        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone number like 8801711001100")]
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}