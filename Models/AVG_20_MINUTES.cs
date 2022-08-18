using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASK.Models
{
    public class AVG_20_MINUTES
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        
        public double Conc_CO { get; set; }
        public double Conc_CO2 { get; set; }
        public double Conc_NO { get; set; }
        public double Conc_NO2 { get; set; }
        public double Conc_NOx { get; set; }
        public double Conc_SO2 { get; set; }
        public double Conc_Dust { get; set; }
        public double Conc_CH4 { get; set; }
        public double Conc_H2S { get; set; }
        public double Conc_D1 { get; set; }
        public double Conc_D2 { get; set; }
        public double Conc_D3 { get; set; }
        public double Conc_D4 { get; set; }
        public double Conc_D5 { get; set; }

        public double Emis_CO { get; set; }
        public double Emis_CO2 { get; set; }
        public double Emis_NO { get; set; }
        public double Emis_NO2 { get; set; }
        public double Emis_NOx { get; set; }
        public double Emis_SO2 { get; set; }
        public double Emis_CH4 { get; set; }
        public double Emis_H2S { get; set; }
        public double Emis_Dust { get; set; }
        public double Emis_D1 { get; set; }
        public double Emis_D2 { get; set; }
        public double Emis_D3 { get; set; }
        public double Emis_D4 { get; set; }
        public double Emis_D5 { get; set; }

        public double O2_Wet { get; set; }
        public double O2_Dry { get; set; }
        public double H2O { get; set; }

        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double Speed { get; set; }
        public double Flow { get; set; }
        public double Temperature_KIP { get; set; }
        public double Temperature_NOx { get; set; }
        
        public int Mode_ASK { get; set; }
        public int PDZ_Fuel { get; set; } = 0;

        //public virtual List<PDZ> PDZ_Items { get; set; }

    }
}
