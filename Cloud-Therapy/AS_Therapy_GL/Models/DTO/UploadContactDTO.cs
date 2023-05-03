using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models.DTO
{
    public class UploadContactDTO
    {
        public Int64 ID { get; set; }
        public Int64 COMPID { get; set; }

        public Int64? From_GROUPID { get; set; }
        public Int64? TO_GROUPID { get; set; }
        public string TO_GROUPNM { get; set; }

        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string MOBNO1 { get; set; }
        public string MOBNO2 { get; set; }
        public string ADDRESS { get; set; }







        public string USERPC { get; set; }
        public Int64 INSUSERID { get; set; }

        //[Display(Name = "Insert Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64 UPDUSERID { get; set; }

        //[Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }

        //public string Insert { get; set; }
        //public string Update { get; set; }
        //public string Delete { get; set; }


        public Int64 count { get; set; }
        public Int64 GetChildDataForDeleteMasterCategory { get; set; } // its used for Delete Group(category) data before check this child data is hold or not.

        //Update group wise data
        public Int64 CheckPreviousData { get; set; }
        public Int64 CheckValidation { get; set; }
    }
}