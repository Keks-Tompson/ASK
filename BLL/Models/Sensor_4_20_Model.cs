using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class Sensor_4_20_Model : ICloneable
    {
        public DateTime Date { get; set; }

        public double CO_4_20mA { get; set; } = 0;
        public double CO2_4_20mA { get; set; } = 0;
        public double NO_4_20mA { get; set; } = 0;
        public double NO2_4_20mA { get; set; } = 0;
        public double NOx_4_20mA { get; set; } = 0;
        public double SO2_4_20mA { get; set; } = 0;
        public double Dust_4_20mA { get; set; } = 0;
        public double CH4_4_20mA { get; set; } = 0;
        public double H2S_4_20mA { get; set; } = 0;

        public double Rezerv_1_4_20mA { get; set; } = 0;
        public double Rezerv_2_4_20mA { get; set; } = 0;
        public double Rezerv_3_4_20mA { get; set; } = 0;
        public double Rezerv_4_4_20mA { get; set; } = 0;
        public double Rezerv_5_4_20mA { get; set; } = 0;

        public double O2_Wet_4_20mA { get; set; } = 0;
        public double O2_Dry_4_20mA { get; set; } = 0;
        public double H2O_4_20mA { get; set; } = 0;

        public double Pressure_4_20mA { get; set; } = 0;
        public double Temperature_4_20mA { get; set; } = 0;
        public double Speed_4_20mA { get; set; } = 0;

        public double Temperature_KIP_4_20mA { get; set; } = 0;
        public double Temperature_NOx_4_20mA { get; set; } = 0;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
