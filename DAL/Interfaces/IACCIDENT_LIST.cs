using ASK.DAL.Models;
using System;
using System.Collections.Generic;

namespace ASK.DAL.Interfaces
{
    public interface IACCIDENT_LIST
    {
        bool Create_ACCIDENT_LIST(ACCIDENT_LIST accident_log); //Создание
        List<ACCIDENT_LIST> Get_All_ACCIDENT_LIST();
    }
}
