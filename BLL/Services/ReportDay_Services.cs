using ASK.BLL.Helper;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using ASK.DAL;
using ASK.DAL.Interfaces;
using ASK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASK.BLL.Services
{
    public class ReportDay_Services : IReportDay
    {
        private readonly IPDZ _PDZ_Repository;
        private readonly IAVG_20_MINUTES _AVG_20_MINUTES_Repository;



        //Коструктор с DI
        public ReportDay_Services(IPDZ PDZ_Repository, IAVG_20_MINUTES AVG_20_MINUTES_Repository)
        {
            _PDZ_Repository = PDZ_Repository;
            _AVG_20_MINUTES_Repository = AVG_20_MINUTES_Repository;
        }



        public ReportDay_Model Generate(DateTime date)
        {
            ReportDay_Model ReportDay = new ReportDay_Model();
            ReportDay.TypeReport = TypeReports.ReportDay;       //Указывает тип отчёта для обработчика в excel

            List<PDZ> PDZs = new List<PDZ>();                   //Вытаскиваем ПДЗ на текущие сутки
            PDZs = _PDZ_Repository.Get_DayAll_PDZ(date).OrderBy(w => w.NumberPDZ).ToList();

            for (int i = 0; i < PDZs.Count; i++)
            {
                if (PDZs[i].Current == true)
                {
                    ReportDay.PDZ.Date = PDZs[i].Date;
                    ReportDay.PDZ.NumberPDZ = PDZs[i].NumberPDZ;

                    ReportDay.PDZ.CO_Conc = PDZs[i].CO_Conc;
                    ReportDay.PDZ.CO2_Conc = PDZs[i].CO2_Conc;
                    ReportDay.PDZ.NO_Conc = PDZs[i].NO_Conc;
                    ReportDay.PDZ.NO2_Conc = PDZs[i].NO2_Conc;
                    ReportDay.PDZ.NOx_Conc = PDZs[i].NOx_Conc;
                    ReportDay.PDZ.SO2_Conc = PDZs[i].SO2_Conc;
                    ReportDay.PDZ.Dust_Conc = PDZs[i].Dust_Conc;
                    ReportDay.PDZ.CH4_Conc = PDZs[i].CH4_Conc;
                    ReportDay.PDZ.H2S_Conc = PDZs[i].H2S_Conc;
                    ReportDay.PDZ.NH3_Conc = PDZs[i].NH3_Conc;
                    ReportDay.PDZ.Add_Conc_1 = PDZs[i].Add_Conc_1;
                    ReportDay.PDZ.Add_Conc_2 = PDZs[i].Add_Conc_2;
                    ReportDay.PDZ.Add_Conc_3 = PDZs[i].Add_Conc_3;
                    ReportDay.PDZ.Add_Conc_4 = PDZs[i].Add_Conc_4;
                    ReportDay.PDZ.Add_Conc_5 = PDZs[i].Add_Conc_5;

                    ReportDay.PDZ.CO_Emis = PDZs[i].CO_Emis;
                    ReportDay.PDZ.CO2_Emis = PDZs[i].CO2_Emis;
                    ReportDay.PDZ.NO_Emis = PDZs[i].NO_Emis;
                    ReportDay.PDZ.NO2_Emis = PDZs[i].NO2_Emis;
                    ReportDay.PDZ.NOx_Emis = PDZs[i].NOx_Emis;
                    ReportDay.PDZ.SO2_Emis = PDZs[i].SO2_Emis;
                    ReportDay.PDZ.Dust_Emis = PDZs[i].Dust_Emis;
                    ReportDay.PDZ.CH4_Emis = PDZs[i].CH4_Emis;
                    ReportDay.PDZ.H2S_Emis = PDZs[i].H2S_Emis;
                    ReportDay.PDZ.H2S_Emis = PDZs[i].NH3_Emis;
                    ReportDay.PDZ.Add_Emis_1 = PDZs[i].Add_Emis_1;
                    ReportDay.PDZ.Add_Emis_2 = PDZs[i].Add_Emis_2;
                    ReportDay.PDZ.Add_Emis_3 = PDZs[i].Add_Emis_3;
                    ReportDay.PDZ.Add_Emis_4 = PDZs[i].Add_Emis_4;
                    ReportDay.PDZ.Add_Emis_5 = PDZs[i].Add_Emis_5;

                    //PDZ[i + 1].Current = PDZs[i].Current;
                    break;
                }
            }
            //Таблица концентраций
            List<AVG_20_MINUTES> avg_20_minutes = new List<AVG_20_MINUTES>();                         //Таблица 20-минуток из БД - за сутки
            avg_20_minutes = _AVG_20_MINUTES_Repository.Get_DayAll_AVG_20_MINUTES(date);

            ReportDay.durationTotal_20M = new TableReport_Model();

            for (int i = 0; i < avg_20_minutes.Count; i++)
            {
                ReportDay.TableReport.Add(new TableReport_Model());

                ReportDay.TableReport[i].Date = avg_20_minutes[i].Date;
                ReportDay.TableReport[i].Mode_ASK = avg_20_minutes[i].Mode_ASK;
                ReportDay.TableReport[i].PDZ_Fuel = avg_20_minutes[i].PDZ_Fuel; //Обязательно должен идти первее подсчёта процентов превышений

                //Концентрации
                ReportDay.TableReport[i].CO_Conc = avg_20_minutes[i].Conc_CO;
                ReportDay.TableReport[i].CO2_Conc = avg_20_minutes[i].Conc_CO2;
                ReportDay.TableReport[i].NO_Conc = avg_20_minutes[i].Conc_NO;
                ReportDay.TableReport[i].NO2_Conc = avg_20_minutes[i].Conc_NO2;
                ReportDay.TableReport[i].NOx_Conc = avg_20_minutes[i].Conc_NOx;
                ReportDay.TableReport[i].SO2_Conc = avg_20_minutes[i].Conc_SO2;
                ReportDay.TableReport[i].Dust_Conc = avg_20_minutes[i].Conc_Dust;
                ReportDay.TableReport[i].CH4_Conc = avg_20_minutes[i].Conc_CH4;
                ReportDay.TableReport[i].H2S_Conc = avg_20_minutes[i].Conc_H2S;
                ReportDay.TableReport[i].NH3_Conc = avg_20_minutes[i].Conc_NH3;
                ReportDay.TableReport[i].Add_Conc_1 = avg_20_minutes[i].Conc_D1;
                ReportDay.TableReport[i].Add_Conc_2 = avg_20_minutes[i].Conc_D2;
                ReportDay.TableReport[i].Add_Conc_3 = avg_20_minutes[i].Conc_D3;
                ReportDay.TableReport[i].Add_Conc_4 = avg_20_minutes[i].Conc_D4;
                ReportDay.TableReport[i].Add_Conc_5 = avg_20_minutes[i].Conc_D5;
                
                //Процент превышений Концентраций
                if (ReportDay.PDZ.CO_Conc != 0.0 && ReportDay.TableReport[i].CO_Conc > ReportDay.PDZ.CO_Conc) { ReportDay.TableReport[i].CO_Conc_Percent = Math.Round((ReportDay.TableReport[i].CO_Conc / (ReportDay.PDZ.CO_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.CO_Conc_Percent++; }
                if (ReportDay.PDZ.CO2_Conc != 0.0 && ReportDay.TableReport[i].CO2_Conc > ReportDay.PDZ.CO2_Conc) { ReportDay.TableReport[i].CO2_Conc_Percent = Math.Round((ReportDay.TableReport[i].CO2_Conc / (ReportDay.PDZ.CO2_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.CO2_Conc_Percent++; }
                if (ReportDay.PDZ.NO_Conc != 0.0 && ReportDay.TableReport[i].NO_Conc > ReportDay.PDZ.NO_Conc) { ReportDay.TableReport[i].NO_Conc_Percent = Math.Round((ReportDay.TableReport[i].NO_Conc / (ReportDay.PDZ.NO_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NO_Conc_Percent++; }
                if (ReportDay.PDZ.NO2_Conc != 0.0 && ReportDay.TableReport[i].NO2_Conc > ReportDay.PDZ.NO2_Conc) { ReportDay.TableReport[i].NO2_Conc_Percent = Math.Round((ReportDay.TableReport[i].NO2_Conc / (ReportDay.PDZ.NO2_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NO2_Conc_Percent++; }
                if (ReportDay.PDZ.NOx_Conc != 0.0 && ReportDay.TableReport[i].NOx_Conc > ReportDay.PDZ.NOx_Conc) { ReportDay.TableReport[i].NOx_Conc_Percent = Math.Round((ReportDay.TableReport[i].NOx_Conc / (ReportDay.PDZ.NOx_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NOx_Conc_Percent++; }
                if (ReportDay.PDZ.SO2_Conc != 0.0 && ReportDay.TableReport[i].SO2_Conc > ReportDay.PDZ.SO2_Conc) { ReportDay.TableReport[i].SO2_Conc_Percent = Math.Round((ReportDay.TableReport[i].SO2_Conc / (ReportDay.PDZ.SO2_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.SO2_Conc_Percent++; }
                if (ReportDay.PDZ.Dust_Conc != 0.0 && ReportDay.TableReport[i].Dust_Conc > ReportDay.PDZ.Dust_Conc) { ReportDay.TableReport[i].Dust_Conc_Percent = Math.Round((ReportDay.TableReport[i].Dust_Conc / (ReportDay.PDZ.Dust_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Dust_Conc_Percent++; }
                if (ReportDay.PDZ.CH4_Conc != 0.0 && ReportDay.TableReport[i].CH4_Conc > ReportDay.PDZ.CH4_Conc) { ReportDay.TableReport[i].CH4_Conc_Percent = Math.Round((ReportDay.TableReport[i].CH4_Conc / (ReportDay.PDZ.CH4_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.CH4_Conc_Percent++; }
                if (ReportDay.PDZ.H2S_Conc != 0.0 && ReportDay.TableReport[i].H2S_Conc > ReportDay.PDZ.H2S_Conc) { ReportDay.TableReport[i].H2S_Conc_Percent = Math.Round((ReportDay.TableReport[i].H2S_Conc / (ReportDay.PDZ.H2S_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.H2S_Conc_Percent++; }
                if (ReportDay.PDZ.NH3_Conc != 0.0 && ReportDay.TableReport[i].NH3_Conc > ReportDay.PDZ.NH3_Conc) { ReportDay.TableReport[i].NH3_Conc_Percent = Math.Round((ReportDay.TableReport[i].NH3_Conc / (ReportDay.PDZ.NH3_Conc / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NH3_Conc_Percent++; }
                if (ReportDay.PDZ.Add_Conc_1 != 0.0 && ReportDay.TableReport[i].Add_Conc_1 > ReportDay.PDZ.Add_Conc_1) { ReportDay.TableReport[i].Add_Conc_1_Percent = Math.Round((ReportDay.TableReport[i].Add_Conc_1 / (ReportDay.PDZ.Add_Conc_1 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Conc_1_Percent++; }
                if (ReportDay.PDZ.Add_Conc_2 != 0.0 && ReportDay.TableReport[i].Add_Conc_2 > ReportDay.PDZ.Add_Conc_2) { ReportDay.TableReport[i].Add_Conc_2_Percent = Math.Round((ReportDay.TableReport[i].Add_Conc_2 / (ReportDay.PDZ.Add_Conc_2 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Conc_2_Percent++; }
                if (ReportDay.PDZ.Add_Conc_3 != 0.0 && ReportDay.TableReport[i].Add_Conc_3 > ReportDay.PDZ.Add_Conc_3) { ReportDay.TableReport[i].Add_Conc_3_Percent = Math.Round((ReportDay.TableReport[i].Add_Conc_3 / (ReportDay.PDZ.Add_Conc_3 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Conc_3_Percent++; }
                if (ReportDay.PDZ.Add_Conc_4 != 0.0 && ReportDay.TableReport[i].Add_Conc_4 > ReportDay.PDZ.Add_Conc_4) { ReportDay.TableReport[i].Add_Conc_4_Percent = Math.Round((ReportDay.TableReport[i].Add_Conc_4 / (ReportDay.PDZ.Add_Conc_4 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Conc_4_Percent++; }
                if (ReportDay.PDZ.Add_Conc_5 != 0.0 && ReportDay.TableReport[i].Add_Conc_5 > ReportDay.PDZ.Add_Conc_5) { ReportDay.TableReport[i].Add_Conc_5_Percent = Math.Round((ReportDay.TableReport[i].Add_Conc_5 / (ReportDay.PDZ.Add_Conc_5 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Conc_5_Percent++; }

                //Выбросы
                ReportDay.TableReport[i].CO_Emis = avg_20_minutes[i].Emis_CO;
                ReportDay.TableReport[i].CO2_Emis = avg_20_minutes[i].Emis_CO2;
                ReportDay.TableReport[i].NO_Emis = avg_20_minutes[i].Emis_NO;
                ReportDay.TableReport[i].NO2_Emis = avg_20_minutes[i].Emis_NO2;
                ReportDay.TableReport[i].NOx_Emis = avg_20_minutes[i].Emis_NOx;
                ReportDay.TableReport[i].SO2_Emis = avg_20_minutes[i].Emis_SO2;
                ReportDay.TableReport[i].Dust_Emis = avg_20_minutes[i].Emis_Dust;
                ReportDay.TableReport[i].CH4_Emis = avg_20_minutes[i].Emis_CH4;
                ReportDay.TableReport[i].H2S_Conc = avg_20_minutes[i].Emis_H2S;
                ReportDay.TableReport[i].NH3_Conc = avg_20_minutes[i].Emis_NH3;
                ReportDay.TableReport[i].Add_Emis_1 = avg_20_minutes[i].Emis_D1;
                ReportDay.TableReport[i].Add_Emis_2 = avg_20_minutes[i].Emis_D2;
                ReportDay.TableReport[i].Add_Emis_3 = avg_20_minutes[i].Emis_D3;
                ReportDay.TableReport[i].Add_Emis_4 = avg_20_minutes[i].Emis_D4;
                ReportDay.TableReport[i].Add_Emis_5 = avg_20_minutes[i].Emis_D5;
                
                //Процент превышений Выбросы
                if (ReportDay.PDZ.CO_Emis != 0.0 && ReportDay.TableReport[i].CO_Emis > ReportDay.PDZ.CO_Emis) { ReportDay.TableReport[i].CO_Emis_Percent = Math.Round((ReportDay.TableReport[i].CO_Emis / (ReportDay.PDZ.CO_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.CO_Emis_Percent++; }
                if (ReportDay.PDZ.CO2_Emis != 0.0 && ReportDay.TableReport[i].CO2_Emis > ReportDay.PDZ.CO2_Emis) { ReportDay.TableReport[i].CO2_Emis_Percent = Math.Round((ReportDay.TableReport[i].CO2_Emis / (ReportDay.PDZ.CO2_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.CO2_Emis_Percent++; }
                if (ReportDay.PDZ.NO_Emis != 0.0 && ReportDay.TableReport[i].NO_Emis > ReportDay.PDZ.NO_Emis) { ReportDay.TableReport[i].NO_Emis_Percent = Math.Round((ReportDay.TableReport[i].NO_Emis / (ReportDay.PDZ.NO_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NO_Emis_Percent++; }
                if (ReportDay.PDZ.NO2_Emis != 0.0 && ReportDay.TableReport[i].NO2_Emis > ReportDay.PDZ.NO2_Emis) { ReportDay.TableReport[i].NO2_Emis_Percent = Math.Round((ReportDay.TableReport[i].NO2_Emis / (ReportDay.PDZ.NO2_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NO2_Emis_Percent++; }
                if (ReportDay.PDZ.NOx_Emis != 0.0 && ReportDay.TableReport[i].NOx_Emis > ReportDay.PDZ.NOx_Emis) { ReportDay.TableReport[i].NOx_Emis_Percent = Math.Round((ReportDay.TableReport[i].NOx_Emis / (ReportDay.PDZ.NOx_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NOx_Emis_Percent++; }
                if (ReportDay.PDZ.SO2_Emis != 0.0 && ReportDay.TableReport[i].SO2_Emis > ReportDay.PDZ.SO2_Emis) { ReportDay.TableReport[i].SO2_Emis_Percent = Math.Round((ReportDay.TableReport[i].SO2_Emis / (ReportDay.PDZ.SO2_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.SO2_Emis_Percent++; }
                if (ReportDay.PDZ.Dust_Emis != 0.0 && ReportDay.TableReport[i].Dust_Emis > ReportDay.PDZ.Dust_Emis) { ReportDay.TableReport[i].Dust_Emis_Percent = Math.Round((ReportDay.TableReport[i].Dust_Emis / (ReportDay.PDZ.Dust_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Dust_Emis_Percent++; }
                if (ReportDay.PDZ.CH4_Emis != 0.0 && ReportDay.TableReport[i].CH4_Emis > ReportDay.PDZ.CH4_Emis) { ReportDay.TableReport[i].CH4_Emis_Percent = Math.Round((ReportDay.TableReport[i].CH4_Emis / (ReportDay.PDZ.CH4_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.CH4_Emis_Percent++; }
                if (ReportDay.PDZ.H2S_Emis != 0.0 && ReportDay.TableReport[i].H2S_Emis > ReportDay.PDZ.H2S_Emis) { ReportDay.TableReport[i].H2S_Emis_Percent = Math.Round((ReportDay.TableReport[i].H2S_Emis / (ReportDay.PDZ.H2S_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.H2S_Emis_Percent++; }
                if (ReportDay.PDZ.NH3_Emis != 0.0 && ReportDay.TableReport[i].NH3_Emis > ReportDay.PDZ.NH3_Emis) { ReportDay.TableReport[i].NH3_Emis_Percent = Math.Round((ReportDay.TableReport[i].NH3_Emis / (ReportDay.PDZ.NH3_Emis / 100.0)) - 100, 1); ReportDay.durationTotal_20M.NH3_Emis_Percent++; }
                if (ReportDay.PDZ.Add_Emis_1 != 0.0 && ReportDay.TableReport[i].Add_Emis_1 > ReportDay.PDZ.Add_Emis_1) { ReportDay.TableReport[i].Add_Emis_1_Percent = Math.Round((ReportDay.TableReport[i].Add_Emis_1 / (ReportDay.PDZ.Add_Emis_1 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Emis_1_Percent++; }
                if (ReportDay.PDZ.Add_Emis_2 != 0.0 && ReportDay.TableReport[i].Add_Emis_2 > ReportDay.PDZ.Add_Emis_2) { ReportDay.TableReport[i].Add_Emis_2_Percent = Math.Round((ReportDay.TableReport[i].Add_Emis_2 / (ReportDay.PDZ.Add_Emis_2 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Emis_2_Percent++; }
                if (ReportDay.PDZ.Add_Emis_3 != 0.0 && ReportDay.TableReport[i].Add_Emis_3 > ReportDay.PDZ.Add_Emis_3) { ReportDay.TableReport[i].Add_Emis_3_Percent = Math.Round((ReportDay.TableReport[i].Add_Emis_3 / (ReportDay.PDZ.Add_Emis_3 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Emis_3_Percent++; }
                if (ReportDay.PDZ.Add_Emis_4 != 0.0 && ReportDay.TableReport[i].Add_Emis_4 > ReportDay.PDZ.Add_Emis_4) { ReportDay.TableReport[i].Add_Emis_4_Percent = Math.Round((ReportDay.TableReport[i].Add_Emis_4 / (ReportDay.PDZ.Add_Emis_4 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Emis_4_Percent++; }
                if (ReportDay.PDZ.Add_Emis_5 != 0.0 && ReportDay.TableReport[i].Add_Emis_5 > ReportDay.PDZ.Add_Emis_5) { ReportDay.TableReport[i].Add_Emis_5_Percent = Math.Round((ReportDay.TableReport[i].Add_Emis_5 / (ReportDay.PDZ.Add_Emis_5 / 100.0)) - 100, 1); ReportDay.durationTotal_20M.Add_Emis_5_Percent++; }

                //Параметры
                ReportDay.TableReport[i].Pressure = avg_20_minutes[i].Pressure;
                ReportDay.TableReport[i].Temperature = avg_20_minutes[i].Temperature;
                ReportDay.TableReport[i].Temperature_KIP = avg_20_minutes[i].Temperature_KIP;
                ReportDay.TableReport[i].Temperature_NOx = avg_20_minutes[i].Temperature_NOx;
                ReportDay.TableReport[i].Speed = avg_20_minutes[i].Speed;
                ReportDay.TableReport[i].Flow = avg_20_minutes[i].Flow;
                ReportDay.TableReport[i].O2_Dry = avg_20_minutes[i].O2_Dry;
                ReportDay.TableReport[i].O2_Wet = avg_20_minutes[i].O2_Wet;
                ReportDay.TableReport[i].H2O = avg_20_minutes[i].H2O;

                ReportDay.TableReport[i].Pressure_KIP = avg_20_minutes[i].Pressure_KIP;
                ReportDay.TableReport[i].Temperature_Point_Dew = avg_20_minutes[i].Temperature_Point_Dew;
                ReportDay.TableReport[i].Temperature_Room = avg_20_minutes[i].Temperature_Room;
                ReportDay.TableReport[i].Temperature_PGS = avg_20_minutes[i].Temperature_PGS;
                ReportDay.TableReport[i].O2_Room = avg_20_minutes[i].O2_Room;
                ReportDay.TableReport[i].O2_PGS = avg_20_minutes[i].O2_PGS;

            }
            ReportDay.TableReport = ReportDay.TableReport.OrderBy(x => x.Date).ToList();

            //Высчитывает среднее и суммараное значение концентрацйи, выбросов и параметров
            ReportDay.avgTotal_20M = new TableReport_Model();
            ReportDay.sumTotal_20M = new TableReport_Model();

            foreach (var tabletDay in ReportDay.TableReport)
            {
                //Концентрации
                ReportDay.sumTotal_20M.CO_Conc += tabletDay.CO_Conc;
                ReportDay.sumTotal_20M.CO2_Conc += tabletDay.CO2_Conc;
                ReportDay.sumTotal_20M.NOx_Conc += tabletDay.NOx_Conc;
                ReportDay.sumTotal_20M.SO2_Conc += tabletDay.SO2_Conc;
                ReportDay.sumTotal_20M.Dust_Conc += tabletDay.Dust_Conc;
                ReportDay.sumTotal_20M.CH4_Conc += tabletDay.CH4_Conc;
                ReportDay.sumTotal_20M.H2S_Conc += tabletDay.H2S_Conc;
                ReportDay.sumTotal_20M.NH3_Conc += tabletDay.NH3_Conc;
                ReportDay.sumTotal_20M.Add_Conc_1 += tabletDay.Add_Conc_1;
                ReportDay.sumTotal_20M.Add_Conc_2 += tabletDay.Add_Conc_2;
                ReportDay.sumTotal_20M.Add_Conc_3 += tabletDay.Add_Conc_3;
                ReportDay.sumTotal_20M.Add_Conc_4 += tabletDay.Add_Conc_4;
                ReportDay.sumTotal_20M.Add_Conc_5 += tabletDay.Add_Conc_5;

                //Выбросы
                ReportDay.sumTotal_20M.CO_Emis += tabletDay.CO_Emis;
                ReportDay.sumTotal_20M.CO2_Emis += tabletDay.CO2_Emis;
                ReportDay.sumTotal_20M.NO_Emis += tabletDay.NO_Emis;
                ReportDay.sumTotal_20M.NOx_Emis += tabletDay.NOx_Emis;
                ReportDay.sumTotal_20M.NO2_Emis += tabletDay.NO2_Emis;
                ReportDay.sumTotal_20M.SO2_Emis += tabletDay.SO2_Emis;
                ReportDay.sumTotal_20M.Dust_Emis += tabletDay.Dust_Emis;
                ReportDay.sumTotal_20M.CH4_Emis += tabletDay.CH4_Emis;
                ReportDay.sumTotal_20M.H2S_Emis += tabletDay.H2S_Emis;
                ReportDay.sumTotal_20M.NH3_Emis += tabletDay.NH3_Emis;
                ReportDay.sumTotal_20M.Add_Emis_1 += tabletDay.Add_Emis_1;
                ReportDay.sumTotal_20M.Add_Emis_2 += tabletDay.Add_Emis_2;
                ReportDay.sumTotal_20M.Add_Emis_3 += tabletDay.Add_Emis_3;
                ReportDay.sumTotal_20M.Add_Emis_4 += tabletDay.Add_Emis_4;
                ReportDay.sumTotal_20M.Add_Emis_5 += tabletDay.Add_Emis_5;

                //Параметры
                ReportDay.sumTotal_20M.Pressure += tabletDay.Pressure;
                ReportDay.sumTotal_20M.Temperature += tabletDay.Temperature;
                ReportDay.sumTotal_20M.Temperature_KIP += tabletDay.Temperature_KIP;
                ReportDay.sumTotal_20M.Temperature_NOx += tabletDay.Temperature_NOx;
                ReportDay.sumTotal_20M.Speed += tabletDay.Speed;
                ReportDay.sumTotal_20M.Flow += tabletDay.Flow;
                ReportDay.sumTotal_20M.O2_Dry += tabletDay.O2_Dry;
                ReportDay.sumTotal_20M.O2_Wet += tabletDay.O2_Wet;
                ReportDay.sumTotal_20M.H2O += tabletDay.H2O;

                ReportDay.sumTotal_20M.Pressure_KIP += tabletDay.Pressure_KIP;
                ReportDay.sumTotal_20M.Temperature_Point_Dew += tabletDay.Temperature_Point_Dew;
                ReportDay.sumTotal_20M.Temperature_Room += tabletDay.Temperature_Room;
                ReportDay.sumTotal_20M.Temperature_PGS += tabletDay.Temperature_PGS;
                ReportDay.sumTotal_20M.O2_Room += tabletDay.O2_Room;
                ReportDay.sumTotal_20M.O2_PGS += tabletDay.O2_PGS;

                //Считаем время простоя
                if (tabletDay.Mode_ASK == 1.0)
                {
                    ReportDay.sumTotal_20M.Mode_ASK = ReportDay.sumTotal_20M.Mode_ASK + 1.0;
                    ReportDay.avgTotal_20M.Mode_ASK = 1.0;
                }
            }
            //Усредняем за 20 мин
            int counter = ReportDay.TableReport.Count;
            if (counter <= 1) counter = 1;

            //Концентрации
            ReportDay.avgTotal_20M.CO_Conc = Math.Round(ReportDay.sumTotal_20M.CO_Conc / counter, 3);
            ReportDay.avgTotal_20M.CO2_Conc = Math.Round(ReportDay.sumTotal_20M.CO2_Conc / counter, 3);
            ReportDay.avgTotal_20M.NO_Conc = Math.Round(ReportDay.sumTotal_20M.NO_Conc / counter, 3);
            ReportDay.avgTotal_20M.NO2_Conc = Math.Round(ReportDay.sumTotal_20M.NO2_Conc / counter, 3);
            ReportDay.avgTotal_20M.NOx_Conc = Math.Round(ReportDay.sumTotal_20M.NOx_Conc / counter, 3);
            ReportDay.avgTotal_20M.SO2_Conc = Math.Round(ReportDay.sumTotal_20M.SO2_Conc / counter, 3);
            ReportDay.avgTotal_20M.Dust_Conc = Math.Round(ReportDay.sumTotal_20M.Dust_Conc / counter, 3);
            ReportDay.avgTotal_20M.CH4_Conc = Math.Round(ReportDay.sumTotal_20M.CH4_Conc / counter, 3);
            ReportDay.avgTotal_20M.H2S_Conc = Math.Round(ReportDay.sumTotal_20M.H2S_Conc / counter, 3);
            ReportDay.avgTotal_20M.NH3_Conc = Math.Round(ReportDay.sumTotal_20M.NH3_Conc / counter, 3);
            ReportDay.avgTotal_20M.Add_Conc_1 = Math.Round(ReportDay.sumTotal_20M.Add_Conc_1 / counter, 3);
            ReportDay.avgTotal_20M.Add_Conc_2 = Math.Round(ReportDay.sumTotal_20M.Add_Conc_2 / counter, 3);
            ReportDay.avgTotal_20M.Add_Conc_3 = Math.Round(ReportDay.sumTotal_20M.Add_Conc_3 / counter, 3);
            ReportDay.avgTotal_20M.Add_Conc_4 = Math.Round(ReportDay.sumTotal_20M.Add_Conc_4 / counter, 3);
            ReportDay.avgTotal_20M.Add_Conc_5 = Math.Round(ReportDay.sumTotal_20M.Add_Conc_5 / counter, 3);
            
            //Процент превышений Концентраций
            if (ReportDay.PDZ.CO_Conc != 0.0 && ReportDay.avgTotal_20M.CO_Conc > ReportDay.PDZ.CO_Conc) ReportDay.avgTotal_20M.CO_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.CO_Conc / (ReportDay.PDZ.CO_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.CO2_Conc != 0.0 && ReportDay.avgTotal_20M.CO2_Conc > ReportDay.PDZ.CO2_Conc) ReportDay.avgTotal_20M.CO2_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.CO2_Conc / (ReportDay.PDZ.CO2_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NO_Conc != 0.0 && ReportDay.avgTotal_20M.NO_Conc > ReportDay.PDZ.NO_Conc) ReportDay.avgTotal_20M.NO_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.NO_Conc / (ReportDay.PDZ.NO_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NO2_Conc != 0.0 && ReportDay.avgTotal_20M.NO2_Conc > ReportDay.PDZ.NO2_Conc) ReportDay.avgTotal_20M.NO2_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.NO2_Conc / (ReportDay.PDZ.NO2_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NOx_Conc != 0.0 && ReportDay.avgTotal_20M.NOx_Conc > ReportDay.PDZ.NOx_Conc) ReportDay.avgTotal_20M.NOx_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.NOx_Conc / (ReportDay.PDZ.NOx_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.SO2_Conc != 0.0 && ReportDay.avgTotal_20M.SO2_Conc > ReportDay.PDZ.SO2_Conc) ReportDay.avgTotal_20M.SO2_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.SO2_Conc / (ReportDay.PDZ.SO2_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Dust_Conc != 0.0 && ReportDay.avgTotal_20M.Dust_Conc > ReportDay.PDZ.Dust_Conc) ReportDay.avgTotal_20M.Dust_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.Dust_Conc / (ReportDay.PDZ.Dust_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.CH4_Conc != 0.0 && ReportDay.avgTotal_20M.CH4_Conc > ReportDay.PDZ.CH4_Conc) ReportDay.avgTotal_20M.CH4_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.CH4_Conc / (ReportDay.PDZ.CH4_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.H2S_Conc != 0.0 && ReportDay.avgTotal_20M.H2S_Conc > ReportDay.PDZ.H2S_Conc) ReportDay.avgTotal_20M.H2S_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.H2S_Conc / (ReportDay.PDZ.H2S_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NH3_Conc != 0.0 && ReportDay.avgTotal_20M.NH3_Conc > ReportDay.PDZ.NH3_Conc) ReportDay.avgTotal_20M.NH3_Conc_Percent = Math.Round((ReportDay.avgTotal_20M.NH3_Conc / (ReportDay.PDZ.NH3_Conc / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Conc_1 != 0.0 && ReportDay.avgTotal_20M.Add_Conc_1 > ReportDay.PDZ.Add_Conc_1) ReportDay.avgTotal_20M.Add_Conc_1_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Conc_1 / (ReportDay.PDZ.Add_Conc_1 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Conc_2 != 0.0 && ReportDay.avgTotal_20M.Add_Conc_2 > ReportDay.PDZ.Add_Conc_2) ReportDay.avgTotal_20M.Add_Conc_2_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Conc_2 / (ReportDay.PDZ.Add_Conc_2 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Conc_3 != 0.0 && ReportDay.avgTotal_20M.Add_Conc_3 > ReportDay.PDZ.Add_Conc_3) ReportDay.avgTotal_20M.Add_Conc_3_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Conc_3 / (ReportDay.PDZ.Add_Conc_3 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Conc_4 != 0.0 && ReportDay.avgTotal_20M.Add_Conc_4 > ReportDay.PDZ.Add_Conc_4) ReportDay.avgTotal_20M.Add_Conc_4_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Conc_4 / (ReportDay.PDZ.Add_Conc_4 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Conc_5 != 0.0 && ReportDay.avgTotal_20M.Add_Conc_5 > ReportDay.PDZ.Add_Conc_5) ReportDay.avgTotal_20M.Add_Conc_5_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Conc_5 / (ReportDay.PDZ.Add_Conc_5 / 100.0)) - 100, 1);

            //Выбросы
            ReportDay.avgTotal_20M.CO_Emis = Math.Round(ReportDay.sumTotal_20M.CO_Emis / counter, 3);
            ReportDay.avgTotal_20M.CO2_Emis = Math.Round(ReportDay.sumTotal_20M.CO2_Emis / counter, 3);
            ReportDay.avgTotal_20M.NO_Emis = Math.Round(ReportDay.sumTotal_20M.NO_Emis / counter, 3);
            ReportDay.avgTotal_20M.NO2_Emis = Math.Round(ReportDay.sumTotal_20M.NO2_Emis / counter, 3);
            ReportDay.avgTotal_20M.NOx_Emis = Math.Round(ReportDay.sumTotal_20M.NOx_Emis / counter, 3);
            ReportDay.avgTotal_20M.SO2_Emis = Math.Round(ReportDay.sumTotal_20M.SO2_Emis / counter, 3);
            ReportDay.avgTotal_20M.Dust_Emis = Math.Round(ReportDay.sumTotal_20M.Dust_Emis / counter, 3);
            ReportDay.avgTotal_20M.CH4_Emis = Math.Round(ReportDay.sumTotal_20M.CH4_Emis / counter, 3);
            ReportDay.avgTotal_20M.H2S_Emis = Math.Round(ReportDay.sumTotal_20M.H2S_Emis / counter, 3);
            ReportDay.avgTotal_20M.NH3_Emis = Math.Round(ReportDay.sumTotal_20M.NH3_Emis / counter, 3);
            ReportDay.avgTotal_20M.Add_Emis_1 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_1 / counter, 3);
            ReportDay.avgTotal_20M.Add_Emis_2 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_2 / counter, 3);
            ReportDay.avgTotal_20M.Add_Emis_3 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_3 / counter, 3);
            ReportDay.avgTotal_20M.Add_Emis_4 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_4 / counter, 3);
            ReportDay.avgTotal_20M.Add_Emis_5 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_5 / counter, 3);
            
            //Процент превышений Выбросов
            if (ReportDay.PDZ.CO_Emis != 0.0 && ReportDay.avgTotal_20M.CO_Emis > ReportDay.PDZ.CO_Emis) ReportDay.avgTotal_20M.CO_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.CO_Emis / (ReportDay.PDZ.CO_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.CO2_Emis != 0.0 && ReportDay.avgTotal_20M.CO2_Emis > ReportDay.PDZ.CO2_Emis) ReportDay.avgTotal_20M.CO2_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.CO2_Emis / (ReportDay.PDZ.CO2_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NO_Emis != 0.0 && ReportDay.avgTotal_20M.NO_Emis > ReportDay.PDZ.NO_Emis) ReportDay.avgTotal_20M.NO_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.NO_Emis / (ReportDay.PDZ.NO_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NO2_Emis != 0.0 && ReportDay.avgTotal_20M.NO2_Emis > ReportDay.PDZ.NO2_Emis) ReportDay.avgTotal_20M.NO2_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.NO2_Emis / (ReportDay.PDZ.NO2_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NOx_Emis != 0.0 && ReportDay.avgTotal_20M.NOx_Emis > ReportDay.PDZ.NOx_Emis) ReportDay.avgTotal_20M.NOx_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.NOx_Emis / (ReportDay.PDZ.NOx_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.SO2_Emis != 0.0 && ReportDay.avgTotal_20M.SO2_Emis > ReportDay.PDZ.SO2_Emis) ReportDay.avgTotal_20M.SO2_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.SO2_Emis / (ReportDay.PDZ.SO2_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Dust_Emis != 0.0 && ReportDay.avgTotal_20M.Dust_Emis > ReportDay.PDZ.Dust_Emis) ReportDay.avgTotal_20M.Dust_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.Dust_Emis / (ReportDay.PDZ.Dust_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.CH4_Emis != 0.0 && ReportDay.avgTotal_20M.CH4_Emis > ReportDay.PDZ.CH4_Emis) ReportDay.avgTotal_20M.CH4_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.CH4_Emis / (ReportDay.PDZ.CH4_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.H2S_Emis != 0.0 && ReportDay.avgTotal_20M.H2S_Emis > ReportDay.PDZ.H2S_Emis) ReportDay.avgTotal_20M.H2S_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.H2S_Emis / (ReportDay.PDZ.H2S_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.NH3_Emis != 0.0 && ReportDay.avgTotal_20M.NH3_Emis > ReportDay.PDZ.NH3_Emis) ReportDay.avgTotal_20M.NH3_Emis_Percent = Math.Round((ReportDay.avgTotal_20M.NH3_Emis / (ReportDay.PDZ.NH3_Emis / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Emis_1 != 0.0 && ReportDay.avgTotal_20M.Add_Emis_1 > ReportDay.PDZ.Add_Emis_1) ReportDay.avgTotal_20M.Add_Emis_1_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Emis_1 / (ReportDay.PDZ.Add_Emis_1 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Emis_2 != 0.0 && ReportDay.avgTotal_20M.Add_Emis_2 > ReportDay.PDZ.Add_Emis_2) ReportDay.avgTotal_20M.Add_Emis_2_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Emis_2 / (ReportDay.PDZ.Add_Emis_2 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Emis_3 != 0.0 && ReportDay.avgTotal_20M.Add_Emis_3 > ReportDay.PDZ.Add_Emis_3) ReportDay.avgTotal_20M.Add_Emis_3_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Emis_3 / (ReportDay.PDZ.Add_Emis_3 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Emis_4 != 0.0 && ReportDay.avgTotal_20M.Add_Emis_4 > ReportDay.PDZ.Add_Emis_4) ReportDay.avgTotal_20M.Add_Emis_4_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Emis_4 / (ReportDay.PDZ.Add_Emis_4 / 100.0)) - 100, 1);
            if (ReportDay.PDZ.Add_Emis_5 != 0.0 && ReportDay.avgTotal_20M.Add_Emis_5 > ReportDay.PDZ.Add_Emis_5) ReportDay.avgTotal_20M.Add_Emis_5_Percent = Math.Round((ReportDay.avgTotal_20M.Add_Emis_5 / (ReportDay.PDZ.Add_Emis_5 / 100.0)) - 100, 1);

            //Параметры
            ReportDay.avgTotal_20M.Pressure = Math.Round(ReportDay.sumTotal_20M.Pressure / counter, 3);
            ReportDay.avgTotal_20M.Temperature = Math.Round(ReportDay.sumTotal_20M.Temperature / counter, 3);
            ReportDay.avgTotal_20M.Temperature_KIP = Math.Round(ReportDay.sumTotal_20M.Temperature_KIP / counter, 3);
            ReportDay.avgTotal_20M.Temperature_NOx = Math.Round(ReportDay.sumTotal_20M.Temperature_NOx / counter, 3);
            ReportDay.avgTotal_20M.Speed = Math.Round(ReportDay.sumTotal_20M.Speed / counter, 3);
            ReportDay.avgTotal_20M.Flow = Math.Round(ReportDay.sumTotal_20M.Flow / counter, 3);
            ReportDay.avgTotal_20M.O2_Dry = Math.Round(ReportDay.sumTotal_20M.O2_Dry / counter, 3);
            ReportDay.avgTotal_20M.O2_Wet = Math.Round(ReportDay.sumTotal_20M.O2_Wet / counter, 3);
            ReportDay.avgTotal_20M.H2O = Math.Round(ReportDay.sumTotal_20M.H2O / counter, 3);

            ReportDay.avgTotal_20M.Pressure_KIP = Math.Round(ReportDay.sumTotal_20M.Pressure_KIP / counter, 3);
            ReportDay.avgTotal_20M.Temperature_Point_Dew = Math.Round(ReportDay.sumTotal_20M.Temperature_Point_Dew / counter, 3);
            ReportDay.avgTotal_20M.Temperature_Room = Math.Round(ReportDay.sumTotal_20M.Temperature_Room / counter, 3);
            ReportDay.avgTotal_20M.Temperature_PGS = Math.Round(ReportDay.sumTotal_20M.Temperature_PGS / counter, 3);
            ReportDay.avgTotal_20M.O2_Room = Math.Round(ReportDay.sumTotal_20M.O2_Room / counter, 3);
            ReportDay.avgTotal_20M.O2_PGS = Math.Round(ReportDay.sumTotal_20M.O2_PGS / counter, 3);

            //Считаем время простоя
            ReportDay.sumTotal_20M.Mode_ASK = Math.Round(ReportDay.sumTotal_20M.Mode_ASK / 3, 3);

            //Сумма за сутки
            ReportDay.sumTotal_20M.CO_Emis = Math.Round(ReportDay.sumTotal_20M.CO_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.CO2_Emis = Math.Round(ReportDay.sumTotal_20M.CO2_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.NO_Emis = Math.Round(ReportDay.sumTotal_20M.NO_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.NOx_Emis = Math.Round(ReportDay.sumTotal_20M.NOx_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.NO2_Emis = Math.Round(ReportDay.sumTotal_20M.NO2_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.SO2_Emis = Math.Round(ReportDay.sumTotal_20M.SO2_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.Dust_Emis = Math.Round(ReportDay.sumTotal_20M.Dust_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.CH4_Emis = Math.Round(ReportDay.sumTotal_20M.CH4_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.H2S_Emis = Math.Round(ReportDay.sumTotal_20M.CH4_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.NH3_Emis = Math.Round(ReportDay.sumTotal_20M.NH3_Emis * 1.2, 3);
            ReportDay.sumTotal_20M.Add_Emis_1 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_1 * 1.2, 3);
            ReportDay.sumTotal_20M.Add_Emis_2 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_2 * 1.2, 3);
            ReportDay.sumTotal_20M.Add_Emis_3 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_3 * 1.2, 3);
            ReportDay.sumTotal_20M.Add_Emis_4 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_4 * 1.2, 3);
            ReportDay.sumTotal_20M.Add_Emis_5 = Math.Round(ReportDay.sumTotal_20M.Add_Emis_5 * 1.2, 3);

            ReportDay.sumTotal_20M.CO_Conc = 0.0;
            ReportDay.sumTotal_20M.CO2_Conc = 0.0;
            ReportDay.sumTotal_20M.NO_Conc = 0.0;
            ReportDay.sumTotal_20M.NOx_Conc = 0.0;
            ReportDay.sumTotal_20M.NO2_Conc = 0.0;
            ReportDay.sumTotal_20M.SO2_Conc = 0.0;
            ReportDay.sumTotal_20M.Dust_Conc = 0.0;
            ReportDay.sumTotal_20M.CH4_Conc = 0.0;
            ReportDay.sumTotal_20M.H2S_Conc = 0.0;
            ReportDay.sumTotal_20M.NH3_Conc = 0.0;
            ReportDay.sumTotal_20M.Add_Conc_1 = 0.0;
            ReportDay.sumTotal_20M.Add_Conc_2 = 0.0;
            ReportDay.sumTotal_20M.Add_Conc_3 = 0.0;
            ReportDay.sumTotal_20M.Add_Conc_4 = 0.0;
            ReportDay.sumTotal_20M.Add_Conc_5 = 0.0;

            //Считаем длительность превышений
            ReportDay.durationTotal_20M.CO_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.CO_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.CO2_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.CO2_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.NO_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.NO_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.NO2_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.NO2_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.NOx_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.NOx_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.SO2_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.SO2_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.Dust_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.Dust_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.CH4_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.CH4_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.H2S_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.H2S_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.NH3_Conc_Percent = Math.Round(ReportDay.durationTotal_20M.NH3_Conc_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Conc_1_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Conc_1_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Conc_2_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Conc_2_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Conc_3_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Conc_3_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Conc_4_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Conc_4_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Conc_5_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Conc_5_Percent / 3, 1);

            ReportDay.durationTotal_20M.CO2_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.CO2_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.NO_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.NO_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.NO2_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.NO2_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.NOx_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.NOx_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.SO2_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.SO2_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.Dust_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.Dust_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.CH4_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.CH4_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.H2S_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.H2S_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.NH3_Emis_Percent = Math.Round(ReportDay.durationTotal_20M.NH3_Emis_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Emis_1_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Emis_1_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Emis_2_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Emis_2_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Emis_3_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Emis_3_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Emis_4_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Emis_4_Percent / 3, 1);
            ReportDay.durationTotal_20M.Add_Emis_5_Percent = Math.Round(ReportDay.durationTotal_20M.Add_Emis_5_Percent / 3, 1);

            return ReportDay;
        }
    }
}
