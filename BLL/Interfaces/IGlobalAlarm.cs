using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface IGlobalAlarm
    {
        public void AlarmLogBuider(bool is_newValue, bool is_oldValue, Alarm_Model alarmName);
        public void AlarmLogBuiderNew(Alarm_Model alarmName);
    }
}
