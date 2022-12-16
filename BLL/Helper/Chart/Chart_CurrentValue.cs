using ASK.BLL.Helper.Setting;
using System;


namespace ASK.BLL.Helper.Chart
{
    public class Chart_CurrentValue : ICloneable
    {
        static Random rnd = new Random();

        public String DateString { get; set; } = DateTime.Now.ToLongTimeString();

        public double CO { get; set; } = 0.0;
        public double CO2 { get; set; } = 0.0;
        public double NO { get; set; } = 0.0;
        public double NO2 { get; set; } = 0.0;
        public double NOx { get; set; } = 0.0;
        public double SO2 { get; set; } = 0.0;
        public double Dust { get; set; } = 0.0;
        public double CH4 { get; set; } = 0.0;
        public double H2S { get; set; } = 0.0;
        public double NH3 { get; set; } = 0.0;
        public double Add_Conc_1 { get; set; } = 0.0;
        public double Add_Conc_2 { get; set; } = 0.0;
        public double Add_Conc_3 { get; set; } = 0.0;
        public double Add_Conc_4 { get; set; } = 0.0;
        public double Add_Conc_5 { get; set; } = 0.0;

        public double O2_Wet { get; set; } = 0.0; // 15.00;
        public double O2_Dry { get; set; } = 0.0; //15.00;


        //int rndMin = -2500;
        //int rndMax = 2500;

        //int rndMaxPredel = 700;

        //double MaxPredel = 1000.0;
        //public void Getsimulation()
        //{


        //    DateString = DateTime.Now.ToLongTimeString();

        //    double O2 = GlobalStaticSettingsASK.ChartCurrent.O2_Dry + rnd.Next(-70, 70) / 100.0;
        //    if (O2 < 1.0)
        //        O2 = 1.0;
        //    if (O2 > 21.0)
        //        O2 = 21.0;


        //    CO = GlobalStaticSettingsASK.ChartCurrent.CO * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (CO < 500.0)
        //    {
        //        CO = GlobalStaticSettingsASK.ChartCurrent.CO + rnd.Next(0, 100);
        //        if (CO < 0.0)
        //            CO = 0.0;
        //    }
        //    if (CO > 1000.0)
        //    {
        //        CO = CO - rnd.Next(0, rndMaxPredel);
        //    }

        //    CO2 = GlobalStaticSettingsASK.ChartCurrent.CO2 * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (CO2 < 0.0)
        //    {
        //        CO2 = GlobalStaticSettingsASK.ChartCurrent.CO2 + rnd.Next(0, 500);
        //        if (CO2 < 0.0)
        //            CO2 = 0.0;
        //    }
        //    if (CO2 > MaxPredel)
        //    {
        //        CO2 = CO2 - rnd.Next(0, rndMaxPredel);
        //    }

        //    NO = GlobalStaticSettingsASK.ChartCurrent.NO * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (NO < 0.0)
        //    {
        //        NO = GlobalStaticSettingsASK.ChartCurrent.NO + rnd.Next(0, 500);
        //        if (NO < 0.0)
        //            NO = 0.0;
        //    }
        //    if (NO > MaxPredel)
        //    {
        //        NO = NO - rnd.Next(0, rndMaxPredel);
        //    }

        //    NO2 = GlobalStaticSettingsASK.ChartCurrent.NO2 * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (NO2 < 150.0)
        //    {
        //        NO2 = GlobalStaticSettingsASK.ChartCurrent.NO2 + rnd.Next(0, 500);
        //        if (NO2 < 0.0)
        //            NO2 = 0.0;
        //    }
        //    if (NO2 > 500.0)
        //    {
        //        NO2 = NO2 - rnd.Next(0, rndMaxPredel);
        //    }

        //    NOx = GlobalStaticSettingsASK.ChartCurrent.NOx * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (NOx < 0.0)
        //    {
        //        NOx = GlobalStaticSettingsASK.ChartCurrent.NOx + rnd.Next(0, 5);
        //        if (NOx < 0.0)
        //            NOx = 0.0;
        //    }
        //    if (NOx > 50.0)
        //    {
        //        NOx = NOx - rnd.Next(0, 10);
        //    }

        //    SO2 = GlobalStaticSettingsASK.ChartCurrent.SO2 * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (SO2 < 250.0)
        //    {
        //        SO2 = GlobalStaticSettingsASK.ChartCurrent.SO2 + rnd.Next(0, 100);
        //        if (SO2 < 0.0)
        //            SO2 = 0.0;
        //    }
        //    if (SO2 > 750.0)
        //    {
        //        SO2 = SO2 - rnd.Next(0, rndMaxPredel);
        //    }

        //    Dust = GlobalStaticSettingsASK.ChartCurrent.Dust * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
        //    if (Dust < 80.0)
        //    {
        //        Dust = GlobalStaticSettingsASK.ChartCurrent.Dust + rnd.Next(0, 100);
        //        if (Dust < 0.0)
        //            Dust = 0.0;

        //    }
        //    if (Dust > 250.0)
        //    {
        //        Dust = Dust - rnd.Next(0, 150);
        //    }

        //    CH4 = 0.0;
        //    H2S = 0.0;

        //    Add_Conc_1 = 0.0;
        //    Add_Conc_2 = 0.0;
        //    Add_Conc_3 = 0.0;
        //    Add_Conc_4 = 0.0;
        //    Add_Conc_5 = 0.0;

        //    O2_Wet = O2;
        //    O2_Dry = O2;
        //}

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
