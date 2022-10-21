using ASK.BLL.Helper.Setting;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Services
{
    public class CurrentPage_Services : ICurrentPage
    {
        IColorSensorParametrError _ColorSensorParametrError_Services;



        public CurrentPage_Services(IColorSensorParametrError ColorSensorParametrError_Services)
        {
            _ColorSensorParametrError_Services = ColorSensorParametrError_Services;
        }



        public CurrentPage_Model Update()
        {
            CurrentPage_Model currentPage = new CurrentPage_Model();

            currentPage.SensorNow = GlobalStaticSettingsASK.SensorNow;
            currentPage.SensorScaledNow = GlobalStaticSettingsASK.SensorScaledNow;
            currentPage.Color_Param = _ColorSensorParametrError_Services.Update();
            currentPage.VisibilityReportOptions = GlobalStaticSettingsASK.VisibilityReportOptions;

            currentPage.CurrentConcEmis = GlobalStaticSettingsASK.CurrentConcEmis;
            currentPage.PDZ_Current_String = GlobalStaticSettingsASK.PDZ_Current_String;

            if (GlobalStaticSettingsASK.globalAlarms.Is_NotConnection.Value)
            {
                currentPage.is_NotConnection_Text = "Отсутствует";
                currentPage.is_NotConnection_Color = "background: #f2aaaa96;";
            }
            else
            {
                currentPage.is_NotConnection_Text = "Установлена";
                currentPage.is_NotConnection_Color = "";
            }
            string notUse = "-/-";

            if (currentPage.CurrentConcEmis.CO_Conc >= GlobalStaticSettingsASK.PDZ_Current.CO_Conc && currentPage.PDZ_Current_String.CO_Conc != notUse)
                currentPage.Color_Conc.CO = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.CO = null;
            if (currentPage.CurrentConcEmis.CO2_Conc >= GlobalStaticSettingsASK.PDZ_Current.CO2_Conc && currentPage.PDZ_Current_String.CO2_Conc != notUse)
                currentPage.Color_Conc.CO2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.CO2 = null;
            if (currentPage.CurrentConcEmis.NO_Conc >= GlobalStaticSettingsASK.PDZ_Current.NO_Conc && currentPage.PDZ_Current_String.NO_Conc != notUse)
                currentPage.Color_Conc.NO = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.NO = null;
            if (currentPage.CurrentConcEmis.NO2_Conc >= GlobalStaticSettingsASK.PDZ_Current.NO2_Conc && currentPage.PDZ_Current_String.NO2_Conc != notUse)
                currentPage.Color_Conc.NO2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.NO2 = null;
            if (currentPage.CurrentConcEmis.NOx_Conc >= GlobalStaticSettingsASK.PDZ_Current.NOx_Conc && currentPage.PDZ_Current_String.NOx_Conc != notUse)
                currentPage.Color_Conc.NOx = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.NOx = null;
            if (currentPage.CurrentConcEmis.SO2_Conc >= GlobalStaticSettingsASK.PDZ_Current.SO2_Conc && currentPage.PDZ_Current_String.SO2_Conc != notUse)
                currentPage.Color_Conc.SO2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.SO2 = null;
            if (currentPage.CurrentConcEmis.Dust_Conc >= GlobalStaticSettingsASK.PDZ_Current.Dust_Conc && currentPage.PDZ_Current_String.Dust_Conc != notUse)
                currentPage.Color_Conc.Dust = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.Dust = null;
            if (currentPage.CurrentConcEmis.CH4_Conc >= GlobalStaticSettingsASK.PDZ_Current.CH4_Conc && currentPage.PDZ_Current_String.CH4_Conc != notUse)
                currentPage.Color_Conc.CH4 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.CH4 = null;
            if (currentPage.CurrentConcEmis.H2S_Conc >= GlobalStaticSettingsASK.PDZ_Current.H2S_Conc && currentPage.PDZ_Current_String.H2S_Conc != notUse)
                currentPage.Color_Conc.H2S = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.H2S = null;

            if (currentPage.CurrentConcEmis.Add_Conc_1 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_1 && currentPage.PDZ_Current_String.Add_Conc_1 != notUse)
                currentPage.Color_Conc.Rezerv_1 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.Rezerv_1 = null;
            if (currentPage.CurrentConcEmis.Add_Conc_2 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_2 && currentPage.PDZ_Current_String.Add_Conc_2 != notUse)
                currentPage.Color_Conc.Rezerv_2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.Rezerv_2 = null;
            if (currentPage.CurrentConcEmis.Add_Conc_3 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_3 && currentPage.PDZ_Current_String.Add_Conc_3 != notUse)
                currentPage.Color_Conc.Rezerv_3 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.Rezerv_3 = null;
            if (currentPage.CurrentConcEmis.Add_Conc_4 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_4 && currentPage.PDZ_Current_String.Add_Conc_4 != notUse)
                currentPage.Color_Conc.Rezerv_4 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.Rezerv_4 = null;
            if (currentPage.CurrentConcEmis.Add_Conc_5 >= GlobalStaticSettingsASK.PDZ_Current.Add_Conc_5 && currentPage.PDZ_Current_String.Add_Conc_5 != notUse)
                currentPage.Color_Conc.Rezerv_5 = "background: #f2aaaa96;";
            else
                currentPage.Color_Conc.Rezerv_5 = null;

            if (currentPage.CurrentConcEmis.CO_Emis >= GlobalStaticSettingsASK.PDZ_Current.CO_Emis && currentPage.PDZ_Current_String.CO_Emis != notUse)
                currentPage.Color_Emis.CO = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.CO = null;
            if (currentPage.CurrentConcEmis.CO2_Emis >= GlobalStaticSettingsASK.PDZ_Current.CO2_Emis && currentPage.PDZ_Current_String.CO2_Emis != notUse)
                currentPage.Color_Emis.CO2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.CO2 = null;
            if (currentPage.CurrentConcEmis.NO_Emis >= GlobalStaticSettingsASK.PDZ_Current.NO_Emis && currentPage.PDZ_Current_String.NO_Emis != notUse)
                currentPage.Color_Emis.NO = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.NO = null;
            if (currentPage.CurrentConcEmis.NO2_Emis >= GlobalStaticSettingsASK.PDZ_Current.NO2_Emis && currentPage.PDZ_Current_String.NO2_Emis != notUse)
                currentPage.Color_Emis.NO2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.NO2 = null;
            if (currentPage.CurrentConcEmis.NOx_Emis >= GlobalStaticSettingsASK.PDZ_Current.NOx_Emis && currentPage.PDZ_Current_String.NOx_Emis != notUse)
                currentPage.Color_Emis.NOx = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.NOx = null;
            if (currentPage.CurrentConcEmis.SO2_Emis >= GlobalStaticSettingsASK.PDZ_Current.SO2_Emis && currentPage.PDZ_Current_String.SO2_Emis != notUse)
                currentPage.Color_Emis.SO2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.SO2 = null;
            if (currentPage.CurrentConcEmis.Dust_Emis >= GlobalStaticSettingsASK.PDZ_Current.Dust_Emis && currentPage.PDZ_Current_String.Dust_Emis != notUse)
                currentPage.Color_Emis.Dust = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.Dust = null;
            if (currentPage.CurrentConcEmis.CH4_Emis >= GlobalStaticSettingsASK.PDZ_Current.CH4_Emis && currentPage.PDZ_Current_String.CH4_Emis != notUse)
                currentPage.Color_Emis.CH4 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.CH4 = null;
            if (currentPage.CurrentConcEmis.H2S_Emis >= GlobalStaticSettingsASK.PDZ_Current.H2S_Emis && currentPage.PDZ_Current_String.H2S_Emis != notUse)
                currentPage.Color_Emis.H2S = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.H2S = null;

            if (currentPage.CurrentConcEmis.Add_Emis_1 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_1 && currentPage.PDZ_Current_String.Add_Emis_1 != notUse)
                currentPage.Color_Emis.Rezerv_1 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.Rezerv_1 = null;
            if (currentPage.CurrentConcEmis.Add_Emis_2 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_2 && currentPage.PDZ_Current_String.Add_Emis_2 != notUse)
                currentPage.Color_Emis.Rezerv_2 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.Rezerv_2 = null;
            if (currentPage.CurrentConcEmis.Add_Emis_3 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_3 && currentPage.PDZ_Current_String.Add_Emis_3 != notUse)
                currentPage.Color_Emis.Rezerv_3 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.Rezerv_3 = null;
            if (currentPage.CurrentConcEmis.Add_Emis_4 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_4 && currentPage.PDZ_Current_String.Add_Emis_4 != notUse)
                currentPage.Color_Emis.Rezerv_4 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.Rezerv_4 = null;
            if (currentPage.CurrentConcEmis.Add_Emis_5 >= GlobalStaticSettingsASK.PDZ_Current.Add_Emis_5 && currentPage.PDZ_Current_String.Add_Emis_5 != notUse)
                currentPage.Color_Emis.Rezerv_5 = "background: #f2aaaa96;";
            else
                currentPage.Color_Emis.Rezerv_5 = null;

            return currentPage;
        }
    }
}
