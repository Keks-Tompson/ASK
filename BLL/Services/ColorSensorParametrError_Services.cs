using ASK.BLL.Helper.Setting;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Services
{
    public class ColorSensorParametrError_Services : IColorSensorParametrError
    {
        public ColorSensorParametrError_Model Update()
        {
            ColorSensorParametrError_Model color_Param = new ColorSensorParametrError_Model();

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
