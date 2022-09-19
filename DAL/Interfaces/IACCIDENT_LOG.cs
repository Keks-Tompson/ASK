using ASK.DAL.Models;
using System;
using System.Collections.Generic;

namespace ASK.DAL.Interfaces
{
    public interface IACCIDENT_LOG
    {
        void StratSystem_ACCIDENT_LOG();            //Уведомление чо система запущена + очистка старых уведомлений
        bool Begin_ACCIDENT_LOG(int id_Accident);   //Записываем событие и дату начала события
        void End_ACCIDENT_LOG(int id_Accident);     //Записываем дату окончания события
        List<ACCIDENT_LOG> Get_All_ACCIDENT_LOG();
        public List<ACCIDENT_LOG> Get_All_ACCIDENT_LOG_Active();
        List<ACCIDENT_LOG> Get_DayAll_ACCIDENT_LOG(DateTime date);
    }
}
