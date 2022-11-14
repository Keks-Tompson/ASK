using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class Alarm_Model
    {
        public int ID { get; set; } = 100;
        public bool Value { get; set; } = false;
        public bool Is_Used { get; set; } = false;
        public bool Is_Info { get; set; } = false;
        public bool Is_Critical { get; set; } = false;
    }
}
