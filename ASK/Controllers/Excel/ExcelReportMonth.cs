using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ASK.Controllers.Add;
using ASK.Controllers.Report;
using ASK.Controllers.Setting;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ASK.Controllers.Excel
{
    public static class ExcelReportMonth
    {
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                  Переменные
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static Color colorTMPFooter;    //Просто переменная для хранения цвета 


        // Необходим для работы со стилями Excel (можно задавть только через буква-цифру адрес ячейки)
        //                                        0    1    2    3    4    5    6    7    8    9    10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27    28    29    30    31    32    33    34    35    36    37    38    39    40    41    42    43    44    45    46    47    48    49    50    51    52    53    54    55    56    57    58    59    60    61    62    63    64    65    66    67    68    69    70    71    72    73    74    75    76    77    78    79    80    81    82    83    84    85    86    87    88    89    90    91    92    93    94    95    96    97    98    99    100   101   102   103   104     
        private static string[] masExcelCell = { "A", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ" };
        static string[] monthName = new string[] { "нулевой", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };


        //Работа с таблицей
        private static int startRow = 4; //Первая строчка таблицы
        private static int col = 1; //Столбец
        private static int row = startRow; //Ручной номер колонки


        //Таблица 20м
        private static ReportMonth reportMonth;


        //Доп. переменные
        public static string[] mode_ASK_String = new string[3] { "Работа", "Простой", "Останов" };


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                  Основной метод создания отчёта за сутки
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static MemoryStream Create(ReportMonth reportmonth, int year, int month)
        {
            reportMonth = reportmonth;
            var stream = new MemoryStream();

            using (var xlPackage = new ExcelPackage(stream))
            {
                row = startRow;
                col = 1;
                var worksheet = xlPackage.Workbook.Worksheets.Add("ReportDailys");


                //Левый первый столбец столбец
                //-----------------------------------------------------------------------
                worksheet.Cells[++row, col].Value = "Ед. изм.";
                worksheet.Cells[++row, col].Value = "ПДЗ";
                for (int i = 0; i < reportMonth.tableReportDay.Count; i++)
                {
                    worksheet.Cells[++row, col].Value = reportMonth.tableReportDay[i].Date.ToShortDateString();
                }
                worksheet.Cells[++row, col].Value = "Среднее";
                worksheet.Cells[++row, col].Value = "Сумма";


                //Концентрации
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO_Conc != "none") ExcelReportMonth.add(ref worksheet, "CO", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.CO_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.CO_Conc_Percent).ToList(), reportMonth.PDZ.CO_Conc, reportMonth.avgTotal_20M.CO_Conc, reportMonth.avgTotal_20M.CO_Conc_Percent, reportMonth.sumTotal_20M.CO_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Conc != "none") ExcelReportMonth.add(ref worksheet, "CO₂", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.CO2_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.CO2_Conc_Percent).ToList(), reportMonth.PDZ.CO2_Conc, reportMonth.avgTotal_20M.CO2_Conc, reportMonth.avgTotal_20M.CO2_Conc_Percent, reportMonth.sumTotal_20M.CO2_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO_Conc != "none") ExcelReportMonth.add(ref worksheet, "NO", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.NO_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.NO_Conc_Percent).ToList(), reportMonth.PDZ.NO_Conc, reportMonth.avgTotal_20M.NO_Conc, reportMonth.avgTotal_20M.NO_Conc_Percent, reportMonth.sumTotal_20M.NO_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Conc != "none") ExcelReportMonth.add(ref worksheet, "NO₂", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.NO2_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.NO2_Conc_Percent).ToList(), reportMonth.PDZ.NO2_Conc, reportMonth.avgTotal_20M.NO2_Conc, reportMonth.avgTotal_20M.NO2_Conc_Percent, reportMonth.sumTotal_20M.NO2_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Conc != "none") ExcelReportMonth.add(ref worksheet, "NOx", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.NOx_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.NOx_Conc_Percent).ToList(), reportMonth.PDZ.NOx_Conc, reportMonth.avgTotal_20M.NOx_Conc, reportMonth.avgTotal_20M.NOx_Conc_Percent, reportMonth.sumTotal_20M.NOx_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Conc != "none") ExcelReportMonth.add(ref worksheet, "SO₂", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.SO2_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.SO2_Conc_Percent).ToList(), reportMonth.PDZ.SO2_Conc, reportMonth.avgTotal_20M.SO2_Conc, reportMonth.avgTotal_20M.SO2_Conc_Percent, reportMonth.sumTotal_20M.SO2_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Conc != "none") ExcelReportMonth.add(ref worksheet, "Тв.ч.", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.Dust_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.Dust_Conc_Percent).ToList(), reportMonth.PDZ.Dust_Conc, reportMonth.avgTotal_20M.Dust_Conc, reportMonth.avgTotal_20M.Dust_Conc_Percent, reportMonth.sumTotal_20M.Dust_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Conc != "none") ExcelReportMonth.add(ref worksheet, "CH₄", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.CH4_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.CH4_Conc_Percent).ToList(), reportMonth.PDZ.CH4_Conc, reportMonth.avgTotal_20M.CH4_Conc, reportMonth.avgTotal_20M.CH4_Conc_Percent, reportMonth.sumTotal_20M.CH4_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Conc != "none") ExcelReportMonth.add(ref worksheet, "H₂S", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.H2S_Conc).ToList(), reportMonth.tableReportDay.Select(s => s.H2S_Conc_Percent).ToList(), reportMonth.PDZ.H2S_Conc, reportMonth.avgTotal_20M.H2S_Conc, reportMonth.avgTotal_20M.H2S_Conc_Percent, reportMonth.sumTotal_20M.H2S_Conc);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_1 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_1", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.Add_Conc_1).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Conc_1_Percent).ToList(), reportMonth.PDZ.Add_Conc_1, reportMonth.avgTotal_20M.Add_Conc_1, reportMonth.avgTotal_20M.Add_Conc_1_Percent, reportMonth.sumTotal_20M.Add_Conc_1);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_2 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_2", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.Add_Conc_2).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Conc_2_Percent).ToList(), reportMonth.PDZ.Add_Conc_2, reportMonth.avgTotal_20M.Add_Conc_2, reportMonth.avgTotal_20M.Add_Conc_2_Percent, reportMonth.sumTotal_20M.Add_Conc_2);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_3 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_3", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.Add_Conc_3).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Conc_3_Percent).ToList(), reportMonth.PDZ.Add_Conc_3, reportMonth.avgTotal_20M.Add_Conc_3, reportMonth.avgTotal_20M.Add_Conc_3_Percent, reportMonth.sumTotal_20M.Add_Conc_3);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_4 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_4", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.Add_Conc_4).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Conc_4_Percent).ToList(), reportMonth.PDZ.Add_Conc_4, reportMonth.avgTotal_20M.Add_Conc_4, reportMonth.avgTotal_20M.Add_Conc_4_Percent, reportMonth.sumTotal_20M.Add_Conc_4);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Conc_5 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_5", "мг/м³, при н.у.", reportMonth.tableReportDay.Select(s => s.Add_Conc_5).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Conc_5_Percent).ToList(), reportMonth.PDZ.Add_Conc_5, reportMonth.avgTotal_20M.Add_Conc_5, reportMonth.avgTotal_20M.Add_Conc_5_Percent, reportMonth.sumTotal_20M.Add_Conc_5);

                //Выбросы
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO_Emis != "none") ExcelReportMonth.add(ref worksheet, "CO", "кг/сутки", reportMonth.tableReportDay.Select(s => s.CO_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.CO_Emis_Percent).ToList(), reportMonth.PDZ.CO_Emis, reportMonth.avgTotal_20M.CO_Emis, reportMonth.avgTotal_20M.CO_Emis_Percent, reportMonth.sumTotal_20M.CO_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CO2_Emis != "none") ExcelReportMonth.add(ref worksheet, "CO₂", "кг/сутки", reportMonth.tableReportDay.Select(s => s.CO2_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.CO2_Emis_Percent).ToList(), reportMonth.PDZ.CO2_Emis, reportMonth.avgTotal_20M.CO2_Emis, reportMonth.avgTotal_20M.CO2_Emis_Percent, reportMonth.sumTotal_20M.CO2_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO_Emis != "none") ExcelReportMonth.add(ref worksheet, "NO", "кг/сутки", reportMonth.tableReportDay.Select(s => s.NO_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.NO_Emis_Percent).ToList(), reportMonth.PDZ.NO_Emis, reportMonth.avgTotal_20M.NO_Emis, reportMonth.avgTotal_20M.NO_Emis_Percent, reportMonth.sumTotal_20M.NO_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NO2_Emis != "none") ExcelReportMonth.add(ref worksheet, "NO₂", "кг/сутки", reportMonth.tableReportDay.Select(s => s.NO2_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.NO2_Emis_Percent).ToList(), reportMonth.PDZ.NO2_Emis, reportMonth.avgTotal_20M.NO2_Emis, reportMonth.avgTotal_20M.NO2_Emis_Percent, reportMonth.sumTotal_20M.NO2_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.NOx_Emis != "none") ExcelReportMonth.add(ref worksheet, "NOx", "кг/сутки", reportMonth.tableReportDay.Select(s => s.NOx_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.NOx_Emis_Percent).ToList(), reportMonth.PDZ.NOx_Emis, reportMonth.avgTotal_20M.NOx_Emis, reportMonth.avgTotal_20M.NOx_Emis_Percent, reportMonth.sumTotal_20M.NOx_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.SO2_Emis != "none") ExcelReportMonth.add(ref worksheet, "SO₂", "кг/сутки", reportMonth.tableReportDay.Select(s => s.SO2_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.SO2_Emis_Percent).ToList(), reportMonth.PDZ.SO2_Emis, reportMonth.avgTotal_20M.SO2_Emis, reportMonth.avgTotal_20M.SO2_Emis_Percent, reportMonth.sumTotal_20M.SO2_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Dust_Emis != "none") ExcelReportMonth.add(ref worksheet, "Тв.ч.", "кг/сутки", reportMonth.tableReportDay.Select(s => s.Dust_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.Dust_Emis_Percent).ToList(), reportMonth.PDZ.Dust_Emis, reportMonth.avgTotal_20M.Dust_Emis, reportMonth.avgTotal_20M.Dust_Emis_Percent, reportMonth.sumTotal_20M.Dust_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.CH4_Emis != "none") ExcelReportMonth.add(ref worksheet, "CH₄", "кг/сутки", reportMonth.tableReportDay.Select(s => s.CH4_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.CH4_Emis_Percent).ToList(), reportMonth.PDZ.CH4_Emis, reportMonth.avgTotal_20M.CH4_Emis, reportMonth.avgTotal_20M.CH4_Emis_Percent, reportMonth.sumTotal_20M.CH4_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.H2S_Emis != "none") ExcelReportMonth.add(ref worksheet, "H₂S", "кг/сутки", reportMonth.tableReportDay.Select(s => s.H2S_Emis).ToList(), reportMonth.tableReportDay.Select(s => s.H2S_Emis_Percent).ToList(), reportMonth.PDZ.H2S_Emis, reportMonth.avgTotal_20M.H2S_Emis, reportMonth.avgTotal_20M.H2S_Emis_Percent, reportMonth.sumTotal_20M.H2S_Emis);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_1 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_1", "кг/сутки", reportMonth.tableReportDay.Select(s => s.Add_Emis_1).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Emis_1_Percent).ToList(), reportMonth.PDZ.Add_Emis_1, reportMonth.avgTotal_20M.Add_Emis_1, reportMonth.avgTotal_20M.Add_Emis_1_Percent, reportMonth.sumTotal_20M.Add_Emis_1);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_2 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_2", "кг/сутки", reportMonth.tableReportDay.Select(s => s.Add_Emis_2).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Emis_2_Percent).ToList(), reportMonth.PDZ.Add_Emis_2, reportMonth.avgTotal_20M.Add_Emis_2, reportMonth.avgTotal_20M.Add_Emis_2_Percent, reportMonth.sumTotal_20M.Add_Emis_2);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_3 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_3", "кг/сутки", reportMonth.tableReportDay.Select(s => s.Add_Emis_3).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Emis_3_Percent).ToList(), reportMonth.PDZ.Add_Emis_3, reportMonth.avgTotal_20M.Add_Emis_3, reportMonth.avgTotal_20M.Add_Emis_3_Percent, reportMonth.sumTotal_20M.Add_Emis_3);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_4 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_4", "кг/сутки", reportMonth.tableReportDay.Select(s => s.Add_Emis_4).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Emis_4_Percent).ToList(), reportMonth.PDZ.Add_Emis_4, reportMonth.avgTotal_20M.Add_Emis_4, reportMonth.avgTotal_20M.Add_Emis_4_Percent, reportMonth.sumTotal_20M.Add_Emis_4);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Add_Emis_5 != "none") ExcelReportMonth.add(ref worksheet, "Add_Conc_5", "кг/сутки", reportMonth.tableReportDay.Select(s => s.Add_Emis_5).ToList(), reportMonth.tableReportDay.Select(s => s.Add_Emis_5_Percent).ToList(), reportMonth.PDZ.Add_Emis_5, reportMonth.avgTotal_20M.Add_Emis_5, reportMonth.avgTotal_20M.Add_Emis_5_Percent, reportMonth.sumTotal_20M.Add_Emis_5);

                //Выделяем ПДК
                ColorTheCell(ref worksheet, "A6:" + masExcelCell[col] + "6", GlobalStaticSettingsASK.ColorHeader1);

                //Параметры
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Pressure != "none") ExcelReportMonth.add(ref worksheet, "Давление ДГ", "кПа", reportMonth.tableReportDay.Select(s => s.Pressure).ToList(), reportMonth.avgTotal_20M.Pressure, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Temperature != "none") ExcelReportMonth.add(ref worksheet, "Температура ДГ", "°С", reportMonth.tableReportDay.Select(s => s.Temperature).ToList(), reportMonth.avgTotal_20M.Temperature, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Speed != "none") ExcelReportMonth.add(ref worksheet, "Скорость ДГ", "м³/с", reportMonth.tableReportDay.Select(s => s.Speed).ToList(), reportMonth.avgTotal_20M.Speed, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Flow != "none") ExcelReportMonth.add(ref worksheet, "Расход ДГ", "м³/с, при н.у.", reportMonth.tableReportDay.Select(s => s.Flow).ToList(), reportMonth.avgTotal_20M.Flow, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_KIP != "none") ExcelReportMonth.add(ref worksheet, "Температура КИП", "°С", reportMonth.tableReportDay.Select(s => s.Temperature_KIP).ToList(), reportMonth.avgTotal_20M.Temperature_KIP, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Temperature_NOx != "none") ExcelReportMonth.add(ref worksheet, "Температура NOx", "°С", reportMonth.tableReportDay.Select(s => s.Temperature_NOx).ToList(), reportMonth.avgTotal_20M.Temperature_NOx, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.O2_Dry != "none") ExcelReportMonth.add(ref worksheet, "О₂ сух.", "%", reportMonth.tableReportDay.Select(s => s.O2_Dry).ToList(), reportMonth.avgTotal_20M.O2_Dry, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.O2_Wet != "none") ExcelReportMonth.add(ref worksheet, "О₂ вл.", "%", reportMonth.tableReportDay.Select(s => s.O2_Wet).ToList(), reportMonth.avgTotal_20M.O2_Wet, false, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.H2O != "none") ExcelReportMonth.add(ref worksheet, "H₂O", "%", reportMonth.tableReportDay.Select(s => s.H2O).ToList(), reportMonth.avgTotal_20M.H2O, false, false);

                if (GlobalStaticSettingsASK.VisibilityOptions20M.Pressure != "none") ExcelReportMonth.add(ref worksheet, "Режим", "", reportMonth.tableReportDay.Select(s => s.Mode_ASK).ToList(), reportMonth.sumTotal_20M.Mode_ASK, true, false);
                if (GlobalStaticSettingsASK.VisibilityOptions20M.Pressure != "none") ExcelReportMonth.add(ref worksheet, "ПДК", "", reportMonth.tableReportDay.Select(s => (double)s.PDZ_Fuel).ToList(), reportMonth.avgTotal_20M.PDZ_Fuel, false, true);


                //Текст 1 стороки Excel
                worksheet.Cells["A1"].Value = GlobalStaticSettingsASK.SettingOptions.ReportMonth_Name + " на " + monthName[month] + " " + (year+2021);

                using (var r = worksheet.Cells["A1:" + masExcelCell[col] + "1"])
                {
                    r.Merge = true; //объеденяет ячейки
                }

                //Текст 2 стороки Excel
                worksheet.Cells["A2"].Value = GlobalStaticSettingsASK.SettingOptions.NameASK;

                using (var r = worksheet.Cells["A2:" + masExcelCell[col] + "2"])
                {
                    r.Merge = true; //объеденяет ячейки
                }

                worksheet.Cells["A3"].Value = GlobalStaticSettingsASK.SettingOptions.ReportMonth_NameTable;

                //Текст 3 строки 
                using (var r = worksheet.Cells["A3:" + masExcelCell[col] + "3"])
                {
                    r.Merge = true; //объеденяет ячейки
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(GlobalStaticSettingsASK.ColorHeader2);
                }


                //Выстовляем границы все таблице, выстовляем шрифт и авто выравнивание
                using (var r = worksheet.Cells["A1:" + masExcelCell[col] + (row).ToString()])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    r.Style.Font.Size = GlobalStaticSettingsASK.SettingOptions.ReportMonth_FontSize;
                    r.Style.Font.Name = GlobalStaticSettingsASK.SettingOptions.ReportMonth_FontName;
                    r.Style.Font.Bold = GlobalStaticSettingsASK.SettingOptions.ReportMonth_FontBoldHeader;

                    double minimumSize = 1;
                    r.AutoFitColumns(minimumSize);
                }

                //Убираем жирные значения 20 минуток
                if (reportMonth.tableReportDay.Count != 0)
                    using (var r = worksheet.Cells["B7:" + masExcelCell[col] + (row - 2).ToString()])
                    {
                        r.Style.Font.Bold = GlobalStaticSettingsASK.SettingOptions.ReportMonth_FontBoldTable;
                    }


                //Задаём цвет времени/первой страке 20 минуток
                ColorTheCell(ref worksheet, "A7:A" + (row).ToString(), GlobalStaticSettingsASK.ColorHeader2);

                //Задаём цвет заголовка (концентрации)
                ColorTheCell(ref worksheet, "A4:" + masExcelCell[col] + "4", GlobalStaticSettingsASK.ColorHeader1);

                //Задаём цвет заголовка (ед. изм.)
                ColorTheCell(ref worksheet, "A5:" + masExcelCell[col] + "5", GlobalStaticSettingsASK.ColorHeader2);

                //Подвал "Среднее" выделяем
                ColorTheCell(ref worksheet, "A" + (row - 1).ToString(), GlobalStaticSettingsASK.ColorHeader1);

                //Подвал "Сумма" выделяем
                ColorTheCell(ref worksheet, "A" + (row).ToString() + ":" + masExcelCell[col] + (row).ToString(), GlobalStaticSettingsASK.ColorHeader1);


                //Подчёркиваем конец заголовка
                using (var r = worksheet.Cells["A6:" + masExcelCell[col] + "6"])
                {
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }

                //Подчёркиваем начало подвала
                using (var r = worksheet.Cells["A" + (row - 1).ToString() + ":" + masExcelCell[col] + (row - 1).ToString()])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                }

                xlPackage.Workbook.Properties.Title = GlobalStaticSettingsASK.SettingOptions.ReportMonth_Name;
                xlPackage.Workbook.Properties.Author = "";
                xlPackage.Save();
            }
            stream.Position = 0;
            return stream;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                                      Методы помощи
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        //Добовляем концентрацию в таблицу
        private static void add(ref ExcelWorksheet worksheet, string nameConc, string unitsConc, List<double> komponent, List<double> procent, double pdz, double avgTotal_20M, double avgTotal_20M_Procent, double sumTotal_20M)
        {
            //Заголовки
            row = startRow;
            worksheet.Cells[row, ++col].Value = nameConc;     //Название компонента концентрации
            worksheet.Cells[row, col+1].Value = "прев.";
            worksheet.Cells[++row, col].Value = unitsConc;    //Ед. изм. концентраций

            if (pdz > 0.0 && pdz < 999999.0) worksheet.Cells[++row, col].Value = pdz;
            else worksheet.Cells[++row, col].Value = GlobalStaticSettingsASK.SettingOptions.NoneValue;


            //Значение 20м
            for (int i = 0; i < komponent.Count; i++)
            {
                worksheet.Cells[++row, col].Value = komponent[i];
            }

            worksheet.Cells[++row, col].Value = avgTotal_20M;
            if (sumTotal_20M > 0.0) worksheet.Cells[++row, col].Value = sumTotal_20M + " кг.";


            //%
            row = startRow;
            worksheet.Cells[++row, ++col].Value = "ч.";
            row++;

            for (int i = 0; i < komponent.Count; i++)
            {
                ++row;
                if (procent[i] > 0.0)
                {
                    worksheet.Cells[row, col].Value = procent[i];
                    ColorTheCell(ref worksheet, masExcelCell[col - 1] + row.ToString() + ":" + masExcelCell[col] + row.ToString(), GlobalStaticSettingsASK.ColorExcess);
                }
            }


            //Значение подвала + процент + подсветка
            row++;
            if (avgTotal_20M_Procent > 0.0)
            {
                worksheet.Cells[row, col].Value = avgTotal_20M_Procent;
                colorTMPFooter = GlobalStaticSettingsASK.ColorExcess;
            }
            else colorTMPFooter = GlobalStaticSettingsASK.ColorHeader1;

            ColorTheCell(ref worksheet, masExcelCell[col - 1] + row.ToString() + ":" + masExcelCell[col] + row.ToString(), colorTMPFooter);
        }



        //Перегрузка для добовления парамтров
        private static void add(ref ExcelWorksheet worksheet, string nameConc, string unitsConc, List<double> komponent, double avgTotal_20M, bool mode, bool pdk)
        {
            //Заголовки
            row = startRow;
            worksheet.Cells[row, ++col].Value = nameConc;     //Название компонента концентрации
            worksheet.Cells[++row, col].Value = unitsConc;    //Ед. изм. концентраций
            row++;


            //Значение 20м
            if (!mode)
            {
                for (int i = 0; i < komponent.Count; i++)
                {
                    worksheet.Cells[++row, col].Value = komponent[i];
                }
                if (!pdk)
                    worksheet.Cells[++row, col].Value = avgTotal_20M;
                else
                    row++;
            }
            else
            {
                for (int i = 0; i < komponent.Count; i++)
                {
                    worksheet.Cells[++row, col].Value = mode_ASK_String[(int)reportMonth.tableReportDay[i].Mode_ASK];
                    switch (komponent[i])
                    {
                        case 1.0:
                            ColorTheCell(ref worksheet, masExcelCell[col] + row.ToString() + ":" + masExcelCell[col] + row.ToString(), GlobalStaticSettingsASK.ColorExcess);
                            break;
                        case 2.0:
                            ColorTheCell(ref worksheet, masExcelCell[col] + row.ToString() + ":" + masExcelCell[col] + row.ToString(), GlobalStaticSettingsASK.ColorHeader1);
                            break;
                    }
                }
                worksheet.Cells[++row, col].Value = avgTotal_20M + " ч.";
            }


            //Значение подвала + процент + подсветка
            colorTMPFooter = GlobalStaticSettingsASK.ColorHeader1;
            if (avgTotal_20M > 0.0 && mode)
                colorTMPFooter = GlobalStaticSettingsASK.ColorExcess;

            ColorTheCell(ref worksheet, masExcelCell[col] + row.ToString() + ":" + masExcelCell[col] + row.ToString(), colorTMPFooter);

            row++;
        }


        //Окрашиваем ячейки
        private static void ColorTheCell(ref ExcelWorksheet worksheet, string cellExcel, Color colorCell)
        {
            using (var r = worksheet.Cells[cellExcel])
            {
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(colorCell);
            }
        }
    }
}
