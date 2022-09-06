using ASK.BLL.Helper;
using ASK.BLL.Services;
using ASK.DAL;
using ASK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ASK.Controllers
{
    public class AlarmListController : Controller
    {
        
        public static List<AlarmList> alarmList;

        public static bool allAlarm;
        
        public IActionResult Alarm()
        {
            allAlarm = false;
            CreateAlarmList();

            return View();
        }


        [HttpPost]
        public IActionResult Alarm2(bool b1, bool b2)
        {
            allAlarm = false;
            CreateAlarmList();

            return RedirectToRoute(new { controller = "AlarmList", action = "Alarm" });

        }

        [HttpPost]
        public IActionResult Alarm(bool b1)
        {
            allAlarm = true;
            CreateAlarmList();


            return View();

        }

        public void CreateAlarmList()
        {
            alarmList = new List<AlarmList>();


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ACCIDENT_LOG_Service accident_log_Service = new ACCIDENT_LOG_Service(db);
                ACCIDENT_LIST_Service accident_list_Service = new ACCIDENT_LIST_Service(db);

                List<ACCIDENT_LOG> accident_log;
                List<ACCIDENT_LIST> accident_list = accident_list_Service.Get_All_ACCIDENT_LIST();


                if (allAlarm)
                    accident_log = accident_log_Service.Get_All_ACCIDENT_LOG();
                else
                    accident_log = accident_log_Service.Get_All_ACCIDENT_LOG_Active();
                


                alarmList.Clear();

                AlarmList buffAlarm;

                for (int i = 0; i < accident_log.Count; i++)
                {
                    buffAlarm = new AlarmList();


                    buffAlarm.Accident = accident_list.First(f => f.Id == accident_log[i].id_accident).Accident;
                    buffAlarm.timeBegin = accident_log[i].Date_Begin;
                    buffAlarm.timeEnd = accident_log[i].Time_End;
                    buffAlarm.is_Error = accident_list.First(f => f.Id == accident_log[i].id_accident).is_Error;
                    buffAlarm.Is_Active = accident_log[i].Is_Active;

                    if (buffAlarm.is_Error)
                    {
                        if (accident_log[i].Is_Active)
                        {
                            buffAlarm.ColorAlarm = "alert-danger";
                            buffAlarm.ColorTr = "table-danger";
                            buffAlarm.IconAlarm = "#exclamation-triangle-fill";
                        }
                        else
                        {
                            buffAlarm.ColorAlarm = "alert-success";
                            buffAlarm.ColorTr = "table-success";
                            buffAlarm.IconAlarm = "#check-circle-fill";
                        }
                    }
                    else
                    {
                        if (accident_log[i].Is_Active)
                        {
                            buffAlarm.ColorAlarm = "alert-warning";
                            buffAlarm.ColorTr = "table-warning";
                            buffAlarm.IconAlarm = "#info-fill";
                        }
                        else
                        {
                            buffAlarm.ColorAlarm = "alert-primary";
                            buffAlarm.ColorTr = "table-info";
                            buffAlarm.IconAlarm = "#info-fill";
                        }

                        
                    }

                   
                    alarmList.Insert(0, buffAlarm);
                   

                    
                }
            }

        }
    }
}
