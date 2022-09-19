using ASK.BLL.Helper.Setting;
using ASK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace ASK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IAVG_20_MINUTES _avg_20_m;



        public HomeController(ILogger<HomeController> logger/*, IAVG_20_MINUTES avg_20_m*/)
        {
            _logger = logger;
            //_avg_20_m = avg_20_m;
        }

        public IActionResult Index()
        {
            Startup.Method();
            //var a = _avg_20_m.Get_All_AVG_20_MINMUTES();
            return View();
        }


        public JsonResult Write20M()
        {
            bool[] masBoolError = new bool[2];

            masBoolError[0] = GlobalStaticSettingsASK.stopGetSernsorNow;
            masBoolError[1] = GlobalStaticSettingsASK.globalAlarms.Is_NotConnection.Value;

            return Json(masBoolError);
        }


        public IActionResult Privacy()
        {
            return View();
        }



        [HttpPost]
        public IActionResult _Layout()
        {
          
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
