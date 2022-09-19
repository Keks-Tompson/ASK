using ASK.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.Style;
using ASK.BLL.Services;
using ASK.BLL.Models;
using ASK.BLL.Interfaces;

namespace ASK.Controllers
{
    public class ReportDailyController : Controller
    {
        public static DateTime authData = DateTime.Now;                         //Текущая дата для календоря

        public static Report_Model reportDay;

        //Временно 
        Random random = new Random();

        ////Нужно сделать лучше, временное решение
        public static string exceedColor = "background-color: #f2aaaa96;";      //Цвет выделения при привышении;
        public static string noneValue = "-/-";
        public static string[] mode_ASK_String = new string[3] { "Работа", "Простой", "Останов" };
        public static string[] mode_ASK_String_color = new string[3] { "", "background-color: #f2aaaa96;", "background-color: #FFE4B5" };

        private readonly IReportDay _ReportDay_Services;
        private readonly IExcelReport _ExcelReport_Services;



        //Конструктор DI
        public ReportDailyController(IReportDay ReportDay_Services, IExcelReport ExcelReportDay_Services)
        { 
            _ReportDay_Services = ReportDay_Services;
            _ExcelReport_Services = ExcelReportDay_Services;
        }



        public void GetValue()
        {
            reportDay = _ReportDay_Services.Generate(authData);

            #region Подключаемся к другой БД - OPTIONS
            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //var options = optionsBuilder
            //        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ASK_SIMATEK_10484;Trusted_Connection=True;MultipleActiveResultSets=true")
            //        .Options;

            //using (ApplicationDbContext db = new ApplicationDbContext(options))
            //{ }
            #endregion
        }



        public IActionResult Index()
        {
            authData = DateTime.Now;
            GetValue();

            return View();
        }

  

        [HttpPost]
        public IActionResult Index(DateTime tripstart)
        {
            authData = tripstart;
           
            GetValue();
            return View();
        }



        public IActionResult ExportToExcel()
        {
            //return File(ExcelReportDay.Create(reportDay, authData), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчёт за сутки на " + authData.ToShortDateString() + ".xlsx");
            return File(_ExcelReport_Services.GenerateDefaultReport(reportDay, authData), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчёт за сутки на " + authData.ToShortDateString() + ".xlsx");
        }
    }
}
