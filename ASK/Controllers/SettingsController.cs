using ASK.BLL.Helper.Setting;
using ASK.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading;

namespace ASK.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Administrator")]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        public SettingsController()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");   //Меняем культуры, что бы из вформы можно было получить 
        }                                                                                                           //тип double (c культурой RU разделитель - запятая, а с 
                                                                                                                    //формы идёт точка и при получение double = 0, по этому с


        [HttpPost]
        public IActionResult GetNamesettings(string NameASKSystem, string NameASK, string NumberASK)
        {
            //if (Accaunt.isValid)
            //{
            GlobalStaticSettingsASK.SettingOptions.NameASKSystem = NameASKSystem;
            GlobalStaticSettingsASK.SettingOptions.NameASK = NameASK;
            GlobalStaticSettingsASK.SettingOptions.NumberASK = NumberASK;

            GlobalStaticSettingsASK.SaveSettingOptionsJSON();
            //}
            //else
            //{
            //    return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            //}

            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult PDZ_Set()
        {
            //if (Accaunt.isValid)
            //{
            if (GlobalStaticSettingsASK.PDZ.UsedNumberFuel == 1)
                GlobalStaticSettingsASK.PDZ.UsedNumberFuel = 3;
            else
                GlobalStaticSettingsASK.PDZ.UsedNumberFuel = 1;

            GlobalStaticSettingsASK.SavePdz_JSON();

            return RedirectToRoute(new { controller = "Settings", action = "Index" });
            //}
            //else
            //{
            //    return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            //}
        }



        [HttpPost]
        public IActionResult PDZ_FUEL(



            //ПДЗ 1
            string PDZ_1_Name,
            //Концентрации
            double PDZ_1_CO_Conc,
            double PDZ_1_CO2_Conc,
            double PDZ_1_NO_Conc,
            double PDZ_1_NO2_Conc,
            double PDZ_1_NOx_Conc,
            double PDZ_1_SO2_Conc,
            double PDZ_1_Dust_Conc,
            double PDZ_1_CH4_Conc,
            double PDZ_1_H2S_Conc,
            double PDZ_1_NH3_Conc,
            double PDZ_1_Add_Conc_1,
            double PDZ_1_Add_Conc_2,
            double PDZ_1_Add_Conc_3,
            double PDZ_1_Add_Conc_4,
            double PDZ_1_Add_Conc_5,
            //Выбросы
            double PDZ_1_CO_Emis,
            double PDZ_1_CO2_Emis,
            double PDZ_1_NO_Emis,
            double PDZ_1_NO2_Emis,
            double PDZ_1_NOx_Emis,
            double PDZ_1_SO2_Emis,
            double PDZ_1_Dust_Emis,
            double PDZ_1_CH4_Emis,
            double PDZ_1_H2S_Emis,
            double PDZ_1_NH3_Emis,
            double PDZ_1_Add_Emis_1,
            double PDZ_1_Add_Emis_2,
            double PDZ_1_Add_Emis_3,
            double PDZ_1_Add_Emis_4,
            double PDZ_1_Add_Emis_5,



            //ПДЗ 2
            string PDZ_2_Name,
            //Концентрации
            double PDZ_2_CO_Conc,
            double PDZ_2_CO2_Conc,
            double PDZ_2_NO_Conc,
            double PDZ_2_NO2_Conc,
            double PDZ_2_NOx_Conc,
            double PDZ_2_SO2_Conc,
            double PDZ_2_Dust_Conc,
            double PDZ_2_CH4_Conc,
            double PDZ_2_H2S_Conc,
            double PDZ_2_NH3_Conc,
            double PDZ_2_Add_Conc_1,
            double PDZ_2_Add_Conc_2,
            double PDZ_2_Add_Conc_3,
            double PDZ_2_Add_Conc_4,
            double PDZ_2_Add_Conc_5,
            //Выбросы
            double PDZ_2_CO_Emis,
            double PDZ_2_CO2_Emis,
            double PDZ_2_NO_Emis,
            double PDZ_2_NO2_Emis,
            double PDZ_2_NOx_Emis,
            double PDZ_2_SO2_Emis,
            double PDZ_2_Dust_Emis,
            double PDZ_2_CH4_Emis,
            double PDZ_2_H2S_Emis,
            double PDZ_2_NH3_Emis,
            double PDZ_2_Add_Emis_1,
            double PDZ_2_Add_Emis_2,
            double PDZ_2_Add_Emis_3,
            double PDZ_2_Add_Emis_4,
            double PDZ_2_Add_Emis_5,


            //ПДЗ 3
            string PDZ_3_Name,
            //Концентрации
            double PDZ_3_CO_Conc,
            double PDZ_3_CO2_Conc,
            double PDZ_3_NO_Conc,
            double PDZ_3_NO2_Conc,
            double PDZ_3_NOx_Conc,
            double PDZ_3_SO2_Conc,
            double PDZ_3_Dust_Conc,
            double PDZ_3_CH4_Conc,
            double PDZ_3_H2S_Conc,
            double PDZ_3_NH3_Conc,
            double PDZ_3_Add_Conc_1,
            double PDZ_3_Add_Conc_2,
            double PDZ_3_Add_Conc_3,
            double PDZ_3_Add_Conc_4,
            double PDZ_3_Add_Conc_5,
            //Выбросы
            double PDZ_3_CO_Emis,
            double PDZ_3_CO2_Emis,
            double PDZ_3_NO_Emis,
            double PDZ_3_NO2_Emis,
            double PDZ_3_NOx_Emis,
            double PDZ_3_SO2_Emis,
            double PDZ_3_Dust_Emis,
            double PDZ_3_CH4_Emis,
            double PDZ_3_H2S_Emis,
            double PDZ_3_NH3_Emis,
            double PDZ_3_Add_Emis_1,
            double PDZ_3_Add_Emis_2,
            double PDZ_3_Add_Emis_3,
            double PDZ_3_Add_Emis_4,
            double PDZ_3_Add_Emis_5,



            int PDZ_Active
            )
        {
            //if (Accaunt.isValid)
            //{



            //ПДЗ 1
            GlobalStaticSettingsASK.PDZ.PDZ_1_Name = PDZ_1_Name;

            GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Conc = PDZ_1_CO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Conc = PDZ_1_CO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Conc = PDZ_1_NO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO2_Conc = PDZ_1_NO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Conc = PDZ_1_NOx_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Conc = PDZ_1_SO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Conc = PDZ_1_Dust_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Conc = PDZ_1_CH4_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Conc = PDZ_1_H2S_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NH3_Conc = PDZ_1_NH3_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_1 = PDZ_1_Add_Conc_1;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_2 = PDZ_1_Add_Conc_2;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_3 = PDZ_1_Add_Conc_3;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_4 = PDZ_1_Add_Conc_4;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_5 = PDZ_1_Add_Conc_5;

            GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Emis = PDZ_1_CO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Emis = PDZ_1_CO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Emis = PDZ_1_NO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO2_Emis = PDZ_1_NO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Emis = PDZ_1_NOx_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Emis = PDZ_1_SO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Emis = PDZ_1_Dust_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Emis = PDZ_1_CH4_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Emis = PDZ_1_H2S_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NH3_Emis = PDZ_1_NH3_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_1 = PDZ_1_Add_Emis_1;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_2 = PDZ_1_Add_Emis_2;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_3 = PDZ_1_Add_Emis_3;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_4 = PDZ_1_Add_Emis_4;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_5 = PDZ_1_Add_Emis_5;


            //ПДЗ 2
            GlobalStaticSettingsASK.PDZ.PDZ_2_Name = PDZ_2_Name;

            GlobalStaticSettingsASK.PDZ.PDZ_2_CO_Conc = PDZ_2_CO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_CO2_Conc = PDZ_2_CO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NO_Conc = PDZ_2_NO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NO2_Conc = PDZ_2_NO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NOx_Conc = PDZ_2_NOx_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_SO2_Conc = PDZ_2_SO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Dust_Conc = PDZ_2_Dust_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_CH4_Conc = PDZ_2_CH4_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_H2S_Conc = PDZ_2_H2S_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NH3_Conc = PDZ_2_NH3_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_1 = PDZ_2_Add_Conc_1;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_2 = PDZ_2_Add_Conc_2;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_3 = PDZ_2_Add_Conc_3;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_4 = PDZ_2_Add_Conc_4;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_5 = PDZ_2_Add_Conc_5;

            GlobalStaticSettingsASK.PDZ.PDZ_2_CO_Emis = PDZ_2_CO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_CO2_Emis = PDZ_2_CO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NO_Emis = PDZ_2_NO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NO2_Emis = PDZ_2_NO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NOx_Emis = PDZ_2_NOx_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_SO2_Emis = PDZ_2_SO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Dust_Emis = PDZ_2_Dust_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_CH4_Emis = PDZ_2_CH4_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_H2S_Emis = PDZ_2_H2S_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_NH3_Emis = PDZ_2_NH3_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Emis_1 = PDZ_2_Add_Emis_1;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Emis_2 = PDZ_2_Add_Emis_2;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Emis_3 = PDZ_2_Add_Emis_3;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Emis_4 = PDZ_2_Add_Emis_4;
            GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Emis_5 = PDZ_2_Add_Emis_5;


            //ПДЗ 3
            GlobalStaticSettingsASK.PDZ.PDZ_3_Name = PDZ_3_Name;

            GlobalStaticSettingsASK.PDZ.PDZ_3_CO_Conc = PDZ_3_CO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_CO2_Conc = PDZ_3_CO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NO_Conc = PDZ_3_NO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NO2_Conc = PDZ_3_NO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NOx_Conc = PDZ_3_NOx_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_SO2_Conc = PDZ_3_SO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Dust_Conc = PDZ_3_Dust_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_CH4_Conc = PDZ_3_CH4_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_H2S_Conc = PDZ_3_H2S_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NH3_Conc = PDZ_3_NH3_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_1 = PDZ_3_Add_Conc_1;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_2 = PDZ_3_Add_Conc_2;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_3 = PDZ_3_Add_Conc_3;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_4 = PDZ_3_Add_Conc_4;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_5 = PDZ_3_Add_Conc_5;

            GlobalStaticSettingsASK.PDZ.PDZ_3_CO_Emis = PDZ_3_CO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_CO2_Emis = PDZ_3_CO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NO_Emis = PDZ_3_NO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NO2_Emis = PDZ_3_NO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NOx_Emis = PDZ_3_NOx_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_SO2_Emis = PDZ_3_SO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Dust_Emis = PDZ_3_Dust_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_CH4_Emis = PDZ_3_CH4_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_H2S_Emis = PDZ_3_H2S_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_NH3_Emis = PDZ_3_NH3_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_1 = PDZ_3_Add_Emis_1;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_2 = PDZ_3_Add_Emis_2;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_3 = PDZ_3_Add_Emis_3;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_4 = PDZ_3_Add_Emis_4;
            GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_5 = PDZ_3_Add_Emis_5;

            GlobalStaticSettingsASK.PDZ.Is_Active = PDZ_Active;

            GlobalStaticSettingsASK.SavePdz_JSON();

            return new NoContentResult();
            //}
            //else
            //{
            //    return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            //}
        }


        [HttpPost]
        public IActionResult PDZ_ONE(  //Концентрации
                                        double PDZ_1_CO_Conc,
                                        double PDZ_1_CO2_Conc,
                                        double PDZ_1_NO_Conc,
                                        double PDZ_1_NO2_Conc,
                                        double PDZ_1_NOx_Conc,
                                        double PDZ_1_SO2_Conc,
                                        double PDZ_1_Dust_Conc,
                                        double PDZ_1_CH4_Conc,
                                        double PDZ_1_H2S_Conc,
                                        double PDZ_1_NH3_Conc,
                                        double PDZ_1_Add_Conc_1,
                                        double PDZ_1_Add_Conc_2,
                                        double PDZ_1_Add_Conc_3,
                                        double PDZ_1_Add_Conc_4,
                                        double PDZ_1_Add_Conc_5,
                                        //Выбросы
                                        double PDZ_1_CO_Emis,
                                        double PDZ_1_CO2_Emis,
                                        double PDZ_1_NO_Emis,
                                        double PDZ_1_NO2_Emis,
                                        double PDZ_1_NOx_Emis,
                                        double PDZ_1_SO2_Emis,
                                        double PDZ_1_Dust_Emis,
                                        double PDZ_1_CH4_Emis,
                                        double PDZ_1_H2S_Emis,
                                        double PDZ_1_NH3_Emis,
                                        double PDZ_1_Add_Emis_1,
                                        double PDZ_1_Add_Emis_2,
                                        double PDZ_1_Add_Emis_3,
                                        double PDZ_1_Add_Emis_4,
                                        double PDZ_1_Add_Emis_5
            )
        {
            //if (Accaunt.isValid)
            //{
            GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Conc = PDZ_1_CO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Conc = PDZ_1_CO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Conc = PDZ_1_NO_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO2_Conc = PDZ_1_NO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Conc = PDZ_1_NOx_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Conc = PDZ_1_SO2_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Conc = PDZ_1_Dust_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Conc = PDZ_1_CH4_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Conc = PDZ_1_H2S_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NH3_Conc = PDZ_1_NH3_Conc;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_1 = PDZ_1_Add_Conc_1;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_2 = PDZ_1_Add_Conc_2;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_3 = PDZ_1_Add_Conc_3;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_4 = PDZ_1_Add_Conc_4;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_5 = PDZ_1_Add_Conc_5;

            GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Emis = PDZ_1_CO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Emis = PDZ_1_CO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Emis = PDZ_1_NO_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NO2_Emis = PDZ_1_NO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Emis = PDZ_1_NOx_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Emis = PDZ_1_SO2_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Emis = PDZ_1_Dust_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Emis = PDZ_1_CH4_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Emis = PDZ_1_H2S_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_NH3_Emis = PDZ_1_NH3_Emis;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_1 = PDZ_1_Add_Emis_1;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_2 = PDZ_1_Add_Emis_2;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_3 = PDZ_1_Add_Emis_3;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_4 = PDZ_1_Add_Emis_4;
            GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_5 = PDZ_1_Add_Emis_5;

            GlobalStaticSettingsASK.SavePdz_JSON();
            return new NoContentResult();




            //}
            //else
            //{
            //    return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            //}
        }


        [HttpPost]
        public IActionResult CheckBox(
                                        //Концентрации
                                        bool check_CO_Conc,
                                        bool check_CO2_Conc,
                                        bool check_NO_Conc,
                                        bool check_NO2_Conc,
                                        bool check_NOx_Conc,
                                        bool check_SO2_Conc,
                                        bool check_Dust_Conc,
                                        bool check_CH4_Conc,
                                        bool check_H2S_Conc,
                                        bool check_NH3_Conc,
                                        bool check_Add_Conc_1,
                                        bool check_Add_Conc_2,
                                        bool check_Add_Conc_3,
                                        bool check_Add_Conc_4,
                                        bool check_Add_Conc_5,

                                        //Выбросы
                                        bool check_CO_Emis,
                                        bool check_CO2_Emis,
                                        bool check_NO_Emis,
                                        bool check_NO2_Emis,
                                        bool check_NOx_Emis,
                                        bool check_SO2_Emis,
                                        bool check_Dust_Emis,
                                        bool check_CH4_Emis,
                                        bool check_H2S_Emis,
                                        bool check_NH3_Emis,
                                        bool check_Add_Emis_1,
                                        bool check_Add_Emis_2,
                                        bool check_Add_Emis_3,
                                        bool check_Add_Emis_4,
                                        bool check_Add_Emis_5,

                                        //Параметры
                                        bool check_O2_Wet,
                                        bool check_O2_Dry,
                                        bool check_H2O,
                                        
                                        bool check_Pressure,
                                        bool check_Temperature,
                                        bool check_Speed,
                                        bool check_Flow,
                                        
                                        bool check_Temperature_KIP,
                                        bool check_Temperature_NOx,
                                        bool check_Pressure_KIP,
                                        bool check_Temperature_Point_Dew,
                                        bool check_Temperature_Room,
                                        bool check_Temperature_PGS,
                                        bool check_O2_Room,
                                        bool check_O2_PGS,

                                        bool check_Mode_ASK,
                                        bool check_PDZ_Fuel

                                        )
        {
            //if (Accaunt.isValid)
            //{
            //Концентрации
            if (check_CO_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.CO_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.CO_Conc = "none";

            if (check_CO2_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.CO2_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.CO2_Conc = "none";

            if (check_NO_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.NO_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NO_Conc = "none";

            if (check_NO2_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.NO2_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NO2_Conc = "none";

            if (check_NOx_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.NOx_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NOx_Conc = "none";

            if (check_SO2_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.SO2_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.SO2_Conc = "none";

            if (check_Dust_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.Dust_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Dust_Conc = "none";

            if (check_CH4_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.CH4_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.CH4_Conc = "none";

            if (check_H2S_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.H2S_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.H2S_Conc = "none";

            if (check_NH3_Conc) GlobalStaticSettingsASK.VisibilityReportOptions.NH3_Conc = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NH3_Conc = "none";

            if (check_Add_Conc_1) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_1 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_1 = "none";

            if (check_Add_Conc_2) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_2 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_2 = "none";

            if (check_Add_Conc_3) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_3 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_3 = "none";

            if (check_Add_Conc_4) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_4 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_4 = "none";

            if (check_Add_Conc_5) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_5 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Conc_5 = "none";



            //Выбросы
            if (check_CO_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.CO_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.CO_Emis = "none";

            if (check_CO2_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.CO2_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.CO2_Emis = "none";

            if (check_NO_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.NO_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NO_Emis = "none";

            if (check_NO2_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.NO2_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NO2_Emis = "none";

            if (check_NOx_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.NOx_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NOx_Emis = "none";

            if (check_SO2_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.SO2_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.SO2_Emis = "none";

            if (check_Dust_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.Dust_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Dust_Emis = "none";

            if (check_CH4_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.CH4_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.CH4_Emis = "none";

            if (check_H2S_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.H2S_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.H2S_Emis = "none";
            
            if (check_NH3_Emis) GlobalStaticSettingsASK.VisibilityReportOptions.NH3_Emis = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.NH3_Emis = "none";

            if (check_Add_Emis_1) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_1 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_1 = "none";

            if (check_Add_Emis_2) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_2 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_2 = "none";

            if (check_Add_Emis_3) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_3 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_3 = "none";

            if (check_Add_Emis_4) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_4 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_4 = "none";

            if (check_Add_Emis_5) GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_5 = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Add_Emis_5 = "none";






            //Параметры
            if (check_O2_Wet) GlobalStaticSettingsASK.VisibilityReportOptions.O2_Wet = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.O2_Wet = "none";

            if (check_O2_Dry) GlobalStaticSettingsASK.VisibilityReportOptions.O2_Dry = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.O2_Dry = "none";

            if (check_H2O) GlobalStaticSettingsASK.VisibilityReportOptions.H2O = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.H2O = "none";

            
            if (check_Pressure) GlobalStaticSettingsASK.VisibilityReportOptions.Pressure = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Pressure = "none";

            if (check_Temperature) GlobalStaticSettingsASK.VisibilityReportOptions.Temperature = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Temperature = "none";

            if (check_Speed) GlobalStaticSettingsASK.VisibilityReportOptions.Speed = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Speed = "none";

            if (check_Flow) GlobalStaticSettingsASK.VisibilityReportOptions.Flow = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Flow = "none";

            
            if (check_Temperature_KIP) GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_KIP = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_KIP = "none";

            if (check_Temperature_NOx) GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_NOx = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_NOx = "none";

            if (check_Pressure_KIP) GlobalStaticSettingsASK.VisibilityReportOptions.Pressure_KIP = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Pressure_KIP = "none";

            if (check_Temperature_Point_Dew) GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_Point_Dew = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_Point_Dew = "none";

            if (check_Temperature_Room) GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_Room = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_Room = "none";

            if (check_Temperature_PGS) GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_PGS = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Temperature_PGS = "none";

            if (check_O2_Room) GlobalStaticSettingsASK.VisibilityReportOptions.O2_Room = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.O2_Room = "none";

            if (check_O2_PGS) GlobalStaticSettingsASK.VisibilityReportOptions.O2_PGS = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.O2_PGS = "none";


            if (check_Mode_ASK) GlobalStaticSettingsASK.VisibilityReportOptions.Mode_ASK = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.Mode_ASK = "none";

            if (check_PDZ_Fuel) GlobalStaticSettingsASK.VisibilityReportOptions.PDZ_Fuel = "table-cell";
            else GlobalStaticSettingsASK.VisibilityReportOptions.PDZ_Fuel = "none";





            GlobalStaticSettingsASK.SaveVisibilityReportOptionsJSON();

            return new NoContentResult();
            //}
            //else
            //{
            //    return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            //}

        }


        [HttpPost]
        public IActionResult SaveSansorRanger(
                                                double SensorRange_Min_CO,
                                                double SensorRange_Max_CO,
                                                double SensorRange_Min_CO2,
                                                double SensorRange_Max_CO2,
                                                double SensorRange_Min_NO,
                                                double SensorRange_Max_NO,
                                                double SensorRange_Min_NO2,
                                                double SensorRange_Max_NO2,
                                                double SensorRange_Min_NOx,
                                                double SensorRange_Max_NOx,
                                                double SensorRange_Min_SO2,
                                                double SensorRange_Max_SO2,
                                                double SensorRange_Min_Dust,
                                                double SensorRange_Max_Dust,
                                                double SensorRange_Min_CH4,
                                                double SensorRange_Max_CH4,
                                                double SensorRange_Min_H2S,
                                                double SensorRange_Max_H2S,
                                                double SensorRange_Min_NH3,
                                                double SensorRange_Max_NH3,

                                                double SensorRange_Min_Rezerv_1,
                                                double SensorRange_Max_Rezerv_1,
                                                double SensorRange_Min_Rezerv_2,
                                                double SensorRange_Max_Rezerv_2,
                                                double SensorRange_Min_Rezerv_3,
                                                double SensorRange_Max_Rezerv_3,
                                                double SensorRange_Min_Rezerv_4,
                                                double SensorRange_Max_Rezerv_4,
                                                double SensorRange_Min_Rezerv_5,
                                                double SensorRange_Max_Rezerv_5,

                                                double SensorRange_Min_O2_Wet,
                                                double SensorRange_Max_O2_Wet,
                                                double SensorRange_Min_O2_Dry,
                                                double SensorRange_Max_O2_Dry,
                                                double SensorRange_Min_H2O,
                                                double SensorRange_Max_H2O,

                                                double SensorRange_Min_Pressure,
                                                double SensorRange_Max_Pressure,
                                                double SensorRange_Min_Temperature,
                                                double SensorRange_Max_Temperature,
                                                double SensorRange_Min_Speed,
                                                double SensorRange_Max_Speed,

                                                double SensorRange_Min_Temperature_KIP,
                                                double SensorRange_Max_Temperature_KIP,
                                                double SensorRange_Min_Temperature_NOx,
                                                double SensorRange_Max_Temperature_NOx,
                                                double SensorRange_Min_Pressure_KIP,
                                                double SensorRange_Max_Pressure_KIP,
                                                double SensorRange_Min_Temperature_Point_Dew,
                                                double SensorRange_Max_Temperature_Point_Dew,
                                                double SensorRange_Min_Temperature_Room,
                                                double SensorRange_Max_Temperature_Room,
                                                double SensorRange_Min_Temperature_PGS,
                                                double SensorRange_Max_Temperature_PGS,
                                                double SensorRange_Min_O2_Room,
                                                double SensorRange_Max_O2_Room,
                                                double SensorRange_Min_O2_PGS,
                                                double SensorRange_Max_O2_PGS,

                                                //Датчики
                                                bool check_CO_Used,
                                                bool check_CO2_Used,
                                                bool check_NO_Used,
                                                bool check_NO2_Used,
                                                bool check_NOx_Used,
                                                bool check_SO2_Used,
                                                bool check_Dust_Used,
                                                bool check_CH4_Used,
                                                bool check_H2S_Used,
                                                bool check_NH3_Used,
                                                bool check_Rezerv_1_Used,
                                                bool check_Rezerv_2_Used,
                                                bool check_Rezerv_3_Used,
                                                bool check_Rezerv_4_Used,
                                                bool check_Rezerv_5_Used,

                                                bool check_O2Wet_Used,
                                                bool check_O2Dry_Used,
                                                bool check_H2O_Used,
                                                bool check_Pressure_Used,
                                                bool check_Temperature_Used,
                                                bool check_Speed_Used,
                                                
                                                bool check_Temperature_KIP_Used,
                                                bool check_Temperature_NOx_Used,
                                                bool check_Pressure_KIP_Used,
                                                bool check_Temperature_Point_Dew_Used,
                                                bool check_Temperature_Room_Used,
                                                bool check_Temperature_PGS_Used,
                                                bool check_O2_Room_Used,
                                                bool check_O2_PGS_Used,

                                                bool check_CO_ppm,
                                                bool check_CO2_ppm,
                                                bool check_NO_ppm,
                                                bool check_NO2_ppm,
                                                bool check_NOx_ppm,
                                                bool check_SO2_ppm,
                                                bool check_Dust_ppm,
                                                bool check_CH4_ppm,
                                                bool check_H2S_ppm,
                                                bool check_NH3_ppm,
                                                bool check_Rezerv_1_ppm,
                                                bool check_Rezerv_2_ppm,
                                                bool check_Rezerv_3_ppm,
                                                bool check_Rezerv_4_ppm,
                                                bool check_Rezerv_5_ppm
                                             )
        {
            //if(Accaunt.isValid)
            //{
            GlobalStaticSettingsASK.SensorRange.CO.Min = SensorRange_Min_CO;
            GlobalStaticSettingsASK.SensorRange.CO.Max = SensorRange_Max_CO;
            GlobalStaticSettingsASK.SensorRange.CO2.Min = SensorRange_Min_CO2;
            GlobalStaticSettingsASK.SensorRange.CO2.Max = SensorRange_Max_CO2;
            GlobalStaticSettingsASK.SensorRange.NO.Min = SensorRange_Min_NO;
            GlobalStaticSettingsASK.SensorRange.NO.Max = SensorRange_Max_NO;
            GlobalStaticSettingsASK.SensorRange.NO2.Min = SensorRange_Min_NO2;
            GlobalStaticSettingsASK.SensorRange.NO2.Max = SensorRange_Max_NO2;
            GlobalStaticSettingsASK.SensorRange.NOx.Min = SensorRange_Min_NOx;
            GlobalStaticSettingsASK.SensorRange.NOx.Max = SensorRange_Max_NOx;
            GlobalStaticSettingsASK.SensorRange.SO2.Min = SensorRange_Min_SO2;
            GlobalStaticSettingsASK.SensorRange.SO2.Max = SensorRange_Max_SO2;
            GlobalStaticSettingsASK.SensorRange.Dust.Min = SensorRange_Min_Dust;
            GlobalStaticSettingsASK.SensorRange.Dust.Max = SensorRange_Max_Dust;

            GlobalStaticSettingsASK.SensorRange.CH4.Min = SensorRange_Min_CH4;
            GlobalStaticSettingsASK.SensorRange.CH4.Max = SensorRange_Max_CH4;
            GlobalStaticSettingsASK.SensorRange.H2S.Min = SensorRange_Min_H2S;
            GlobalStaticSettingsASK.SensorRange.H2S.Max = SensorRange_Max_H2S;
            GlobalStaticSettingsASK.SensorRange.NH3.Min = SensorRange_Min_NH3;
            GlobalStaticSettingsASK.SensorRange.NH3.Max = SensorRange_Max_NH3;

            GlobalStaticSettingsASK.SensorRange.Rezerv_1.Min = SensorRange_Min_Rezerv_1;
            GlobalStaticSettingsASK.SensorRange.Rezerv_1.Max = SensorRange_Max_Rezerv_1;
            GlobalStaticSettingsASK.SensorRange.Rezerv_2.Min = SensorRange_Min_Rezerv_2;
            GlobalStaticSettingsASK.SensorRange.Rezerv_2.Max = SensorRange_Max_Rezerv_2;
            GlobalStaticSettingsASK.SensorRange.Rezerv_3.Min = SensorRange_Min_Rezerv_3;
            GlobalStaticSettingsASK.SensorRange.Rezerv_3.Max = SensorRange_Max_Rezerv_3;
            GlobalStaticSettingsASK.SensorRange.Rezerv_4.Min = SensorRange_Min_Rezerv_4;
            GlobalStaticSettingsASK.SensorRange.Rezerv_4.Max = SensorRange_Max_Rezerv_4;
            GlobalStaticSettingsASK.SensorRange.Rezerv_5.Min = SensorRange_Min_Rezerv_5;
            GlobalStaticSettingsASK.SensorRange.Rezerv_5.Max = SensorRange_Max_Rezerv_5;

            GlobalStaticSettingsASK.SensorRange.O2Wet.Min = SensorRange_Min_O2_Wet;
            GlobalStaticSettingsASK.SensorRange.O2Wet.Max = SensorRange_Max_O2_Wet;
            GlobalStaticSettingsASK.SensorRange.O2Dry.Min = SensorRange_Min_O2_Dry;
            GlobalStaticSettingsASK.SensorRange.O2Dry.Max = SensorRange_Max_O2_Dry;
            GlobalStaticSettingsASK.SensorRange.H2O.Min = SensorRange_Min_H2O;
            GlobalStaticSettingsASK.SensorRange.H2O.Max = SensorRange_Max_H2O;

            GlobalStaticSettingsASK.SensorRange.Pressure.Min = SensorRange_Min_Pressure;
            GlobalStaticSettingsASK.SensorRange.Pressure.Max = SensorRange_Max_Pressure;
            GlobalStaticSettingsASK.SensorRange.Temperature.Min = SensorRange_Min_Temperature;
            GlobalStaticSettingsASK.SensorRange.Temperature.Max = SensorRange_Max_Temperature;
            GlobalStaticSettingsASK.SensorRange.Speed.Min = SensorRange_Min_Speed;
            GlobalStaticSettingsASK.SensorRange.Speed.Max = SensorRange_Max_Speed;
            
            GlobalStaticSettingsASK.SensorRange.Temperature_KIP.Min = SensorRange_Min_Temperature_KIP;
            GlobalStaticSettingsASK.SensorRange.Temperature_KIP.Max = SensorRange_Max_Temperature_KIP;
            GlobalStaticSettingsASK.SensorRange.Temperature_NOx.Min = SensorRange_Min_Temperature_NOx;
            GlobalStaticSettingsASK.SensorRange.Temperature_NOx.Max = SensorRange_Max_Temperature_NOx;
            GlobalStaticSettingsASK.SensorRange.Pressure_KIP.Min = SensorRange_Min_Pressure_KIP;
            GlobalStaticSettingsASK.SensorRange.Pressure_KIP.Max = SensorRange_Max_Pressure_KIP;
            GlobalStaticSettingsASK.SensorRange.Temperature_Point_Dew.Min = SensorRange_Min_Temperature_Point_Dew;
            GlobalStaticSettingsASK.SensorRange.Temperature_Point_Dew.Max = SensorRange_Max_Temperature_Point_Dew;
            GlobalStaticSettingsASK.SensorRange.Temperature_Room.Min = SensorRange_Min_Temperature_Room;
            GlobalStaticSettingsASK.SensorRange.Temperature_Room.Max = SensorRange_Max_Temperature_Room;
            GlobalStaticSettingsASK.SensorRange.Temperature_PGS.Min = SensorRange_Min_Temperature_PGS;
            GlobalStaticSettingsASK.SensorRange.Temperature_PGS.Max = SensorRange_Max_Temperature_PGS;
            GlobalStaticSettingsASK.SensorRange.O2_Room.Min = SensorRange_Min_O2_Room;
            GlobalStaticSettingsASK.SensorRange.O2_Room.Max = SensorRange_Max_O2_Room;
            GlobalStaticSettingsASK.SensorRange.O2_PGS.Min = SensorRange_Min_O2_PGS;
            GlobalStaticSettingsASK.SensorRange.O2_PGS.Max = SensorRange_Max_O2_PGS;


            GlobalStaticSettingsASK.SensorRange.CO.Is_Used = check_CO_Used;
            GlobalStaticSettingsASK.SensorRange.CO2.Is_Used = check_CO2_Used;
            GlobalStaticSettingsASK.SensorRange.NO.Is_Used = check_NO_Used;
            GlobalStaticSettingsASK.SensorRange.NO2.Is_Used = check_NO2_Used;
            GlobalStaticSettingsASK.SensorRange.NOx.Is_Used = check_NOx_Used;
            GlobalStaticSettingsASK.SensorRange.SO2.Is_Used = check_SO2_Used;
            GlobalStaticSettingsASK.SensorRange.Dust.Is_Used = check_Dust_Used;
            GlobalStaticSettingsASK.SensorRange.CH4.Is_Used = check_CH4_Used;
            GlobalStaticSettingsASK.SensorRange.H2S.Is_Used = check_H2S_Used;
            GlobalStaticSettingsASK.SensorRange.NH3.Is_Used = check_NH3_Used;
            GlobalStaticSettingsASK.SensorRange.Rezerv_1.Is_Used = check_Rezerv_1_Used;
            GlobalStaticSettingsASK.SensorRange.Rezerv_2.Is_Used = check_Rezerv_2_Used;
            GlobalStaticSettingsASK.SensorRange.Rezerv_3.Is_Used = check_Rezerv_3_Used;
            GlobalStaticSettingsASK.SensorRange.Rezerv_4.Is_Used = check_Rezerv_4_Used;
            GlobalStaticSettingsASK.SensorRange.Rezerv_5.Is_Used = check_Rezerv_5_Used;

            GlobalStaticSettingsASK.SensorRange.O2Wet.Is_Used = check_O2Wet_Used;
            GlobalStaticSettingsASK.SensorRange.O2Dry.Is_Used = check_O2Dry_Used;
            GlobalStaticSettingsASK.SensorRange.H2O.Is_Used = check_H2O_Used;
            GlobalStaticSettingsASK.SensorRange.Pressure.Is_Used = check_Pressure_Used;
            GlobalStaticSettingsASK.SensorRange.Temperature.Is_Used = check_Temperature_Used;
            GlobalStaticSettingsASK.SensorRange.Speed.Is_Used = check_Speed_Used;

            GlobalStaticSettingsASK.SensorRange.Temperature_KIP.Is_Used = check_Temperature_KIP_Used;
            GlobalStaticSettingsASK.SensorRange.Temperature_NOx.Is_Used = check_Temperature_NOx_Used;
            GlobalStaticSettingsASK.SensorRange.Pressure_KIP.Is_Used = check_Pressure_KIP_Used;
            GlobalStaticSettingsASK.SensorRange.Temperature_Point_Dew.Is_Used = check_Temperature_Point_Dew_Used;
            GlobalStaticSettingsASK.SensorRange.Temperature_Room.Is_Used = check_Temperature_Room_Used;
            GlobalStaticSettingsASK.SensorRange.Temperature_PGS.Is_Used = check_Temperature_PGS_Used;
            GlobalStaticSettingsASK.SensorRange.O2_Room.Is_Used = check_O2_Room_Used;
            GlobalStaticSettingsASK.SensorRange.O2_PGS.Is_Used = check_O2_PGS_Used;

            GlobalStaticSettingsASK.SensorRange.CO.Is_ppm = check_CO_ppm;
            GlobalStaticSettingsASK.SensorRange.CO2.Is_ppm = check_CO2_ppm;
            GlobalStaticSettingsASK.SensorRange.NO.Is_ppm = check_NO_ppm;
            GlobalStaticSettingsASK.SensorRange.NO2.Is_ppm = check_NO2_ppm;
            GlobalStaticSettingsASK.SensorRange.NOx.Is_ppm = check_NOx_ppm;
            GlobalStaticSettingsASK.SensorRange.SO2.Is_ppm = check_SO2_ppm;
            GlobalStaticSettingsASK.SensorRange.Dust.Is_ppm = check_Dust_ppm;
            GlobalStaticSettingsASK.SensorRange.CH4.Is_ppm = check_CH4_ppm;
            GlobalStaticSettingsASK.SensorRange.H2S.Is_ppm = check_H2S_ppm;
            GlobalStaticSettingsASK.SensorRange.NH3.Is_ppm = check_NH3_ppm;
            GlobalStaticSettingsASK.SensorRange.Rezerv_1.Is_ppm = check_Rezerv_1_ppm;
            GlobalStaticSettingsASK.SensorRange.Rezerv_2.Is_ppm = check_Rezerv_2_ppm;
            GlobalStaticSettingsASK.SensorRange.Rezerv_3.Is_ppm = check_Rezerv_3_ppm;
            GlobalStaticSettingsASK.SensorRange.Rezerv_4.Is_ppm = check_Rezerv_4_ppm;
            GlobalStaticSettingsASK.SensorRange.Rezerv_5.Is_ppm = check_Rezerv_5_ppm;

            GlobalStaticSettingsASK.SaveSensorRange_JSON();

            return new NoContentResult();
            //}
            //else
            //{
            //    return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            //}
        }

        [HttpPost]
        public IActionResult CalculationSetting(    //Настройки расчётов
                                                    bool is_Normalization,
                                                    bool is_Normalization_H2O,
                                                    bool is_NormalizationDust,
                                                    bool is_NormalizationDust_H2O,
                                                    bool is_NotO2,
                                                    bool is_O2Dust,
                                                    bool is_Alpha_O2Wet,
                                                    bool is_NotFlowMeter,
                                        
                                                    int typeH2O,
                                                    double koeff_H2O,
                                                    int typeNOx,
                                                    int typeDust,
                                                    double koeff_O2_Normalization,
                                                    double pipeDiameter,
                                                    double dust_DF

                                                )
        {
            GlobalStaticSettingsASK.CalculationSetting.Is_Normalization = is_Normalization;
            GlobalStaticSettingsASK.CalculationSetting.Is_NormalizationDust = is_NormalizationDust;
            GlobalStaticSettingsASK.CalculationSetting.Is_Normalization_H2O = is_Normalization_H2O;
            GlobalStaticSettingsASK.CalculationSetting.Is_NormalizationDust_H2O = is_NormalizationDust_H2O;
            GlobalStaticSettingsASK.CalculationSetting.Is_NotO2 = is_NotO2;
            GlobalStaticSettingsASK.CalculationSetting.Is_O2Dust = is_O2Dust;
            GlobalStaticSettingsASK.CalculationSetting.Is_Alpha_O2Wet = is_Alpha_O2Wet;
            GlobalStaticSettingsASK.CalculationSetting.Is_NotFlowMeter = is_NotFlowMeter;

            GlobalStaticSettingsASK.CalculationSetting.TypeH2O = (TypeH2OConc)typeH2O;
            GlobalStaticSettingsASK.CalculationSetting.Koeff_H2O = koeff_H2O;
            GlobalStaticSettingsASK.CalculationSetting.TypeNOx = (TypeNOxConc)typeNOx;
            GlobalStaticSettingsASK.CalculationSetting.Koeff_O2_Normalization = koeff_O2_Normalization;
            GlobalStaticSettingsASK.CalculationSetting.PipeDiameter = pipeDiameter;
            
            GlobalStaticSettingsASK.CalculationSetting.TypeDust = (TypeDustConc)typeDust;
            GlobalStaticSettingsASK.CalculationSetting.Dust_DF = dust_DF;

            GlobalStaticSettingsASK.SaveCalculationSettingJSON();

            return new NoContentResult();
        }


        [HttpPost]
        public IActionResult EmisStop(    //Выбросы в простое
                                                    int EmisStopType,

                                                    double EmisStop_Manual_CO,
                                                    double EmisStop_Manual_CO2,
                                                    double EmisStop_Manual_NO,
                                                    double EmisStop_Manual_NO2,
                                                    double EmisStop_Manual_NOx,
                                                    double EmisStop_Manual_SO2,
                                                    double EmisStop_Manual_Dust,
                                                    double EmisStop_Manual_CH4,
                                                    double EmisStop_Manual_H2S,
                                                    double EmisStop_Manual_NH3,
                                                    double EmisStop_Manual_Add_1,
                                                    double EmisStop_Manual_Add_2,
                                                    double EmisStop_Manual_Add_3,
                                                    double EmisStop_Manual_Add_4,
                                                    double EmisStop_Manual_Add_5,

                                                    int EmisStop_Day
                                                )
        {
            if (EmisStopType == 1)
            {
                GlobalStaticSettingsASK.emis_IsStop.isAuto = true;
            }
            else
            {
                GlobalStaticSettingsASK.emis_IsStop.isAuto = false;
            }

            GlobalStaticSettingsASK.emis_IsStop.CO_Manual = EmisStop_Manual_CO;
            GlobalStaticSettingsASK.emis_IsStop.CO_Manual = EmisStop_Manual_CO2;
            GlobalStaticSettingsASK.emis_IsStop.NO_Manual = EmisStop_Manual_NO;
            GlobalStaticSettingsASK.emis_IsStop.NO2_Manual = EmisStop_Manual_NO2;
            GlobalStaticSettingsASK.emis_IsStop.NOx_Manual = EmisStop_Manual_NOx;
            GlobalStaticSettingsASK.emis_IsStop.SO2_Manual = EmisStop_Manual_SO2;
            GlobalStaticSettingsASK.emis_IsStop.Dust_Manual = EmisStop_Manual_Dust;
            GlobalStaticSettingsASK.emis_IsStop.CH4_Manual = EmisStop_Manual_CH4;
            GlobalStaticSettingsASK.emis_IsStop.H2S_Manual = EmisStop_Manual_H2S;
            GlobalStaticSettingsASK.emis_IsStop.NH3_Manual = EmisStop_Manual_NH3;
            GlobalStaticSettingsASK.emis_IsStop.Rezerv_1_Manual = EmisStop_Manual_Add_1;
            GlobalStaticSettingsASK.emis_IsStop.Rezerv_2_Manual = EmisStop_Manual_Add_2;
            GlobalStaticSettingsASK.emis_IsStop.Rezerv_3_Manual = EmisStop_Manual_Add_3;
            GlobalStaticSettingsASK.emis_IsStop.Rezerv_4_Manual = EmisStop_Manual_Add_4;
            GlobalStaticSettingsASK.emis_IsStop.Rezerv_5_Manual = EmisStop_Manual_Add_5;

            GlobalStaticSettingsASK.emis_IsStop.Day = EmisStop_Day;



            GlobalStaticSettingsASK.SaveEmis_IsStop_JSON();

            return new NoContentResult();
        }


        [HttpPost]
        public IActionResult Q1()
        {
            if (GlobalStaticSettingsASK.Open_Valve_Ventilation)
                GlobalStaticSettingsASK.Open_Valve_Ventilation = false;
            else
                GlobalStaticSettingsASK.Open_Valve_Ventilation = true;
            
            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }


        [HttpPost]
        public IActionResult Q2()
        {
            if (GlobalStaticSettingsASK.Pusk_Fan_Ventilation)
                GlobalStaticSettingsASK.Pusk_Fan_Ventilation = false;
            else
                GlobalStaticSettingsASK.Pusk_Fan_Ventilation = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }



        [HttpPost]
        public IActionResult Q3()
        {
            if (GlobalStaticSettingsASK.Open_Heater_Ventilation)
                GlobalStaticSettingsASK.Open_Heater_Ventilation = false;
            else
                GlobalStaticSettingsASK.Open_Heater_Ventilation = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult Q4()
        {
            if (GlobalStaticSettingsASK.Close_Valve_Sample)
                GlobalStaticSettingsASK.Close_Valve_Sample = false;
            else
                GlobalStaticSettingsASK.Close_Valve_Sample = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult Q5()
        {
            if (GlobalStaticSettingsASK.Purge_Probe)
                GlobalStaticSettingsASK.Purge_Probe = false;
            else
                GlobalStaticSettingsASK.Purge_Probe = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult Q6()
        {
            if (GlobalStaticSettingsASK.Calibration_Zero_Analyzer)
                GlobalStaticSettingsASK.Calibration_Zero_Analyzer = false;
            else
                GlobalStaticSettingsASK.Calibration_Zero_Analyzer = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult Q7()
        {
            if (GlobalStaticSettingsASK.Siren_Gassed)
                GlobalStaticSettingsASK.Siren_Gassed = false;
            else
                GlobalStaticSettingsASK.Siren_Gassed = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult Q32() //НЕ DQ бит отвечающий за включение вентилатора
        {
            if (GlobalStaticSettingsASK.RUN_VENTILATION)
                GlobalStaticSettingsASK.RUN_VENTILATION = false;
            else
                GlobalStaticSettingsASK.RUN_VENTILATION = true;

            GlobalStaticSettingsASK.Is_WritePLC = true;

            return new NoContentResult();
        }
    }
}
