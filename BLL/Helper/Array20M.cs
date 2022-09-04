using System;
using System.Collections.Generic;

namespace ASK.BLL.Helper
{
    public  class Array20M : ICloneable
    {
        public DateTime Date { get; set; } 


        public double CO_Conc { get; set;} 
        public  double CO2_Conc { get; set; }
        public  double NO_Conc { get; set; } 
        public  double NO2_Conc { get; set; } 
        public  double NOx_Conc { get; set; } 
        public  double SO2_Conc { get; set; }
        public  double Dust_Conc { get; set; }
        public  double CH4_Conc { get; set; }
        public  double H2S_Conc { get; set; }
        public  double Add_Conc_1 { get; set; }
        public  double Add_Conc_2 { get; set; } 
        public  double Add_Conc_3 { get; set; }
        public  double Add_Conc_4 { get; set; }
        public  double Add_Conc_5 { get; set; }

        public  double CO_Emis { get; set; }
        public  double CO2_Emis { get; set; }
        public  double NO_Emis { get; set; }
        public  double NO2_Emis { get; set; } 
        public  double NOx_Emis { get; set; }
        public  double SO2_Emis { get; set; }
        public  double Dust_Emis { get; set; }
        public  double CH4_Emis { get; set; }
        public  double H2S_Emis { get; set; }
        public  double Add_Emis_1 { get; set; }
        public  double Add_Emis_2 { get; set; }
        public  double Add_Emis_3 { get; set; }
        public  double Add_Emis_4 { get; set; }
        public  double Add_Emis_5 { get; set; }




        public  double O2_Wet { get; set; }
        public  double O2_Dry { get; set; }
        public  double H2O { get; set; }
        public  double Pressure { get; set; }
        public  double Temperature { get; set; }
        public  double Speed { get; set; }
        public  double Flow { get; set; }
        public  double Temperature_KIP { get; set; }
        public  double Temperature_NOx { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
