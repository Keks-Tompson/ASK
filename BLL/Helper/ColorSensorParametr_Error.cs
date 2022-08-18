

using ASK.BLL.Helper.Setting;

namespace ASK.BLL.Helper
{
    public  class ColorSensorParametr_Error
    {
        public  string CO { get; set; }
        public  string CO2 { get; set; }
        public  string NO { get; set; }
        public  string NO2 { get; set; }
        public  string NOx { get; set; }
        public  string SO2 { get; set; }
        public  string Dust { get; set; }
        public  string CH4 { get; set; }
        public  string H2S { get; set; }
        public  string Rezerv_1 { get; set; }
        public  string Rezerv_2 { get; set; }
        public  string Rezerv_3 { get; set; }
        public  string Rezerv_4 { get; set; }
        public  string Rezerv_5 { get; set; }

        public  string O2_Wet { get; set; }
        public  string O2_Dry { get; set; }
        public  string H2O { get; set; }
        public  string Pressure { get; set; }
        public  string Temperature { get; set; }
        public  string Speed { get; set; }
        public  string Temperature_KIP { get; set; }
        public  string Temperature_NOx { get; set; }



        public static ColorSensorParametr_Error Update()
        {
            ColorSensorParametr_Error color_Param = new ColorSensorParametr_Error();

            const double mA_Min = 4.0;
            const double mA_Max = 20.0;


            if (GlobalStaticSettingsASK.SensorNow.CO_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.CO_4_20mA < mA_Min)
                color_Param.CO = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.CO2_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.CO2_4_20mA < mA_Min)
                color_Param.CO2 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.NO_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.NO_4_20mA < mA_Min)
                color_Param.NO = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.NO2_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.NO2_4_20mA < mA_Min)
                color_Param.NO2 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.NOx_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.NOx_4_20mA < mA_Min)
                color_Param.NOx = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.SO2_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.SO2_4_20mA < mA_Min)
                color_Param.SO2 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Dust_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Dust_4_20mA < mA_Min)
                color_Param.Dust = "background: #f2aaaa96;";

            if (GlobalStaticSettingsASK.SensorNow.CH4_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.CH4_4_20mA < mA_Min)
                color_Param.CH4 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.H2S_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.H2S_4_20mA < mA_Min)
                color_Param.H2S = "background: #f2aaaa96;";

            if (GlobalStaticSettingsASK.SensorNow.Rezerv_1_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Rezerv_1_4_20mA < mA_Min)
                color_Param.Rezerv_1 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Rezerv_2_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Rezerv_2_4_20mA < mA_Min)
                color_Param.Rezerv_2 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Rezerv_3_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Rezerv_3_4_20mA < mA_Min)
                color_Param.Rezerv_3 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Rezerv_4_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Rezerv_4_4_20mA < mA_Min)
                color_Param.Rezerv_4 = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Rezerv_5_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Rezerv_5_4_20mA < mA_Min)
                color_Param.Rezerv_5 = "background: #f2aaaa96;";

            if (GlobalStaticSettingsASK.SensorNow.O2_Wet_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.O2_Wet_4_20mA < mA_Min)
                color_Param.O2_Wet = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.O2_Dry_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.O2_Dry_4_20mA < mA_Min)
                color_Param.O2_Dry = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.H2O_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.H2O_4_20mA < mA_Min)
                color_Param.H2O = "background: #f2aaaa96;";

            if (GlobalStaticSettingsASK.SensorNow.Pressure_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Pressure_4_20mA < mA_Min)
                color_Param.Pressure = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Temperature_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Temperature_4_20mA < mA_Min)
                color_Param.Temperature = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Speed_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Speed_4_20mA < mA_Min)
                color_Param.Speed = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Temperature_KIP_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Temperature_KIP_4_20mA < mA_Min)
                color_Param.Temperature_KIP = "background: #f2aaaa96;";
            if (GlobalStaticSettingsASK.SensorNow.Temperature_NOx_4_20mA > mA_Max || GlobalStaticSettingsASK.SensorNow.Temperature_NOx_4_20mA < mA_Min)
                color_Param.Temperature_NOx = "background: #f2aaaa96;";


            return color_Param;
        }
    }


}
