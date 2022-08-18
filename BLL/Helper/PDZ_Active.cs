
using System;

namespace ASK.BLL.Helper
{
    public class PDZ_Active
    {
        public DateTime Date { get; set; }
        public int NumberPDZ { get; set; }

        public double CO_Conc { get; set; }
        public double CO2_Conc { get; set; }
        public double NO_Conc { get; set; }
        public double NO2_Conc { get; set; }
        public double NOx_Conc { get; set; }
        public double SO2_Conc { get; set; }
        public double Dust_Conc { get; set; }
        public double CH4_Conc { get; set; }
        public double H2S_Conc { get; set; }
        public double Add_Conc_1 { get; set; }
        public double Add_Conc_2 { get; set; }
        public double Add_Conc_3 { get; set; }
        public double Add_Conc_4 { get; set; }
        public double Add_Conc_5 { get; set; }

        public double CO_Emis { get; set; }
        public double CO2_Emis { get; set; }
        public double NO_Emis { get; set; }
        public double NO2_Emis { get; set; }
        public double NOx_Emis { get; set; }
        public double SO2_Emis { get; set; }
        public double Dust_Emis { get; set; }
        public double CH4_Emis { get; set; }
        public double H2S_Emis { get; set; }
        public double Add_Emis_1 { get; set; }
        public double Add_Emis_2 { get; set; }
        public double Add_Emis_3 { get; set; }
        public double Add_Emis_4 { get; set; }
        public double Add_Emis_5 { get; set; }

        public bool Current { get; set; }



        public PDZ_Active()
        {
            NumberPDZ = 0;

            CO_Conc = 9999999.0;
            CO2_Conc = 9999999.0;
            NO_Conc = 9999999.0;
            NO2_Conc = 9999999.0;
            NOx_Conc = 9999999.0;
            SO2_Conc = 9999999.0;
            Dust_Conc = 9999999.0;
            CH4_Conc = 9999999.0;
            H2S_Conc = 9999999.0;
            Add_Conc_1 = 9999999.0;
            Add_Conc_2 = 9999999.0;
            Add_Conc_3 = 9999999.0;
            Add_Conc_4 = 9999999.0;
            Add_Conc_5 = 9999999.0;

            CO_Emis = 9999999.0;
            CO2_Emis = 9999999.0;
            NO_Emis = 9999999.0;
            NO2_Emis = 9999999.0;
            NOx_Emis = 9999999.0;
            SO2_Emis = 9999999.0;
            Dust_Emis = 9999999.0;
            CH4_Emis = 9999999.0;
            H2S_Emis = 9999999.0;
            Add_Emis_1 = 9999999.0;
            Add_Emis_2 = 9999999.0;
            Add_Emis_3 = 9999999.0;
            Add_Emis_4 = 9999999.0;
            Add_Emis_5 = 9999999.0;

            Current = false;
        }

      
    }
}
