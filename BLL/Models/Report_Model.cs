using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class Report_Model
    {
        public PDZ_Active_Model PDZ { get; set; } = new PDZ_Active_Model();                       //Текущее значение ПДК

        public List<TableReport_Model> TableReport { get; set; } = new List<TableReport_Model>(); //Значение 20-минуток
        public TableReport_Model avgTotal_20M { get; set; } = new TableReport_Model();            //Подвал, среднее занчение и сумма концентраций(+парамеры) и выбросов соответственно
        public TableReport_Model sumTotal_20M { get; set; } = new TableReport_Model();            //Подвал, сумма выбросов

        public TypeReports TypeReport { get; set; } = TypeReports.None;
    }



    public enum TypeReports
    {
        None,
        ReportDay,
        ReportMonth,
        ReportYear
    }
}
