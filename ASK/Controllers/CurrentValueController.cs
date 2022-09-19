using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System;
using ASK.BLL.Helper;
using ASK.BLL.Helper.Setting;
using ASK.BLL.Models;
using ASK.BLL.Interfaces;

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



        public JsonResult GetCurrentTable()
        {
            //currentPageAnalog.Update();
            return Json(_CurrentPage_Services.Update());
        }



        public JsonResult GetCurrentChart()
        {
            return Json(GlobalStaticSettingsASK.ChartCurrent);
        }
    }
}
