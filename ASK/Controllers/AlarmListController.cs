using ASK.BLL.Helper;
using ASK.BLL.Interfaces;
using ASK.BLL.Services;
using ASK.BLL.Models;
using ASK.DAL;
using ASK.DAL.Models;
using ASK.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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



        public IActionResult Alarm()
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

            return RedirectToRoute(new { controller = "AlarmList", action = "Alarm" });
        }



        [HttpPost]
        public IActionResult Alarm(bool b1)
        {
            allAlarm = true;
            alarmList = _IAlarmLog.CreateAlarmLog(allAlarm);

            return View();

        }
    }
}
