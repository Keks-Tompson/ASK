using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GasAnalyzer_SICK_GM32_Model
    {
        public bool Is_Used { get; set; } = true;
        //                                                                                                              True                            /   False    
        public Alarm_Model AutomaticProtection { get; set; } = new Alarm_Model();   //Автомат защиты                    Отключен                        /   Запистан
        public Alarm_Model Error { get; set; } = new Alarm_Model();                 //Состояние                         В ошибке                        /   В работе
        public Alarm_Model ServiceRequest { get; set; } = new Alarm_Model();        //Запрос на обслуживание            Есть запрос на обслуживание     /   Нет запроса на обслуживание
        public Alarm_Model NoMeasurements { get; set; } = new Alarm_Model();        //Нет измерений                     Измерения отсутствуют           /   измерения в норме
        public Alarm_Model FalseData { get; set; } = new Alarm_Model();             //Недостоверные данные              Данные недостоверны             /   Данные коректны
        public Alarm_Model PurgeUnitMalfunction { get; set; } = new Alarm_Model();  //Неисправность блока продувки      Неисправен                      /   Исправен


        public Alarm_Model Rezerv_Error { get; set; } = new Alarm_Model();          //Резерваня вария                   true                            /   False

        public Alarm_Model Range_1 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 1   2 диапазон (больший)            /   1 диапазон (меньший)
        public Alarm_Model Range_2 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 2   2 диапазон (больший)            /   1 диапазон (меньший)


        //ID расшифровки аварии в БД/ссылка на текст в БД
        public GasAnalyzer_SICK_GM32_Model()
        {
            AutomaticProtection.ID = 44;
            AutomaticProtection.Is_Used = false;
            AutomaticProtection.Is_Critical = true;

            Error.ID = 45;
            Error.Is_Used = false;
            Error.Is_Critical = true;

            ServiceRequest.ID = 46;
            ServiceRequest.Is_Used = false;
            ServiceRequest.Is_Critical = false;

            NoMeasurements.ID = 47;
            NoMeasurements.Is_Used = false;
            NoMeasurements.Is_Critical = true;

            FalseData.ID = 48;
            FalseData.Is_Used = false;
            FalseData.Is_Critical = true;

            PurgeUnitMalfunction.ID = 49;
            PurgeUnitMalfunction.Is_Used = false;
            PurgeUnitMalfunction.Is_Critical = true;


            Rezerv_Error.ID = 50;
            Rezerv_Error.Is_Used = false;
            Rezerv_Error.Is_Critical = true;


            Range_1.ID = 51;
            Range_1.Is_Used = false;
            Range_1.Is_Critical = false;

            Range_2.ID = 52;
            Range_2.Is_Used = false;
            Range_2.Is_Critical = false;
        }
    }
}
