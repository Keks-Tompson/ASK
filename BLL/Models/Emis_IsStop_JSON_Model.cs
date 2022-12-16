using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class Emis_IsStop_JSON_Model
    {
        public bool isAuto { get; set; }


        public int Day { get; set; }


        public double CO_Auto { get; set; }
        public double CO2_Auto { get; set; }
        public double NO_Auto { get; set; }
        public double NO2_Auto { get; set; }
        public double NOx_Auto { get; set; }
        public double SO2_Auto { get; set; }
        public double Dust_Auto { get; set; }
        public double CH4_Auto { get; set; }
        public double H2S_Auto { get; set; }
        public double NH3_Auto { get; set; }
        public double Rezerv_1_Auto { get; set; }
        public double Rezerv_2_Auto { get; set; }
        public double Rezerv_3_Auto { get; set; }
        public double Rezerv_4_Auto { get; set; }
        public double Rezerv_5_Auto { get; set; }


        public double CO_Manual { get; set; }
        public double CO2_Manual { get; set; }
        public double NO_Manual { get; set; }
        public double NO2_Manual { get; set; }
        public double NOx_Manual { get; set; }
        public double SO2_Manual { get; set; }
        public double Dust_Manual { get; set; }
        public double CH4_Manual { get; set; }
        public double H2S_Manual { get; set; }
        public double NH3_Manual { get; set; }
        public double Rezerv_1_Manual { get; set; }
        public double Rezerv_2_Manual { get; set; }
        public double Rezerv_3_Manual { get; set; }
        public double Rezerv_4_Manual { get; set; }
        public double Rezerv_5_Manual { get; set; }
    }
}
