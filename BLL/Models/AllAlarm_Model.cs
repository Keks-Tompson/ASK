using ASK.BLL.Helper.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class AllAlarm_Model
    {
        public SensorAlarm_Model SensorAlarm { get; set; } = new SensorAlarm_Model();       //Аварии обырывов датчиков

        public GasAnalyzer_Model GasAnalyzer { get; set; } = new GasAnalyzer_Model();       //Аварии Газоанализаторов

        public ASK10626_Alarms ASK10626 { get; set; } = new ASK10626_Alarms();              //Аварии для проекта 10626
    }
}
