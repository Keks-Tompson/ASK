using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using ASK.DAL.Interfaces;
using ASK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASK.BLL.Services
{
    public class AlarmLog_Services : IAlarmLog
    {
        private readonly IACCIDENT_LOG _IACCIDENT_LOG;
        private readonly IACCIDENT_LIST _ACCIDENT_LIST;



        //Конструктор DI
        public AlarmLog_Services(IACCIDENT_LOG IACCIDENT_LOG, IACCIDENT_LIST IACCIDENT_LIST)
        {
            _IACCIDENT_LOG = IACCIDENT_LOG;
            _ACCIDENT_LIST = IACCIDENT_LIST;
        }



        public List<AlarmLog_Model> CreateAlarmLog(bool allAlarm)
        {
            var alarmList = new List<AlarmLog_Model>();

            List<ACCIDENT_LOG> accident_log;
            List<ACCIDENT_LIST> accident_list = _ACCIDENT_LIST.Get_All_ACCIDENT_LIST();

            if (allAlarm)
                accident_log = _IACCIDENT_LOG.Get_All_ACCIDENT_LOG();
            else
                accident_log = _IACCIDENT_LOG.Get_All_ACCIDENT_LOG_Active();

            AlarmLog_Model buffAlarm;

            for (int i = 0; i < accident_log.Count; i++)
            {
                buffAlarm = new AlarmLog_Model();

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
            return alarmList;
        }
    }
}
