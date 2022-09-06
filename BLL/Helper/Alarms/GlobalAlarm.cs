using ASK.BLL.Services;
using ASK.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Helper.Alarms
{
    public class GlobalAlarm
    {
        private ACCIDENT_LOG_Service accident_log_Service;

        static GlobalAlarm()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var accident_log_Service = new ACCIDENT_LOG_Service(db);
                accident_log_Service.StratSystem_ACCIDENT_LOG();
            }
        }


        public bool Is_NotConnection    //Нет связи с контроллером
        {
            get { return is_NotConnection; } 
            set
            {
                if (value != is_NotConnection)
                    AlarmLogBuider(1, value);
                is_NotConnection = value;
            }
        }
        public bool Is_Stop             //Простой
        {
            get { return is_Stop; }
            set
            {
                if (value != is_Stop)
                    AlarmLogBuider(2, value);
                is_Stop = value;
            }
        }
        public bool Is_NotProcess       //Нет выбросов
        {
            get { return is_NotProcess; }
            set
            {
                if (value != is_NotProcess)
                    AlarmLogBuider(3, value);
                is_NotProcess = value;
            }
        }
        public bool Is_Excess           //Превышение
        {
            get { return is_Excess; }
            set
            {
                if (value != is_Excess)
                    AlarmLogBuider(4, value);
                is_Excess = value;
            }
        }
        public bool Is_Approximation    //Приближение 
        {
            get { return is_Approximation; }
            set
            {
                if (value != is_Approximation)
                    AlarmLogBuider(5, value);
                is_Approximation = value;
            }
        }
        public bool Is_Error            //Авария
        {
            get { return is_Error; }
            set
            {
                if (value != is_Error)
                    AlarmLogBuider(6, value);
                is_Error = value;
            }
        }
        public bool Is_Info             //Информационное сообщение
        {
            get { return is_Info; }
            set
            {
                if (value != is_Info)
                    AlarmLogBuider(7, value);
                is_Info = value;
            }
        }
        public bool Is_Maintenance      //Тех. обслуживание
        {
            get { return is_Maintenance; }
            set
            {
                if (value != is_Maintenance)
                    AlarmLogBuider(8, value);
                is_Maintenance = value;
            }
        }
        public bool Is_OpenDoor         //Дверь контейнера открыта
        {
            get { return is_OpenDoor; }
            set
            {
                if (value != is_OpenDoor)
                    AlarmLogBuider(9, value);
                is_OpenDoor = value;
            }
        }
        public bool Is_Fire             //Сигнал пожара с контейнера
        {
            get { return is_Fire; }
            set
            {
                if (value != is_Fire)
                    AlarmLogBuider(10, value);
                is_Fire = value;
            }
        }


        bool is_NotConnection = false;  //Нет связи с контроллером
        bool is_Stop = false;           //Простой
        bool is_NotProcess = false;     //Нет выбросов
        bool is_Excess = false;         //Превышение
        bool is_Approximation = false;  //Приближение 
        bool is_Error = false;          //Авария
        bool is_Info = false;           //Информационное сообщение
        bool is_Maintenance = false;    //Тех. обслуживание
        bool is_OpenDoor = false;       //Дверь контейнера открыта
        bool is_Fire = false;           //Сигнал пожара с контейнера

        public void AlarmLogBuider(int id_accident, bool is_State)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                accident_log_Service = new ACCIDENT_LOG_Service(db);

                if (is_State)
                    accident_log_Service.Begin_ACCIDENT_LOG(id_accident);
                else
                    accident_log_Service.End_ACCIDENT_LOG(id_accident);
            }
        }
    }
}
