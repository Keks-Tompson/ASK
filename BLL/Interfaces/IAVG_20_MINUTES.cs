using ASK.Models;
using System;
using System.Collections.Generic;

namespace ASK.BLL.Interfaces
{
    public interface IAVG_20_MINUTES
    {
        bool Create_AVG_20_MINUTES(AVG_20_MINUTES avg_20_M); //Создание

        List<AVG_20_MINUTES> Get_All_AVG_20_MINMUTES();

        List<AVG_20_MINUTES> Get_DayAll_AVG_20_MINUTES(DateTime date);
    }
}
