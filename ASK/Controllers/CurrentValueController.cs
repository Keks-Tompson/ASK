using Microsoft.AspNetCore.Mvc;


using System.Collections.Generic;
using System;
using ASK.BLL.Helper;
using ASK.BLL.Helper.Setting;
using ASK.BLL.Models;
using ASK.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;


namespace ASK.Controllers
{
    public class CurrentValueController : Controller
    {
        public static CurrentPage_Model currentPageAnalog = new CurrentPage_Model();

        public static ColorSensorEror_Model color_Conc;
        public static ColorSensorEror_Model color_Emis;
        public static ColorSensorParametrError_Model color_Param;

        private readonly ICurrentPage _CurrentPage_Services;



        public CurrentValueController(ICurrentPage CurrentPage_Services)
        {
            _CurrentPage_Services = CurrentPage_Services;
        }


        public IActionResult Index()
        {
            currentPageAnalog = _CurrentPage_Services.Update();

            return View();
        }



        //public JsonResult GetCurrentTable()
        //{
        //    //currentPageAnalog.Update();

        //    //var d = PartialView("~/Views/Calculation/Index.cshtml");

        //    //var ss = d.ContentType.ToString();

        //    return Json(_CurrentPage_Services.Update());
        //}





        public JsonResult GetCurrentChart()
        {
            return Json(GlobalStaticSettingsASK.ChartCurrent);
        }



        //Пример, если нужно передать модель с представление
        //[HttpGet]
        //public ActionResult InfoBox01(string Value)
        //{
        //    List<FoodGet> food = new List<FoodGet>
        //     {
        //     new FoodGet { Id = "1",  Value = "1", FoodName="Cheese Cake" },
        //     new FoodGet { Id = "2",  Value = "1", FoodName="Pizza" },
        //     new FoodGet { Id = "3",  Value = "1", FoodName="Hot Dog" },
        //     new FoodGet { Id = "4",  Value = "2", FoodName="Spaghetti" },
        //     new FoodGet { Id = "5",  Value = "2", FoodName="Carbonara" },
        //     new FoodGet { Id = "6",  Value = "3", FoodName="Soba" },
        //     new FoodGet { Id = "7",  Value = "4", FoodName="Samgyetang" },
        //     new FoodGet { Id = "8",  Value = "4", FoodName="Bulgogi" },
        //     };

        //    var queryFoods = from _food in food
        //                     where _food.Value == Value
        //                     select _food;

        //    return PartialView("InfoBox01", queryFoods);
        //}



        //передаём таблицу текущих концентраций
        [HttpGet]
        public ActionResult _CurrentConcTable(string Value)
        {

            return PartialView("_CurrentConcTable");
        }



        //передаём таблицу текущих выбросов
        [HttpGet]
        public ActionResult _CurrentEmisTable(string Value)
        {

            return PartialView("_CurrentEmisTable");
        }



        //передаём таблицу текущих аналогов с прямыми значениями
        [HttpGet]
        public ActionResult _CurrentAnalogTable(string Value)
        {
            currentPageAnalog = _CurrentPage_Services.Update(); ////Убрать!!!!

            return PartialView("_CurrentAnalogTable");
        }
    }
}
