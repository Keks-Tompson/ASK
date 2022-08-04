using ASK.Models;
using System;
using System.Collections.Generic;

namespace ASK.Data.Interfaces
{
    public interface IACCIDENT_LOG
    {
        bool Create_ACCIDENT_LOG(ACCIDENT_LOG accident_log); //Создание

        List<ACCIDENT_LOG> Get_All_ACCIDENT_LOG();

        List<ACCIDENT_LOG> Get_DayAll_ACCIDENT_LOG(DateTime date);
    }
}
