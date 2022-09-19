using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface IReportMonth
    {
        public ReportMonth_Model Generate(DateTime yearMonth);
    }
}
