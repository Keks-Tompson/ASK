using ASK.BLL.Helper.Setting;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ASK.BLL.Services
{
    public class ExcelReport_Services : IExcelReport
    {
        private ExcelReport_Model excelReportDay;



        //Основной метод создания отчёта за сутки
        public MemoryStream GenerateDefaultReport(Report_Model report_day, DateTime authData)
        {
            excelReportDay = new ExcelReport_Model();

            excelReportDay.ReportDay = report_day;

            var stream = new MemoryStream();

            using (var xlPackage = new ExcelPackage(stream))
            {
                excelReportDay.row = excelReportDay.startRow;
                excelReportDay.col = 1;
                var worksheet = xlPackage.Workbook.Worksheets.Add("Report");

                string unitEmis;

                //Определяем тип отчёта и подстовляемые велечины
                switch (excelReportDay.ReportDay.TypeReport)
                {
                    case TypeReports.ReportDay:
                        unitEmis = "г/c";

                        //Текст 1 стороки Excel
                        worksheet.Cells["A1"].Value = GlobalStaticSettingsASK.SettingOptions.ReportDay_Name + " на " + authData.ToShortDateString();
                        break;

                    case TypeReports.ReportMonth:
                        unitEmis = "кг/сутки";

                        //Текст 1 стороки Excel
                        worksheet.Cells["A1"].Value = GlobalStaticSettingsASK.SettingOptions.ReportMonth_Name + " на " + authData.ToString("MMMM yyyy");
                        break;

                    default:
                        unitEmis = "Не обработано";

                        //Текст 1 стороки Excel
                        worksheet.Cells["A1"].Value = GlobalStaticSettingsASK.SettingOptions.ReportMonth_Name + " - не обработан";
                        break;
                }

                //Левый первый столбец столбец
                //-----------------------------------------------------------------------
                worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = "Ед. изм.";
                worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = "ПДЗ";
                for (int i = 0; i < excelReportDay.ReportDay.TableReport.Count; i++)
                {
                    switch (excelReportDay.ReportDay.TypeReport)
                    {
                        case TypeReports.ReportDay:
                            worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = excelReportDay.ReportDay.TableReport[i].Date.Hour.ToString() + ":" + excelReportDay.ReportDay.TableReport[i].Date.Minute.ToString();
                            break;

                        case TypeReports.ReportMonth:
                            worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = excelReportDay.ReportDay.TableReport[i].Date.ToShortDateString();
                            break;

                        default:
                            worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = "не обработано";
                            break;
                    }
                }
                worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = "Среднее";
                worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = "Сумма";

                //Концентрации
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO_Conc != "none") AddColumn(ref worksheet, "CO", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.CO_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.CO_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.CO_Conc, excelReportDay.ReportDay.avgTotal_20M.CO_Conc, excelReportDay.ReportDay.avgTotal_20M.CO_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.CO_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Conc != "none") AddColumn(ref worksheet, "CO₂", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.CO2_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.CO2_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.CO2_Conc, excelReportDay.ReportDay.avgTotal_20M.CO2_Conc, excelReportDay.ReportDay.avgTotal_20M.CO2_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.CO2_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO_Conc != "none") AddColumn(ref worksheet, "NO", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.NO_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.NO_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.NO_Conc, excelReportDay.ReportDay.avgTotal_20M.NO_Conc, excelReportDay.ReportDay.avgTotal_20M.NO_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.NO_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Conc != "none") AddColumn(ref worksheet, "NO₂", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.NO2_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.NO2_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.NO2_Conc, excelReportDay.ReportDay.avgTotal_20M.NO2_Conc, excelReportDay.ReportDay.avgTotal_20M.NO2_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.NO2_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Conc != "none") AddColumn(ref worksheet, "NOx", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.NOx_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.NOx_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.NOx_Conc, excelReportDay.ReportDay.avgTotal_20M.NOx_Conc, excelReportDay.ReportDay.avgTotal_20M.NOx_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.NOx_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Conc != "none") AddColumn(ref worksheet, "SO₂", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.SO2_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.SO2_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.SO2_Conc, excelReportDay.ReportDay.avgTotal_20M.SO2_Conc, excelReportDay.ReportDay.avgTotal_20M.SO2_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.SO2_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Conc != "none") AddColumn(ref worksheet, "Тв.ч.", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Dust_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Dust_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.Dust_Conc, excelReportDay.ReportDay.avgTotal_20M.Dust_Conc, excelReportDay.ReportDay.avgTotal_20M.Dust_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.Dust_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Conc != "none") AddColumn(ref worksheet, "CH₄", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.CH4_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.CH4_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.CH4_Conc, excelReportDay.ReportDay.avgTotal_20M.CH4_Conc, excelReportDay.ReportDay.avgTotal_20M.CH4_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.CH4_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Conc != "none") AddColumn(ref worksheet, "H₂S", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.H2S_Conc).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.H2S_Conc_Percent).ToList(), excelReportDay.ReportDay.PDZ.H2S_Conc, excelReportDay.ReportDay.avgTotal_20M.H2S_Conc, excelReportDay.ReportDay.avgTotal_20M.H2S_Conc_Percent, excelReportDay.ReportDay.sumTotal_20M.H2S_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_1 != "none") AddColumn(ref worksheet, "Add_Conc_1", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_1).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_1_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Conc_1, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_1, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_1_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Conc_1);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_2 != "none") AddColumn(ref worksheet, "Add_Conc_2", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_2).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_2_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Conc_2, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_2, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_2_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Conc_2);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_3 != "none") AddColumn(ref worksheet, "Add_Conc_3", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_3).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_3_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Conc_3, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_3, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_3_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Conc_3);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_4 != "none") AddColumn(ref worksheet, "Add_Conc_4", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_4).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_4_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Conc_4, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_4, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_4_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Conc_4);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_5 != "none") AddColumn(ref worksheet, "Add_Conc_5", "мг/м³, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_5).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Conc_5_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Conc_5, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_5, excelReportDay.ReportDay.avgTotal_20M.Add_Conc_5_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Conc_5);

                //Выбросы
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO_Emis != "none") AddColumn(ref worksheet, "CO", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.CO_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.CO_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.CO_Emis, excelReportDay.ReportDay.avgTotal_20M.CO_Emis, excelReportDay.ReportDay.avgTotal_20M.CO_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.CO_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Emis != "none") AddColumn(ref worksheet, "CO₂", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.CO2_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.CO2_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.CO2_Emis, excelReportDay.ReportDay.avgTotal_20M.CO2_Emis, excelReportDay.ReportDay.avgTotal_20M.CO2_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.CO2_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO_Emis != "none") AddColumn(ref worksheet, "NO", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.NO_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.NO_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.NO_Emis, excelReportDay.ReportDay.avgTotal_20M.NO_Emis, excelReportDay.ReportDay.avgTotal_20M.NO_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.NO_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Emis != "none") AddColumn(ref worksheet, "NO₂", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.NO2_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.NO2_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.NO2_Emis, excelReportDay.ReportDay.avgTotal_20M.NO2_Emis, excelReportDay.ReportDay.avgTotal_20M.NO2_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.NO2_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Emis != "none") AddColumn(ref worksheet, "NOx", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.NOx_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.NOx_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.NOx_Emis, excelReportDay.ReportDay.avgTotal_20M.NOx_Emis, excelReportDay.ReportDay.avgTotal_20M.NOx_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.NOx_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Emis != "none") AddColumn(ref worksheet, "SO₂", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.SO2_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.SO2_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.SO2_Emis, excelReportDay.ReportDay.avgTotal_20M.SO2_Emis, excelReportDay.ReportDay.avgTotal_20M.SO2_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.SO2_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Emis != "none") AddColumn(ref worksheet, "Тв.ч.", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.Dust_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Dust_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.Dust_Emis, excelReportDay.ReportDay.avgTotal_20M.Dust_Emis, excelReportDay.ReportDay.avgTotal_20M.Dust_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.Dust_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Emis != "none") AddColumn(ref worksheet, "CH₄", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.CH4_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.CH4_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.CH4_Emis, excelReportDay.ReportDay.avgTotal_20M.CH4_Emis, excelReportDay.ReportDay.avgTotal_20M.CH4_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.CH4_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Emis != "none") AddColumn(ref worksheet, "H₂S", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.H2S_Emis).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.H2S_Emis_Percent).ToList(), excelReportDay.ReportDay.PDZ.H2S_Emis, excelReportDay.ReportDay.avgTotal_20M.H2S_Emis, excelReportDay.ReportDay.avgTotal_20M.H2S_Emis_Percent, excelReportDay.ReportDay.sumTotal_20M.H2S_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_1 != "none") AddColumn(ref worksheet, "Add_Conc_1", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_1).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_1_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Emis_1, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_1, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_1_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Emis_1);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_2 != "none") AddColumn(ref worksheet, "Add_Conc_2", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_2).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_2_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Emis_2, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_2, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_2_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Emis_2);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_3 != "none") AddColumn(ref worksheet, "Add_Conc_3", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_3).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_3_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Emis_3, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_3, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_3_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Emis_3);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_4 != "none") AddColumn(ref worksheet, "Add_Conc_4", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_4).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_4_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Emis_4, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_4, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_4_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Emis_4);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_5 != "none") AddColumn(ref worksheet, "Add_Conc_5", unitEmis, excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_5).ToList(), excelReportDay.ReportDay.TableReport.Select(s => s.Add_Emis_5_Percent).ToList(), excelReportDay.ReportDay.PDZ.Add_Emis_5, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_5, excelReportDay.ReportDay.avgTotal_20M.Add_Emis_5_Percent, excelReportDay.ReportDay.sumTotal_20M.Add_Emis_5);

                //Выделяем ПДК
                ColorTheCell(ref worksheet, "A6:" + excelReportDay.masExcelCell[excelReportDay.col] + "6", GlobalStaticSettingsASK.ColorHeader1);

                //Параметры
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Pressure != "none") AddColumn(ref worksheet, "Давление ДГ", "кПа", excelReportDay.ReportDay.TableReport.Select(s => s.Pressure).ToList(), excelReportDay.ReportDay.avgTotal_20M.Pressure, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Temperature != "none") AddColumn(ref worksheet, "Температура ДГ", "°С", excelReportDay.ReportDay.TableReport.Select(s => s.Temperature).ToList(), excelReportDay.ReportDay.avgTotal_20M.Temperature, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Speed != "none") AddColumn(ref worksheet, "Скорость ДГ", "м³/с", excelReportDay.ReportDay.TableReport.Select(s => s.Speed).ToList(), excelReportDay.ReportDay.avgTotal_20M.Speed, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Flow != "none") AddColumn(ref worksheet, "Расход ДГ", "м³/с, при н.у.", excelReportDay.ReportDay.TableReport.Select(s => s.Flow).ToList(), excelReportDay.ReportDay.avgTotal_20M.Flow, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_KIP != "none") AddColumn(ref worksheet, "Температура КИП", "°С", excelReportDay.ReportDay.TableReport.Select(s => s.Temperature_KIP).ToList(), excelReportDay.ReportDay.avgTotal_20M.Temperature_KIP, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_NOx != "none") AddColumn(ref worksheet, "Температура NOx", "°С", excelReportDay.ReportDay.TableReport.Select(s => s.Temperature_NOx).ToList(), excelReportDay.ReportDay.avgTotal_20M.Temperature_NOx, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.O2_Dry != "none") AddColumn(ref worksheet, "О₂ сух.", "%", excelReportDay.ReportDay.TableReport.Select(s => s.O2_Dry).ToList(), excelReportDay.ReportDay.avgTotal_20M.O2_Dry, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.O2_Wet != "none") AddColumn(ref worksheet, "О₂ вл.", "%", excelReportDay.ReportDay.TableReport.Select(s => s.O2_Wet).ToList(), excelReportDay.ReportDay.avgTotal_20M.O2_Wet, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.H2O != "none") AddColumn(ref worksheet, "H₂O", "%", excelReportDay.ReportDay.TableReport.Select(s => s.H2O).ToList(), excelReportDay.ReportDay.avgTotal_20M.H2O, false, false);

                if (GlobalStaticSettingsASK.VisibilityOptions20M.Pressure != "none") AddColumn(ref worksheet, "Режим", "", excelReportDay.ReportDay.TableReport.Select(s => s.Mode_ASK).ToList(), excelReportDay.ReportDay.sumTotal_20M.Mode_ASK, true, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Pressure != "none") AddColumn(ref worksheet, "ПДК", "", excelReportDay.ReportDay.TableReport.Select(s => (double)s.PDZ_Fuel).ToList(), excelReportDay.ReportDay.avgTotal_20M.PDZ_Fuel, false, true);

                using (var r = worksheet.Cells["A1:" + excelReportDay.masExcelCell[excelReportDay.col] + "1"])
                {
                    r.Merge = true; //объеденяет ячейки
                }

                //Текст 2 стороки Excel
                worksheet.Cells["A2"].Value = GlobalStaticSettingsASK.SettingOptions.NameASK;

                using (var r = worksheet.Cells["A2:" + excelReportDay.masExcelCell[excelReportDay.col] + "2"])
                {
                    r.Merge = true; //объеденяет ячейки
                }

                worksheet.Cells["A3"].Value = GlobalStaticSettingsASK.SettingOptions.ReportDay_NameTable;

                //Текст 3 строки 
                using (var r = worksheet.Cells["A3:" + excelReportDay.masExcelCell[excelReportDay.col] + "3"])
                {
                    r.Merge = true; //объеденяет ячейки
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(GlobalStaticSettingsASK.ColorHeader2);
                }

                //Выстовляем границы все таблице, выстовляем шрифт и авто выравнивание
                using (var r = worksheet.Cells["A1:" + excelReportDay.masExcelCell[excelReportDay.col] + (excelReportDay.row).ToString()])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    r.Style.Font.Size = GlobalStaticSettingsASK.SettingOptions.ReportDay_FontSize;
                    r.Style.Font.Name = GlobalStaticSettingsASK.SettingOptions.ReportDay_FontName;
                    r.Style.Font.Bold = GlobalStaticSettingsASK.SettingOptions.ReportDay_FontBoldHeader;

                    double minimumSize = 1;
                    r.AutoFitColumns(minimumSize);
                }

                //Убираем жирные значения 20 минуток
                if (excelReportDay.ReportDay.TableReport.Count != 0)
                    using (var r = worksheet.Cells["B7:" + excelReportDay.masExcelCell[excelReportDay.col] + (excelReportDay.row - 2).ToString()])
                    {
                        r.Style.Font.Bold = GlobalStaticSettingsASK.SettingOptions.ReportDay_FontBoldTable;
                    }

                //Задаём цвет времени/первой страке 20 минуток
                ColorTheCell(ref worksheet, "A7:A" + (excelReportDay.row).ToString(), GlobalStaticSettingsASK.ColorHeader2);

                //Задаём цвет заголовка (концентрации)
                ColorTheCell(ref worksheet, "A4:" + excelReportDay.masExcelCell[excelReportDay.col] + "4", GlobalStaticSettingsASK.ColorHeader1);

                //Задаём цвет заголовка (ед. изм.)
                ColorTheCell(ref worksheet, "A5:" + excelReportDay.masExcelCell[excelReportDay.col] + "5", GlobalStaticSettingsASK.ColorHeader2);

                //Подвал "Среднее" выделяем
                ColorTheCell(ref worksheet, "A" + (excelReportDay.row - 1).ToString(), GlobalStaticSettingsASK.ColorHeader1);

                //Подвал "Сумма" выделяем
                ColorTheCell(ref worksheet, "A" + (excelReportDay.row).ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + (excelReportDay.row).ToString(), GlobalStaticSettingsASK.ColorHeader1);


                //Подчёркиваем конец заголовка
                using (var r = worksheet.Cells["A6:" + excelReportDay.masExcelCell[excelReportDay.col] + "6"])
                {
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }

                //Подчёркиваем начало подвала
                using (var r = worksheet.Cells["A" + (excelReportDay.row - 1).ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + (excelReportDay.row - 1).ToString()])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                }

                xlPackage.Workbook.Properties.Title = GlobalStaticSettingsASK.SettingOptions.ReportDay_Name;
                xlPackage.Workbook.Properties.Author = "";
                xlPackage.Save();
            }
            stream.Position = 0;
            return stream;
        }



        //Добовляем концентрацию в таблицу
        private void AddColumn(ref ExcelWorksheet worksheet, string nameConc, string unitsConc, List<double> komponent, List<double> procent, double pdz, double avgTotal_20M, double avgTotal_20M_Procent, double sumTotal_20M)
        {
            string unit, unitEmiss;
            //Определяем тип отчёта и подстовляемые велечины
            switch (excelReportDay.ReportDay.TypeReport)
            {
                case TypeReports.ReportDay:
                    unit = "";
                    unitEmiss = "%";
                    break;

                case TypeReports.ReportMonth:
                    unit = "прев.";
                    unitEmiss = "ч.";
                    break;

                default:
                    unit = "Не обработано";
                    unitEmiss = "Не обработано";
                    break;
            }

            //Заголовки
            excelReportDay.row = excelReportDay.startRow;
            worksheet.Cells[excelReportDay.row, ++excelReportDay.col].Value = nameConc;     //Название компонента концентрации
            worksheet.Cells[excelReportDay.row, excelReportDay.col + 1].Value = unit;
            worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = unitsConc;    //Ед. изм. концентраций

            if (pdz > 0.0 && pdz < 999999.0) worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = pdz;
            else worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = GlobalStaticSettingsASK.SettingOptions.NoneValue;

            //Значение 20м
            for (int i = 0; i < komponent.Count; i++)
            {
                worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = komponent[i];
            }

            worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = avgTotal_20M;
            if (sumTotal_20M > 0.0) worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = sumTotal_20M + " кг.";

            //%
            excelReportDay.row = excelReportDay.startRow;
            worksheet.Cells[++excelReportDay.row, ++excelReportDay.col].Value = unitEmiss;
            excelReportDay.row++;

            for (int i = 0; i < komponent.Count; i++)
            {
                ++excelReportDay.row;
                if (procent[i] > 0.0)
                {
                    worksheet.Cells[excelReportDay.row, excelReportDay.col].Value = procent[i];
                    ColorTheCell(ref worksheet, excelReportDay.masExcelCell[excelReportDay.col - 1] + excelReportDay.row.ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString(), GlobalStaticSettingsASK.ColorExcess);
                }
            }

            //Значение подвала + процент + подсветка
            excelReportDay.row++;
            if (avgTotal_20M_Procent > 0.0)
            {
                worksheet.Cells[excelReportDay.row, excelReportDay.col].Value = avgTotal_20M_Procent;
                excelReportDay.colorTMPFooter = GlobalStaticSettingsASK.ColorExcess;
            }
            else excelReportDay.colorTMPFooter = GlobalStaticSettingsASK.ColorHeader1;

            ColorTheCell(ref worksheet, excelReportDay.masExcelCell[excelReportDay.col - 1] + excelReportDay.row.ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString(), excelReportDay.colorTMPFooter);
        }



        //Перегрузка для добовления парамтров
        private void AddColumn(ref ExcelWorksheet worksheet, string nameConc, string unitsConc, List<double> komponent, double avgTotal_20M, bool mode, bool pdk)
        {
            //Заголовки
            excelReportDay.row = excelReportDay.startRow;
            worksheet.Cells[excelReportDay.row, ++excelReportDay.col].Value = nameConc;     //Название компонента концентрации
            worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = unitsConc;    //Ед. изм. концентраций
            excelReportDay.row++;

            //Значение 20м
            if (!mode)
            {
                for (int i = 0; i < komponent.Count; i++)
                {
                    worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = komponent[i];
                }
                if (!pdk)
                    worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = avgTotal_20M;
                else
                    excelReportDay.row++;
            }
            else
            {
                for (int i = 0; i < komponent.Count; i++)
                {
                    worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = excelReportDay.mode_ASK_String[(int)excelReportDay.ReportDay.TableReport[i].Mode_ASK];
                    switch (komponent[i])
                    {
                        case 1.0:
                            ColorTheCell(ref worksheet, excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString(), GlobalStaticSettingsASK.ColorExcess);
                            break;
                        case 2.0:
                            ColorTheCell(ref worksheet, excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString(), GlobalStaticSettingsASK.ColorHeader1);
                            break;
                    }
                }
                worksheet.Cells[++excelReportDay.row, excelReportDay.col].Value = avgTotal_20M + " ч.";
            }

            //Значение подвала + процент + подсветка
            excelReportDay.colorTMPFooter = GlobalStaticSettingsASK.ColorHeader1;
            if (avgTotal_20M > 0.0 && mode)
                excelReportDay.colorTMPFooter = GlobalStaticSettingsASK.ColorExcess;

            ColorTheCell(ref worksheet, excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString() + ":" + excelReportDay.masExcelCell[excelReportDay.col] + excelReportDay.row.ToString(), excelReportDay.colorTMPFooter);

            excelReportDay.row++;
        }



        //Окрашиваем ячейки
        private void ColorTheCell(ref ExcelWorksheet worksheet, string cellExcel, Color colorCell)
        {
            using (var r = worksheet.Cells[cellExcel])
            {
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(colorCell);
            }
        }
    }
}
