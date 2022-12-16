using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    //Все возможные газоанализаторы SICK
    public class GasAnalyzer_SICK_Model
    {
        public bool Is_Used { get; set; } = true;

        public GasAnalyzer_SICK_GMS810_Model GMS810 { get; set; } = new GasAnalyzer_SICK_GMS810_Model();            //NH3, SO2, NO, NO2, CO, CO2, NOx (На выбор, но не более 5)
        public GasAnalyzer_SICK_GM32_Model GM32 { get; set; } = new GasAnalyzer_SICK_GM32_Model();                  //NH3, SO2, NO, NO2, NOx (На выбор, но не более 2)
        public GasAnalyzer_SICK_GM35_Model GM35 { get; set; } = new GasAnalyzer_SICK_GM35_Model();                  //CO, CO2, H2O
    }
}
