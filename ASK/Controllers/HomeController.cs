using ASK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASK.Controllers.Setting;

namespace ASK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult Write20M()
        {
            bool[] masBoolError = new bool[2];

            masBoolError[0] = GlobalStaticSettingsASK.stopGetSernsorNow;
            masBoolError[1] = GlobalStaticSettingsASK.isNotConnection;

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
