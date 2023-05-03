using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AS_Therapy_GL.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name can not be empty!")]
        public string LOGINID { get; set; }

        [Required(ErrorMessage = "Password can not be empty!")]
        public string LOGINPW { get; set; }

        public string DEPTNM { get; set; }
        public string OPTP { get; set; }
        public Int64? COMPID { get; set; }
        public Int64? USERID { get; set; }


        public string LOGTIME { get; set; }
        public string LOGIPNO { get; set; }
        public string LOGLTUDE { get; set; }
        public string LOGDATA { get; set; }
        public string USERPC { get; set; }

    }
}
