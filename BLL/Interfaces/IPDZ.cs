
using ASK.Models;
using System;
using System.Collections.Generic;

namespace ASK.BLL.Interfaces
{
    public interface IPDZ_Service
    {
        bool Create_PDZ(PDZ new_pdz); //Создание

        List<PDZ> Get_All_PDZ();

        List<PDZ> Get_DayAll_PDZ(DateTime date);
    }
}
