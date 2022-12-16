using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ASK.Controllers
{
    
    public class StatesController : Controller
    {
        private readonly ILogger<HomeController> _logger;



        public StatesController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }



        //передаём таблицу текущих глобальных аварий
        [HttpGet]
        public ActionResult _GlobalAlarmTable(string Value)
        {

            return PartialView("_GlobalAlarmTable");
        }

        //Аварии/Состояния газоаналитического оборудования
        [HttpGet]
        public ActionResult _CurrentDI_DQ(string Value)
        {
            return PartialView("_CurrentDI_DQ");
        }

        //передаём таблицу текущих аналогов с прямыми значениями
        [HttpGet]
        public ActionResult _CurrentAnalogTable(string Value)
        {
            return PartialView("_CurrentAnalogTable");
        }
    }
}
