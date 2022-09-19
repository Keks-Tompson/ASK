using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface IAlarmLog
    {
        public List<AlarmLog_Model> CreateAlarmLog(bool allAlarm);
    }
}
