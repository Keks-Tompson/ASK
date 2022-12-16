using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class ASK10626_Alarms
    {
        public bool Is_Used { get; set; } = true;                                   //Испоьзуются ли аварии     
                                                                                                                                    //False                            /    True
        public Alarm_Model Input_1_Power { get; set; } = new Alarm_Model();         //Ввод 1 запитан                                Запитан                                 Авария
        public Alarm_Model Input_1_Used { get; set; } = new Alarm_Model();          //Ввод 1 используется                           Не используется                         Используется
        public Alarm_Model Input_2_Power { get; set; } = new Alarm_Model();         //Ввод 2 запитан                                Запитан                                 Авария
        public Alarm_Model Input_2_Used { get; set; } = new Alarm_Model();          //Ввод 2 используется                           Используется                            Не используется
        public Alarm_Model AZ_Konditioner { get; set; } = new Alarm_Model();        //Розетка кондиционера запитана                 Запитана                                Авария
        public Alarm_Model AZ_ServerCabinet{ get; set; } = new Alarm_Model();       //Шкаф сервера запитан                          Запитан                                 Авария
        public Alarm_Model ON_FanVentilation { get; set; } = new Alarm_Model();     //Вентилятор вентиляции включен                 Отключен                                Запущен
        public Alarm_Model AZ_Input { get; set; } = new Alarm_Model();              //Вводной автомат ШС включен                    Запитан                                 Авария
        public Alarm_Model AZ_UPS{ get; set; } = new Alarm_Model();                 //Автомат ИБП включен                           Запитан                                 Авария
        public Alarm_Model AZ_ServerSwitch { get; set; } = new Alarm_Model();       //Автомат сервера и ком. оборудования включен   Запитан                                 Авария
        public Alarm_Model AZ_SpeedMeter { get; set; } = new Alarm_Model();         //Автомат измерю скорости включен               Запитан                                 Авария 
        public Alarm_Model AZ_BP_A1 { get; set; } = new Alarm_Model();              //Автомат БП A1 включен                         Запитан                                 Авария 
        public Alarm_Model AZ_BP_B1 { get; set; } = new Alarm_Model();              //Автомат БП В2 включен                         Запитан                                 Авария
        public Alarm_Model AZ_AnalyzerBA1  { get; set; } = new Alarm_Model();       //Автомат анализатора BA1                       Запитан                                 Авария

        public Alarm_Model AZ_Zond { get; set; } = new Alarm_Model();               //Автомат зонда включен                         Запитан                                 Авария
        public Alarm_Model Accident_FlowMeter { get; set; } = new Alarm_Model();    //Авария расходомера                            В аварии                                В норме                                 
        public Alarm_Model Supply_ZeroGas { get; set; } = new Alarm_Model();        //Подача нулевого газа в газоанл. (калибр нуля) Подаётся                                Не подаётся
        public Alarm_Model Service_Request_BA1 { get; set; } = new Alarm_Model();   //Запрос на обслуживание BA1                    Есть запрос                             Нет запроса
        public Alarm_Model Error_GasAnalyzerBA1 { get; set; } = new Alarm_Model();  //Ошибка газоанализатора BA1                    Есть ошибка                             Нет ошибки
        public Alarm_Model Underheating_Zond { get; set; } = new Alarm_Model();     //Недогрев пробоотборного зонда                 Недогрев                                догрев
      


        //ID расшифровки аварии в БД/ссылка на текст в БД
        public ASK10626_Alarms()
        {
            Input_1_Power.ID = 65;
            Input_1_Power.Is_Used = true;
            Input_1_Power.Is_Critical = true;

            Input_1_Used.ID = 66;
            Input_1_Used.Is_Used = true;
            Input_1_Used.Is_Critical = false;

            Input_2_Power.ID = 67;
            Input_2_Power.Is_Used = true;
            Input_2_Power.Is_Critical = true;

            Input_2_Used.ID = 68;
            Input_2_Used.Is_Used = true;
            Input_2_Used.Is_Critical = false;

            AZ_Konditioner.ID = 69;
            AZ_Konditioner.Is_Used = true;
            AZ_Konditioner.Is_Critical = true;

            AZ_ServerCabinet.ID = 70;
            AZ_ServerCabinet.Is_Used = true;
            AZ_ServerCabinet.Is_Critical = true;

            ON_FanVentilation.ID = 71;
            ON_FanVentilation.Is_Used = true;
            ON_FanVentilation.Is_Critical = false;

            AZ_Input.ID = 72;
            AZ_Input.Is_Used = true;
            AZ_Input.Is_Critical = true;

            AZ_UPS.ID = 73;
            AZ_UPS.Is_Used = true;
            AZ_UPS.Is_Critical = true;

            AZ_ServerSwitch.ID = 74;
            AZ_ServerSwitch.Is_Used = true;
            AZ_ServerSwitch.Is_Critical = true;

            AZ_SpeedMeter.ID = 75;
            AZ_SpeedMeter.Is_Used = true;
            AZ_SpeedMeter.Is_Critical = true;

            AZ_BP_A1.ID = 76;
            AZ_BP_A1.Is_Used = true;
            AZ_BP_A1.Is_Critical = true;

            AZ_BP_B1.ID = 77;
            AZ_BP_B1.Is_Used = true;
            AZ_BP_B1.Is_Critical = true;

            AZ_AnalyzerBA1.ID = 78;
            AZ_AnalyzerBA1.Is_Used = true;
            AZ_AnalyzerBA1.Is_Critical = true;
            


            AZ_Zond.ID = 79;
            AZ_Zond.Is_Used = true;
            AZ_Zond.Is_Critical = true;

            Accident_FlowMeter.ID = 80;
            Accident_FlowMeter.Is_Used = true;
            Accident_FlowMeter.Is_Critical = true;

            Supply_ZeroGas.ID = 81;
            Supply_ZeroGas.Is_Used = true;
            Supply_ZeroGas.Is_Critical = false;

            Service_Request_BA1.ID = 82;
            Service_Request_BA1.Is_Used = true;
            Service_Request_BA1.Is_Critical = false;

            Error_GasAnalyzerBA1.ID = 83;
            Error_GasAnalyzerBA1.Is_Used = true;
            Error_GasAnalyzerBA1.Is_Critical = true;

            Underheating_Zond.ID = 84;
            Underheating_Zond.Is_Used = true;
            Underheating_Zond.Is_Critical = true;
        }
    }
}
