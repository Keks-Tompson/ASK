using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASK.DAL.Models
{
    public class ACCIDENT_LOG
    {
        [Key]
        public int Id { get; set; }
        public int id_accident { get; set; }

        public DateTime Date_Begin { get; set; }
        public DateTime Time_End { get; set; }

        public bool Is_Active { get; set; }

        //public virtual List<ACCIDENT_LIST> Accident_Items { get; set; }
    }
}
