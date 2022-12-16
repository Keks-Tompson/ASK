using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GasAnalyzer_SICK_GMS810_Model
    {
        public bool Is_Used { get; set; } = true;
        //                                                                                                              True                            /   False    
        public Alarm_Model AutomaticProtection { get; set; } = new Alarm_Model();   //Автомат защиты                    Отключен                        /   Запистан
        public Alarm_Model Error { get; set; } = new Alarm_Model();                 //Состояние                         В ошибке                        /   В работе
        public Alarm_Model ServiceRequest { get; set; } = new Alarm_Model();        //Запрос на обслуживание            Есть запрос на обслуживание     /   Нет запроса на обслуживание
        public Alarm_Model Service { get; set; } = new Alarm_Model();               //Обслуживание сейчас               Идёт обслуживание               /   Нет обслуживания


        public Alarm_Model Rezerv_Error { get; set; } = new Alarm_Model();          //Резерваня вария                   true                            /   False


        public Alarm_Model Range_1 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 1   2 диапазон (больший)            /   1 диапазон (меньший)
        public Alarm_Model Range_2 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 2   2 диапазон (больший)            /   1 диапазон (меньший)
        public Alarm_Model Range_3 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 3   2 диапазон (больший)            /   1 диапазон (меньший)
        public Alarm_Model Range_4 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 4   2 диапазон (больший)            /   1 диапазон (меньший)
        public Alarm_Model Range_5 { get; set; } = new Alarm_Model();               //Переключаемый диапазон канала 5   2 диапазон (больший)            /   1 диапазон (меньший)




        //ID расшифровки аварии в БД/ссылка на текст в БД
        public GasAnalyzer_SICK_GMS810_Model()
        {
            AutomaticProtection.ID = 34;
            AutomaticProtection.Is_Used = false;
            AutomaticProtection.Is_Critical = true;

            Error.ID = 35;
            Error.Is_Used = false;
            Error.Is_Critical = true;

            ServiceRequest.ID = 36;
            ServiceRequest.Is_Used = false;
            ServiceRequest.Is_Critical = false;

            Service.ID = 37;
            Service.Is_Used = false;
            Service.Is_Critical = false;


            Rezerv_Error.ID = 38;
            Rezerv_Error.Is_Used = false;
            Rezerv_Error.Is_Critical = true;


            Range_1.ID = 39;
            Range_1.Is_Used = false;
            Range_1.Is_Critical = false;

            Range_2.ID = 40;
            Range_2.Is_Used = false;
            Range_2.Is_Critical = false;

            Range_3.ID = 41;
            Range_3.Is_Used = false;
            Range_3.Is_Critical = false;

            Range_4.ID = 42;
            Range_4.Is_Used = false;
            Range_4.Is_Critical = false;

            Range_5.ID = 43;
            Range_5.Is_Used = false;
            Range_5.Is_Critical = false;
        }
    }
}
