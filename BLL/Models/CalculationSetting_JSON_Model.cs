using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class CalculationSetting_JSON_Model
    {
        public bool Is_Normalization { get; set; } = false;                     //Приводить ли концентрации к температуре и давлению
        public bool Is_NotO2 { get; set; } = false;                             //О2 не используется в вычислениях, коэф. избытка воздуха равен 1 тем самым O₂ не учитывается в расчётах.
        public bool Is_NotFlowMeter { get; set; } = false;                      //Будет отсутствовать расходометр ДГ (true - расчёт расхода ДГ через расходмер топлива - другая формула)
        public bool Is_Alpha_O2Wet { get; set; } = false;                       //По дефолту альфа считается через сухой кислород, но возможен вариант и через влажный

        public TypeNOxConc TypeNOx { get; set; } = TypeNOxConc.Convertor;       //Отвечает за то как будут расчитываться концентрации NOx, NO2, NO
        public TypeH2OConc TypeH2O { get; set; } = TypeH2OConc.O2Wet_O2Dry;     //Использовать ли H2O в вычислениях
        public TypeDustConc TypeDust { get; set; } = TypeDustConc.None;         //Отвеччает за то какой пылемер будет использоваться 

        public double PipeDiameter { get; set; } = 1;                           //Радиус трубы
        public double Koeff_H2O { get; set; } = 1;                              //Коэф H2O если нет датчика и задётся не отношением кислородов влажный/сухой
        public double Koeff_O2_Normalization { get; set; } = 1.4;               //Коэфициет к которому просисходит приведение по кислороду

        public double Dust_k1 { get; set; } = 0;                                //Коэфициент к1 для расчёта пыли через светопроницаемость
        public double Dust_k2 { get; set; } = 1;                                //Коэфициент к2 для расчёта пыли через светопроницаемость
        public double Dust_k3 { get; set; } = 0;                                //Коэфициент к3 для расчёта пыли через светопроницаемость
    }



    public enum TypeNOxConc
    {
        OnlyNO = 0,         //Считаем только через NO 
        NO_and_NO2 = 1,     //Считаем через NO + NO2
        Convertor = 2       //NO после конвертора -> NOx+
    }

    public enum TypeH2OConc
    {
        Sensor = 0,         //Исполльзуется датчик
        Koeff_H20 = 1,      //Используется коэффициент
        O2Wet_O2Dry = 2     //Используется отношение влажный/сухой
    }

    public enum TypeDustConc
    {
        None = 0,           //Пылемер отсутствует, пыль не рассчитывается.
        DustHunter = 1,     //Используется пылемер DustHunter
        CodelACEMT = 2,     //Используется пылемер Codel
        Ecomer = 3          //Используется пылемер Ecomer
    }
}


