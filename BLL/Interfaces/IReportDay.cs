using ASK.BLL.Helper;
using ASK.BLL.Services;
using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface IReportDay
    {
        public ReportDay_Model Generate(DateTime date);
    }
}
