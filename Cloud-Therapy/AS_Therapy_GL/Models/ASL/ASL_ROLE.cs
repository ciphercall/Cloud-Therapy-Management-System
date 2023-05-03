using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models
{
    public class ASL_ROLE
    {
        [Key]
        public Int64 ASL_ROLEId { get; set; }

        public Int64 COMPID { get; set; }

        [Required(ErrorMessage = "User Name Field can not be empty!")]
        public Int64 USERID { get; set; }

        [Required(ErrorMessage = "Select a valid Module!")]
        public string MODULEID { get; set; }

        [Required(ErrorMessage = "Menu Type field can not be empty!")]
        [Display(Name = "Menu Type")]
        public string MENUTP { get; set; }
        public string MENUID { get; set; }

        public Int64 SERIAL { get; set; }

        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [Display(Name = "Insert")]
        public string INSERTR { get; set; }

        [Display(Name = "Update")]
        public string UPDATER { get; set; }

        [Display(Name = "Delete")]
        public string DELETER { get; set; }


        [Display(Name = "Menu Name")]
        public string MENUNAME { get; set; }

        [Display(Name = "Action Name")]
        public string ACTIONNAME { get; set; }

        [Display(Name = "Controller Name")]
        public string CONTROLLERNAME { get; set; }





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