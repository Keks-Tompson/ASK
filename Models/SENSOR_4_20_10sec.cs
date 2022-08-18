using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ASK.Models
{
    public class SENSOR_4_20_10sec
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double CO { get; set; }
        public double CO2 { get; set; }
        public double NO { get; set; }
        public double NO2 { get; set; }
        public double NOx { get; set; }
        public double SO2 { get; set; }
        public double Dust { get; set; }
        public double CH4 { get; set; }
        public double H2S { get; set; }

        public double Rezerv_1 { get; set; }
        public double Rezerv_2 { get; set; }
        public double Rezerv_3 { get; set; }
        public double Rezerv_4 { get; set; }
        public double Rezerv_5 { get; set; }

        public double O2_Wet { get; set; }
        public double O2_Dry { get; set; }
        public double H2O { get; set; }

        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double Speed { get; set; }
        
        public double Temperature_KIP { get; set; }
        public double Temperature_NOx { get; set; }
    }
}
