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

       


















     
    }
}
