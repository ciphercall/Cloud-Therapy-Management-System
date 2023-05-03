using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.Models.ASL
{
    [Table("ASL_PCalendarImage")]
    public class ASL_PCalendarImage
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Year { get; set; }

        [Key, Column(Order = 2)]
        public String Month { get; set; }
        
        public String FilePath { get; set; }
    }
}