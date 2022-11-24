using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GasAnalyzer_Model
    {
        public bool Is_Used { get; set; } = true;                                                               //Испоьзуются ли газоанализаторы

        public GasAnalyzerComponent_Model SICK_GM32 { get; set; } = new GasAnalyzerComponent_Model();           //NH3, SO2, NO, NO2
        public GasAnalyzerComponent_Model SICK_GM35 { get; set; } = new GasAnalyzerComponent_Model();           //CO, CO2, H2O
        public GasAnalyzerComponent_Model Codel_GCEM4000 { get; set; } = new GasAnalyzerComponent_Model();      //NO, NO2, SO2, CO, CO2
        public GasAnalyzerComponent_Model Codel_GCEM4100 { get; set; } = new GasAnalyzerComponent_Model();      //NO, NO2, SO2, CO, CO2, CH4
        public GasAnalyzerComponent_Model Siemens_ULTRAMAT { get; set; } = new GasAnalyzerComponent_Model();    //CO, CO2, NO, SO2, CH4, O2?
        public GasAnalyzerComponent_Model Ecomer { get; set; } = new GasAnalyzerComponent_Model();              //Описать!!!
    }
}
