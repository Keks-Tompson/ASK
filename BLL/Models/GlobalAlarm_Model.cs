using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class GlobalAlarm_Model
    {
        public Alarm_Model Is_NotConnection { get; set; } = new Alarm_Model();  //Нет связи с контроллером
        public Alarm_Model Is_Stop { get; set; } = new Alarm_Model();           //Простой
        public Alarm_Model Is_NotProcess { get; set; } = new Alarm_Model();     //Нет выбросов
        public Alarm_Model Is_Excess { get; set; } = new Alarm_Model();         //Превышение
        public Alarm_Model Is_Approximation { get; set; } = new Alarm_Model();  //Приближение 
        public Alarm_Model Is_Error { get; set; } = new Alarm_Model();          //Авария
        public Alarm_Model Is_Info { get; set; } = new Alarm_Model();           //Информационное сообщение
        public Alarm_Model Is_Maintenance { get; set; } = new Alarm_Model();    //Тех. обслуживание
        public Alarm_Model Is_OpenDoor { get; set; } = new Alarm_Model();       //Дверь контейнера открыта
        public Alarm_Model Is_Fire { get; set; } = new Alarm_Model();           //Сигнал пожара с контейнера



        //ID расшифровки аварии в БД/ссылка на текст в БД
        public GlobalAlarm_Model()
        {
            Is_NotConnection.ID = 1;
            Is_NotConnection.Is_Used = true;

            Is_Stop.ID = 2;
            Is_Stop.Is_Used = true;

            Is_NotProcess.ID = 3;
            Is_NotProcess.Is_Used = true;

            Is_Excess.ID = 4;
            Is_Excess.Is_Used = true;

            Is_Approximation.ID = 5;
            Is_Approximation.Is_Used = true;

            Is_Error.ID = 6;
            Is_Error.Is_Used = true;

            Is_Info.ID = 7;
            Is_Info.Is_Used = true;

            Is_Maintenance.ID = 8;
            Is_Maintenance.Is_Used = true;

            Is_OpenDoor.ID = 9;
            Is_OpenDoor.Is_Used = true;

            Is_Fire.ID = 10;
            Is_Fire.Is_Used = true;
        }
    }
}
