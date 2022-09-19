using ASK.DAL.Models;
using System;
using System.Collections.Generic;

namespace ASK.DAL.Interfaces
{
    public interface IPDZ
    {
        bool Create_PDZ(PDZ new_pdz); //Создание
        List<PDZ> Get_All_PDZ();
        List<PDZ> Get_DayAll_PDZ(DateTime date);
        public PDZ FisrsPDZMonth(DateTime date);
        public bool FindPDZDay();
    }
}
