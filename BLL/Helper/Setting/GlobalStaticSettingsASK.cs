using ASK.BLL.Helper.Chart;
using ASK.BLL.Models;
using ASK.BLL.Services;
using ASK.DAL;
using ASK.DAL.Models;
using ASK.DAL.Repository;
using NModbus;
using NModbus.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASK.BLL.Helper.Setting
{
    public class GlobalStaticSettingsASK
    {
        public static Emis_IsStop_JSON_Model emis_IsStop = new Emis_IsStop_JSON_Model();

        public static bool Is_WritePLC = false;              //Пишем ли в ПЛК что-то?


        public static bool Open_Valve_Ventilation = false;     //Временно! Открыть клапан вентиляции                   Q1
        public static bool Pusk_Fan_Ventilation = false;       //Временно! Включить вентелятор вентиляции                  Q2
        public static bool Open_Heater_Ventilation = false;    //Временно! Включить клапан подогреватель вентиляции    Q3
        public static bool Close_Valve_Sample = false;         //Временно! Закрыть клапан отбора пробы пробы           Q4
        public static bool Purge_Probe = false;                //Временно! Продувка зонда                              Q5
        public static bool Calibration_Zero_Analyzer = false;  //Временно! Калибровка нуля анализатора                 Q6
        public static bool Siren_Gassed = false;               //Временно! Сирена загазованность                       Q7
        public static bool RUN_VENTILATION = false;            //Временно! Полноценный запуск вентиляции               Q32 (не дискрет)



        static int delayedStart = 0;                //Отложенный старт для записис аварий в БД

        static double approximation = 0.8;          //Завести отделбную переменную"! ?//Порог приближения

        public static bool is_simulation = false;   //Режим симуляции 4-20 сигналов
        private static double O2_Last = 1;          //Режим симуляции 4-20 сигналов старый O2
        private static Random rnd = new Random();   //Режим симуляции 4-20 рандом

        //Chart
        public static Chart_CurrentValue ChartCurrent { get; set; } = new Chart_CurrentValue();
        public static List<Chart_CurrentValue> ChartList { get; set; } = new List<Chart_CurrentValue>();
        public static int CounterChart { get; set; } = 0;

        public static SettingOptions_JSON_Model SettingOptions { get; set; }
        public static VisibilityReportOptions_JSON_Model VisibilityReportOptions { get; set; }
        public static CalculationSetting_JSON_Model CalculationSetting { get; set; }                            //Хранит параметры/необходимые условия для расчёта концентрацй/выбросов
        public static PDZ_JSON_Model PDZ { get; set; }                                                          //Все 3 вида топлива ПДЗ 
        public static PDZ_Active_Model PDZ_Current { get; set; } = new PDZ_Active_Model();                      //Текущие ПДЗ в double
        public static PDZ_String_Active_Model PDZ_Current_String { get; set; } = new PDZ_String_Active_Model(); //Текущие ПДЗ в string (-/-)
        public static SensorRange_JSON_Model SensorRange { get; set; }
        public static List<Sensor_4_20_Model> Sensor_4_20s = new List<Sensor_4_20_Model>();                     //Будем хранить значение 4-20мА 
        public static Sensor_4_20_Model SensorNow { get; set; } = new Sensor_4_20_Model();                      //Будем хранить текущее значения датчиков (4-20)
        public static Sensor_4_20_Model SensorScaledNow { get; set; } = new Sensor_4_20_Model();                //Будет хранить текущие значения прямых показаний датчиков 
        public static List<Array20M_Model> Array20Ms { get; set; } = new List<Array20M_Model>();                //Текущие значения формируемой 20 минутки
        public static Array20M_Model CurrentConcEmis { get; set; } = new Array20M_Model();                      //Текущие значение концентрацйи и выбросов после приведения

        //Цвета 
        public static Color ColorExcess { get; set; } = Color.FromArgb(247, 213, 213);                          //Цвет превышения
        //public static Color ColorExcess { get; set; } = Color.FromArgb(0, 14, 14, 3);                         //Цвет превышения
        public static Color ColorHeader1 { get; set; } = Color.FromArgb(255, 230, 168);                         //Цвет заголовка 1
        public static Color ColorHeader2 { get; set; } = Color.FromArgb(182, 242, 250);                         //Цвет заголовка 2

        public static bool stopGetSernsorNow = false; //Если идёт запись в БД 20М тормозит поток обновления данных, пока запись не запишется; 

        //всё для пинга ПЛК 
        static Ping ping = new Ping();
        static PingReply connected;

        //Нет связи с плк
        //public static bool isNotConnection = false;

        //Аварии
        public static GlobalAlarm_Model globalAlarms = new GlobalAlarm_Model();                                 //Глобальные аварии
        public static AllAlarm_Model allAlarm = new AllAlarm_Model();                                           //Все остальные варии будут хранится тут          
        public static List<Alarm_Model> alarmList = new List<Alarm_Model>();                                    //Список провереямых аварий (именно дискретных) для журнала аварий
        public static List<Alarm_Model> alarmList_Delegate = new List<Alarm_Model>();                            //Список проаеряемых аварий (именно высчитываемых) для журнала аварий

        public static void SaveSettingOptionsJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\SettingOptionsJSON.json");
            file.WriteLine(JsonSerializer.Serialize(SettingOptions, typeof(SettingOptions_JSON_Model)));
            file.Close();
        }



        public static void SaveVisibilityReportOptionsJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\VisibilityReportOptionsJSON.json");
            file.WriteLine(JsonSerializer.Serialize(VisibilityReportOptions, typeof(VisibilityReportOptions_JSON_Model)));
            file.Close();
        }



        public static void SaveCalculationSettingJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\CalculationSettingJSON.json");
            file.WriteLine(JsonSerializer.Serialize(CalculationSetting, typeof(CalculationSetting_JSON_Model)));
            file.Close();
        }



        public static void SavePdz_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\PdzJSON.json");
            file.WriteLine(JsonSerializer.Serialize(PDZ, typeof(PDZ_JSON_Model)));
            file.Close();
            GetCurrentPDZ();
        }

        public static void SaveEmis_IsStop_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\Emis_IsStopJSON.JSON");
            file.WriteLine(JsonSerializer.Serialize(emis_IsStop, typeof(Emis_IsStop_JSON_Model)));
            file.Close();
        }

        public static void SaveSensorRange_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\SensorRangeJSON.JSON");
            file.WriteLine(JsonSerializer.Serialize(SensorRange, typeof(SensorRange_JSON_Model)));
            file.Close();

            UpdateVisibilityAlarm();
        }


        



        static GlobalStaticSettingsASK()
        {
            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SettingOptionsJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\SettingOptionsJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\SettingOptionsJSON.json");
                SettingOptions = JsonSerializer.Deserialize<SettingOptions_JSON_Model>(data);
            }
            else //не существует
            {
                SettingOptions = new SettingOptions_JSON_Model();

                SaveSettingOptionsJSON();
            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  VisibilityReportOptionsJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\VisibilityReportOptionsJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\VisibilityReportOptionsJSON.json");
                VisibilityReportOptions = JsonSerializer.Deserialize<VisibilityReportOptions_JSON_Model>(data);
            }
            else //не существует
            {
                VisibilityReportOptions = new VisibilityReportOptions_JSON_Model();

                SaveVisibilityReportOptionsJSON();
            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  CalculationSettingJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\CalculationSettingJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\CalculationSettingJSON.json");
                CalculationSetting = JsonSerializer.Deserialize<CalculationSetting_JSON_Model>(data);
            }
            else //не существует
            {
                CalculationSetting = new CalculationSetting_JSON_Model();

                SaveCalculationSettingJSON();
            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  PdzJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\PdzJSON.json"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\PdzJSON.json");
                PDZ = JsonSerializer.Deserialize<PDZ_JSON_Model>(data);
            }
            else
            {
                PDZ = new PDZ_JSON_Model();
                SavePdz_JSON();

            }


            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                 Emis_IsStop
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\Emis_IsStopJSON.json"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\Emis_IsStopJSON.json");
                emis_IsStop = JsonSerializer.Deserialize<Emis_IsStop_JSON_Model>(data);
            }
            else
            {
                emis_IsStop = new Emis_IsStop_JSON_Model();
                SaveEmis_IsStop_JSON();

            }


            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SensorRangeJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\SensorRangeJSON.JSON"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\SensorRangeJSON.JSON");
                SensorRange = JsonSerializer.Deserialize<SensorRange_JSON_Model>(data);
            }
            else
            {
                SensorRange = new SensorRange_JSON_Model();
                SaveSensorRange_JSON();
            }

            UpdateVisibilityAlarm();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var accident_log_Service = new ACCIDENT_LOG_Repository(db);
                accident_log_Service.StratSystem_ACCIDENT_LOG();
            }

            GetCurrentPDZ();
            ChartList.Add(new Chart_CurrentValue());    //Первая нулевая точка //Потом удалить!

            ChekActiveAlarms();                         //Заполняем делегаты в авариях котороые высчитываются
        }



        //Обновляем используемые аварии
        public static void UpdateVisibilityAlarm()
        {
            if (SensorRange.CO.Is_Used)
                allAlarm.SensorAlarm.CO.Is_Used = true;
            else
                allAlarm.SensorAlarm.CO.Is_Used = false;


            if (SensorRange.CO2.Is_Used)
                allAlarm.SensorAlarm.CO2.Is_Used = true;
            else
                allAlarm.SensorAlarm.CO2.Is_Used = false;


            if (SensorRange.NO.Is_Used)
                allAlarm.SensorAlarm.NO.Is_Used = true;
            else
                allAlarm.SensorAlarm.NO.Is_Used = false;


            if (SensorRange.NO2.Is_Used)
                allAlarm.SensorAlarm.NO2.Is_Used = true;
            else
                allAlarm.SensorAlarm.NO2.Is_Used = false;


            if (SensorRange.NOx.Is_Used)
                allAlarm.SensorAlarm.NOx.Is_Used = true;
            else
                allAlarm.SensorAlarm.NOx.Is_Used = false;


            if (SensorRange.SO2.Is_Used)
                allAlarm.SensorAlarm.SO2.Is_Used = true;
            else
                allAlarm.SensorAlarm.SO2.Is_Used = false;


            if (SensorRange.Dust.Is_Used)
                allAlarm.SensorAlarm.Dust.Is_Used = true;
            else
                allAlarm.SensorAlarm.Dust.Is_Used = false;


            if (SensorRange.CH4.Is_Used)
                allAlarm.SensorAlarm.CH4.Is_Used = true;
            else
                allAlarm.SensorAlarm.CH4.Is_Used = false;


            if (SensorRange.H2S.Is_Used)
                allAlarm.SensorAlarm.H2S.Is_Used = true;
            else
                allAlarm.SensorAlarm.H2S.Is_Used = false;


            if (SensorRange.NH3.Is_Used)
                allAlarm.SensorAlarm.NH3.Is_Used = true;
            else
                allAlarm.SensorAlarm.NH3.Is_Used = false;
        }



        public static async Task Add_20M_Async()
        {
            stopGetSernsorNow = true;                   //стопим запись данных в Array20Ms

            await Task.Delay(TimeSpan.FromSeconds(1));  //Выжидаем, что бы поток который прошёл успел остановится
            await Task.Run(() => Add_20M());

            stopGetSernsorNow = false;                  //Рарешаем работу записи
        }
        public static void Add_20M()
        {
            if (Array20Ms.Count > 0)
            {
                Random random = new Random();
                AVG_20_MINUTES new20M = new AVG_20_MINUTES();

                try
                {
                    new20M.Conc_CO = Math.Round(Array20Ms.Average(a => a.CO_Conc), 3);
                    new20M.Conc_CO2 = Math.Round(Array20Ms.Average(a => a.CO2_Conc), 3);
                    new20M.Conc_NO = Math.Round(Array20Ms.Average(a => a.NO_Conc), 3);
                    new20M.Conc_NO2 = Math.Round(Array20Ms.Average(a => a.NO2_Conc), 3);
                    new20M.Conc_NOx = Math.Round(Array20Ms.Average(a => a.NOx_Conc), 3);
                    new20M.Conc_SO2 = Math.Round(Array20Ms.Average(a => a.SO2_Conc), 3);
                    new20M.Conc_Dust = Math.Round(Array20Ms.Average(a => a.Dust_Conc), 3);
                    new20M.Conc_CH4 = Math.Round(Array20Ms.Average(a => a.CH4_Conc), 3);
                    new20M.Conc_H2S = Math.Round(Array20Ms.Average(a => a.H2S_Conc), 3);
                    new20M.Conc_NH3 = Math.Round(Array20Ms.Average(a => a.NH3_Conc), 3);
                    new20M.Conc_D1 = Math.Round(Array20Ms.Average(a => a.Add_Conc_1), 3);
                    new20M.Conc_D2 = Math.Round(Array20Ms.Average(a => a.Add_Conc_2), 3);
                    new20M.Conc_D3 = Math.Round(Array20Ms.Average(a => a.Add_Conc_3), 3);
                    new20M.Conc_D4 = Math.Round(Array20Ms.Average(a => a.Add_Conc_4), 3);
                    new20M.Conc_D5 = Math.Round(Array20Ms.Average(a => a.Add_Conc_5), 3);

                    new20M.Emis_CO = Math.Round(Array20Ms.Average(a => a.CO_Emis), 3);
                    new20M.Emis_CO2 = Math.Round(Array20Ms.Average(a => a.CO2_Emis), 3);
                    new20M.Emis_NO = Math.Round(Array20Ms.Average(a => a.NO_Emis), 3);
                    new20M.Emis_NO2 = Math.Round(Array20Ms.Average(a => a.NO2_Emis), 3);
                    new20M.Emis_NOx = Math.Round(Array20Ms.Average(a => a.NOx_Emis), 3);
                    new20M.Emis_SO2 = Math.Round(Array20Ms.Average(a => a.SO2_Emis), 3);
                    new20M.Emis_CH4 = Math.Round(Array20Ms.Average(a => a.CH4_Emis), 3);
                    new20M.Emis_H2S = Math.Round(Array20Ms.Average(a => a.H2S_Emis), 3);
                    new20M.Emis_NH3 = Math.Round(Array20Ms.Average(a => a.NH3_Emis), 3);
                    new20M.Emis_Dust = Math.Round(Array20Ms.Average(a => a.Dust_Emis), 3);
                    new20M.Emis_D1 = Math.Round(Array20Ms.Average(a => a.Add_Emis_1), 3);
                    new20M.Emis_D2 = Math.Round(Array20Ms.Average(a => a.Add_Emis_2), 3);
                    new20M.Emis_D3 = Math.Round(Array20Ms.Average(a => a.Add_Emis_3), 3);
                    new20M.Emis_D4 = Math.Round(Array20Ms.Average(a => a.Add_Emis_4), 3);
                    new20M.Emis_D5 = Math.Round(Array20Ms.Average(a => a.Add_Emis_5), 3);

                    new20M.O2_Wet = Math.Round(Array20Ms.Average(a => a.O2_Wet), 3);
                    new20M.O2_Dry = Math.Round(Array20Ms.Average(a => a.O2_Dry), 3);
                    new20M.H2O = Math.Round(Array20Ms.Average(a => a.H2O), 3);

                    new20M.Pressure = Math.Round(Array20Ms.Average(a => a.Pressure), 3);
                    new20M.Temperature = Math.Round(Array20Ms.Average(a => a.Temperature), 3);
                    new20M.Speed = Math.Round(Array20Ms.Average(a => a.Speed), 3);
                    new20M.Flow = Math.Round(Array20Ms.Average(a => a.Flow), 3);

                    new20M.Temperature_KIP = Math.Round(Array20Ms.Average(a => a.Temperature_KIP), 3);
                    new20M.Temperature_NOx = Math.Round(Array20Ms.Average(a => a.Temperature_NOx), 3);
                    new20M.Pressure_KIP = Math.Round(Array20Ms.Average(a => a.Pressure_KIP), 3);
                    new20M.Temperature_Point_Dew = Math.Round(Array20Ms.Average(a => a.Temperature_Point_Dew), 3);
                    new20M.Temperature_Room = Math.Round(Array20Ms.Average(a => a.Temperature_Room), 3);
                    new20M.Temperature_PGS = Math.Round(Array20Ms.Average(a => a.Temperature_PGS), 3);
                    new20M.O2_Room = Math.Round(Array20Ms.Average(a => a.O2_Room), 3);
                    new20M.O2_PGS = Math.Round(Array20Ms.Average(a => a.O2_PGS), 3);


                    if (globalAlarms.Is_Stop.Value) new20M.Mode_ASK = 1;
                    else
                    {
                        if (globalAlarms.Is_NotProcess.Value) 
                            new20M.Mode_ASK = 2;
                        else
                            new20M.Mode_ASK = 0;
                    }
                    new20M.PDZ_Fuel = PDZ.Is_Active;

                }
                catch
                {

                }
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        AVG_20_MINUTES_Repository avg_20_M_Service = new AVG_20_MINUTES_Repository(db);
                        avg_20_M_Service.Create_AVG_20_MINUTES(new AVG_20_MINUTES()
                        {
                            Date = DateTime.Now,

                            //Временно 
                            Conc_CO = new20M.Conc_CO,
                            Conc_CO2 = new20M.Conc_CO2,
                            Conc_NO = new20M.Conc_NO,
                            Conc_NO2 = new20M.Conc_NO2,
                            Conc_NOx = new20M.Conc_NOx,
                            Conc_SO2 = new20M.Conc_SO2,
                            Conc_Dust = new20M.Conc_Dust,
                            Conc_CH4 = new20M.Conc_CH4,
                            Conc_H2S = new20M.Conc_H2S,
                            Conc_NH3 = new20M.Conc_NH3,
                            Conc_D1 = new20M.Conc_D1,
                            Conc_D2 = new20M.Conc_D2,
                            Conc_D3 = new20M.Conc_D3,
                            Conc_D4 = new20M.Conc_D4,
                            Conc_D5 = new20M.Conc_D5,

                            Emis_CO = new20M.Emis_CO,
                            Emis_CO2 = new20M.Emis_CO2,
                            Emis_NO = new20M.Emis_NO,
                            Emis_NO2 = new20M.Emis_NO2,
                            Emis_NOx = new20M.Emis_NOx,
                            Emis_SO2 = new20M.Emis_SO2,
                            Emis_CH4 = new20M.Emis_CH4,
                            Emis_H2S = new20M.Emis_H2S,
                            Emis_NH3 = new20M.Emis_NH3,
                            Emis_Dust = new20M.Emis_Dust,
                            Emis_D1 = new20M.Emis_D1,
                            Emis_D2 = new20M.Emis_D2,
                            Emis_D3 = new20M.Emis_D3,
                            Emis_D4 = new20M.Emis_D4,
                            Emis_D5 = new20M.Emis_D5,

                            O2_Wet = new20M.O2_Wet,
                            O2_Dry = new20M.O2_Dry,
                            H2O = new20M.H2O,

                            Pressure = new20M.Pressure,
                            Temperature = new20M.Temperature,
                            Speed = new20M.Speed,
                            Flow = new20M.Flow,
                            
                            Temperature_KIP = new20M.Temperature_KIP,
                            Temperature_NOx = new20M.Temperature_NOx,
                            Pressure_KIP = new20M.Pressure_KIP,
                            Temperature_Point_Dew = new20M.Temperature_Point_Dew,
                            Temperature_Room = new20M.Temperature_Room,
                            Temperature_PGS = new20M.Temperature_PGS,
                            O2_Room = new20M.O2_Room,
                            O2_PGS = new20M.O2_PGS,


                            Mode_ASK = new20M.Mode_ASK,
                            PDZ_Fuel = new20M.PDZ_Fuel
                        });
                    }
                    catch
                    {

                    }
                    //GlobalStaticSettingsASK.VisibilityReportOptions.data_add_20M = GlobalStaticSettingsASK.VisibilityReportOptions.data_add_20M.AddMinutes(20);
                }
            }
            Array20Ms.Clear();
        }



        public static async Task DeleteOldSensor_4_20m_Async()
        {
            await Task.Run(() => DeleteOldSensor_4_20m());
        }



        public static void DeleteOldSensor_4_20m()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    SENSOR_4_20_10sec_Repository Sensors_4_20db = new SENSOR_4_20_10sec_Repository(db);
                    Sensors_4_20db.DeleteOld(DateTime.Now.AddDays(-1));
                }
                catch
                {

                }
            }
        }



        public static async Task GetNow_ConcEmisAsync()
        {
            await Task.Run(() => GetNow_ConcEmis());
        }



        public static void GetNow_ConcEmis()
        {
            GetSensorNow();
            RunConvertSernsor();
            Normalization_ConcEmis();

            if (delayedStart > 0)
                //ChekSensorBreak();
                UpdateAlarmList();
            else
                delayedStart++;

            Sensor_4_20s.Add((Sensor_4_20_Model)SensorNow.Clone());

            if (Sensor_4_20s.Count > 999)
                Sensor_4_20s.RemoveAt(0);

            if (!stopGetSernsorNow)
                Array20Ms.Add((Array20M_Model)CurrentConcEmis.Clone());

            if (CounterChart > 3) //Важная настройка, зависит от скорости проса ПЛК (1 сек - 8, 2 сек - 3 = частата записи 10 сек тогда и обновл. знач. графика)
            {
                CounterChart = 0;

                //ChartCurrent.Getsimulation();
                if (ChartList.Count > 699)
                    ChartList.RemoveAt(0);

                //График
                ChartCurrent.DateString = DateTime.Now.ToLongTimeString();

                ChartCurrent.CO = CurrentConcEmis.CO_Conc;
                ChartCurrent.CO2 = CurrentConcEmis.CO2_Conc;
                ChartCurrent.NO = CurrentConcEmis.NO_Conc;
                ChartCurrent.NO2 = CurrentConcEmis.NO2_Conc;
                ChartCurrent.NOx = CurrentConcEmis.NOx_Conc;
                ChartCurrent.SO2 = CurrentConcEmis.SO2_Conc;
                ChartCurrent.Dust = CurrentConcEmis.Dust_Conc;
                ChartCurrent.CH4 = CurrentConcEmis.CH4_Conc;
                ChartCurrent.H2S = CurrentConcEmis.H2S_Conc;
                ChartCurrent.NH3 = CurrentConcEmis.NH3_Conc;
                ChartCurrent.Add_Conc_1 = CurrentConcEmis.Add_Conc_1;
                ChartCurrent.Add_Conc_2 = CurrentConcEmis.Add_Conc_2;
                ChartCurrent.Add_Conc_3 = CurrentConcEmis.Add_Conc_3;
                ChartCurrent.Add_Conc_4 = CurrentConcEmis.Add_Conc_4;
                ChartCurrent.Add_Conc_5 = CurrentConcEmis.Add_Conc_5;

                ChartCurrent.O2_Wet = CurrentConcEmis.O2_Wet;
                ChartCurrent.O2_Dry = CurrentConcEmis.O2_Dry;

                ChartList.Add((Chart_CurrentValue)ChartCurrent.Clone());

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        SENSOR_4_20_10sec_Repository Sensors_4_20db = new SENSOR_4_20_10sec_Repository(db);
                        Sensors_4_20db.Create_SENSOR_4_20_10sec(new SENSOR_4_20_10sec
                        {
                            Date = SensorNow.Date,

                            CO = SensorNow.CO_4_20mA,
                            CO2 = SensorNow.CO2_4_20mA,
                            NO = SensorNow.NO_4_20mA,
                            NO2 = SensorNow.NO2_4_20mA,
                            NOx = SensorNow.NOx_4_20mA,
                            SO2 = SensorNow.SO2_4_20mA,
                            Dust = SensorNow.Dust_4_20mA,
                            CH4 = SensorNow.CH4_4_20mA,
                            H2S = SensorNow.H2S_4_20mA,
                            NH3 = SensorNow.NH3_4_20mA,

                            Rezerv_1 = SensorNow.Rezerv_1_4_20mA,
                            Rezerv_2 = SensorNow.Rezerv_2_4_20mA,
                            Rezerv_3 = SensorNow.Rezerv_3_4_20mA,
                            Rezerv_4 = SensorNow.Rezerv_4_4_20mA,
                            Rezerv_5 = SensorNow.Rezerv_5_4_20mA,

                            O2_Wet = SensorNow.O2_Wet_4_20mA,
                            O2_Dry = SensorNow.O2_Dry_4_20mA,
                            H2O = SensorNow.H2S_4_20mA,

                            Pressure = SensorNow.Pressure_4_20mA,
                            Temperature = SensorNow.Temperature_4_20mA,
                            Speed = SensorNow.Speed_4_20mA,

                            Temperature_KIP = SensorNow.Temperature_KIP_4_20mA,
                            Temperature_NOx = SensorNow.Temperature_NOx_4_20mA,
                            Pressure_KIP = SensorNow.Pressure_KIP_4_20mA,
                            Temperature_Point_Dew = SensorNow.Temperature_Point_Dew_4_20mA,
                            Temperature_Room = SensorNow.Temperature_Room_4_20mA,
                            Temperature_PGS = SensorNow.Temperature_PGS_4_20mA,
                            O2_Room = SensorNow.O2_Room_4_20mA,
                            O2_PGS = SensorNow.O2_PGS_4_20mA
                        });
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                CounterChart++;
            }
        }



        public static async Task Add_PDZ_Async()
        {
            await Task.Run(() => Add_PDZ());
        }



        public static void Add_PDZ()
        {
            GetCurrentPDZ();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<PDZ> PDZs = new List<PDZ>();
                PDZ_Repository pdz_Service = new PDZ_Repository(db);

                if (!pdz_Service.FindPDZDay())
                {
                    bool buff_IsActive;

                    if (PDZ.Is_Active == 1)
                        buff_IsActive = true;
                    else
                        buff_IsActive = false;

                    pdz_Service.Create_PDZ(new PDZ()
                    {
                        Date = DateTime.Now,

                        CO_Conc = PDZ.PDZ_1_CO_Conc,
                        CO2_Conc = PDZ.PDZ_1_CO2_Conc,
                        NO_Conc = PDZ.PDZ_1_NO_Conc,
                        NO2_Conc = PDZ.PDZ_1_NO2_Conc,
                        NOx_Conc = PDZ.PDZ_1_NOx_Conc,
                        SO2_Conc = PDZ.PDZ_1_SO2_Conc,
                        Dust_Conc = PDZ.PDZ_1_Dust_Conc,
                        CH4_Conc = PDZ.PDZ_1_CH4_Conc,
                        H2S_Conc = PDZ.PDZ_1_H2S_Conc,
                        NH3_Conc = PDZ.PDZ_1_NH3_Conc,
                        Add_Conc_1 = PDZ.PDZ_1_Add_Conc_1,
                        Add_Conc_2 = PDZ.PDZ_1_Add_Conc_2,
                        Add_Conc_3 = PDZ.PDZ_1_Add_Conc_3,
                        Add_Conc_4 = PDZ.PDZ_1_Add_Conc_4,
                        Add_Conc_5 = PDZ.PDZ_1_Add_Conc_5,

                        CO_Emis = PDZ.PDZ_1_CO_Emis,
                        CO2_Emis = PDZ.PDZ_1_CO2_Emis,
                        NO_Emis = PDZ.PDZ_1_NO_Emis,
                        NO2_Emis = PDZ.PDZ_1_NO2_Emis,
                        NOx_Emis = PDZ.PDZ_1_NOx_Emis,
                        SO2_Emis = PDZ.PDZ_1_SO2_Emis,
                        CH4_Emis = PDZ.PDZ_1_CH4_Emis,
                        H2S_Emis = PDZ.PDZ_1_H2S_Emis,
                        NH3_Emis = PDZ.PDZ_1_NH3_Emis,
                        Dust_Emis = PDZ.PDZ_1_Dust_Emis,
                        Add_Emis_1 = PDZ.PDZ_1_Add_Emis_1,
                        Add_Emis_2 = PDZ.PDZ_1_Add_Emis_2,
                        Add_Emis_3 = PDZ.PDZ_1_Add_Emis_3,
                        Add_Emis_4 = PDZ.PDZ_1_Add_Emis_4,
                        Add_Emis_5 = PDZ.PDZ_1_Add_Emis_5,

                        NumberPDZ = PDZ.PDZ_1_Number,

                        Current = buff_IsActive
                    });

                    if (PDZ.Is_Active == 2)
                        buff_IsActive = true;
                    else
                        buff_IsActive = false;

                    pdz_Service.Create_PDZ(new PDZ()
                    {
                        Date = DateTime.Now,

                        CO_Conc = PDZ.PDZ_2_CO_Conc,
                        CO2_Conc = PDZ.PDZ_2_CO2_Conc,
                        NO_Conc = PDZ.PDZ_2_NO_Conc,
                        NO2_Conc = PDZ.PDZ_2_NO2_Conc,
                        NOx_Conc = PDZ.PDZ_2_NOx_Conc,
                        SO2_Conc = PDZ.PDZ_2_SO2_Conc,
                        Dust_Conc = PDZ.PDZ_2_Dust_Conc,
                        CH4_Conc = PDZ.PDZ_2_CH4_Conc,
                        H2S_Conc = PDZ.PDZ_2_H2S_Conc,
                        NH3_Conc = PDZ.PDZ_2_NH3_Conc,
                        Add_Conc_1 = PDZ.PDZ_2_Add_Conc_1,
                        Add_Conc_2 = PDZ.PDZ_2_Add_Conc_2,
                        Add_Conc_3 = PDZ.PDZ_2_Add_Conc_3,
                        Add_Conc_4 = PDZ.PDZ_2_Add_Conc_4,
                        Add_Conc_5 = PDZ.PDZ_2_Add_Conc_5,

                        CO_Emis = PDZ.PDZ_2_CO_Emis,
                        CO2_Emis = PDZ.PDZ_2_CO2_Emis,
                        NO_Emis = PDZ.PDZ_2_NO_Emis,
                        NO2_Emis = PDZ.PDZ_2_NO2_Emis,
                        NOx_Emis = PDZ.PDZ_2_NOx_Emis,
                        SO2_Emis = PDZ.PDZ_2_SO2_Emis,
                        CH4_Emis = PDZ.PDZ_2_CH4_Emis,
                        H2S_Emis = PDZ.PDZ_2_H2S_Emis,
                        NH3_Emis = PDZ.PDZ_2_NH3_Emis,
                        Dust_Emis = PDZ.PDZ_2_Dust_Emis,
                        Add_Emis_1 = PDZ.PDZ_2_Add_Emis_1,
                        Add_Emis_2 = PDZ.PDZ_2_Add_Emis_2,
                        Add_Emis_3 = PDZ.PDZ_2_Add_Emis_3,
                        Add_Emis_4 = PDZ.PDZ_2_Add_Emis_4,
                        Add_Emis_5 = PDZ.PDZ_2_Add_Emis_5,

                        NumberPDZ = PDZ.PDZ_2_Number,

                        Current = buff_IsActive
                    });

                    if (PDZ.Is_Active == 3)
                        buff_IsActive = true;
                    else
                        buff_IsActive = false;

                    pdz_Service.Create_PDZ(new PDZ()
                    {
                        Date = DateTime.Now,

                        CO_Conc = PDZ.PDZ_3_CO_Conc,
                        CO2_Conc = PDZ.PDZ_3_CO2_Conc,
                        NO_Conc = PDZ.PDZ_3_NO_Conc,
                        NO2_Conc = PDZ.PDZ_3_NO2_Conc,
                        NOx_Conc = PDZ.PDZ_3_NOx_Conc,
                        SO2_Conc = PDZ.PDZ_3_SO2_Conc,
                        Dust_Conc = PDZ.PDZ_3_Dust_Conc,
                        CH4_Conc = PDZ.PDZ_3_CH4_Conc,
                        H2S_Conc = PDZ.PDZ_3_H2S_Conc,
                        NH3_Conc = PDZ.PDZ_3_NH3_Conc,
                        Add_Conc_1 = PDZ.PDZ_3_Add_Conc_1,
                        Add_Conc_2 = PDZ.PDZ_3_Add_Conc_2,
                        Add_Conc_3 = PDZ.PDZ_3_Add_Conc_3,
                        Add_Conc_4 = PDZ.PDZ_3_Add_Conc_4,
                        Add_Conc_5 = PDZ.PDZ_3_Add_Conc_5,

                        CO_Emis = PDZ.PDZ_3_CO_Emis,
                        CO2_Emis = PDZ.PDZ_3_CO2_Emis,
                        NO_Emis = PDZ.PDZ_3_NO_Emis,
                        NO2_Emis = PDZ.PDZ_3_NO2_Emis,
                        NOx_Emis = PDZ.PDZ_3_NOx_Emis,
                        SO2_Emis = PDZ.PDZ_3_SO2_Emis,
                        CH4_Emis = PDZ.PDZ_3_CH4_Emis,
                        H2S_Emis = PDZ.PDZ_3_H2S_Emis,
                        NH3_Emis = PDZ.PDZ_3_NH3_Emis,
                        Dust_Emis = PDZ.PDZ_3_Dust_Emis,
                        Add_Emis_1 = PDZ.PDZ_3_Add_Emis_1,
                        Add_Emis_2 = PDZ.PDZ_3_Add_Emis_2,
                        Add_Emis_3 = PDZ.PDZ_3_Add_Emis_3,
                        Add_Emis_4 = PDZ.PDZ_3_Add_Emis_4,
                        Add_Emis_5 = PDZ.PDZ_3_Add_Emis_5,

                        NumberPDZ = PDZ.PDZ_3_Number,

                        Current = buff_IsActive
                    });
                }
            }
        }



        public static void GetSensorNow()
        {
            string IpAdres = "192.168.10.3";

            

            try
            {
                connected = ping.Send(IpAdres, 1000);    //Проверяем соедение с таймингом 0.5 сек

                ushort[] registers;                     //Будующий масиив считываемых WORD из ПЛК
                ushort[] registers_DI;

                if (connected.Status == IPStatus.Success)
                {
                    //if (globalAlarms.Is_NotConnection.Value)
                    //{
                    //    using (ApplicationDbContext db = new ApplicationDbContext())
                    //    {
                    //        var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
                    //        globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_NotConnection);
                    //    }
                    //    globalAlarms.Is_NotConnection.Value = false;
                    //}

                    globalAlarms.Is_NotConnection.New_Value = false;

                    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //                       SIEMENS
                    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                    //Simens s7 -1200 modbus (Считали 0.0 адресс и 4.0, real в ПЛК)
                    //ushort[] registers = master.ReadHoldingRegisters(1, startAddress, 4);
                    //txtNR1.Text = NModbus.Utility.ModbusUtility.GetSingle(registers[0], registers[1]).ToString(); //Считыва
                    //txtNR4.Text = NModbus.Utility.ModbusUtility.GetSingle(registers[2], registers[3]).ToString();

                    using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        using (TcpClient client = new TcpClient(IpAdres, 502))
                        {
                            var factory = new ModbusFactory();
                            IModbusMaster master = factory.CreateMaster(client);
                            

                            //Считываем Float из ПЛК (SIEMENS S7-1200)
                            byte slaveId = 1;
                            ushort startAddress = 0; //32000;
                            ushort numInputs = 58;

                            registers = master.ReadHoldingRegisters(slaveId, startAddress, numInputs);

                            //Записывае биты в ПЛК если нужно (SIEMENS S7-1200)
                            if (Is_WritePLC) 
                            {                         //  Q8        Q7                  Q6                  Q5              Q4                      Q3                      Q2                  Q1
                                bool[] b6 = new bool[] {false, Siren_Gassed, Calibration_Zero_Analyzer, Purge_Probe, Close_Valve_Sample, Open_Heater_Ventilation, Pusk_Fan_Ventilation, Open_Valve_Ventilation,
                                              // Q16    Q15    Q14    Q13    Q12    Q11    Q10     Q9 
                                                false, false, false, false, false, false, false, false,
                                              // Q24    Q23    Q22    Q21    Q20    Q19    Q8     Q17 
                                                false, false, false, false, false, false, false, false,
                                              //       Q32        Q31    Q30    Q29    Q28    Q27    Q26     Q25 
                                                RUN_VENTILATION, false, false, false, false, false, false, false,};

                                var g1 = ToByteArray(b6);
                                var date2 = ModbusUtility.NetworkBytesToHostUInt16(g1);
                                master.WriteMultipleRegisters(1, 78, date2);

                                Is_WritePLC = false;
                            }

                            //Считываем биты из ПЛК  (SIEMENS S7-1200)
                            registers_DI = master.ReadHoldingRegisters(slaveId, 80, 2);
                            

                        }
                    }
                    int i = 0; //Первый байт
                    int j = 1; //Второй байт

                    SensorNow.Date = DateTime.Now;


                    //CO_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.CO_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //CO2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.CO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NO_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.NO_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NO2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.NO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NOx_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.NOx_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //SO2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.SO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Dust_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Dust_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //CH4_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.CH4_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //H2S_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.H2S_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_1_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Rezerv_1_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Rezerv_2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_3_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Rezerv_3_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_4_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Rezerv_4_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_5_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Rezerv_5_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //O2_Wet_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.O2_Wet_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //O2_Dry_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.O2_Dry_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //H2O_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.H2O_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Pressure_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Pressure_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Temperature_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Temperature_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Speed_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Speed_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Temperature_KIP_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Temperature_KIP_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }


                    i += 2;
                    j += 2;
                    //Temperature_NOx_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Temperature_NOx_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //P KIP 4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Pressure_KIP_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Температура воздуха точки росы 4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Temperature_Point_Dew_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Температура в измер. помещении 4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Temperature_Room_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Температура в помещении ПГС 4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.Temperature_PGS_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //O2 в измер. помещении 4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.O2_Room_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //O2 в помещении ПГС 4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.O2_PGS_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NH3_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j])))
                    {
                        SensorNow.NH3_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[i], registers[j]) / 1000), 3);
                    }

                    //Обрабатываем DI дискреты
                    var x = NModbus.Utility.ModbusUtility.GetUInt32(registers_DI[1], registers_DI[0]);
                    List<bool> StateWord_1 = new List<bool>();
                    for (int h = 0; h < 32; ++h)
                    {
                        bool isBitSet = ((x >> h) & 0x01) > 0;
                        StateWord_1.Add(isBitSet);
                    }

                    //1->8
                    //9->0
                    //17->24
                    //25->16
                    //Придумать алгоритм лучше
                    allAlarm.ASK10626.Input_1_Power.New_Value = StateWord_1[8];         //1     Ввод 1 запитан                                Запитан                                 Авария
                    allAlarm.ASK10626.Input_1_Used.New_Value = StateWord_1[9];          //2     Ввод 1 используется                           Не используется                         Используется
                    allAlarm.ASK10626.Input_2_Power.New_Value = StateWord_1[10];        //3     Ввод 2 запитан                                Запитан                                 Авария
                    allAlarm.ASK10626.Input_2_Used.New_Value = StateWord_1[11];         //4     Ввод 2 используется                           Используется                            Не используется
                    allAlarm.ASK10626.AZ_Konditioner.New_Value = StateWord_1[12];       //5     Розетка кондиционера запитана                 Запитана                                Авария
                    allAlarm.ASK10626.AZ_ServerCabinet.New_Value = StateWord_1[13];     //6     Шкаф сервера запитан                          Запитан                                 Авария
                    allAlarm.ASK10626.ON_FanVentilation.New_Value = StateWord_1[14];    //7     Вентилятор вентиляции включен                 Отключен                                Запущен
                    allAlarm.ASK10626.AZ_Input.New_Value = StateWord_1[15];             //8     Вводной автомат ШС включен                    Запитан                                 Авария
                    
                    allAlarm.ASK10626.AZ_UPS.New_Value = StateWord_1[0];                //9     Автомат ИБП включен                           Запитан                                 Авария
                    allAlarm.ASK10626.AZ_ServerSwitch.New_Value = StateWord_1[1];       //10    Автомат сервера и ком. оборудования включен   Запитан                                 Авария
                    allAlarm.ASK10626.AZ_SpeedMeter.New_Value = StateWord_1[2];         //11    Автомат измерю скорости включен               Запитан                                 Авария 
                    allAlarm.ASK10626.AZ_BP_A1.New_Value = StateWord_1[3];              //12    Автомат БП A1 включен                         Запитан                                 Авария 
                    allAlarm.ASK10626.AZ_BP_B1.New_Value = StateWord_1[4];              //13    Автомат БП В2 включен                         Запитан                                 Авария
                    allAlarm.ASK10626.AZ_AnalyzerBA1.New_Value = StateWord_1[5];        //14    Автомат анализатора BA1                       Запитан                                 Авария
                    allAlarm.ASK10626.AZ_Zond.New_Value = StateWord_1[6];               //15    Автомат зонда включен                         Запитан                                 Авария
                    allAlarm.ASK10626.Accident_FlowMeter.New_Value = StateWord_1[7];    //16    Авария расходомера                            В аварии                                В норме                                 
                    
                    allAlarm.ASK10626.Supply_ZeroGas.New_Value = StateWord_1[24];       //17    Подача нулевого газа в газоанл. (калибр нуля) Подаётся                                Не подаётся
                    allAlarm.ASK10626.Service_Request_BA1.New_Value = StateWord_1[25];  //18    Запрос на обслуживание BA1                    Есть запрос                             Нет запроса
                    allAlarm.ASK10626.Error_GasAnalyzerBA1.New_Value = StateWord_1[26]; //19    Ошибка газоанализатора BA1                    Есть ошибка                             Нет ошибки
                    allAlarm.ASK10626.Underheating_Zond.New_Value = StateWord_1[27];    //20    Недогрев пробоотборного зонда                 Недогрев                                догрев
                                                                
                                                                // = StateWord_1[28];   //21
                                                                // = StateWord_1[29];   //22
                                                                // = StateWord_1[30];   //23
                                                                // = StateWord_1[31];   //24
                                                                
                                                                // = StateWord_1[16];   //25
                                                                // = StateWord_1[17];   //26
                                                                // = StateWord_1[18];   //27
                                                                // = StateWord_1[19];   //28
                                                                // = StateWord_1[20];   //29
                                                                // = StateWord_1[21];   //30
                                                                // = StateWord_1[22];   //31
                                                                // = StateWord_1[23];   //32


                    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //                      Европрибор
                    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                    //using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    //{
                    //    using (TcpClient client = new TcpClient(IpAdres, 502))
                    //    {
                    //        var factory = new ModbusFactory();
                    //        IModbusMaster master = factory.CreateMaster(client);

                    //        byte slaveId = 1;
                    //        ushort startAddress = 0; //32000;
                    //        ushort numInputs = 2;

                    //        registers = master.ReadInputRegisters(slaveId, startAddress, numInputs);
                    //    }
                    //}
                    //int i = 0; //Первый байт
                    //int j = 1; //Второй байт

                    //SensorNow.Date = DateTime.Now;


                    ////CO_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.CO_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////CO2_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.CO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////NO_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.NO_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////NO2_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.NO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////NOx_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.NOx_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////SO2_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.SO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Dust_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Dust_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////CH4_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.CH4_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////H2S_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.H2S_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}


                    //i += 2;
                    //j += 2;
                    ////Rezerv_1_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Rezerv_1_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Rezerv_2_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Rezerv_2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Rezerv_3_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Rezerv_3_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Rezerv_4_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Rezerv_4_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Rezerv_5_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Rezerv_5_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////O2_Wet_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.O2_Wet_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////O2_Dry_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.O2_Dry_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////H2O_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.H2O_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Pressure_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Pressure_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Temperature_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Temperature_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Speed_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Speed_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}

                    //i += 2;
                    //j += 2;
                    ////Temperature_KIP_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Temperature_KIP_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}


                    //i += 2;
                    //j += 2;
                    ////Temperature_NOx_4-20mA
                    //if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    //{
                    //    SensorNow.Temperature_NOx_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    //}
                }
                else
                {
                    //if (!globalAlarms.Is_NotConnection.Value)
                    //{
                    //    using (ApplicationDbContext db = new ApplicationDbContext())
                    //    {
                    //        var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
                    //        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_NotConnection);
                    //    }
                    //    globalAlarms.Is_NotConnection.Value = true;
                    //}

                    globalAlarms.Is_NotConnection.New_Value = true;

                    SensorNow.Date = DateTime.Now;

                    //Режим симуляции
                    if (is_simulation)
                    {
                        SensorNow.CO_4_20mA = Simulation_4_20(SensorNow.CO_4_20mA);
                        SensorNow.CO2_4_20mA = Simulation_4_20(SensorNow.CO2_4_20mA);
                        SensorNow.NO_4_20mA = Simulation_4_20(SensorNow.NO_4_20mA);
                        SensorNow.NO2_4_20mA = Simulation_4_20(SensorNow.NO2_4_20mA);
                        SensorNow.NOx_4_20mA = Simulation_4_20(SensorNow.NOx_4_20mA);
                        SensorNow.SO2_4_20mA = Simulation_4_20(SensorNow.SO2_4_20mA);
                        SensorNow.Dust_4_20mA = Simulation_4_20(SensorNow.Dust_4_20mA);
                        SensorNow.CH4_4_20mA = Simulation_4_20(SensorNow.CH4_4_20mA);
                        SensorNow.H2S_4_20mA = Simulation_4_20(SensorNow.H2S_4_20mA);
                        SensorNow.NH3_4_20mA = Simulation_4_20(SensorNow.NH3_4_20mA);

                        SensorNow.Rezerv_1_4_20mA = Simulation_4_20(SensorNow.Rezerv_1_4_20mA); 
                        SensorNow.Rezerv_2_4_20mA = Simulation_4_20(SensorNow.Rezerv_2_4_20mA);
                        SensorNow.Rezerv_3_4_20mA = Simulation_4_20(SensorNow.Rezerv_3_4_20mA);
                        SensorNow.Rezerv_4_4_20mA = Simulation_4_20(SensorNow.Rezerv_3_4_20mA);
                        SensorNow.Rezerv_5_4_20mA = Simulation_4_20(SensorNow.Rezerv_5_4_20mA);

                        SensorNow.O2_Wet_4_20mA = Simulation_4_20(SensorNow.O2_Wet_4_20mA, true);
                        SensorNow.O2_Dry_4_20mA = SensorNow.O2_Wet_4_20mA;
                        O2_Last = SensorNow.O2_Wet_4_20mA;
                        SensorNow.H2O_4_20mA = Simulation_4_20(SensorNow.H2O_4_20mA);

                        SensorNow.Pressure_4_20mA = Simulation_4_20(SensorNow.Pressure_4_20mA);
                        SensorNow.Temperature_4_20mA = Simulation_4_20(SensorNow.Temperature_4_20mA);
                        SensorNow.Speed_4_20mA = Simulation_4_20(SensorNow.Speed_4_20mA);

                        SensorNow.Temperature_KIP_4_20mA = Simulation_4_20(SensorNow.Temperature_KIP_4_20mA);
                        SensorNow.Temperature_NOx_4_20mA = Simulation_4_20(SensorNow.Temperature_NOx_4_20mA);
                        SensorNow.Pressure_KIP_4_20mA = Simulation_4_20(SensorNow.Pressure_KIP_4_20mA);
                        SensorNow.Temperature_Point_Dew_4_20mA = Simulation_4_20(SensorNow.Temperature_Point_Dew_4_20mA);
                        SensorNow.Temperature_Room_4_20mA = Simulation_4_20(SensorNow.Temperature_Room_4_20mA);
                        SensorNow.Temperature_PGS_4_20mA = Simulation_4_20(SensorNow.Temperature_PGS_4_20mA);
                        SensorNow.O2_Room_4_20mA = Simulation_4_20(SensorNow.O2_Room_4_20mA);
                        SensorNow.O2_PGS_4_20mA = Simulation_4_20(SensorNow.O2_PGS_4_20mA);

                    }
                    else
                    {
                        SensorNow.CO_4_20mA = 0.0;
                        SensorNow.CO2_4_20mA = 0.0;
                        SensorNow.NO_4_20mA = 0.0;
                        SensorNow.NO2_4_20mA = 0.0;
                        SensorNow.NOx_4_20mA = 0.0;
                        SensorNow.SO2_4_20mA = 0.0;
                        SensorNow.Dust_4_20mA = 0.0;
                        SensorNow.CH4_4_20mA = 0.0;
                        SensorNow.H2S_4_20mA = 0.0;
                        SensorNow.NH3_4_20mA = 0.0;

                        SensorNow.Rezerv_1_4_20mA = 0.0;
                        SensorNow.Rezerv_2_4_20mA = 0.0;
                        SensorNow.Rezerv_3_4_20mA = 0.0;
                        SensorNow.Rezerv_4_4_20mA = 0.0;
                        SensorNow.Rezerv_5_4_20mA = 0.0;

                        SensorNow.O2_Wet_4_20mA = 0.0;
                        SensorNow.O2_Dry_4_20mA = 0.0;
                        SensorNow.H2O_4_20mA = 0.0;

                        SensorNow.Pressure_4_20mA = 0.0;
                        SensorNow.Temperature_4_20mA = 0.0;
                        SensorNow.Speed_4_20mA = 0.0;

                        SensorNow.Temperature_KIP_4_20mA = 0.0;
                        SensorNow.Temperature_NOx_4_20mA = 0.0;
                        SensorNow.Pressure_KIP_4_20mA = 0.0;
                        SensorNow.Temperature_Point_Dew_4_20mA = 0.0;
                        SensorNow.Temperature_Room_4_20mA = 0.0;
                        SensorNow.Temperature_PGS_4_20mA = 0.0;
                        SensorNow.O2_Room_4_20mA = 0.0;
                        SensorNow.O2_PGS_4_20mA = 0.0;
                    }
                }
            }
            catch
            {
                //if (!globalAlarms.Is_NotConnection.Value)
                //{
                //    using (ApplicationDbContext db = new ApplicationDbContext())
                //    {
                //        var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
                //        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_NotConnection);
                //    }
                //    globalAlarms.Is_NotConnection.Value = true;
                //}

                globalAlarms.Is_NotConnection.New_Value = true;

                SensorNow.Date = DateTime.Now;

                SensorNow.CO_4_20mA = 0.0;
                SensorNow.CO2_4_20mA = 0.0;
                SensorNow.NO_4_20mA = 0.0;
                SensorNow.NO2_4_20mA = 0.0;
                SensorNow.NOx_4_20mA = 0.0;
                SensorNow.SO2_4_20mA = 0.0;
                SensorNow.Dust_4_20mA = 0.0;
                SensorNow.CH4_4_20mA = 0.0;
                SensorNow.H2S_4_20mA = 0.0;
                SensorNow.NH3_4_20mA = 0.0;

                SensorNow.Rezerv_1_4_20mA = 0.0;
                SensorNow.Rezerv_2_4_20mA = 0.0;
                SensorNow.Rezerv_3_4_20mA = 0.0;
                SensorNow.Rezerv_4_4_20mA = 0.0;
                SensorNow.Rezerv_5_4_20mA = 0.0;

                SensorNow.O2_Wet_4_20mA = 0.0;
                SensorNow.O2_Dry_4_20mA = 0.0;
                SensorNow.H2O_4_20mA = 0.0;

                SensorNow.Pressure_4_20mA = 0.0;
                SensorNow.Temperature_4_20mA = 0.0;
                SensorNow.Speed_4_20mA = 0.0;

                SensorNow.Temperature_KIP_4_20mA = 0.0;
                SensorNow.Temperature_NOx_4_20mA = 0.0;
                SensorNow.Pressure_KIP_4_20mA = 0.0;
                SensorNow.Temperature_Point_Dew_4_20mA = 0.0;
                SensorNow.Temperature_Room_4_20mA = 0.0;
                SensorNow.Temperature_PGS_4_20mA = 0.0;
                SensorNow.O2_Room_4_20mA = 0.0;
                SensorNow.O2_PGS_4_20mA = 0.0;
            }
        }



        //Проверяем какие аварии нам необходимо отслеживать (проверяется только при изменении натроек и при запуске приложения)
        public static void ChekActiveAlarms() //Оптимизировать весь процесс с нуля! Есть проблемы! //1 попытка оптимизации есть
        {
            //Обрыв датчиков
            //CO обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.CO.CheckState = () =>
            {
                if (SensorNow.CO_4_20mA < SensorRange.CO.mA.Min || SensorNow.CO_4_20mA > SensorRange.CO.mA.Max)
                    return true;
                return false;
            };

            //CO2 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.CO2.CheckState = () =>
            {
                if (SensorNow.CO2_4_20mA < SensorRange.CO2.mA.Min || SensorNow.CO2_4_20mA > SensorRange.CO2.mA.Max)
                    return true;
                return false;
            };

            //NO обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.NO.CheckState = () =>
            {
                if (SensorNow.NO_4_20mA < SensorRange.NO.mA.Min || SensorNow.NO_4_20mA > SensorRange.NO.mA.Max)
                    return true;
                return false;
            };

            //NO2 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.NO2.CheckState = () =>
            {
                if (SensorNow.NO2_4_20mA < SensorRange.NO2.mA.Min || SensorNow.NO2_4_20mA > SensorRange.NO2.mA.Max)
                    return true;
                return false;
            };

            //NOx обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.NOx.CheckState = () =>
            {
                if (SensorNow.NOx_4_20mA < SensorRange.NOx.mA.Min || SensorNow.NOx_4_20mA > SensorRange.NOx.mA.Max)
                    return true;
                return false;
            };

            //SO2 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.SO2.CheckState = () =>
            {
                if (SensorNow.SO2_4_20mA < SensorRange.SO2.mA.Min || SensorNow.SO2_4_20mA > SensorRange.SO2.mA.Max)
                    return true;
                return false;
            };

            //Dust обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Dust.CheckState = () =>
            {
                if (SensorNow.Dust_4_20mA < SensorRange.Dust.mA.Min || SensorNow.Dust_4_20mA > SensorRange.Dust.mA.Max)
                    return true;
                return false;
            };

            //CH4 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.CH4.CheckState = () =>
            {
                if (SensorNow.CH4_4_20mA < SensorRange.CH4.mA.Min || SensorNow.CH4_4_20mA > SensorRange.CH4.mA.Max)
                    return true;
                return false;
            };

            //H2S обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.H2S.CheckState = () =>
            {
                if (SensorNow.H2S_4_20mA < SensorRange.H2S.mA.Min || SensorNow.H2S_4_20mA > SensorRange.H2S.mA.Max)
                    return true;
                return false;
            };

            //NH3 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.NH3.CheckState = () =>
            {
                if (SensorNow.NH3_4_20mA < SensorRange.NH3.mA.Min || SensorNow.NH3_4_20mA > SensorRange.NH3.mA.Max)
                    return true;
                return false;
            };

            //Rezerv_1 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Rezerv_1.CheckState = () =>
            {
                if (SensorNow.Rezerv_1_4_20mA < SensorRange.Rezerv_1.mA.Min || SensorNow.Rezerv_1_4_20mA > SensorRange.Rezerv_1.mA.Max)
                    return true;
                return false;
            };

            //Rezerv_2 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Rezerv_2.CheckState = () =>
            {
                if (SensorNow.Rezerv_2_4_20mA < SensorRange.Rezerv_2.mA.Min || SensorNow.Rezerv_2_4_20mA > SensorRange.Rezerv_2.mA.Max)
                    return true;
                return false;
            };

            //Rezerv_3 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Rezerv_3.CheckState = () =>
            {
                if (SensorNow.Rezerv_3_4_20mA < SensorRange.Rezerv_3.mA.Min || SensorNow.Rezerv_3_4_20mA > SensorRange.Rezerv_3.mA.Max)
                    return true;
                return false;
            };

            //Rezerv_4 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Rezerv_4.CheckState = () =>
            {
                if (SensorNow.Rezerv_4_4_20mA < SensorRange.Rezerv_4.mA.Min || SensorNow.Rezerv_4_4_20mA > SensorRange.Rezerv_4.mA.Max)
                    return true;
                return false;
            };

            //Rezerv_5 обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Rezerv_5.CheckState = () =>
            {
                if (SensorNow.Rezerv_5_4_20mA < SensorRange.Rezerv_5.mA.Min || SensorNow.Rezerv_5_4_20mA > SensorRange.Rezerv_5.mA.Max)
                    return true;
                return false;
            };

            //O2_Wet обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.O2_Wet.CheckState = () =>
            {
                if (SensorNow.O2_Wet_4_20mA < SensorRange.O2Wet.mA.Min || SensorNow.O2_Wet_4_20mA > SensorRange.O2Wet.mA.Max)
                    return true;
                return false;
            };

            //O2_Dry обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.O2_Dry.CheckState = () =>
            {
                if (SensorNow.O2_Dry_4_20mA < SensorRange.O2Dry.mA.Min || SensorNow.O2_Dry_4_20mA > SensorRange.O2Dry.mA.Max)
                    return true;
                return false;
            };

            //H2O обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.H2O.CheckState = () =>
            {
                if (SensorNow.H2O_4_20mA < SensorRange.H2O.mA.Min || SensorNow.H2O_4_20mA > SensorRange.H2O.mA.Max)
                    return true;
                return false;
            };

            //Pressure обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Pressure.CheckState = () =>
            {
                if (SensorNow.Pressure_4_20mA < SensorRange.Pressure.mA.Min || SensorNow.Pressure_4_20mA > SensorRange.Pressure.mA.Max)
                    return true;
                return false;
            };

            //Speed обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Speed.CheckState = () =>
            {
                if (SensorNow.Speed_4_20mA < SensorRange.Speed.mA.Min || SensorNow.Speed_4_20mA > SensorRange.Speed.mA.Max)
                    return true;
                return false;
            };

            //Temperature обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Temperature.CheckState = () =>
            {
                if (SensorNow.Temperature_4_20mA < SensorRange.Temperature.mA.Min || SensorNow.Temperature_4_20mA > SensorRange.Temperature.mA.Max)
                    return true;
                return false;
            };

            //Temperature KIP обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Temperature_KIP.CheckState = () =>
            {
                if (SensorNow.Temperature_KIP_4_20mA < SensorRange.Temperature_KIP.mA.Min || SensorNow.Temperature_KIP_4_20mA > SensorRange.Temperature_KIP.mA.Max)
                    return true;
                return false;
            };

            //Temperature NOx обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Temperature_NOx.CheckState = () =>
            {
                if (SensorNow.Temperature_NOx_4_20mA < SensorRange.Temperature_NOx.mA.Min || SensorNow.Temperature_NOx_4_20mA > SensorRange.Temperature_NOx.mA.Max)
                    return true;
                return false;
            };

            //Pressure KIP обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Pressure_KIP.CheckState = () =>
            {
                if (SensorNow.Pressure_KIP_4_20mA < SensorRange.Pressure_KIP.mA.Min || SensorNow.Pressure_KIP_4_20mA > SensorRange.Pressure_KIP.mA.Max)
                    return true;
                return false;
            };

            //Temperature воздуха точки росы КИП обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Temperature_Point_Dew.CheckState = () =>
            {
                if (SensorNow.Temperature_Point_Dew_4_20mA < SensorRange.Temperature_Point_Dew.mA.Min || SensorNow.Temperature_Point_Dew_4_20mA > SensorRange.Temperature_Point_Dew.mA.Max)
                    return true;
                return false;
            };

            //Temperature воздуха в измер. помещении обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Temperature_Room.CheckState = () =>
            {
                if (SensorNow.Temperature_Room_4_20mA < SensorRange.Temperature_Room.mA.Min || SensorNow.Temperature_Room_4_20mA > SensorRange.Temperature_Room.mA.Max)
                    return true;
                return false;
            };

            //Temperature в помещение ПГС обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.Temperature_PGS.CheckState = () =>
            {
                if (SensorNow.Temperature_PGS_4_20mA < SensorRange.Temperature_PGS.mA.Min || SensorNow.Temperature_PGS_4_20mA > SensorRange.Temperature_PGS.mA.Max)
                    return true;
                return false;
            };

            //О2 в измер. помещении обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.O2_Room.CheckState = () =>
            {
                if (SensorNow.O2_Room_4_20mA < SensorRange.O2_Room.mA.Min || SensorNow.O2_Room_4_20mA > SensorRange.O2_Room.mA.Max)
                    return true;
                return false;
            };

            //О2 в помещении GUC обрыв, создаём анонимный метод и помещаем в делегат
            allAlarm.SensorAlarm.O2_PGS.CheckState = () =>
            {
                if (SensorNow.O2_PGS_4_20mA < SensorRange.O2_PGS.mA.Min || SensorNow.O2_PGS_4_20mA > SensorRange.O2_PGS.mA.Max)
                    return true;
                return false;
            };


            //Глобальные аварии и уведомления
            //Общая авария
            globalAlarms.Is_Error.CheckState = () =>
            {
                if ((allAlarm.SensorAlarm.CO.Value && allAlarm.SensorAlarm.CO.Is_Critical) ||
                    (allAlarm.SensorAlarm.CO2.Value && allAlarm.SensorAlarm.CO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO.Value && allAlarm.SensorAlarm.NO.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO2.Value && allAlarm.SensorAlarm.NO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NOx.Value && allAlarm.SensorAlarm.NOx.Is_Critical) ||
                    (allAlarm.SensorAlarm.SO2.Value && allAlarm.SensorAlarm.SO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Dust.Value && allAlarm.SensorAlarm.Dust.Is_Critical) ||
                    (allAlarm.SensorAlarm.CH4.Value && allAlarm.SensorAlarm.CH4.Is_Critical) ||
                    (allAlarm.SensorAlarm.NH3.Value && allAlarm.SensorAlarm.NH3.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2S.Value && allAlarm.SensorAlarm.H2S.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_1.Value && allAlarm.SensorAlarm.Rezerv_1.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_2.Value && allAlarm.SensorAlarm.Rezerv_2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_3.Value && allAlarm.SensorAlarm.Rezerv_3.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_4.Value && allAlarm.SensorAlarm.Rezerv_4.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_5.Value && allAlarm.SensorAlarm.Rezerv_5.Is_Critical) ||
                    
                    (allAlarm.SensorAlarm.O2_Wet.Value && allAlarm.SensorAlarm.O2_Wet.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Dry.Value && allAlarm.SensorAlarm.O2_Dry.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2O.Value && allAlarm.SensorAlarm.H2O.Is_Critical) ||
                    
                    (allAlarm.SensorAlarm.Pressure.Value && allAlarm.SensorAlarm.Pressure.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature.Value && allAlarm.SensorAlarm.Temperature.Is_Critical) ||
                    (allAlarm.SensorAlarm.Speed.Value && allAlarm.SensorAlarm.Speed.Is_Critical) ||
                    
                    (allAlarm.SensorAlarm.Temperature_KIP.Value && allAlarm.SensorAlarm.Temperature_KIP.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_NOx.Value && allAlarm.SensorAlarm.Temperature_NOx.Is_Critical) ||
                    (allAlarm.SensorAlarm.Pressure_KIP.Value && allAlarm.SensorAlarm.Pressure_KIP.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_Point_Dew.Value && allAlarm.SensorAlarm.Temperature_Point_Dew.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_Room.Value && allAlarm.SensorAlarm.Temperature_Room.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_PGS.Value && allAlarm.SensorAlarm.Temperature_PGS.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Room.Value && allAlarm.SensorAlarm.O2_Room.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_PGS.Value && allAlarm.SensorAlarm.O2_PGS.Is_Critical))
                    return true;
                return false;
            };

            //Информационное сообщение
            globalAlarms.Is_Info.CheckState = () =>
            {
                if ((allAlarm.SensorAlarm.CO.Value && !allAlarm.SensorAlarm.CO.Is_Critical) ||
                    (allAlarm.SensorAlarm.CO2.Value && !allAlarm.SensorAlarm.CO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO.Value && !allAlarm.SensorAlarm.NO.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO2.Value && !allAlarm.SensorAlarm.NO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NOx.Value && !allAlarm.SensorAlarm.NOx.Is_Critical) ||
                    (allAlarm.SensorAlarm.SO2.Value && !allAlarm.SensorAlarm.SO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Dust.Value && !allAlarm.SensorAlarm.Dust.Is_Critical) ||
                    (allAlarm.SensorAlarm.CH4.Value && !allAlarm.SensorAlarm.CH4.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2S.Value && !allAlarm.SensorAlarm.H2S.Is_Critical) ||
                    (allAlarm.SensorAlarm.NH3.Value && !allAlarm.SensorAlarm.NH3.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_1.Value && !allAlarm.SensorAlarm.Rezerv_1.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_2.Value && !allAlarm.SensorAlarm.Rezerv_2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_3.Value && !allAlarm.SensorAlarm.Rezerv_3.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_4.Value && !allAlarm.SensorAlarm.Rezerv_4.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_5.Value && !allAlarm.SensorAlarm.Rezerv_5.Is_Critical) ||
                    
                    (allAlarm.SensorAlarm.O2_Wet.Value && !allAlarm.SensorAlarm.O2_Wet.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Dry.Value && !allAlarm.SensorAlarm.O2_Dry.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2O.Value && !allAlarm.SensorAlarm.H2O.Is_Critical) ||
                    (allAlarm.SensorAlarm.Pressure.Value && !allAlarm.SensorAlarm.Pressure.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature.Value && !allAlarm.SensorAlarm.Temperature.Is_Critical) ||
                    (allAlarm.SensorAlarm.Speed.Value && !allAlarm.SensorAlarm.Speed.Is_Critical) ||
                    
                    (allAlarm.SensorAlarm.Temperature_KIP.Value && !allAlarm.SensorAlarm.Temperature_KIP.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_NOx.Value && !allAlarm.SensorAlarm.Temperature_NOx.Is_Critical) ||
                    (allAlarm.SensorAlarm.Pressure_KIP.Value && !allAlarm.SensorAlarm.Pressure_KIP.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_Point_Dew.Value && !allAlarm.SensorAlarm.Temperature_Point_Dew.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_Room.Value && !allAlarm.SensorAlarm.Temperature_Room.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_PGS.Value && !allAlarm.SensorAlarm.Temperature_PGS.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Room.Value && !allAlarm.SensorAlarm.O2_Room.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_PGS.Value && !allAlarm.SensorAlarm.O2_PGS.Is_Critical))
                    return true;
                return false;
            };

            //Простой  //Условия простоя (на данный момент, есть авария - ппростой)
            globalAlarms.Is_Stop.CheckState = () =>
            {
                if (globalAlarms.Is_Error.Value)
                    return true;
                return false;
            };

            //Нет выбросов  //Пока смотрим только по кислороду (сухому, если нет сухого по влажн)
            globalAlarms.Is_NotProcess.CheckState = () =>
            {
                if (SensorRange.O2Dry.Is_Used)
                {
                    if (CurrentConcEmis.O2_Dry >= 20.0 && !globalAlarms.Is_Stop.Value && !allAlarm.SensorAlarm.O2_Dry.Value)
                        return true;
                    return false;
                }
                else
                {
                    if (SensorRange.O2Wet.Is_Used)
                    {
                        if (CurrentConcEmis.O2_Wet >= 20.0 && !globalAlarms.Is_Stop.Value && !allAlarm.SensorAlarm.O2_Wet.Value)
                            return true;
                        return false;
                    }
                    return false; //Датчики кислородов не используются
                }
            };

            //Превышение  //Превышение ПДЗ
            globalAlarms.Is_Excess.CheckState = () =>
            {
                if (CurrentConcEmis.CO_Conc > PDZ_Current.CO_Conc || CurrentConcEmis.CO_Emis > PDZ_Current.CO_Emis ||
                    CurrentConcEmis.CO2_Conc > PDZ_Current.CO2_Conc || CurrentConcEmis.CO2_Emis > PDZ_Current.CO2_Emis ||
                    CurrentConcEmis.NO_Conc > PDZ_Current.NO_Conc || CurrentConcEmis.NO_Emis > PDZ_Current.NO_Emis ||
                    CurrentConcEmis.NO2_Conc > PDZ_Current.NO2_Conc || CurrentConcEmis.NO2_Emis > PDZ_Current.NO2_Emis ||
                    CurrentConcEmis.NOx_Conc > PDZ_Current.NOx_Conc || CurrentConcEmis.NOx_Emis > PDZ_Current.NOx_Emis ||
                    CurrentConcEmis.Dust_Conc > PDZ_Current.Dust_Conc || CurrentConcEmis.Dust_Emis > PDZ_Current.Dust_Emis ||
                    CurrentConcEmis.CH4_Conc > PDZ_Current.CH4_Conc || CurrentConcEmis.CH4_Emis > PDZ_Current.CH4_Emis ||
                    CurrentConcEmis.H2S_Conc > PDZ_Current.H2S_Conc || CurrentConcEmis.H2S_Emis > PDZ_Current.H2S_Emis ||
                    CurrentConcEmis.NH3_Conc > PDZ_Current.NH3_Conc || CurrentConcEmis.NH3_Emis > PDZ_Current.NH3_Emis ||
                    CurrentConcEmis.Add_Conc_1 > PDZ_Current.Add_Conc_1 || CurrentConcEmis.Add_Emis_1 > PDZ_Current.Add_Emis_1 ||
                    CurrentConcEmis.Add_Conc_2 > PDZ_Current.Add_Conc_2 || CurrentConcEmis.Add_Emis_2 > PDZ_Current.Add_Emis_2 ||
                    CurrentConcEmis.Add_Conc_3 > PDZ_Current.Add_Conc_3 || CurrentConcEmis.Add_Emis_3 > PDZ_Current.Add_Emis_3 ||
                    CurrentConcEmis.Add_Conc_4 > PDZ_Current.Add_Conc_4 || CurrentConcEmis.Add_Emis_4 > PDZ_Current.Add_Emis_4 ||
                    CurrentConcEmis.Add_Conc_5 > PDZ_Current.Add_Conc_5 || CurrentConcEmis.Add_Emis_5 > PDZ_Current.Add_Emis_5)
                    return true;
                return false;
            };

            //Приближение 
            globalAlarms.Is_Approximation.CheckState = () =>
            {
                // approximation = 0.8; // Приближение //Завести отделбную переменную"!
                if (CurrentConcEmis.CO_Conc > PDZ_Current.CO_Conc * approximation && CurrentConcEmis.CO_Conc < PDZ_Current.CO_Conc || CurrentConcEmis.CO_Emis > PDZ_Current.CO_Emis * approximation && CurrentConcEmis.CO_Emis < PDZ_Current.CO_Emis ||
                    CurrentConcEmis.CO2_Conc > PDZ_Current.CO2_Conc * approximation && CurrentConcEmis.CO2_Conc < PDZ_Current.CO2_Conc || CurrentConcEmis.CO2_Emis > PDZ_Current.CO2_Emis * approximation && CurrentConcEmis.CO2_Emis < PDZ_Current.CO2_Emis ||
                    CurrentConcEmis.NO_Conc > PDZ_Current.NO_Conc * approximation && CurrentConcEmis.NO_Conc < PDZ_Current.NO_Conc || CurrentConcEmis.NO_Emis > PDZ_Current.NO_Emis * approximation && CurrentConcEmis.NO_Emis < PDZ_Current.NO_Emis ||
                    CurrentConcEmis.NO2_Conc > PDZ_Current.NO2_Conc * approximation && CurrentConcEmis.NO2_Conc < PDZ_Current.NO2_Conc || CurrentConcEmis.NO2_Emis > PDZ_Current.NO2_Emis * approximation && CurrentConcEmis.NO2_Emis < PDZ_Current.NO2_Emis ||
                    CurrentConcEmis.NOx_Conc > PDZ_Current.NOx_Conc * approximation && CurrentConcEmis.NOx_Conc < PDZ_Current.NOx_Conc || CurrentConcEmis.NOx_Emis > PDZ_Current.NOx_Emis * approximation && CurrentConcEmis.NOx_Emis < PDZ_Current.NOx_Emis ||
                    CurrentConcEmis.Dust_Conc > PDZ_Current.Dust_Conc * approximation && CurrentConcEmis.Dust_Conc < PDZ_Current.Dust_Conc || CurrentConcEmis.Dust_Emis > PDZ_Current.Dust_Emis * approximation && CurrentConcEmis.Dust_Emis < PDZ_Current.Dust_Emis ||
                    CurrentConcEmis.CH4_Conc > PDZ_Current.CH4_Conc * approximation && CurrentConcEmis.CH4_Conc < PDZ_Current.CH4_Conc || CurrentConcEmis.CH4_Emis > PDZ_Current.CH4_Emis * approximation && CurrentConcEmis.CH4_Emis < PDZ_Current.CH4_Emis ||
                    CurrentConcEmis.H2S_Conc > PDZ_Current.H2S_Conc * approximation && CurrentConcEmis.H2S_Conc < PDZ_Current.H2S_Conc || CurrentConcEmis.H2S_Emis > PDZ_Current.H2S_Emis * approximation && CurrentConcEmis.H2S_Emis < PDZ_Current.H2S_Emis ||
                    CurrentConcEmis.NH3_Conc > PDZ_Current.NH3_Conc * approximation && CurrentConcEmis.NH3_Conc < PDZ_Current.NH3_Conc || CurrentConcEmis.NH3_Emis > PDZ_Current.NH3_Emis * approximation && CurrentConcEmis.NH3_Emis < PDZ_Current.NH3_Emis ||
                    CurrentConcEmis.Add_Conc_1 > PDZ_Current.Add_Conc_1 * approximation && CurrentConcEmis.Add_Conc_1 < PDZ_Current.Add_Conc_1 || CurrentConcEmis.Add_Emis_1 > PDZ_Current.Add_Emis_1 * approximation && CurrentConcEmis.Add_Emis_1 < PDZ_Current.Add_Emis_1 ||
                    CurrentConcEmis.Add_Conc_2 > PDZ_Current.Add_Conc_2 * approximation && CurrentConcEmis.Add_Conc_2 < PDZ_Current.Add_Conc_2 || CurrentConcEmis.Add_Emis_2 > PDZ_Current.Add_Emis_2 * approximation && CurrentConcEmis.Add_Emis_2 < PDZ_Current.Add_Emis_2 ||
                    CurrentConcEmis.Add_Conc_3 > PDZ_Current.Add_Conc_3 * approximation && CurrentConcEmis.Add_Conc_3 < PDZ_Current.Add_Conc_3 || CurrentConcEmis.Add_Emis_3 > PDZ_Current.Add_Emis_3 * approximation && CurrentConcEmis.Add_Emis_3 < PDZ_Current.Add_Emis_3 ||
                    CurrentConcEmis.Add_Conc_4 > PDZ_Current.Add_Conc_4 * approximation && CurrentConcEmis.Add_Conc_4 < PDZ_Current.Add_Conc_4 || CurrentConcEmis.Add_Emis_4 > PDZ_Current.Add_Emis_4 * approximation && CurrentConcEmis.Add_Emis_4 < PDZ_Current.Add_Emis_4 ||
                    CurrentConcEmis.Add_Conc_5 > PDZ_Current.Add_Conc_5 * approximation && CurrentConcEmis.Add_Conc_5 < PDZ_Current.Add_Conc_5 || CurrentConcEmis.Add_Emis_5 > PDZ_Current.Add_Emis_5 * approximation && CurrentConcEmis.Add_Emis_5 < PDZ_Current.Add_Emis_5)
                    return true;
                return false;
            };

            //Техническое обслуживание
            globalAlarms.Is_Maintenance.CheckState = () =>
            {
                if (false) //Отключено
                    return true;
                return false;
            };

            //Дверь контейнера
            globalAlarms.Is_OpenDoor.CheckState = () =>
            {
                if (false)  //Отключено
                    return true;
                return false;
            };

            //Сигнал пожара
            globalAlarms.Is_Fire.CheckState = () =>
            {
                if (false) //Отключено
                    return true;
                return false;
            };

            CreatActiveAlarmList(); //Проверяем какие аварии будут задействованы
        }



        //На данный момент создаём 2 массива, с активными авариями: список обычными дискретами и список с авариями которые поределяем в процессе (проверяется только при изменении натроек и при запуске приложения)
        public static void CreatActiveAlarmList()
        {
            alarmList_Delegate.Clear();
            alarmList.Clear();

            //Аварии с делегатами
            //Обрывы датчиков
            if (allAlarm.SensorAlarm.CO.Is_Used && SensorRange.CO.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.CO);
            if (allAlarm.SensorAlarm.CO2.Is_Used && SensorRange.CO2.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.CO2);
            if (allAlarm.SensorAlarm.NO.Is_Used && SensorRange.NO.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.NO);
            if (allAlarm.SensorAlarm.NO2.Is_Used && SensorRange.NO2.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.NO2);
            if (allAlarm.SensorAlarm.NOx.Is_Used && SensorRange.NOx.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.NOx);
            if (allAlarm.SensorAlarm.SO2.Is_Used && SensorRange.SO2.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.SO2);
            if (allAlarm.SensorAlarm.Dust.Is_Used && SensorRange.Dust.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Dust);
            if (allAlarm.SensorAlarm.CH4.Is_Used && SensorRange.CH4.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.CH4);
            if (allAlarm.SensorAlarm.H2S.Is_Used && SensorRange.H2S.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.H2S);
            if (allAlarm.SensorAlarm.NH3.Is_Used && SensorRange.NH3.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.NH3);

            if (allAlarm.SensorAlarm.Rezerv_1.Is_Used && SensorRange.Rezerv_1.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Rezerv_1);
            if (allAlarm.SensorAlarm.Rezerv_2.Is_Used && SensorRange.Rezerv_2.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Rezerv_2);
            if (allAlarm.SensorAlarm.Rezerv_3.Is_Used && SensorRange.Rezerv_3.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Rezerv_3);
            if (allAlarm.SensorAlarm.Rezerv_4.Is_Used && SensorRange.Rezerv_4.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Rezerv_4);
            if (allAlarm.SensorAlarm.Rezerv_5.Is_Used && SensorRange.Rezerv_5.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Rezerv_5);

            if (allAlarm.SensorAlarm.O2_Wet.Is_Used && SensorRange.O2Wet.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.O2_Wet);
            if (allAlarm.SensorAlarm.O2_Dry.Is_Used && SensorRange.O2Dry.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.O2_Dry);
            if (allAlarm.SensorAlarm.H2O.Is_Used && SensorRange.H2O.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.H2O);

            if (allAlarm.SensorAlarm.Pressure.Is_Used && SensorRange.Pressure.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Pressure);
            if (allAlarm.SensorAlarm.Temperature.Is_Used && SensorRange.Temperature.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Temperature);
            if (allAlarm.SensorAlarm.Speed.Is_Used && SensorRange.Speed.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Speed);
            
            if (allAlarm.SensorAlarm.Temperature_KIP.Is_Used && SensorRange.Temperature_KIP.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Temperature_KIP);
            if (allAlarm.SensorAlarm.Temperature_NOx.Is_Used && SensorRange.Temperature_NOx.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Temperature_NOx);
            if (allAlarm.SensorAlarm.Pressure_KIP.Is_Used && SensorRange.Pressure_KIP.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Pressure_KIP);
            if (allAlarm.SensorAlarm.Temperature_Point_Dew.Is_Used && SensorRange.Temperature_Point_Dew.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Temperature_Point_Dew);
            if (allAlarm.SensorAlarm.Temperature_Room.Is_Used && SensorRange.Temperature_Room.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Temperature_Room);
            if (allAlarm.SensorAlarm.Temperature_PGS.Is_Used && SensorRange.Temperature_PGS.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.Temperature_PGS);
            if (allAlarm.SensorAlarm.O2_Room.Is_Used && SensorRange.O2_Room.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.O2_Room);
            if (allAlarm.SensorAlarm.O2_PGS.Is_Used && SensorRange.O2_PGS.Is_Used) alarmList_Delegate.Add(allAlarm.SensorAlarm.O2_PGS);

            //Общие
            if (globalAlarms.Is_Stop.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Stop);
            if (globalAlarms.Is_NotProcess.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_NotProcess);
            if (globalAlarms.Is_Excess.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Excess);
            if (globalAlarms.Is_Approximation.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Approximation);
            if (globalAlarms.Is_Error.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Error);
            if (globalAlarms.Is_Info.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Info);
            if (globalAlarms.Is_Maintenance.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Maintenance);
            if (globalAlarms.Is_OpenDoor.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_OpenDoor);
            if (globalAlarms.Is_Fire.Is_Used) alarmList_Delegate.Add(globalAlarms.Is_Fire);

            //10626
            if (allAlarm.ASK10626.Input_1_Power.Is_Used) alarmList.Add(allAlarm.ASK10626.Input_1_Power);               //Ввод 1 запитан                                Запитан                                 Авария
            if (allAlarm.ASK10626.Input_1_Used.Is_Used) alarmList.Add(allAlarm.ASK10626.Input_1_Used);                 //Ввод 1 используется                           Не используется                         Используется
            if (allAlarm.ASK10626.Input_2_Power.Is_Used) alarmList.Add(allAlarm.ASK10626.Input_2_Power);               //Ввод 2 запитан                                Запитан                                 Авария
            if (allAlarm.ASK10626.Input_2_Used.Is_Used) alarmList.Add(allAlarm.ASK10626.Input_2_Used);                 //Ввод 2 используется                           Используется                            Не используется
            if (allAlarm.ASK10626.AZ_Konditioner.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_Konditioner);             //Розетка кондиционера запитана                 Запитана                                Авария
            if (allAlarm.ASK10626.AZ_ServerCabinet.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_ServerCabinet);         //Шкаф сервера запитан                          Запитан                                 Авария
            if (allAlarm.ASK10626.ON_FanVentilation.Is_Used) alarmList.Add(allAlarm.ASK10626.ON_FanVentilation);       //Вентилятор вентиляции включен                 Отключен                                Запущен
            if (allAlarm.ASK10626.AZ_Input.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_Input);                         //Вводной автомат ШС включен                    Запитан                                 Авария
            if (allAlarm.ASK10626.AZ_UPS.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_UPS);                             //Автомат ИБП включен                           Запитан                                 Авария
            if (allAlarm.ASK10626.AZ_ServerSwitch.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_ServerSwitch);           //Автомат сервера и ком. оборудования включен   Запитан                                 Авария
            if (allAlarm.ASK10626.AZ_SpeedMeter.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_SpeedMeter);               //Автомат измерю скорости включен               Запитан                                 Авария 
            if (allAlarm.ASK10626.AZ_BP_A1.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_BP_A1);                         //Автомат БП A1 включен                         Запитан                                 Авария 
            if (allAlarm.ASK10626.AZ_BP_B1.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_BP_B1);                         //Автомат БП В2 включен                         Запитан                                 Авария
            if (allAlarm.ASK10626.AZ_AnalyzerBA1.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_AnalyzerBA1);             //Автомат анализатора BA1                       Запитан                                 Авария
            if (allAlarm.ASK10626.AZ_Zond.Is_Used) alarmList.Add(allAlarm.ASK10626.AZ_Zond);                           //Автомат зонда включен                         Запитан                                 Авария
            if (allAlarm.ASK10626.Accident_FlowMeter.Is_Used) alarmList.Add(allAlarm.ASK10626.Accident_FlowMeter);     //Авария расходомера                            В аварии                                В норме                                 
            if (allAlarm.ASK10626.Supply_ZeroGas.Is_Used) alarmList.Add(allAlarm.ASK10626.Supply_ZeroGas);             //Подача нулевого газа в газоанл. (калибр нуля) Подаётся                                Не подаётся
            if (allAlarm.ASK10626.Service_Request_BA1.Is_Used) alarmList.Add(allAlarm.ASK10626.Service_Request_BA1);   //Запрос на обслуживание BA1                    Есть запрос                             Нет запроса
            if (allAlarm.ASK10626.Error_GasAnalyzerBA1.Is_Used) alarmList.Add(allAlarm.ASK10626.Error_GasAnalyzerBA1); //Ошибка газоанализатора BA1                    Есть ошибка                             Нет ошибки
            if (allAlarm.ASK10626.Underheating_Zond.Is_Used) alarmList.Add(allAlarm.ASK10626.Underheating_Zond);       //Недогрев пробоотборного зонда                 Недогрев                                догрев


            //Аварии без делегатов
            //Общие 
            if (globalAlarms.Is_NotConnection.Is_Used) alarmList.Add(globalAlarms.Is_NotConnection);
        }


        //Данные используемых аварий
        public static void UpdateAlarmList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
            
                //Аварии с делегатами
                for (int i=0; i < alarmList_Delegate.Count; i++)
                {
                    alarmList_Delegate[i].New_Value = alarmList_Delegate[i].CheckState();

                    if (alarmList_Delegate[i].Value != alarmList_Delegate[i].New_Value)
                    {
                        globalAlarm_Services.AlarmLogBuiderNew(alarmList_Delegate[i]);
                    }
                }

                //Аварии без делегатов
                for (int i = 0; i < alarmList.Count; i++)
                {
                    if (alarmList[i].Value != alarmList[i].New_Value)
                    {
                        globalAlarm_Services.AlarmLogBuiderNew(alarmList[i]);
                    }
                }
            }
        }



        public static double ScaleRange(double value, Range_Model range)
        {
            double min = 4;  //4
            double max = 20;  //20

            if (value < min)
                return range.Min;

            if (value > max)
                return range.Max;

            return Math.Round(range.Min + (value - min) / (max - min) * (range.Max - range.Min), 3);
        }


        //Симуляция 4-20
        public static double Simulation_4_20(double valueNow, bool O2 = false)
        {
            double min = 3.4;
            double max = 20.6;

            int rndMin = -250;
            int rndMax = 250;

            if(O2_Last < 1)
                O2_Last = 1;

            double newValue;

            if (O2)
                newValue = valueNow + rnd.Next(rndMin, rndMax) / 1000.0;
            else
                newValue = valueNow /** (GlobalStaticSettingsASK.SensorNow.O2_Wet_4_20mA / O2_Last)*/ + rnd.Next(rndMin, rndMax) / 10000.0;

            if (newValue < min)
            {
                newValue = newValue + rnd.Next(0, 100) / 10;
                //if (CO < 0.0)
                //    CO = 0.0;
            }
            if (newValue > max)
            {
                newValue = newValue - rnd.Next(0, 70)/10;
            }

            return Math.Round(newValue, 3);
        }

        public static void RunConvertSernsor()
        {
            //SensorScaledNow = new Sensor_4_20();

            SensorScaledNow.Date = DateTime.Now;

            SensorScaledNow.CO_4_20mA = ScaleRange(SensorNow.CO_4_20mA, SensorRange.CO);
            SensorScaledNow.CO2_4_20mA = ScaleRange(SensorNow.CO2_4_20mA, SensorRange.CO2);
            SensorScaledNow.NO_4_20mA = ScaleRange(SensorNow.NO_4_20mA, SensorRange.NO);
            SensorScaledNow.NO2_4_20mA = ScaleRange(SensorNow.NO2_4_20mA, SensorRange.NO2);
            SensorScaledNow.NOx_4_20mA = ScaleRange(SensorNow.NOx_4_20mA, SensorRange.NOx);
            SensorScaledNow.SO2_4_20mA = ScaleRange(SensorNow.SO2_4_20mA, SensorRange.SO2);
            SensorScaledNow.Dust_4_20mA = ScaleRange(SensorNow.Dust_4_20mA, SensorRange.Dust);
            SensorScaledNow.CH4_4_20mA = ScaleRange(SensorNow.CH4_4_20mA, SensorRange.CH4);
            SensorScaledNow.H2S_4_20mA = ScaleRange(SensorNow.H2S_4_20mA, SensorRange.H2S);
            SensorScaledNow.NH3_4_20mA = ScaleRange(SensorNow.NH3_4_20mA, SensorRange.NH3);
            SensorScaledNow.Rezerv_1_4_20mA = ScaleRange(SensorNow.Rezerv_1_4_20mA, SensorRange.Rezerv_1);
            SensorScaledNow.Rezerv_2_4_20mA = ScaleRange(SensorNow.Rezerv_2_4_20mA, SensorRange.Rezerv_2);
            SensorScaledNow.Rezerv_3_4_20mA = ScaleRange(SensorNow.Rezerv_3_4_20mA, SensorRange.Rezerv_3);
            SensorScaledNow.Rezerv_4_4_20mA = ScaleRange(SensorNow.Rezerv_4_4_20mA, SensorRange.Rezerv_4);
            SensorScaledNow.Rezerv_5_4_20mA = ScaleRange(SensorNow.Rezerv_5_4_20mA, SensorRange.Rezerv_5);

            SensorScaledNow.O2_Wet_4_20mA = ScaleRange(SensorNow.O2_Wet_4_20mA, SensorRange.O2Wet);
            SensorScaledNow.O2_Dry_4_20mA = ScaleRange(SensorNow.O2_Dry_4_20mA, SensorRange.O2Dry);
            SensorScaledNow.H2O_4_20mA = ScaleRange(SensorNow.H2O_4_20mA, SensorRange.H2O);

            SensorScaledNow.Pressure_4_20mA = ScaleRange(SensorNow.Pressure_4_20mA, SensorRange.Pressure);
            SensorScaledNow.Temperature_4_20mA = ScaleRange(SensorNow.Temperature_4_20mA, SensorRange.Temperature);
            SensorScaledNow.Speed_4_20mA = ScaleRange(SensorNow.Speed_4_20mA, SensorRange.Speed);

            SensorScaledNow.Temperature_KIP_4_20mA = ScaleRange(SensorNow.Temperature_KIP_4_20mA, SensorRange.Temperature_KIP);
            SensorScaledNow.Temperature_NOx_4_20mA = ScaleRange(SensorNow.Temperature_NOx_4_20mA, SensorRange.Temperature_NOx);
            SensorScaledNow.Pressure_KIP_4_20mA = ScaleRange(SensorNow.Pressure_KIP_4_20mA, SensorRange.Pressure_KIP);
            SensorScaledNow.Temperature_Point_Dew_4_20mA = ScaleRange(SensorNow.Temperature_Point_Dew_4_20mA, SensorRange.Temperature_Point_Dew);
            SensorScaledNow.Temperature_Room_4_20mA = ScaleRange(SensorNow.Temperature_Room_4_20mA, SensorRange.Temperature_Room);
            SensorScaledNow.Temperature_PGS_4_20mA = ScaleRange(SensorNow.Temperature_PGS_4_20mA, SensorRange.Temperature_PGS);
            SensorScaledNow.O2_Room_4_20mA = ScaleRange(SensorNow.O2_Room_4_20mA, SensorRange.O2_Room);
            SensorScaledNow.O2_PGS_4_20mA = ScaleRange(SensorNow.O2_PGS_4_20mA, SensorRange.O2_PGS);

        }



        public static void Normalization_ConcEmis()
        {
            Calculation_Services calculat = new Calculation_Services();

            //CurrentConcEmis = new Array20M();

            CurrentConcEmis.Date = DateTime.Now;


            CurrentConcEmis.O2_Wet = SensorScaledNow.O2_Wet_4_20mA;
            CurrentConcEmis.O2_Dry = SensorScaledNow.O2_Dry_4_20mA;
            CurrentConcEmis.H2O = SensorScaledNow.H2O_4_20mA;

            CurrentConcEmis.Pressure = SensorScaledNow.Pressure_4_20mA;
            CurrentConcEmis.Temperature = SensorScaledNow.Temperature_4_20mA;
            CurrentConcEmis.Speed = SensorScaledNow.Speed_4_20mA;
            CurrentConcEmis.Flow = 0.0;

            CurrentConcEmis.Temperature_KIP = SensorScaledNow.Temperature_KIP_4_20mA;
            CurrentConcEmis.Temperature_NOx = SensorScaledNow.Temperature_NOx_4_20mA;
            CurrentConcEmis.Pressure_KIP = SensorScaledNow.Pressure_KIP_4_20mA;
            CurrentConcEmis.Temperature_Point_Dew = SensorScaledNow.Temperature_Point_Dew_4_20mA;
            CurrentConcEmis.Temperature_Room = SensorScaledNow.Temperature_Room_4_20mA;
            CurrentConcEmis.Temperature_PGS = SensorScaledNow.Temperature_PGS_4_20mA;
            CurrentConcEmis.O2_Room = SensorScaledNow.O2_Room_4_20mA;
            CurrentConcEmis.O2_PGS = SensorScaledNow.O2_PGS_4_20mA;

            if (SensorRange.CO.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.CO_4_20mA, SensorRange.CO.Is_ppm, 1.14);
                CurrentConcEmis.CO_Conc = cal.C;
                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.CO_Emis = emis_IsStop.CO_Auto;
                    else
                        CurrentConcEmis.CO_Emis = emis_IsStop.CO_Manual;
                }
                else
                    if(!double.IsNaN(cal.M))
                        CurrentConcEmis.CO_Emis = cal.M;
            }

            if (SensorRange.CO2.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.CO2_4_20mA, SensorRange.CO2.Is_ppm, 1.98);
                CurrentConcEmis.CO2_Conc = cal.C;
                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.CO2_Emis = emis_IsStop.CO2_Auto;
                    else
                        CurrentConcEmis.CO2_Emis = emis_IsStop.CO2_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.CO2_Emis = cal.M;
            }

            //Эксклюзивный расчёт для NO, NO2, NOx
            if (SensorRange.NO.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.NO_4_20mA, SensorRange.NO.Is_ppm, 1.34);
                CurrentConcEmis.NOx_Conc = cal.C_NOx;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.NOx_Emis = emis_IsStop.NOx_Auto;
                    else
                        CurrentConcEmis.NOx_Emis = emis_IsStop.NOx_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.NOx_Emis = cal.M;

                CurrentConcEmis.NO_Conc = cal.C_NO;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.NO_Emis = emis_IsStop.NO_Auto;
                    else
                        CurrentConcEmis.NO_Emis = emis_IsStop.NO_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.NO_Emis = cal.M;

                CurrentConcEmis.NO2_Conc = cal.C_NO2;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.NO2_Emis = emis_IsStop.NO2_Auto;
                    else
                        CurrentConcEmis.NO2_Emis = emis_IsStop.NO2_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.NO2_Emis = cal.M;
            }

            if (SensorRange.SO2.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.SO2_4_20mA, SensorRange.SO2.Is_ppm, 2.92);
                CurrentConcEmis.SO2_Conc = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.SO2_Emis = emis_IsStop.SO2_Auto;
                    else
                        CurrentConcEmis.SO2_Emis = emis_IsStop.SO2_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.SO2_Emis = cal.M;
            }

            if (SensorRange.Dust.Is_Used || !(CalculationSetting.TypeDust == TypeDustConc.None)) //!!!Проверить!!!
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Dust_4_20mA, SensorRange.Dust.Is_ppm, 1);
                if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Dust_Conc =  cal.C_Dust;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.Dust_Emis = emis_IsStop.Dust_Auto;
                    else
                        CurrentConcEmis.Dust_Emis = emis_IsStop.Dust_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Dust_Emis = cal.M;
            }

            if (SensorRange.CH4.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.CH4_4_20mA, SensorRange.CH4.Is_ppm, 0.65);
                CurrentConcEmis.CH4_Conc = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.CH4_Emis = emis_IsStop.CH4_Auto;
                    else
                        CurrentConcEmis.CH4_Emis = emis_IsStop.CH4_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.CH4_Emis = cal.M;
            }

            if (SensorRange.H2S.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.H2S_4_20mA, SensorRange.H2S.Is_ppm, 1.36);
                CurrentConcEmis.H2S_Conc = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.H2S_Emis = emis_IsStop.H2S_Auto;
                    else
                        CurrentConcEmis.H2S_Emis = emis_IsStop.H2S_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.H2S_Emis = cal.M;
            }

            if (SensorRange.NH3.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.NH3_4_20mA, SensorRange.NH3.Is_ppm, 1.36);
                CurrentConcEmis.NH3_Conc = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.NH3_Emis = emis_IsStop.NH3_Auto;
                    else
                        CurrentConcEmis.NH3_Emis = emis_IsStop.NH3_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.NH3_Emis = cal.M;
            }

            if (SensorRange.Rezerv_1.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_1_4_20mA, SensorRange.Rezerv_1.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_1 = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.Add_Emis_1 = emis_IsStop.Rezerv_1_Auto;
                    else
                        CurrentConcEmis.Add_Emis_1 = emis_IsStop.Rezerv_1_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Add_Emis_1 = cal.M;
            }

            if (SensorRange.Rezerv_2.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_2_4_20mA, SensorRange.Rezerv_2.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_2 = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.Add_Emis_2 = emis_IsStop.Rezerv_2_Auto;
                    else
                        CurrentConcEmis.Add_Emis_2 = emis_IsStop.Rezerv_2_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Add_Emis_2 = cal.M;
            }

            if (SensorRange.Rezerv_3.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_3_4_20mA, SensorRange.Rezerv_3.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_3 = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.Add_Emis_3 = emis_IsStop.Rezerv_3_Auto;
                    else
                        CurrentConcEmis.Add_Emis_3 = emis_IsStop.Rezerv_3_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Add_Emis_3 = cal.M;
            }

            if (SensorRange.Rezerv_4.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_4_4_20mA, SensorRange.Rezerv_4.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_4 = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.Add_Emis_4 = emis_IsStop.Rezerv_4_Auto;
                    else
                        CurrentConcEmis.Add_Emis_4 = emis_IsStop.Rezerv_4_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Add_Emis_4 = cal.M;
            }

            if (SensorRange.Rezerv_5.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_5_4_20mA, SensorRange.Rezerv_5.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_5 = cal.C;

                if (globalAlarms.Is_Stop.Value)
                {
                    if (emis_IsStop.isAuto)
                        CurrentConcEmis.Add_Emis_5 = emis_IsStop.Rezerv_5_Auto;
                    else
                        CurrentConcEmis.Add_Emis_5 = emis_IsStop.Rezerv_5_Manual;
                }
                else
                    if (!double.IsNaN(cal.M))
                    CurrentConcEmis.Add_Emis_5 = cal.M;
            }
        }



        public static void GetCurrentPDZ()
        {
            if (PDZ.Is_Active == 1)
            {
                PDZ_Current.CO_Conc = PDZ.PDZ_1_CO_Conc;
                PDZ_Current.CO2_Conc = PDZ.PDZ_1_CO2_Conc;
                PDZ_Current.NO_Conc = PDZ.PDZ_1_NO_Conc;
                PDZ_Current.NO2_Conc = PDZ.PDZ_1_NO2_Conc;
                PDZ_Current.NOx_Conc = PDZ.PDZ_1_NOx_Conc;
                PDZ_Current.SO2_Conc = PDZ.PDZ_1_SO2_Conc;
                PDZ_Current.Dust_Conc = PDZ.PDZ_1_Dust_Conc;
                PDZ_Current.CH4_Conc = PDZ.PDZ_1_CH4_Conc;
                PDZ_Current.H2S_Conc = PDZ.PDZ_1_H2S_Conc;
                PDZ_Current.NH3_Conc = PDZ.PDZ_1_NH3_Conc;
                PDZ_Current.Add_Conc_1 = PDZ.PDZ_1_Add_Conc_1;
                PDZ_Current.Add_Conc_2 = PDZ.PDZ_1_Add_Conc_2;
                PDZ_Current.Add_Conc_3 = PDZ.PDZ_1_Add_Conc_3;
                PDZ_Current.Add_Conc_4 = PDZ.PDZ_1_Add_Conc_4;
                PDZ_Current.Add_Conc_5 = PDZ.PDZ_1_Add_Conc_5;

                PDZ_Current.CO_Emis = PDZ.PDZ_1_CO_Emis;
                PDZ_Current.CO2_Emis = PDZ.PDZ_1_CO2_Emis;
                PDZ_Current.NO_Emis = PDZ.PDZ_1_NO_Emis;
                PDZ_Current.NO2_Emis = PDZ.PDZ_1_NO2_Emis;
                PDZ_Current.NOx_Emis = PDZ.PDZ_1_NOx_Emis;
                PDZ_Current.SO2_Emis = PDZ.PDZ_1_SO2_Emis;
                PDZ_Current.CH4_Emis = PDZ.PDZ_1_CH4_Emis;
                PDZ_Current.H2S_Emis = PDZ.PDZ_1_H2S_Emis;
                PDZ_Current.NH3_Emis = PDZ.PDZ_1_NH3_Emis;
                PDZ_Current.Dust_Emis = PDZ.PDZ_1_Dust_Emis;
                PDZ_Current.Add_Emis_1 = PDZ.PDZ_1_Add_Emis_1;
                PDZ_Current.Add_Emis_2 = PDZ.PDZ_1_Add_Emis_2;
                PDZ_Current.Add_Emis_3 = PDZ.PDZ_1_Add_Emis_3;
                PDZ_Current.Add_Emis_4 = PDZ.PDZ_1_Add_Emis_4;
                PDZ_Current.Add_Emis_5 = PDZ.PDZ_1_Add_Emis_5;
            }
            if (PDZ.Is_Active == 2)
            {
                PDZ_Current.CO_Conc = PDZ.PDZ_2_CO_Conc;
                PDZ_Current.CO2_Conc = PDZ.PDZ_2_CO2_Conc;
                PDZ_Current.NO_Conc = PDZ.PDZ_2_NO_Conc;
                PDZ_Current.NO2_Conc = PDZ.PDZ_2_NO2_Conc;
                PDZ_Current.NOx_Conc = PDZ.PDZ_2_NOx_Conc;
                PDZ_Current.SO2_Conc = PDZ.PDZ_2_SO2_Conc;
                PDZ_Current.Dust_Conc = PDZ.PDZ_2_Dust_Conc;
                PDZ_Current.CH4_Conc = PDZ.PDZ_2_CH4_Conc;
                PDZ_Current.H2S_Conc = PDZ.PDZ_2_H2S_Conc;
                PDZ_Current.NH3_Conc = PDZ.PDZ_2_NH3_Conc;
                PDZ_Current.Add_Conc_1 = PDZ.PDZ_2_Add_Conc_1;
                PDZ_Current.Add_Conc_2 = PDZ.PDZ_2_Add_Conc_2;
                PDZ_Current.Add_Conc_3 = PDZ.PDZ_2_Add_Conc_3;
                PDZ_Current.Add_Conc_4 = PDZ.PDZ_2_Add_Conc_4;
                PDZ_Current.Add_Conc_5 = PDZ.PDZ_2_Add_Conc_5;

                PDZ_Current.CO_Emis = PDZ.PDZ_2_CO_Emis;
                PDZ_Current.CO2_Emis = PDZ.PDZ_2_CO2_Emis;
                PDZ_Current.NO_Emis = PDZ.PDZ_2_NO_Emis;
                PDZ_Current.NO2_Emis = PDZ.PDZ_2_NO2_Emis;
                PDZ_Current.NOx_Emis = PDZ.PDZ_2_NOx_Emis;
                PDZ_Current.SO2_Emis = PDZ.PDZ_2_SO2_Emis;
                PDZ_Current.CH4_Emis = PDZ.PDZ_2_CH4_Emis;
                PDZ_Current.H2S_Emis = PDZ.PDZ_2_H2S_Emis;
                PDZ_Current.NH3_Emis = PDZ.PDZ_2_NH3_Emis;
                PDZ_Current.Dust_Emis = PDZ.PDZ_2_Dust_Emis;
                PDZ_Current.Add_Emis_1 = PDZ.PDZ_2_Add_Emis_1;
                PDZ_Current.Add_Emis_2 = PDZ.PDZ_2_Add_Emis_2;
                PDZ_Current.Add_Emis_3 = PDZ.PDZ_2_Add_Emis_3;
                PDZ_Current.Add_Emis_4 = PDZ.PDZ_2_Add_Emis_4;
                PDZ_Current.Add_Emis_5 = PDZ.PDZ_2_Add_Emis_5;
            }
            if (PDZ.Is_Active == 3)
            {
                PDZ_Current.CO_Conc = PDZ.PDZ_3_CO_Conc;
                PDZ_Current.CO2_Conc = PDZ.PDZ_3_CO2_Conc;
                PDZ_Current.NO_Conc = PDZ.PDZ_3_NO_Conc;
                PDZ_Current.NO2_Conc = PDZ.PDZ_3_NO2_Conc;
                PDZ_Current.NOx_Conc = PDZ.PDZ_3_NOx_Conc;
                PDZ_Current.SO2_Conc = PDZ.PDZ_3_SO2_Conc;
                PDZ_Current.Dust_Conc = PDZ.PDZ_3_Dust_Conc;
                PDZ_Current.CH4_Conc = PDZ.PDZ_3_CH4_Conc;
                PDZ_Current.H2S_Conc = PDZ.PDZ_3_H2S_Conc;
                PDZ_Current.NH3_Conc = PDZ.PDZ_3_NH3_Conc;
                PDZ_Current.Add_Conc_1 = PDZ.PDZ_3_Add_Conc_1;
                PDZ_Current.Add_Conc_2 = PDZ.PDZ_3_Add_Conc_2;
                PDZ_Current.Add_Conc_3 = PDZ.PDZ_3_Add_Conc_3;
                PDZ_Current.Add_Conc_4 = PDZ.PDZ_3_Add_Conc_4;
                PDZ_Current.Add_Conc_5 = PDZ.PDZ_3_Add_Conc_5;

                PDZ_Current.CO_Emis = PDZ.PDZ_3_CO_Emis;
                PDZ_Current.CO2_Emis = PDZ.PDZ_3_CO2_Emis;
                PDZ_Current.NO_Emis = PDZ.PDZ_3_NO_Emis;
                PDZ_Current.NO2_Emis = PDZ.PDZ_3_NO2_Emis;
                PDZ_Current.NOx_Emis = PDZ.PDZ_3_NOx_Emis;
                PDZ_Current.SO2_Emis = PDZ.PDZ_3_SO2_Emis;
                PDZ_Current.CH4_Emis = PDZ.PDZ_3_CH4_Emis;
                PDZ_Current.H2S_Emis = PDZ.PDZ_3_H2S_Emis;
                PDZ_Current.NH3_Emis = PDZ.PDZ_3_NH3_Emis;
                PDZ_Current.Dust_Emis = PDZ.PDZ_3_Dust_Emis;
                PDZ_Current.Add_Emis_1 = PDZ.PDZ_3_Add_Emis_1;
                PDZ_Current.Add_Emis_2 = PDZ.PDZ_3_Add_Emis_2;
                PDZ_Current.Add_Emis_3 = PDZ.PDZ_3_Add_Emis_3;
                PDZ_Current.Add_Emis_4 = PDZ.PDZ_3_Add_Emis_4;
                PDZ_Current.Add_Emis_5 = PDZ.PDZ_3_Add_Emis_5;
            }
            GetCurrentStringPDZ();
        }



        public static void GetCurrentStringPDZ()
        {
            if (PDZ_Current.CO_Conc != 0.0 && PDZ_Current.CO_Conc != 9999999.0)
                PDZ_Current_String.CO_Conc = PDZ_Current.CO_Conc.ToString();
            else
                PDZ_Current_String.CO_Conc = "-/-";

            if (PDZ_Current.CO2_Conc != 0.0 && PDZ_Current.CO2_Conc != 9999999.0)
                PDZ_Current_String.CO2_Conc = PDZ_Current.CO2_Conc.ToString();
            else
                PDZ_Current_String.CO2_Conc = "-/-";

            if (PDZ_Current.NO_Conc != 0.0 && PDZ_Current.NO_Conc != 9999999.0)
                PDZ_Current_String.NO_Conc = PDZ_Current.NO_Conc.ToString();
            else
                PDZ_Current_String.NO_Conc = "-/-";

            if (PDZ_Current.NO2_Conc != 0.0 && PDZ_Current.NO2_Conc != 9999999.0)
                PDZ_Current_String.NO2_Conc = PDZ_Current.NO2_Conc.ToString();
            else
                PDZ_Current_String.NO2_Conc = "-/-";

            if (PDZ_Current.NOx_Conc != 0.0 && PDZ_Current.NOx_Conc != 9999999.0)
                PDZ_Current_String.NOx_Conc = PDZ_Current.NOx_Conc.ToString();
            else
                PDZ_Current_String.NOx_Conc = "-/-";

            if (PDZ_Current.SO2_Conc != 0.0 && PDZ_Current.SO2_Conc != 9999999.0)
                PDZ_Current_String.SO2_Conc = PDZ_Current.SO2_Conc.ToString();
            else
                PDZ_Current_String.SO2_Conc = "-/-";

            if (PDZ_Current.Dust_Conc != 0.0 && PDZ_Current.Dust_Conc != 9999999.0)
                PDZ_Current_String.Dust_Conc = PDZ_Current.Dust_Conc.ToString();
            else
                PDZ_Current_String.Dust_Conc = "-/-";

            if (PDZ_Current.CH4_Conc != 0.0 && PDZ_Current.CH4_Conc != 9999999.0)
                PDZ_Current_String.CH4_Conc = PDZ_Current.CH4_Conc.ToString();
            else
                PDZ_Current_String.CH4_Conc = "-/-";

            if (PDZ_Current.H2S_Conc != 0.0 && PDZ_Current.H2S_Conc != 9999999.0)
                PDZ_Current_String.H2S_Conc = PDZ_Current.H2S_Conc.ToString();
            else
                PDZ_Current_String.H2S_Conc = "-/-";

            if (PDZ_Current.NH3_Conc != 0.0 && PDZ_Current.NH3_Conc != 9999999.0)
                PDZ_Current_String.NH3_Conc = PDZ_Current.NH3_Conc.ToString();
            else
                PDZ_Current_String.NH3_Conc = "-/-";

            if (PDZ_Current.Add_Conc_1 != 0.0 && PDZ_Current.Add_Conc_1 != 9999999.0)
                PDZ_Current_String.Add_Conc_1 = PDZ_Current.Add_Conc_1.ToString();
            else
                PDZ_Current_String.Add_Conc_1 = "-/-";

            if (PDZ_Current.Add_Conc_2 != 0.0 && PDZ_Current.Add_Conc_2 != 9999999.0)
                PDZ_Current_String.Add_Conc_2 = PDZ_Current.Add_Conc_2.ToString();
            else
                PDZ_Current_String.Add_Conc_2 = "-/-";

            if (PDZ_Current.Add_Conc_3 != 0.0 && PDZ_Current.Add_Conc_3 != 9999999.0)
                PDZ_Current_String.Add_Conc_3 = PDZ_Current.Add_Conc_3.ToString();
            else
                PDZ_Current_String.Add_Conc_3 = "-/-";

            if (PDZ_Current.Add_Conc_4 != 0.0 && PDZ_Current.Add_Conc_4 != 9999999.0)
                PDZ_Current_String.Add_Conc_4 = PDZ_Current.Add_Conc_4.ToString();
            else
                PDZ_Current_String.Add_Conc_4 = "-/-";

            if (PDZ_Current.Add_Conc_5 != 0.0 && PDZ_Current.Add_Conc_5 != 9999999.0)
                PDZ_Current_String.Add_Conc_5 = PDZ_Current.Add_Conc_5.ToString();
            else
                PDZ_Current_String.Add_Conc_5 = "-/-";


            if (PDZ_Current.CO_Emis != 0.0 && PDZ_Current.CO_Emis != 9999999.0)
                PDZ_Current_String.CO_Emis = PDZ_Current.CO_Emis.ToString();
            else
                PDZ_Current_String.CO_Emis = "-/-";

            if (PDZ_Current.CO2_Emis != 0.0 && PDZ_Current.CO2_Emis != 9999999.0)
                PDZ_Current_String.CO2_Emis = PDZ_Current.CO2_Emis.ToString();
            else
                PDZ_Current_String.CO2_Emis = "-/-";

            if (PDZ_Current.NO_Emis != 0.0 && PDZ_Current.NO_Emis != 9999999.0)
                PDZ_Current_String.NO_Emis = PDZ_Current.NO_Emis.ToString();
            else
                PDZ_Current_String.NO_Emis = "-/-";

            if (PDZ_Current.NO2_Emis != 0.0 && PDZ_Current.NO2_Emis != 9999999.0)
                PDZ_Current_String.NO2_Emis = PDZ_Current.NO2_Emis.ToString();
            else
                PDZ_Current_String.NO2_Emis = "-/-";

            if (PDZ_Current.NOx_Emis != 0.0 && PDZ_Current.NOx_Emis != 9999999.0)
                PDZ_Current_String.NOx_Emis = PDZ_Current.NOx_Emis.ToString();
            else
                PDZ_Current_String.NOx_Emis = "-/-";

            if (PDZ_Current.SO2_Emis != 0.0 && PDZ_Current.SO2_Emis != 9999999.0)
                PDZ_Current_String.SO2_Emis = PDZ_Current.SO2_Emis.ToString();
            else
                PDZ_Current_String.SO2_Emis = "-/-";

            if (PDZ_Current.CH4_Emis != 0.0 && PDZ_Current.CH4_Emis != 9999999.0)
                PDZ_Current_String.CH4_Emis = PDZ_Current.CH4_Emis.ToString();
            else
                PDZ_Current_String.CH4_Emis = "-/-";

            if (PDZ_Current.H2S_Emis != 0.0 && PDZ_Current.H2S_Emis != 9999999.0)
                PDZ_Current_String.H2S_Emis = PDZ_Current.H2S_Emis.ToString();
            else
                PDZ_Current_String.H2S_Emis = "-/-";

            if (PDZ_Current.NH3_Emis != 0.0 && PDZ_Current.NH3_Emis != 9999999.0)
                PDZ_Current_String.NH3_Emis = PDZ_Current.NH3_Emis.ToString();
            else
                PDZ_Current_String.NH3_Emis = "-/-";

            if (PDZ_Current.Dust_Emis != 0.0 && PDZ_Current.Dust_Emis != 9999999.0)
                PDZ_Current_String.Dust_Emis = PDZ_Current.Dust_Emis.ToString();
            else
                PDZ_Current_String.Dust_Emis = "-/-";

            if (PDZ_Current.Add_Emis_1 != 0.0 && PDZ_Current.Add_Emis_1 != 9999999.0)
                PDZ_Current_String.Add_Emis_1 = PDZ_Current.Add_Emis_1.ToString();
            else
                PDZ_Current_String.Add_Emis_1 = "-/-";

            if (PDZ_Current.Add_Emis_2 != 0.0 && PDZ_Current.Add_Emis_2 != 9999999.0)
                PDZ_Current_String.Add_Emis_2 = PDZ_Current.Add_Emis_2.ToString();
            else
                PDZ_Current_String.Add_Emis_2 = "-/-";

            if (PDZ_Current.Add_Emis_3 != 0.0 && PDZ_Current.Add_Emis_3 != 9999999.0)
                PDZ_Current_String.Add_Emis_3 = PDZ_Current.Add_Emis_3.ToString();
            else
                PDZ_Current_String.Add_Emis_3 = "-/-";

            if (PDZ_Current.Add_Emis_4 != 0.0 && PDZ_Current.Add_Emis_4 != 9999999.0)
                PDZ_Current_String.Add_Emis_4 = PDZ_Current.Add_Emis_4.ToString();
            else
                PDZ_Current_String.Add_Emis_4 = "-/-";

            if (PDZ_Current.Add_Emis_5 != 0.0 && PDZ_Current.Add_Emis_5 != 9999999.0)
                PDZ_Current_String.Add_Emis_5 = PDZ_Current.Add_Emis_5.ToString();
            else
                PDZ_Current_String.Add_Emis_5 = "-/-";
        }


        //Обработка массива бул в стейт ворд
        static byte[] ToByteArray(bool[] input)
        {
            if (input.Length % 8 != 0)
            {
                throw new ArgumentException("input");
            }
            byte[] ret = new byte[input.Length / 8];
            for (int i = 0; i < input.Length; i += 8)
            {
                int value = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (input[i + j])
                    {
                        value += 1 << (7 - j);
                    }
                }
                ret[i / 8] = (byte)value;
            }
            return ret;
        }

    }
}
