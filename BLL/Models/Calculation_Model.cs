using ASK.BLL.Helper.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class Calculation_Model
    {
        public string Name { get; set; } = "NoneName_Conc";
        public double M { get; set; }
        public double C { get; set; }
        public double C_NOx { get; set; }
        public double C_NO { get; set; }
        public double C_NO2 { get; set; }
        public double C_Dust { get; set; }
        public double Vdry { get; set; }
        public double Cizm { get; set; }
        public double a { get; set; }
        public double a_O2 { get; set; }
        public double a_cast { get; set; } = GlobalStaticSettingsASK.CalculationSetting.Koeff_O2_Normalization;
        public double V { get; set; }
        public double k { get; set; }
        public double Pb_Pi { get; set; }
        public double tg { get; set; } = GlobalStaticSettingsASK.CurrentConcEmis.Temperature;
        public double Sfg { get; set; } = GlobalStaticSettingsASK.CurrentConcEmis.Speed;
        public double S_section { get; set; }
        public double d { get; set; } = GlobalStaticSettingsASK.CalculationSetting.PipeDiameter;
        public double pi { get; set; } = 3.14;
        public double H2O { get; set; } = GlobalStaticSettingsASK.CurrentConcEmis.H2O;
        public double O2_Dry { get; set; } = GlobalStaticSettingsASK.CurrentConcEmis.O2_Dry;
        public double O2_Wet { get; set; } = GlobalStaticSettingsASK.CurrentConcEmis.O2_Wet;
        


        public double M_NOx { get; set; }
        public double M_NO { get; set; }
        public double M_NO2 { get; set; }
        public double M_Dust { get; set; }
        public double I { get; set; }
        public double Izm_NO { get; set; } = GlobalStaticSettingsASK.SensorScaledNow.NO2_4_20mA;
        public double Izm_NO2 { get; set; } = GlobalStaticSettingsASK.SensorScaledNow.NO2_4_20mA;
        public double Izm_Dust { get; set; } = GlobalStaticSettingsASK.SensorScaledNow.Dust_4_20mA;
        public double rNO{ get; set; } = 1.34;
        public double rNO2 { get; set; } = 2.05;
        public double r { get; set; }
        public double Pa { get; set; } = GlobalStaticSettingsASK.CurrentConcEmis.Pressure;
        public double Bs { get; set; }  
        public double q4 { get; set; } = 0;
        public double B { get; set; }
        public double Vdry1_4 { get; set; } = 12.37;
        public double Normalization { get; set; } = 1.0;
        public double Dust_T { get; set; }
        public double Dust_E { get; set; }
    }
}
