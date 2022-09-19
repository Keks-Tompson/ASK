using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class PDZ_Active_Model
    {
        public DateTime Date { get; set; }
        public int NumberPDZ { get; set; } = 0;

        public double CO_Conc { get; set; } = 9999999.0;
        public double CO2_Conc { get; set; } = 9999999.0;
        public double NO_Conc { get; set; } = 9999999.0;
        public double NO2_Conc { get; set; } = 9999999.0;
        public double NOx_Conc { get; set; } = 9999999.0;
        public double SO2_Conc { get; set; } = 9999999.0;
        public double Dust_Conc { get; set; } = 9999999.0;
        public double CH4_Conc { get; set; } = 9999999.0;
        public double H2S_Conc { get; set; } = 9999999.0;
        public double Add_Conc_1 { get; set; } = 9999999.0;
        public double Add_Conc_2 { get; set; } = 9999999.0;
        public double Add_Conc_3 { get; set; } = 9999999.0;
        public double Add_Conc_4 { get; set; } = 9999999.0;
        public double Add_Conc_5 { get; set; } = 9999999.0;

        public double CO_Emis { get; set; } = 9999999.0;
        public double CO2_Emis { get; set; } = 9999999.0;
        public double NO_Emis { get; set; } = 9999999.0;
        public double NO2_Emis { get; set; } = 9999999.0;
        public double NOx_Emis { get; set; } = 9999999.0;
        public double SO2_Emis { get; set; } = 9999999.0;
        public double Dust_Emis { get; set; } = 9999999.0;
        public double CH4_Emis { get; set; } = 9999999.0;
        public double H2S_Emis { get; set; } = 9999999.0;
        public double Add_Emis_1 { get; set; } = 9999999.0;
        public double Add_Emis_2 { get; set; } = 9999999.0;
        public double Add_Emis_3 { get; set; } = 9999999.0;
        public double Add_Emis_4 { get; set; } = 9999999.0;
        public double Add_Emis_5 { get; set; } = 9999999.0;

        public bool Current { get; set; } = false;
    }
}
