using ASK.Controllers.Excel;
using ASK.Controllers.Report;
using Microsoft.AspNetCore.Mvc;
using System;


namespace ASK.Controllers
{
    public class ReportMonthlyController : Controller
    {
        public static bool[] masMonth = new bool[13];
        public static bool[] masYear = new bool[16];

        public static DateTime buffauthData = DateTime.Now;

        public static DateTime ReportData = DateTime.Now;

        public static int defaultMonth;
        public static int defaultYear;

        public static ReportMonth reportMonth;





        public static string exceedColor = "background: #f2aaaa96;";      //Цвет выделения при привышении;
        public static string noneValue = "-/-";
        public static string[] mode_ASK_String = new string[3] { "Работа", "Простой", "Останов" };
        public static string[] mode_ASK_String_color = new string[3] { "", "background-color: #f2aaaa96;", "background-color: #FFE4B5" };

        static string[] month = new string[] {"нулевой", "Январь", "Февраль", "Март" , "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" }; 



        //Очищаем наши combobox-ы
        public void CleareComboBox()
        {
            for (int i = 0; i < masMonth.Length; i++)
            {
                masMonth[i] = false;
            }

            for (int i = 0; i < masYear.Length; i++)
            {
                masYear[i] = false;
            }
        }




        public void GetValue()
        {
            reportMonth = new ReportMonth(new DateTime(defaultYear + 2021, defaultMonth, 1));
        }



        public IActionResult Index()
        {
            CleareComboBox();
            masMonth[buffauthData.Month] = true;
            masYear[buffauthData.Year-2021] = true;
            defaultMonth = buffauthData.Month;
            defaultYear = buffauthData.Year - 2021;


            GetValue();


            return View();
        }



        



        [HttpPost]
        public IActionResult Index(string month, string year)
        {
            defaultMonth = Convert.ToInt32(month);
            defaultYear = Convert.ToInt32(year);

            ReportData = new DateTime(defaultYear, defaultMonth, 1);

            CleareComboBox();
            

            masMonth[defaultMonth] = true;
            masYear[defaultYear] = true;


            GetValue();

            return View();
        }



        public IActionResult ExportToExcel()
        {
            //K();
            return File(ExcelReportMonth.Create(reportMonth, defaultYear, defaultMonth), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчёт за " + month[defaultMonth] + " " + (defaultYear + 2021) + ".xlsx");
        }


        //[HttpPost]
        //public IActionResult Index()
        //{
        //    //authData = $"Login: {login}   Password: {password}";
        //    //return Content(authData);

        //    authData = tripstart;
        //    //return Content(authData);
        //    GetValue();
        //    return View();
        //}

    }
}
