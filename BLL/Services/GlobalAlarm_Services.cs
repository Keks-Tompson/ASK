using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using ASK.DAL;
using ASK.DAL.Interfaces;
using ASK.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Services
{
    public class GlobalAlarm_Services : IGlobalAlarm
    {
        private readonly IACCIDENT_LOG _ACCIDENT_LOG_Repository;



        //static GlobalAlarm_Services()
        //{
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        var accident_log_Service = new ACCIDENT_LOG_Repository(db);
        //        accident_log_Service.StratSystem_ACCIDENT_LOG();
        //    }
        //}



        public GlobalAlarm_Services(IACCIDENT_LOG ACCIDENT_LOG_Repository)
        {
            _ACCIDENT_LOG_Repository = ACCIDENT_LOG_Repository;
        }



        public void AlarmLogBuider(bool is_newValue, bool is_oldValue, Alarm_Model alarmName)
        {
            if (alarmName.Used) //Если авария используется
            {
                //if (is_newValue != is_oldValue)
                //{
                    if (is_newValue)
                        _ACCIDENT_LOG_Repository.Begin_ACCIDENT_LOG(alarmName.ID);
                    else
                        _ACCIDENT_LOG_Repository.End_ACCIDENT_LOG(alarmName.ID);
                    
                    //alarmName.Value = is_newValue;
                //}
            }
        }
    }
}
