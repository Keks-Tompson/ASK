using ASK.BLL.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class ReportDay_Model : Report_Model
    {
        public TableReport_Model durationTotal_20M { get; set; } = new TableReport_Model();       //Подвал, сумма выбросов
    }
}
