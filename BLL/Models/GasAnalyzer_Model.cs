using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GasAnalyzer_Model
    {
        public bool Is_Used { get; set; } = true;                                                               //Испоьзуются ли газоанализаторы

        public GasAnalyzer_SICK_Model SICK { get; set; } = new GasAnalyzer_SICK_Model();                        //Газоанализаторы SICK
    }
}
