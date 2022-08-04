﻿using ASK.Controllers.Setting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading;

namespace ASK.Controllers
{
    public class SettingsController : Controller
    {


        public IActionResult Index()
        {


            if (!Accaunt.isValid)
                return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            else
                return View();
        }

        public SettingsController()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");   //Меняем культуры, что бы из вформы можно было получить 
        }                                                                                                           //тип double (c культурой RU разделитель - запятая, а с 
                                                                                                                    //формы идёт точка и при получение double = 0, по этому с
                                                                                                                    //с культурой en-US разделитель - точка и double конвертируется                                                                                                 



        [HttpPost]
        public IActionResult Index(string NameASKSystem, string NameASK, string NumberASK)
        {
            if (Accaunt.isValid)
            {
                GlobalStaticSettingsASK.SettingOptions.NameASKSystem = NameASKSystem;
                GlobalStaticSettingsASK.SettingOptions.NameASK = NameASK;
                GlobalStaticSettingsASK.SettingOptions.NumberASK = NumberASK;

                GlobalStaticSettingsASK.SaveSettingOptionsJSON();
            }
            else
            {
                return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            }

            return View();
        }

        [HttpPost]
        public IActionResult PDZ_Set()
        {
            if(Accaunt.isValid)
            {
                if (GlobalStaticSettingsASK.PDZ.UsedNumberFuel == 1)
                    GlobalStaticSettingsASK.PDZ.UsedNumberFuel = 3;
                else
                    GlobalStaticSettingsASK.PDZ.UsedNumberFuel = 1;

                GlobalStaticSettingsASK.SavePdz_JSON();

                return RedirectToRoute(new { controller = "Settings", action = "Index" });
            }
            else
            {
                return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            }
        }



        [HttpPost]
        public IActionResult PDZ_FUEL(



            //ПДЗ 1
            string PDZ_1_Name,
            //Концентрации
            double PDZ_1_CO_Conc,
            double PDZ_1_CO2_Conc,
            double PDZ_1_NO_Conc,
            double PDZ_1_NOx_Conc,
            double PDZ_1_SO2_Conc,
            double PDZ_1_Dust_Conc,
            double PDZ_1_CH4_Conc,
            double PDZ_1_H2S_Conc,
            double PDZ_1_Add_Conc_1,
            double PDZ_1_Add_Conc_2,
            double PDZ_1_Add_Conc_3,
            double PDZ_1_Add_Conc_4,
            double PDZ_1_Add_Conc_5,
            //Выбросы
            double PDZ_1_CO_Emis,
            double PDZ_1_CO2_Emis,
            double PDZ_1_NO_Emis,
            double PDZ_1_NOx_Emis,
            double PDZ_1_SO2_Emis,
            double PDZ_1_Dust_Emis,
            double PDZ_1_CH4_Emis,
            double PDZ_1_H2S_Emis,
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
            double PDZ_2_NOx_Conc,
            double PDZ_2_SO2_Conc,
            double PDZ_2_Dust_Conc,
            double PDZ_2_CH4_Conc,
            double PDZ_2_H2S_Conc,
            double PDZ_2_Add_Conc_1,
            double PDZ_2_Add_Conc_2,
            double PDZ_2_Add_Conc_3,
            double PDZ_2_Add_Conc_4,
            double PDZ_2_Add_Conc_5,
            //Выбросы
            double PDZ_2_CO_Emis,
            double PDZ_2_CO2_Emis,
            double PDZ_2_NO_Emis,
            double PDZ_2_NOx_Emis,
            double PDZ_2_SO2_Emis,
            double PDZ_2_Dust_Emis,
            double PDZ_2_CH4_Emis,
            double PDZ_2_H2S_Emis,
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
            double PDZ_3_NOx_Conc,
            double PDZ_3_SO2_Conc,
            double PDZ_3_Dust_Conc,
            double PDZ_3_CH4_Conc,
            double PDZ_3_H2S_Conc,
            double PDZ_3_Add_Conc_1,
            double PDZ_3_Add_Conc_2,
            double PDZ_3_Add_Conc_3,
            double PDZ_3_Add_Conc_4,
            double PDZ_3_Add_Conc_5,
            //Выбросы
            double PDZ_3_CO_Emis,
            double PDZ_3_CO2_Emis,
            double PDZ_3_NO_Emis,
            double PDZ_3_NOx_Emis,
            double PDZ_3_SO2_Emis,
            double PDZ_3_Dust_Emis,
            double PDZ_3_CH4_Emis,
            double PDZ_3_H2S_Emis,
            double PDZ_3_Add_Emis_1,
            double PDZ_3_Add_Emis_2,
            double PDZ_3_Add_Emis_3,
            double PDZ_3_Add_Emis_4,
            double PDZ_3_Add_Emis_5,



            int PDZ_Active
            )
        {
            if (Accaunt.isValid)
            {



                //ПДЗ 1
                GlobalStaticSettingsASK.PDZ.PDZ_1_Name = PDZ_1_Name;

                GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Conc = PDZ_1_CO_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Conc = PDZ_1_CO2_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Conc = PDZ_1_NO_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Conc = PDZ_1_NOx_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Conc = PDZ_1_SO2_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Conc = PDZ_1_Dust_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Conc = PDZ_1_CH4_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Conc = PDZ_1_H2S_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_1 = PDZ_1_Add_Conc_1;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_2 = PDZ_1_Add_Conc_2;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_3 = PDZ_1_Add_Conc_3;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_4 = PDZ_1_Add_Conc_4;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_5 = PDZ_1_Add_Conc_5;

                GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Emis = PDZ_1_CO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Emis = PDZ_1_CO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Emis = PDZ_1_NO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Emis = PDZ_1_NOx_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Emis = PDZ_1_SO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Emis = PDZ_1_Dust_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Emis = PDZ_1_CH4_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Emis = PDZ_1_H2S_Emis;
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
                GlobalStaticSettingsASK.PDZ.PDZ_2_NOx_Conc = PDZ_2_NOx_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_2_SO2_Conc = PDZ_2_SO2_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Dust_Conc = PDZ_2_Dust_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_2_CH4_Conc = PDZ_2_CH4_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_2_H2S_Conc = PDZ_2_H2S_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_1 = PDZ_2_Add_Conc_1;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_2 = PDZ_2_Add_Conc_2;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_3 = PDZ_2_Add_Conc_3;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_4 = PDZ_2_Add_Conc_4;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Add_Conc_5 = PDZ_2_Add_Conc_5;

                GlobalStaticSettingsASK.PDZ.PDZ_2_CO_Emis = PDZ_2_CO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_CO2_Emis = PDZ_2_CO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_NO_Emis = PDZ_2_NO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_NOx_Emis = PDZ_2_NOx_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_SO2_Emis = PDZ_2_SO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_Dust_Emis = PDZ_2_Dust_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_CH4_Emis = PDZ_2_CH4_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_2_H2S_Emis = PDZ_2_H2S_Emis;
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
                GlobalStaticSettingsASK.PDZ.PDZ_3_NOx_Conc = PDZ_3_NOx_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_3_SO2_Conc = PDZ_3_SO2_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Dust_Conc = PDZ_3_Dust_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_3_CH4_Conc = PDZ_3_CH4_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_3_H2S_Conc = PDZ_3_H2S_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_1 = PDZ_3_Add_Conc_1;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_2 = PDZ_3_Add_Conc_2;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_3 = PDZ_3_Add_Conc_3;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_4 = PDZ_3_Add_Conc_4;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Conc_5 = PDZ_3_Add_Conc_5;

                GlobalStaticSettingsASK.PDZ.PDZ_3_CO_Emis = PDZ_3_CO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_CO2_Emis = PDZ_3_CO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_NO_Emis = PDZ_3_NO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_NOx_Emis = PDZ_3_NOx_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_SO2_Emis = PDZ_3_SO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Dust_Emis = PDZ_3_Dust_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_CH4_Emis = PDZ_3_CH4_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_H2S_Emis = PDZ_3_H2S_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_1 = PDZ_3_Add_Emis_1;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_2 = PDZ_3_Add_Emis_2;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_3 = PDZ_3_Add_Emis_3;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_4 = PDZ_3_Add_Emis_4;
                GlobalStaticSettingsASK.PDZ.PDZ_3_Add_Emis_5 = PDZ_3_Add_Emis_5;

                GlobalStaticSettingsASK.PDZ.Is_Active = PDZ_Active;

                GlobalStaticSettingsASK.SavePdz_JSON();
                return new NoContentResult();
            }
            else
            {
                return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            }
        }


        [HttpPost]
        public  IActionResult PDZ_ONE(  //Концентрации
                                        double PDZ_1_CO_Conc,
                                        double PDZ_1_CO2_Conc,
                                        double PDZ_1_NO_Conc,
                                        double PDZ_1_NOx_Conc,
                                        double PDZ_1_SO2_Conc,
                                        double PDZ_1_Dust_Conc,
                                        double PDZ_1_CH4_Conc,
                                        double PDZ_1_H2S_Conc,
                                        double PDZ_1_Add_Conc_1,
                                        double PDZ_1_Add_Conc_2,
                                        double PDZ_1_Add_Conc_3,
                                        double PDZ_1_Add_Conc_4,
                                        double PDZ_1_Add_Conc_5,
                                        //Выбросы
                                        double PDZ_1_CO_Emis,
                                        double PDZ_1_CO2_Emis,
                                        double PDZ_1_NO_Emis,
                                        double PDZ_1_NOx_Emis,
                                        double PDZ_1_SO2_Emis,
                                        double PDZ_1_Dust_Emis,
                                        double PDZ_1_CH4_Emis,
                                        double PDZ_1_H2S_Emis,
                                        double PDZ_1_Add_Emis_1,
                                        double PDZ_1_Add_Emis_2,
                                        double PDZ_1_Add_Emis_3,
                                        double PDZ_1_Add_Emis_4,
                                        double PDZ_1_Add_Emis_5
            )
        {
            if(Accaunt.isValid)
            {
                GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Conc = PDZ_1_CO_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Conc = PDZ_1_CO2_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Conc = PDZ_1_NO_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Conc = PDZ_1_NOx_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Conc = PDZ_1_SO2_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Conc = PDZ_1_Dust_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Conc = PDZ_1_CH4_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Conc = PDZ_1_H2S_Conc;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_1 = PDZ_1_Add_Conc_1;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_2 = PDZ_1_Add_Conc_2;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_3 = PDZ_1_Add_Conc_3;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_4 = PDZ_1_Add_Conc_4;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Conc_5 = PDZ_1_Add_Conc_5;

                GlobalStaticSettingsASK.PDZ.PDZ_1_CO_Emis = PDZ_1_CO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CO2_Emis = PDZ_1_CO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NO_Emis = PDZ_1_NO_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_NOx_Emis = PDZ_1_NOx_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_SO2_Emis = PDZ_1_SO2_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Dust_Emis = PDZ_1_Dust_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_CH4_Emis = PDZ_1_CH4_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_H2S_Emis = PDZ_1_H2S_Emis;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_1 = PDZ_1_Add_Emis_1;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_2 = PDZ_1_Add_Emis_2;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_3 = PDZ_1_Add_Emis_3;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_4 = PDZ_1_Add_Emis_4;
                GlobalStaticSettingsASK.PDZ.PDZ_1_Add_Emis_5 = PDZ_1_Add_Emis_5;

                GlobalStaticSettingsASK.SavePdz_JSON();
                return new NoContentResult();

                


            }
            else
            {
                return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            }
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
                                        bool check_Mode_ASK, 
                                        bool check_PDZ_Fuel

                                        )
        {
            if (Accaunt.isValid)
            {
                //Концентрации
                if (check_CO_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.CO_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.CO_Conc = "none";

                if (check_CO2_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Conc = "none";

                if (check_NO_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.NO_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.NO_Conc = "none";

                if (check_NO2_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Conc = "none";

                if (check_NOx_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Conc = "none";

                if (check_SO2_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Conc = "none";

                if (check_Dust_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Conc = "none";

                if (check_CH4_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Conc = "none";

                if (check_H2S_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Conc = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Conc = "none";

                if (check_Add_Conc_1) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_1 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_1 = "none";

                if (check_Add_Conc_2) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_2 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_2 = "none";

                if (check_Add_Conc_3) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_3 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_3 = "none";

                if (check_Add_Conc_4) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_4 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_4 = "none";

                if (check_Add_Conc_5) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_5 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_5 = "none";



                //Выбросы
                if (check_CO_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.CO_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.CO_Emis = "none";

                if (check_CO2_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Emis = "none";

                if (check_NO_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.NO_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.NO_Emis = "none";

                if (check_NO2_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Emis = "none";

                if (check_NOx_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Emis = "none";

                if (check_SO2_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Emis = "none";

                if (check_Dust_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Emis = "none";

                if (check_CH4_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Emis = "none";

                if (check_H2S_Conc) GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Emis = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Emis = "none";

                if (check_Add_Conc_1) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_1 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_1 = "none";

                if (check_Add_Conc_2) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_2 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_2 = "none";

                if (check_Add_Conc_3) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_3 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_3 = "none";

                if (check_Add_Conc_4) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_4 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_4 = "none";

                if (check_Add_Conc_5) GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_5 = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_5 = "none";






                //Параметры
                if (check_O2_Wet) GlobalStaticSettingsASK.VisibilityOptions20M.O2_Wet = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.O2_Wet = "none";

                if (check_O2_Dry) GlobalStaticSettingsASK.VisibilityOptions20M.O2_Dry = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.O2_Dry = "none";

                if (check_H2O) GlobalStaticSettingsASK.VisibilityOptions20M.H2O = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.H2O = "none";

                if (check_Pressure) GlobalStaticSettingsASK.VisibilityOptions20M.Pressure = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Pressure = "none";

                if (check_Temperature) GlobalStaticSettingsASK.VisibilityOptions20M.Temperature = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Temperature = "none";

                if (check_Speed) GlobalStaticSettingsASK.VisibilityOptions20M.Speed = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Speed = "none";

                if (check_Flow) GlobalStaticSettingsASK.VisibilityOptions20M.Flow = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Flow = "none";

                if (check_Temperature_KIP) GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_KIP = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_KIP = "none";

                if (check_Temperature_NOx) GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_NOx = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_NOx = "none";

                if (check_Mode_ASK) GlobalStaticSettingsASK.VisibilityOptions20M.Mode_ASK = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.Mode_ASK = "none";

                if (check_PDZ_Fuel) GlobalStaticSettingsASK.VisibilityOptions20M.PDZ_Fuel = "table-cell";
                else GlobalStaticSettingsASK.VisibilityOptions20M.PDZ_Fuel = "none";





                GlobalStaticSettingsASK.SaveVisibilityOptions20MJSON();

                return new NoContentResult();
            }
            else
            {
                return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
            }
            
        }
    }
}