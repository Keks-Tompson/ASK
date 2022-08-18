using System;
using System.ComponentModel.DataAnnotations;

namespace ASK.Models
{
    public class ACCIDENT_LIST
    {
        [Key]
        public int  Id { get; set; }
        
        [MaxLength(100)]
        public string Accident { get; set; }

        public bool is_Error { get; set; }
    }
}
