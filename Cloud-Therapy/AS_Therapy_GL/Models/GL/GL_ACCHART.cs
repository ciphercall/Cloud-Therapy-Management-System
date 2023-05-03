using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models
{
    public class GL_ACCHART
    {
        [Key]
        public Int64 ACCHARTId { get; set; }
        [Display(Name = "Company ID")]
        public Int64? COMPID { get; set; }

        [Display(Name = "Head Type")]
        public int HEADTP { get; set; }

        [Display(Name = "Head CD")]
        public Int64? HEADCD { get; set; }

        [Display(Name = "Account CD")]
        public Int64? ACCOUNTCD { get; set; }

        //[Required(ErrorMessage = "Category name can not be empty!")]
        [Display(Name = "Account Name")]
        public string ACCOUNTNM { get; set; }

        //[Required(ErrorMessage = "Remarks field can not be empty!")]
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }





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