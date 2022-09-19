using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface IGlobalAlarm
    {
        public void AlarmLogBuider(bool is_newValue, Alarm_Model alarmName);
    }
}
