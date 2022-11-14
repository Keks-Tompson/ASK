using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class Range_Model
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public bool Is_Used { get; set; }
        public mA_Model mA { get; set; } = new mA_Model();
    }
}
