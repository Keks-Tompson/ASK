
using ASK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.DAL.Interfaces
{
    internal interface ISENSOR_4_20_10sec
    {
        bool Create_SENSOR_4_20_10sec(SENSOR_4_20_10sec newSernsors); //Создание
        List<SENSOR_4_20_10sec> Get_All_SENSOR_4_20_10sec();
        List<SENSOR_4_20_10sec> Get_Range_SENSOR_4_20_10sec(DateTime date1, DateTime date2);
        void DeleteOld(DateTime date);
    }
}
