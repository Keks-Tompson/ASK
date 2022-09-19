using ASK.BLL.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface IExcelReport
    {
        //Основной метод создания отчёта за сутки
        public MemoryStream GenerateDefaultReport(Report_Model reportday, DateTime authData);
    }
}
