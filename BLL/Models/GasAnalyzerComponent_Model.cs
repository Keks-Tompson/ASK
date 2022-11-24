using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GasAnalyzerComponent_Model
    {
        public bool Is_Used { get; set; } = true;
        //                                                                                                          True                            /   False    
        public Alarm_Model AutomaticProtection { get; set; } = new Alarm_Model();   //Автомат защиты                Отключен                        /   Запистан
        public Alarm_Model State { get; set; } = new Alarm_Model();                 //Состояние                     В ошибке                        /   В работе
        public Alarm_Model ServiceRequest { get; set; } = new Alarm_Model();        //Запрос на обслуживание        Есть запрос на обслуживание     /   Нет запроса на обслуживание
        public Alarm_Model Service { get; set; } = new Alarm_Model();               //Обслуживание сейчас           Идёт обслуживание               /   Нет обслуживания
        public Alarm_Model AutoCalibration { get; set; } = new Alarm_Model();       //Автокалибровка                Идёт автокалибровка             /   Нет автокалибровки
        public Alarm_Model ControlCycle { get; set; } = new Alarm_Model();          //Контрольный цикл              Контрольный цикл запущен        /   Контрольный цикл отключен
    }
}
