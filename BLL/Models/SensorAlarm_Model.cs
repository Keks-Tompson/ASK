using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class SensorAlarm_Model
    {
        public bool Is_Used { get; set; } = true;                                 //Испоьзуются ли аварии сенсоров

        public Alarm_Model CO { get; set; } = new Alarm_Model();                  //Обрыв CO
        public Alarm_Model CO2 { get; set; } = new Alarm_Model();                 //Обрыв CO2
        public Alarm_Model NO { get; set; } = new Alarm_Model();                  //Обрыв NO
        public Alarm_Model NO2 { get; set; } = new Alarm_Model();                 //Обрыв NO2
        public Alarm_Model NOx { get; set; } = new Alarm_Model();                 //Обрыв NOx
        public Alarm_Model SO2 { get; set; } = new Alarm_Model();                 //Обрыв SO2
        public Alarm_Model Dust { get; set; } = new Alarm_Model();                //Обрыв Dust

        public Alarm_Model CH4 { get; set; } = new Alarm_Model();                 //Обрыв CH4
        public Alarm_Model H2S { get; set; } = new Alarm_Model();                 //Обрыв H2S

        public Alarm_Model Rezerv_1 { get; set; } = new Alarm_Model();            //Обрыв Rezerv_1
        public Alarm_Model Rezerv_2 { get; set; } = new Alarm_Model();            //Обрыв Rezerv_2
        public Alarm_Model Rezerv_3 { get; set; } = new Alarm_Model();            //Обрыв Rezerv_3
        public Alarm_Model Rezerv_4 { get; set; } = new Alarm_Model();            //Обрыв Rezerv_4
        public Alarm_Model Rezerv_5 { get; set; } = new Alarm_Model();            //Обрыв Rezerv_5

        public Alarm_Model O2_Wet { get; set; } = new Alarm_Model();              //Обрыв O2 Влажный
        public Alarm_Model O2_Dry { get; set; } = new Alarm_Model();              //Обрыв O2 Сухой
        public Alarm_Model H2O { get; set; } = new Alarm_Model();                 //Обрыв H2O

        public Alarm_Model Pressure { get; set; } = new Alarm_Model();            //Обрыв Давления
        public Alarm_Model Temperature { get; set; } = new Alarm_Model();         //Обрыв Температуры
        public Alarm_Model Speed { get; set; } = new Alarm_Model();               //Обрыв Скорости
        public Alarm_Model Temperature_KIP { get; set; } = new Alarm_Model();     //Обрыв Температуры КИП
        public Alarm_Model Temperature_NOx { get; set; } = new Alarm_Model();     //Обрыв Тепературы конвертора NOx



        //ID расшифровки аварии в БД/ссылка на текст в БД
        public SensorAlarm_Model()
        {
            CO.ID = 12;
            CO.Is_Used = true;
            CO.Is_Critical = true;

            CO2.ID = 13;
            CO2.Is_Used = true;
            CO2.Is_Critical = true;

            NO.ID = 14;
            NO.Is_Used = true;
            NO.Is_Critical = true;

            NO2.ID = 15;
            NO2.Is_Used = true;
            NO2.Is_Critical = true;

            NOx.ID = 16;
            NOx.Is_Used = true;
            NOx.Is_Critical = true;

            SO2.ID = 17;
            SO2.Is_Used = true;
            SO2.Is_Critical = true;

            Dust.ID = 18;
            Dust.Is_Used = true;
            Dust.Is_Critical = true;



            CH4.ID = 19;
            CH4.Is_Used = true;
            CH4.Is_Critical = true;

            H2S.ID = 20;
            H2S.Is_Used = true;
            H2S.Is_Critical = true;



            Rezerv_1.ID = 21;
            Rezerv_1.Is_Used = true;
            Rezerv_1.Is_Critical = true;

            Rezerv_2.ID = 22;
            Rezerv_2.Is_Used = true;
            Rezerv_2.Is_Critical = true;

            Rezerv_3.ID = 23;
            Rezerv_3.Is_Used = true;
            Rezerv_3.Is_Critical = true;

            Rezerv_4.ID = 24;
            Rezerv_4.Is_Used = true;
            Rezerv_4.Is_Critical = true;

            Rezerv_5.ID = 25;
            Rezerv_5.Is_Used = true;
            Rezerv_5.Is_Critical = true;



            O2_Wet.ID = 26;
            O2_Wet.Is_Used = true;
            O2_Wet.Is_Critical = true;

            O2_Dry.ID = 27;
            O2_Dry.Is_Used = true;
            O2_Dry.Is_Critical = true;

            H2O.ID = 28;
            H2O.Is_Used = true;
            H2O.Is_Critical = true;



            Pressure.ID = 29;
            Pressure.Is_Used = true;
            Pressure.Is_Critical = true;

            Temperature.ID = 30;
            Temperature.Is_Used = true;
            Temperature.Is_Critical = true;

            Speed.ID = 31;
            Speed.Is_Used = true;
            Speed.Is_Critical = true;

            Temperature_KIP.ID = 32;
            Temperature_KIP.Is_Used = true;
            Temperature_KIP.Is_Critical = false;

            Temperature_NOx.ID = 33;
            Temperature_NOx.Is_Used = true;
            Temperature_NOx.Is_Critical = false;
        }
    }
}
