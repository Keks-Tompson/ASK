

using ASK.BLL.Helper.Alarms;
using ASK.BLL.Helper.Setting;

namespace ASK.BLL.Helper.AJAX_Page.CurrentValue
{
    public class CurrentPageAnalog
    {
        public ColorSensorParametr_Error Color_Param { get; set; } = new ColorSensorParametr_Error();
        public VisibilityOptions20MJSON VisibilityOptions20M { get; set; } = new VisibilityOptions20MJSON();


        public Sensor_4_20 SensorNow { get; set; } = new Sensor_4_20();
        public Sensor_4_20 SensorScaledNow { get; set; } = new Sensor_4_20();



        public Array20M CurrentConcEmis { get; set; } = new Array20M();
        public PDZ_String_Active PDZ_Current_String { get; set; } = new PDZ_String_Active();
        public ColorSensor_Eror Color_Conc { get; set; } = new ColorSensor_Eror();
        public ColorSensor_Eror Color_Emis { get; set; } = new ColorSensor_Eror();


        public string is_NotConnection_Text { get; set; } //Удалить и всё что будет связано заменить на нормальный код
        public string is_NotConnection_Color { get; set; }//Удалить и всё что будет связано заменить на нормальный код



        public void Update()
        {
            SensorNow = GlobalStaticSettingsASK.SensorNow;
            SensorScaledNow = GlobalStaticSettingsASK.SensorScaledNow;
            Color_Param = ColorSensorParametr_Error.Update();
            VisibilityOptions20M = GlobalStaticSettingsASK.VisibilityOptions20M;

            CurrentConcEmis = GlobalStaticSettingsASK.CurrentConcEmis;
            PDZ_Current_String = GlobalStaticSettingsASK.PDZ_Current_String;

            if (GlobalStaticSettingsASK.globalAlarms.Is_NotConnection)
            {
                is_NotConnection_Text = "Отсутствует";
                is_NotConnection_Color = "background: #f2aaaa96;";
            }
            else
            {
                is_NotConnection_Text = "Установлена";
                is_NotConnection_Color = "";
            }

            UpdateConcEmis_Color();

        }


        public void UpdateConcEmis_Color()
        {
            string notUse = "-/-";



            if (CurrentConcEmis.CO_Conc >= GlobalStaticSettingsASK.PDZ_Current.CO_Conc && PDZ_Current_String.CO_Conc != notUse)
                Color_Conc.CO = "background: #f2aaaa96;";
            else 
                Color_Conc.CO = null;
            if (CurrentConcEmis.CO2_Conc >= GlobalStaticSettingsASK.PDZ_Current.CO2_Conc && PDZ_Current_String.CO2_Conc != notUse)
                Color_Conc.CO2 = "background: #f2aaaa96;";
            else 
                Color_Conc.CO2 = null;
            if (CurrentConcEmis.NO_Conc >= GlobalStaticSettingsASK.PDZ_Current.NO_Conc && PDZ_Current_String.NO_Conc != notUse)
                Color_Conc.NO = "background: #f2aaaa96;";
            else
                Color_Conc.NO = null;
            if (CurrentConcEmis.NO2_Conc >= GlobalStaticSettingsASK.PDZ_Current.NO2_Conc && PDZ_Current_String.NO2_Conc != notUse)
                Color_Conc.NO2 = "background: #f2aaaa96;";
            else
                Color_Conc.NO2 = null;
            if (CurrentConcEmis.NOx_Conc >= GlobalStaticSettingsASK.PDZ_Current.NOx_Conc && PDZ_Current_String.NOx_Conc != notUse)
                Color_Conc.NOx = "background: #f2aaaa96;";
            else
                Color_Conc.NOx = null;
            if (CurrentConcEmis.SO2_Conc >= GlobalStaticSettingsASK.PDZ_Current.SO2_Conc && PDZ_Current_String.SO2_Conc != notUse)
                Color_Conc.SO2 = "background: #f2aaaa96;";
            else
                Color_Conc.SO2 = null;
            if (CurrentConcEmis.Dust_Conc >= GlobalStaticSettingsASK.PDZ_Current.Dust_Conc && PDZ_Current_String.Dust_Conc != notUse)
                Color_Conc.Dust = "background: #f2aaaa96;";
            else
                Color_Conc.Dust = null;
            if (CurrentConcEmis.CH4_Conc >= GlobalStaticSettingsASK.PDZ_Current.CH4_Conc && PDZ_Current_String.CH4_Conc != notUse)
                Color_Conc.CH4 = "background: #f2aaaa96;";
            else
                Color_Conc.CH4 = null;
            if (CurrentConcEmis.H2S_Conc >= GlobalStaticSettingsASK.PDZ_Current.H2S_Conc && PDZ_Current_String.H2S_Conc != notUse)
                Color_Conc.H2S = "background: #f2aaaa96;";
            else
                Color_Conc.H2S = null;

            if (CurrentConcEmis.Add_Conc_1 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_1 && PDZ_Current_String.Add_Conc_1 != notUse)
                Color_Conc.Rezerv_1 = "background: #f2aaaa96;";
            else
                Color_Conc.Rezerv_1 = null;
            if (CurrentConcEmis.Add_Conc_2 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_2 && PDZ_Current_String.Add_Conc_2 != notUse)
                Color_Conc.Rezerv_2 = "background: #f2aaaa96;";
            else
                Color_Conc.Rezerv_2 = null;
            if (CurrentConcEmis.Add_Conc_3 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_3 && PDZ_Current_String.Add_Conc_3 != notUse)
                Color_Conc.Rezerv_3 = "background: #f2aaaa96;";
            else
                Color_Conc.Rezerv_3 = null;
            if (CurrentConcEmis.Add_Conc_4 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_4 && PDZ_Current_String.Add_Conc_4 != notUse)
                Color_Conc.Rezerv_4 = "background: #f2aaaa96;";
            else
                Color_Conc.Rezerv_4 = null;
            if (CurrentConcEmis.Add_Conc_5 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_5 && PDZ_Current_String.Add_Conc_5 != notUse)
                Color_Conc.Rezerv_5 = "background: #f2aaaa96;";
            else
                Color_Conc.Rezerv_5 = null;



            if (CurrentConcEmis.CO_Emis >= GlobalStaticSettingsASK.PDZ_Current.CO_Emis && PDZ_Current_String.CO_Emis != notUse)
                Color_Emis.CO = "background: #f2aaaa96;";
            else
                Color_Emis.CO = null;
            if (CurrentConcEmis.CO2_Emis >= GlobalStaticSettingsASK.PDZ_Current.CO2_Emis && PDZ_Current_String.CO2_Emis != notUse)
                Color_Emis.CO2 = "background: #f2aaaa96;";
            else
                Color_Emis.CO2 = null;
            if (CurrentConcEmis.NO_Emis >= GlobalStaticSettingsASK.PDZ_Current.NO_Emis && PDZ_Current_String.NO_Emis != notUse)
                Color_Emis.NO = "background: #f2aaaa96;";
            else
                Color_Emis.NO = null;
            if (CurrentConcEmis.NO2_Emis >= GlobalStaticSettingsASK.PDZ_Current.NO2_Emis && PDZ_Current_String.NO2_Emis != notUse)
                Color_Emis.NO2 = "background: #f2aaaa96;";
            else
                Color_Emis.NO2 = null;
            if (CurrentConcEmis.NOx_Emis >= GlobalStaticSettingsASK.PDZ_Current.NOx_Emis && PDZ_Current_String.NOx_Emis != notUse)
                Color_Emis.NOx = "background: #f2aaaa96;";
            else
                Color_Emis.NOx = null;
            if (CurrentConcEmis.SO2_Emis >= GlobalStaticSettingsASK.PDZ_Current.SO2_Emis && PDZ_Current_String.SO2_Emis != notUse)
                Color_Emis.SO2 = "background: #f2aaaa96;";
            else
                Color_Emis.SO2 = null;
            if (CurrentConcEmis.Dust_Emis >= GlobalStaticSettingsASK.PDZ_Current.Dust_Emis && PDZ_Current_String.Dust_Emis != notUse)
                Color_Emis.Dust = "background: #f2aaaa96;";
            else
                Color_Emis.Dust = null;
            if (CurrentConcEmis.CH4_Emis >= GlobalStaticSettingsASK.PDZ_Current.CH4_Emis && PDZ_Current_String.CH4_Emis != notUse)
                Color_Emis.CH4 = "background: #f2aaaa96;";
            else
                Color_Emis.CH4 = null;
            if (CurrentConcEmis.H2S_Emis >= GlobalStaticSettingsASK.PDZ_Current.H2S_Emis && PDZ_Current_String.H2S_Emis != notUse)
                Color_Emis.H2S = "background: #f2aaaa96;";
            else
                Color_Emis.H2S = null;

            if (CurrentConcEmis.Add_Emis_1 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_1 && PDZ_Current_String.Add_Emis_1 != notUse)
                Color_Emis.Rezerv_1 = "background: #f2aaaa96;";
            else
                Color_Emis.Rezerv_1 = null;
            if (CurrentConcEmis.Add_Emis_2 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_2 && PDZ_Current_String.Add_Emis_2 != notUse)
                Color_Emis.Rezerv_2 = "background: #f2aaaa96;";
            else
                Color_Emis.Rezerv_2 = null;
            if (CurrentConcEmis.Add_Emis_3 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_3 && PDZ_Current_String.Add_Emis_3 != notUse)
                Color_Emis.Rezerv_3 = "background: #f2aaaa96;";
            else
                Color_Emis.Rezerv_3 = null;
            if (CurrentConcEmis.Add_Emis_4 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_4 && PDZ_Current_String.Add_Emis_4 != notUse)
                Color_Emis.Rezerv_4 = "background: #f2aaaa96;";
            else
                Color_Emis.Rezerv_4 = null;
            if (CurrentConcEmis.Add_Emis_5 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_5 && PDZ_Current_String.Add_Emis_5 != notUse)
                Color_Emis.Rezerv_5 = "background: #f2aaaa96;";
            else
                Color_Emis.Rezerv_5 = null;
        }
    }
}
