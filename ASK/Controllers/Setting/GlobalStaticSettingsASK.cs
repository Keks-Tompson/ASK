using ASK.Controllers.Add;
using ASK.Data;
using ASK.Data.Services;
using ASK.Models;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASK.Controllers.Setting
{
    public static class GlobalStaticSettingsASK
    {
        public static SettingOptionsJSON SettingOptions { get; set; }
        public static VisibilityOptions20MJSON VisibilityOptions20M { get; set; }
        public static PdzJSON PDZ { get; set; }


        



        //Цвета 
        public static Color ColorExcess { get; set; } = Color.FromArgb(247, 213, 213); //Цвет превышения
        //public static Color ColorExcess { get; set; } = Color.FromArgb(0, 14, 14, 3); //Цвет превышения

        public static Color ColorHeader1 { get; set; } = Color.FromArgb(255, 230, 168); //Цвет заголовка 1
        public static Color ColorHeader2 { get; set; } = Color.FromArgb(182, 242, 250); //Цвет заголовка 2




        public static void SaveSettingOptionsJSON()
        {
            StreamWriter file = File.CreateText("SettingOptionsJSON.json");
            file.WriteLine(JsonSerializer.Serialize(SettingOptions, typeof(SettingOptionsJSON)));
            file.Close();
        }

        public static void SaveVisibilityOptions20MJSON()
        {
            StreamWriter file = File.CreateText("VisibilityOptions20MJSON.json");
            file.WriteLine(JsonSerializer.Serialize(VisibilityOptions20M, typeof(VisibilityOptions20MJSON)));
            file.Close();
        }

        public static void SavePdz_JSON()
        {
            StreamWriter file = File.CreateText("PdzJSON.json");
            file.WriteLine(JsonSerializer.Serialize(PDZ, typeof(PdzJSON)));
            file.Close();
        }




        static GlobalStaticSettingsASK()
        {
            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SettingOptionsJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SettingOptionsJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SettingOptionsJSON.json");
                SettingOptions = JsonSerializer.Deserialize<SettingOptionsJSON>(data);
            }
            else //не существует
            {
                SettingOptions = new SettingOptionsJSON();

                SaveSettingOptionsJSON();
            }






            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  VisibilityOptions20MJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("VisibilityOptions20MJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("VisibilityOptions20MJSON.json");
                VisibilityOptions20M = JsonSerializer.Deserialize<VisibilityOptions20MJSON>(data);
            }
            else //не существует
            {
                VisibilityOptions20M = new VisibilityOptions20MJSON();

                SaveVisibilityOptions20MJSON();
            }


            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  PdzJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("PdzJSON.json"))
            {
                string data = File.ReadAllText("PdzJSON.json");
                PDZ = JsonSerializer.Deserialize<PdzJSON>(data);
            }
            else
            {
                PDZ = new PdzJSON();
                SavePdz_JSON();

            }

        
           
        }

        public static async Task Add_20M_Async()
        {
            await Task.Run(() => Add_20M());
        }

        public static void Add_20M()
        {
            Random random = new Random();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                AVG_20_MINUTES_Service avg_20_M_Service = new AVG_20_MINUTES_Service(db);
                avg_20_M_Service.Create_AVG_20_MINUTES(new AVG_20_MINUTES()
                {
                    Date = GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M, // 20.06.2022 18:30:25 - формат

                    //Временно 
                   


                    Conc_CO = random.Next(0, 700),
                    Conc_CO2 = 0.0,
                    Conc_NO = 0.0,
                    Conc_NO2 = 0.0,
                    Conc_NOx = random.Next(0, 300),
                    Conc_SO2 = random.Next(0, 14),
                    Conc_Dust = random.Next(0, 700),
                    Conc_CH4 = 0.0,
                    Conc_H2S = 0.0,
                    Conc_D1 = 0.0,
                    Conc_D2 = 0.0,
                    Conc_D3 = 0.0,
                    Conc_D4 = 0.0,
                    Conc_D5 = 0.0,

                    Emis_CO = 1.19,
                    Emis_CO2 = 0.0,
                    Emis_NO = 0.054,
                    Emis_NO2 = random.Next(0, 2),
                    Emis_NOx = random.Next(0, 3),
                    Emis_SO2 = random.Next(0, 1),
                    Emis_CH4 = 0.0,
                    Emis_H2S = 0.0,
                    Emis_Dust = random.Next(0, 1),
                    Emis_D1 = 0.0,
                    Emis_D2 = 0.0,
                    Emis_D3 = 0.0,
                    Emis_D4 = 0.0,
                    Emis_D5 = 0.0,

                    O2_Wet = random.Next(16, 21),
                    O2_Dry = random.Next(15, 21),
                    H2O = 0.0,

                    Pressure = random.Next(90, 99),
                    Temperature = random.Next(100, 199),
                    Speed = random.Next(1, 23),
                    Flow = random.Next(1, 34),
                    Temperature_KIP = 0.0,
                    Temperature_NOx = 0.0,

                    Mode_ASK = random.Next(0, 2),
                    PDZ_Fuel = random.Next(0, 2)
                });
                GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M = GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M.AddMinutes(20);
            }


        }
    }



   


}
