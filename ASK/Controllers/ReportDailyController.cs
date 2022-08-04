using ASK.Models;
using ASK.Controllers.Add;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ASK.Data;
using ASK.Controllers.Excel;
using ASK.Data.Interfaces;
using ASK.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.Configuration;
using ASK.Controllers.Setting;
using OfficeOpenXml.Style;

namespace ASK.Controllers
{
    public class ReportDailyController : Controller
    {
        public static DateTime authData = DateTime.Now;                         //Текущая дата для календоря


        public static ReportDay reportDay;



        //Временно 
        Random random = new Random();

        



        ////Нужно сделать лучше, временное решение
        public static string exceedColor = "background-color: #f2aaaa96;";      //Цвет выделения при привышении;
        public static string noneValue = "-/-";
        public static string[] mode_ASK_String = new string[3] { "Работа", "Простой", "Останов" };
        public static string[] mode_ASK_String_color = new string[3] { "", "background-color: #f2aaaa96;", "background-color: #FFE4B5" };



        public void GetValue()
        {
            reportDay = new ReportDay(authData);

            //Add_20M();
            //Add_PDZ();
            //Add_ACCIDENT_LIST();
            //Add_ACCIDENT_LOG();

            //Возможно пригодится
            #region Подключаемся к другой БД - OPTIONS
            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //var options = optionsBuilder
            //        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ASK_SIMATEK_10484;Trusted_Connection=True;MultipleActiveResultSets=true")
            //        .Options;

            //using (ApplicationDbContext db = new ApplicationDbContext(options))
            //{ }
            #endregion

            //Заносим данные в таблицу выбросов
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //Parameter_20mService Parameter_20M_Service = new Parameter_20mService(db);
            //Concentration_20mService Concentration_20M_Service = new Concentration_20mService(db);
            //Emission_20mService Emission_20M_Service = new Emission_20mService(db);

            //Param_20_DAY = Parameter_20M_Service.Get_DayAll_Parameter_20m(authData);
            //Concentration_20_DAY = Concentration_20M_Service.Get_DayAll_Concentration_20m(authData);
            //Emission_20_DAY = Emission_20M_Service.Get_DayAll_Emission_20m(authData);

            //reportDay.Clear();


            //if (Param_20_DAY.Count == Concentration_20_DAY.Count && Concentration_20_DAY.Count == Emission_20_DAY.Count) //Исключаем ситуацию, что в таблицах (параметры, концентрации, выбросы) разное количество строк 
            //    for (int i = 0; i < Param_20_DAY.Count; i++)
            //    {
            //        reportDay.Add(new ReportDay(authData));


            //        reportDay[i].Date = Param_20_DAY[i].Date;

            //        reportDay[i].Mode_ASK = Param_20_DAY[i].isStop;
            //        reportDay[i].PDZ_Fuel = Param_20_DAY[i].PDZ_number; //Обязательно должен идти первее присвоения концентраций


            //        reportDay[i].CO_Conc = Concentration_20_DAY[i].CO;
            //        reportDay[i].CO2_Conc = Concentration_20_DAY[i].CO2;
            //        reportDay[i].NO_Conc = Concentration_20_DAY[i].NO;
            //        reportDay[i].NO2_Conc = Concentration_20_DAY[i].NO2;
            //        reportDay[i].NOx_Conc = Concentration_20_DAY[i].NOx;
            //        reportDay[i].SO2_Conc = Concentration_20_DAY[i].SO2;
            //        reportDay[i].Dust_Conc = Concentration_20_DAY[i].Dust;


            //        reportDay[i].CO_Emis = Emission_20_DAY[i].CO;
            //        reportDay[i].CO2_Emis = Emission_20_DAY[i].CO2;
            //        reportDay[i].NO_Emis = Emission_20_DAY[i].NO;
            //        reportDay[i].NO2_Emis = Emission_20_DAY[i].NO2;
            //        reportDay[i].NOx_Emis = Emission_20_DAY[i].NOx;
            //        reportDay[i].SO2_Emis = Emission_20_DAY[i].SO2;
            //        reportDay[i].Dust_Emis = Emission_20_DAY[i].Dust;


            //        reportDay[i].Pressure = Param_20_DAY[i].Pressure;
            //        reportDay[i].Temperature = Param_20_DAY[i].Temperature;
            //        reportDay[i].Speed = Param_20_DAY[i].Speed;
            //        reportDay[i].Сonsumption = Param_20_DAY[i].Flow;
            //        reportDay[i].O2_Dry = Param_20_DAY[i].O2_Dry;
            //        reportDay[i].O2_Wet = Param_20_DAY[i].O2_Wet;
            //        reportDay[i].H2O = Param_20_DAY[i].H2O;
            //    }
            //int pdz_fuel = 0;
            //if (reportDay.Count > 0)
            //    pdz_fuel = reportDay[0].PDZ_Fuel;

            //avgAndTotal_20M = ReportDay.AverageAndTotalList(reportDay, authData, pdz_fuel);
            //sumAndTotal_20M = ReportDay.SumTotalList(reportDay, authData, pdz_fuel);
            //}
            //ReportDay.AverageAndTotalList(reportDay, authData, pdz_fuel);
        }

        public IActionResult Index()
        {
            authData = DateTime.Now;
            GetValue();

            //var ReportDailys = GetReportDailyList();
            return View();
            //return View(ReportDailys);
        }

  

        [HttpPost]
        public IActionResult Index(DateTime tripstart)
        {
            //authData = $"Login: {login}   Password: {password}";
            //return Content(authData);

            authData = tripstart;
            //return Content(authData);
            GetValue();
            return View();
        }






        public IActionResult ExportToExcel()
        {
            //K();
            return File(ExcelReportDay.Create(reportDay, authData), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчёт за сутки на " + authData.ToShortDateString() + ".xlsx");
        }





        public void Add_20M()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                AVG_20_MINUTES_Service avg_20_M_Service = new AVG_20_MINUTES_Service(db);
                avg_20_M_Service.Create_AVG_20_MINUTES(new AVG_20_MINUTES()
                {
                    Date = GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M, // 20.06.2022 18:30:25 - формат
                    


                    Conc_CO = random.Next(0, 700),
                    Conc_CO2 = 0.0,
                    Conc_NO = 0.0,
                    Conc_NO2 = 0.0,
                    Conc_NOx = random.Next(0, 300),
                    Conc_SO2 = random.Next(0, 14),
                    Conc_Dust = random.Next(0, 700),
                    Conc_CH4 = 0.0,
                    Conc_H2S = 0.0,
                    Conc_D1 = 0.0,
                    Conc_D2 = 0.0,
                    Conc_D3 = 0.0,
                    Conc_D4 = 0.0,
                    Conc_D5 = 0.0,

                    Emis_CO = 1.19,
                    Emis_CO2 = 0.0,
                    Emis_NO = 0.054,
                    Emis_NO2 = random.Next(0, 2),
                    Emis_NOx = random.Next(0, 3),
                    Emis_SO2 = random.Next(0, 1),
                    Emis_CH4 = 0.0,
                    Emis_H2S = 0.0,
                    Emis_Dust = random.Next(0, 1),
                    Emis_D1 = 0.0,
                    Emis_D2 = 0.0,
                    Emis_D3 = 0.0,
                    Emis_D4 = 0.0,
                    Emis_D5 = 0.0,

                    O2_Wet = random.Next(16, 21),
                    O2_Dry = random.Next(15, 21),
                    H2O = 0.0,

                    Pressure = random.Next(90, 99),
                    Temperature = random.Next(100, 199),
                    Speed = random.Next(1, 23),
                    Flow = random.Next(1, 34),
                    Temperature_KIP = 0.0,
                    Temperature_NOx = 0.0,

                    Mode_ASK = random.Next(0, 2),
                    PDZ_Fuel = random.Next(0, 2)
                });
                GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M = GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M.AddMinutes(20);
            }


        }


        public void Add_ACCIDENT_LIST()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ACCIDENT_LIST_Service aCCIDENT_LIST_Service = new ACCIDENT_LIST_Service(db);

                aCCIDENT_LIST_Service.Create_ACCIDENT_LIST(new ACCIDENT_LIST()
                {
                    Accident = "1111111",
                    is_Error = true
                });
            }
        }


        public void Add_ACCIDENT_LOG()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ACCIDENT_LOG_Service aCCIDENT_LOG_Service = new ACCIDENT_LOG_Service(db);

                aCCIDENT_LOG_Service.Create_ACCIDENT_LOG(new ACCIDENT_LOG()
                {
                    Date_Begin = DateTime.Now,
                    id_accident = 1,
                    Time_End = DateTime.Now,
                    Is_Active = true
                }) ;
            }
        }

        public void Add_PDZ()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                PDZ_Service pdz_Service = new PDZ_Service(db);
                pdz_Service.Create_PDZ(new PDZ()
                {

                    Date = new DateTime(2022, 07, 26, 00, 19, 03), // 20.06.2022 18:30:25

                    CO_Conc = 409.0,
                    CO2_Conc = 9999999.0,
                    NO_Conc = 9999999.0,
                    NO2_Conc = 9999999.0,
                    NOx_Conc = 201.0,
                    SO2_Conc = 9.0,
                    Dust_Conc = 45.0,
                    CH4_Conc = 9999999.0,
                    H2S_Conc = 9999999.0,
                    Add_Conc_1 = 9999999.0,
                    Add_Conc_2 = 9999999.0,
                    Add_Conc_3 = 9999999.0,
                    Add_Conc_4 = 9999999.0,
                    Add_Conc_5 = 9999999.0,

                    CO_Emis = 8.897,
                    CO2_Emis = 9999999.0,
                    NO_Emis = 9999999.0,
                    NO2_Emis = 9999999.0,
                    NOx_Emis = 4.402,
                    SO2_Emis = 0.206,
                    CH4_Emis = 9999999.0,
                    H2S_Emis = 9999999.0,
                    Dust_Emis = 0.988,
                    Add_Emis_1 = 9999999.0,
                    Add_Emis_2 = 9999999.0,
                    Add_Emis_3 = 9999999.0,
                    Add_Emis_4 = 9999999.0,
                    Add_Emis_5 = 9999999.0,

                    NumberPDZ = 1,
                    Current = true

                });
            }
        }


















     
    }
}
