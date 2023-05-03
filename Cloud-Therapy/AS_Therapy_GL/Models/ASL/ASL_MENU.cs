using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models
{
    public class ASL_MENU
    {
        [Key]
        public Int64 Id { get; set; }
        public string MODULEID { get; set; }

        [Display(Name = "Menu Type")]
        //[Required(ErrorMessage = "Menu Type can not be empty!")]
        public string MENUTP { get; set; }
        public string MENUID { get; set; }

        [Display(Name = "Menu Name")]
        public string MENUNM { get; set; }

        [Display(Name = "Action Name")]
        public string ACTIONNAME { get; set; }

        [Display(Name = "Controller Name")]
        public string CONTROLLERNAME { get; set; }

        [Display(Name = "Serial")]
        public Int64 SERIAL { get; set; }


        [Display(Name = "User PC")]
        public string USERPC { get; set; }
        public Int64? INSUSERID { get; set; }

        [Display(Name = "Insert Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }

        [Display(Name = "Inesrt IP ADDRESS")]
        public string INSIPNO { get; set; }

        public Int64? UPDUSERID { get; set; }

        [Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }

        [Display(Name = "Update IP ADDRESS")]
        public string UPDIPNO { get; set; }


    }
}