using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace AS_Therapy_GL.Models
{
    public class PageModel
    {

        public PageModel()
        {
            this.aslMenumst = new ASL_MENUMST();
            this.aslMenu = new ASL_MENU();
            this.aslUserco = new AslUserco();
            this.aslCompany = new AslCompany();
            this.aslLog = new ASL_LOG();
            this.AGL_accharmst = new GL_ACCHARMST();
            this.AGL_acchart = new GL_ACCHART();


            this.AGlStrans = new GL_STRANS();
            this.AGlMaster=new GL_MASTER();

            this.PST_Itemmst = new PST_ITEMMST();
            this.PST_Item = new PST_ITEM();
            this.pst_Trans =new PST_TRANS();
            this.Pst_Transmst =new PST_TRANSMST();
            this.Pst_Patient =new PST_PATIENT();
            this.Pst_Refer =new PST_REFER();
            

        }

        public ASL_MENUMST aslMenumst { get; set; }
        public ASL_MENU aslMenu { get; set; }
        public AslUserco aslUserco { get; set; }
        public AslCompany aslCompany { get; set; }
        public ASL_LOG aslLog { get; set; }


        public GL_ACCHARMST AGL_accharmst { get; set; }
        public GL_ACCHART AGL_acchart { get; set; }
        public GL_STRANS AGlStrans { get; set; }
        public GL_MASTER AGlMaster { get; set; }



        public PST_ITEMMST PST_Itemmst { get; set; }
        public PST_ITEM PST_Item { get; set; }
        public PST_TRANS pst_Trans { get; set; }
        public PST_TRANSMST Pst_Transmst { get; set; }
        public PST_PATIENT Pst_Patient { get; set; }
        public PST_REFER Pst_Refer { get; set; }






        [Display(Name = "HeadType")]
        public string HeadType { get; set; }
        
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }





        //SaleController
        public string Empty { get; set; } //It used for readonly value(HtmlTextBoxfor) hold.



        //ReportController
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Report_FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Report_ToDate { get; set; }

        [Required(ErrorMessage = "Item Name field can not be empty!")]
        public Int64 ITEMID { get; set; }

        [Required(ErrorMessage = "Store Name field can not be empty!")]
        public Int64 STOREID { get; set; }

        [Required(ErrorMessage = "Transaction Type field can not be empty!")]
        public string TRANSTP { get; set; }



        //Schedular Calendar
        public Int64? Userid { get; set; }

    }
}