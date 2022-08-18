using ASK.BLL.Helper.Setting;
using System;


namespace ASK.BLL.Helper.Chart
{
    public class Chart_CurrentValue
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
        public double Add_Conc_1 { get; set; } = 0.0;
        public double Add_Conc_2 { get; set; } = 0.0;
        public double Add_Conc_3 { get; set; } = 0.0;
        public double Add_Conc_4 { get; set; } = 0.0;
        public double Add_Conc_5 { get; set; } = 0.0;

        public double O2_Wet { get; set; } = 15.00;
        public double O2_Dry { get; set; } = 15.00;


        int rndMin = -2500;
        int rndMax = 2500;

        int rndMaxPredel = 100;

        double MaxPredel = 1000.0;
        public Chart_CurrentValue Getsimulation()
        {
            Chart_CurrentValue chartSimulationValue = new Chart_CurrentValue();

            chartSimulationValue.DateString = DateTime.Now.ToLongTimeString();

            double O2 = GlobalStaticSettingsASK.ChartCurrent.O2_Dry + rnd.Next(-50, 50) / 100.0;
            if (O2 < 1.0)
                O2 = 1.0;
            if (O2 > 21.0)
                O2 = 21.0;


            chartSimulationValue.CO = GlobalStaticSettingsASK.ChartCurrent.CO * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.CO < 500.0)
            {
                chartSimulationValue.CO = GlobalStaticSettingsASK.ChartCurrent.CO + rnd.Next(0, 100);
            }
            if (chartSimulationValue.CO > 1000.0)
            {
                chartSimulationValue.CO = chartSimulationValue.CO - rnd.Next(0, rndMaxPredel);
            }

            chartSimulationValue.CO2 = GlobalStaticSettingsASK.ChartCurrent.CO2 * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.CO2 < 0.0)
            {
                chartSimulationValue.CO2 = GlobalStaticSettingsASK.ChartCurrent.CO2 + rnd.Next(0, 500) ;
            }
            if (chartSimulationValue.CO2 > MaxPredel)
            {
                chartSimulationValue.CO2 = chartSimulationValue.CO2 - rnd.Next(0, rndMaxPredel);
            }

            chartSimulationValue.NO = GlobalStaticSettingsASK.ChartCurrent.NO * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.NO < 0.0)
            {
                chartSimulationValue.NO = GlobalStaticSettingsASK.ChartCurrent.NO + rnd.Next(0, 500);
            }
            if (chartSimulationValue.NO > MaxPredel)
            {
                chartSimulationValue.NO = chartSimulationValue.NO - rnd.Next(0, rndMaxPredel);
            }

            chartSimulationValue.NO2 = GlobalStaticSettingsASK.ChartCurrent.NO2 * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.NO2 < 150.0)
            {
                chartSimulationValue.NO2 = GlobalStaticSettingsASK.ChartCurrent.NO2 + rnd.Next(0, 500);
            }
            if (chartSimulationValue.NO2 > 500.0)
            {
                chartSimulationValue.NO2 = chartSimulationValue.NO2 - rnd.Next(0, rndMaxPredel);
            }

            chartSimulationValue.NOx = GlobalStaticSettingsASK.ChartCurrent.NOx * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.NOx < 0.0)
            {
                chartSimulationValue.NOx = GlobalStaticSettingsASK.ChartCurrent.NOx + rnd.Next(0, 5);
            }
            if (chartSimulationValue.NOx > 50.0)
            {
                chartSimulationValue.NOx = chartSimulationValue.NOx - rnd.Next(0, 10);
            }

            chartSimulationValue.SO2 = GlobalStaticSettingsASK.ChartCurrent.SO2 * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.SO2 < 250.0)
            {
                chartSimulationValue.SO2 = GlobalStaticSettingsASK.ChartCurrent.SO2 + rnd.Next(0, 100);
            }
            if (chartSimulationValue.SO2 > 750.0)
            {
                chartSimulationValue.SO2 = chartSimulationValue.SO2 - rnd.Next(0, rndMaxPredel);
            }

            chartSimulationValue.Dust = GlobalStaticSettingsASK.ChartCurrent.Dust * (GlobalStaticSettingsASK.ChartCurrent.O2_Dry / O2) + rnd.Next(rndMin, rndMax) / 100.0;
            if (chartSimulationValue.Dust < 80.0)
            {
                chartSimulationValue.Dust = GlobalStaticSettingsASK.ChartCurrent.Dust + rnd.Next(0, 100);
            }
            if (chartSimulationValue.Dust > 250.0)
            {
                chartSimulationValue.Dust = chartSimulationValue.Dust - rnd.Next(0, rndMaxPredel);
            }



            chartSimulationValue.CH4 = 0.0;


            chartSimulationValue.H2S = 0.0;


            chartSimulationValue.Add_Conc_1 = 0.0;


            chartSimulationValue.Add_Conc_2 = 0.0;


            chartSimulationValue.Add_Conc_3 = 0.0;


            chartSimulationValue.Add_Conc_4 = 0.0;


            chartSimulationValue.Add_Conc_5 = 0.0;

            chartSimulationValue.O2_Wet = O2;
            chartSimulationValue.O2_Dry = O2;

            return chartSimulationValue;
        }
    }
}
