using Microsoft.AspNetCore.Mvc;
using ASK.Controllers.Add;
using ASK.Controllers.Setting;
using ASK.Controllers.Add.AJAX_Page.CurrentValue;
using System.Collections.Generic;
using System;

namespace ASK.Controllers
{
    public class CurrentValueController : Controller
    {
        public static CurrentPageAnalog currentPageAnalog = new CurrentPageAnalog();

        public static ColorSensor_Eror color_Conc;
        public static ColorSensor_Eror color_Emis;
        public static ColorSensorParametr_Error color_Param;

        public IActionResult Index()
        {
            //GetColor();

            currentPageAnalog.Update();

            return View();
        }



        public JsonResult GetCurrentTable()
        {
            currentPageAnalog.Update();


            return Json(currentPageAnalog);
        }


        public JsonResult GetCurrentChart()
        {
            return Json(GlobalStaticSettingsASK.ChartCurrent);
        }


        //public JsonResult GetArrayChart()
        //{
        //    List<Array20M> testArr = new List<Array20M>();

        //    List<double> testArr1 = new List<double>();

        //    double[] Mass = new double[3];

        //    testArr.Add(new Array20M());
        //    testArr.Add(new Array20M());
        //    testArr.Add(new Array20M());

        //    testArr[0].Date = DateTime.Now;
        //    testArr[1].Date = DateTime.Now;
        //    testArr[2].Date = DateTime.Now;

        //    testArr[0].CO_Conc = 10.0;
        //    testArr[1].CO_Conc = 15.0;
        //    testArr[2].CO_Conc = 11.0;

        //    testArr1[0] = 10.0;
        //    testArr1[1] = 15.0;
        //    testArr1[2] = 11.0;


        //    Mass[0] = 10.0;
        //    Mass[1] = 15.0;
        //    Mass[2] = 11.0;

            

        //    return Json(Mass);
        //}


        //public void GetColor()
        //{
        //    color_Conc = new ColorSensor_Eror();
        //    color_Emis = new ColorSensor_Eror();

        //    color_Param = ColorSensorParametr_Error.Update();
        //}
    }

    
}
