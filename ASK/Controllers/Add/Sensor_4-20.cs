using System;

namespace ASK.Controllers.Add
{
    public class Sensor_4_20
    {
        public DateTime Date { get; set; }

        public double CO_4_20mA { get; set; }
        public double CO2_4_20mA { get; set; }
        public double NO_4_20mA { get; set; }
        public double NO2_4_20mA { get; set; }
        public double NOx_4_20mA { get; set; }
        public double SO2_4_20mA { get; set; }
        public double Dust_4_20mA { get; set; }
        public double CH4_4_20mA { get; set; }
        public double H2S_4_20mA { get; set; }

        public double Rezerv_1_4_20mA { get; set; }
        public double Rezerv_2_4_20mA { get; set; }
        public double Rezerv_3_4_20mA { get; set; }
        public double Rezerv_4_4_20mA { get; set; }
        public double Rezerv_5_4_20mA { get; set; }

        public double O2_Wet_4_20mA { get; set; }
        public double O2_Dry_4_20mA { get; set; }
        public double H2O_4_20mA { get; set; }

        public double Pressure_4_20mA { get; set; }
        public double Temperature_4_20mA { get; set; }
        public double Speed_4_20mA { get; set; }

        public double Temperature_KIP_4_20mA { get; set; }
        public double Temperature_NOx_4_20mA { get; set; }
    }
}
