using System;

namespace ASK.Controllers.Add
{
    public class AlarmList
    {
        //public int ID { get; set; }
        
        public string IconAlarm { get; set; }
        public string Accident { get; set; }
        public DateTime timeBegin { get; set; }
        public DateTime timeEnd { get; set; }

        public bool is_Error { get; set; }
        public string ColorAlarm { get; set; }
        public string ColorTr { get; set; }

        public bool Is_Active { get; set; }
    }
}
