using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GasAnalyzer_SICK_GM35_Model
    {
        public bool Is_Used { get; set; } = true;
        //                                                                                                              True                            /   False    
        public Alarm_Model AutomaticProtection { get; set; } = new Alarm_Model();   //Автомат защиты                    Отключен                        /   Запистан
        public Alarm_Model Error { get; set; } = new Alarm_Model();                 //Состояние                         В ошибке                        /   В работе
        public Alarm_Model ServiceRequest { get; set; } = new Alarm_Model();        //Запрос на обслуживание            Есть запрос на обслуживание     /   Нет запроса на обслуживание
        public Alarm_Model ControlCycle { get; set; } = new Alarm_Model();          //Контрольный цикл                  Контрольный цикл запущен   /   Контрольный цикл отключен


        public Alarm_Model Rezerv_Error { get; set; } = new Alarm_Model();          //Резерваня вария                   true                            /   False



        //ID расшифровки аварии в БД/ссылка на текст в БД
        public GasAnalyzer_SICK_GM35_Model()
        {
            AutomaticProtection.ID = 53;
            AutomaticProtection.Is_Used = false;
            AutomaticProtection.Is_Critical = true;

            Error.ID = 54;
            Error.Is_Used = false;
            Error.Is_Critical = true;

            ServiceRequest.ID = 55;
            ServiceRequest.Is_Used = false;
            ServiceRequest.Is_Critical = false;

            ControlCycle.ID = 56;
            ControlCycle.Is_Used = false;
            ControlCycle.Is_Critical = false;


            Rezerv_Error.ID = 57;
            Rezerv_Error.Is_Used = false;
            Rezerv_Error.Is_Critical = true;
        }
    }
}
