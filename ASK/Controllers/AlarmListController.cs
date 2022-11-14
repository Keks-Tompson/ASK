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
using ASK.BLL.Models;
using ASK.BLL.Interfaces;

namespace ASK.Controllers
{
    public class AlarmListController : Controller
    {
        public static List<AlarmLog_Model> alarmList;
        public static bool allAlarm;

        private readonly IAlarmLog _IAlarmLog;



        //Конструктор DI
        public AlarmListController(IAlarmLog AlarmLog)
        {
            _IAlarmLog = AlarmLog;
        }



        public IActionResult Index()
        {
            allAlarm = false;
            alarmList = _IAlarmLog.CreateAlarmLog(allAlarm);

            return View();
        }



        [HttpPost]
        public IActionResult Alarm2(bool b1, bool b2)
        {
            allAlarm = false;
            alarmList = _IAlarmLog.CreateAlarmLog(allAlarm);

            return RedirectToRoute(new { controller = "AlarmList", action = "Index" });
        }



        [HttpPost]
        public IActionResult Index(bool b1)
        {
            allAlarm = true;
            alarmList = _IAlarmLog.CreateAlarmLog(allAlarm);

            return View();

        }




    
        [HttpGet]
        public ActionResult _CurrentAlarmList(string Value)
        {
            if (allAlarm)
            {
                return new NoContentResult();
            }
            else
            {
                alarmList = _IAlarmLog.CreateAlarmLog(allAlarm);

                return PartialView("_CurrentAlarmList");
            }
        }


    }
}
