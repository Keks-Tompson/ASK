using ASK.BLL.Helper.Setting;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Services
{
    public class Calculation_Services : ICalculation
    {

        public Calculation_Model Count(CalculationSetting_JSON_Model calculationSetting, double sensorNow, bool is_ppm, double r_ppm = 1)
        {
            Calculation_Model calculation = new Calculation_Model();
            double С_k = 1;
            double Dust_k = 1;

            //Нет выбросов
            if (GlobalStaticSettingsASK.globalAlarms.Is_NotProcess.Value)
            {
                //нужно подставт значения при отсутствии тех процесса (кислород больше 20%)
                calculation.M = 0;
                calculation.C = 0;
            }
            else
            {
                //Просой системы
                if (GlobalStaticSettingsASK.globalAlarms.Is_Stop.Value)
                {
                    //нужно подставить значения при простое
                    calculation.M = 1.1;
                    calculation.C = 1.1;
                }
                else
                {
                    //Расчитываем NOx? (+ NO, + NO2)


                    //Кислород не участвует в расчётах?
                    if (calculationSetting.Is_NotO2)
                    {
                        calculation.a_cast = 1.0;
                        calculation.a = 1.0;
                    }
                    else
                    {
                        //Расчёт альфы через влажный кислород?
                        if (calculationSetting.Is_Alpha_O2Wet)
                        {
                            calculation.a_O2 = calculation.O2_Wet;
                        }
                        else
                        {
                            calculation.a_O2 = calculation.O2_Dry;
                        }


                        //Если вдруг кислород больше или равен 21 - делим на ноль, исключаем ошибку и если больше 20 альфа получается заоблачная
                        if (calculation.a_O2 > 20.0)
                        {
                            calculation.a_O2 = 20.0;
                        }

                        calculation.a = Math.Round(21 / (21 - calculation.a_O2), 3);
                    }

                    //Расходомер ДГ отсутствует? Рассчитываем через расход топлива (в данном случа природный газ)
                    if (calculationSetting.Is_NotFlowMeter)
                    {
                        calculation.B = 0.0; //нужно предусмотреть датчик рассхода топлива, пока = 0!!!!!!!!!!!!!!!!!!!!!!
                        calculation.Bs = calculation.B;

                        calculation.Vdry = Math.Round(calculation.Bs * calculation.Vdry1_4, 3);

                    }
                    else
                    {
                        switch (calculationSetting.TypeH2O)
                        {
                            case TypeH2OConc.Sensor:
                                calculation.k = 1 - Math.Round((calculation.H2O / 100), 3);
                                break;

                            case TypeH2OConc.O2Wet_O2Dry:
                                calculation.k = Math.Round(calculation.O2_Wet / calculation.O2_Dry, 3);
                                break;

                            case TypeH2OConc.Koeff_H20:
                                calculation.k = Math.Round(calculationSetting.Koeff_H2O, 3);
                                break;
                        }

                        calculation.S_section = Math.Round(((calculation.d * calculation.d) / 4) * calculation.pi, 3);

                        calculation.V = Math.Round(calculation.Sfg * calculation.S_section, 3);

                        calculation.Pb_Pi = Math.Round(calculation.Pa, 3);
                        calculation.Vdry = Math.Round((calculation.V * calculation.a_cast * calculation.k * 273 * calculation.Pb_Pi) / (calculation.a * (273 + calculation.tg) * 101.3), 3);
                    }

                    //Приводить концентрации к н.у.
                    if (calculationSetting.Is_Normalization)
                    {
                        calculation.Normalization = Math.Round(((273 + calculation.tg) * 101.3) / (273 * calculation.Pa), 3);
                    }
                    else
                    {
                        calculation.Normalization = 1.0;
                    }

                    // это ppm? (Внимание! Считаем что ppm при н.у.! Если нет - рассчёт произведён не верно)
                    if (is_ppm)
                    {
                        calculation.r = r_ppm;
                    }
                    else
                    {
                        calculation.r = 1.0;
                    }

                    //Рассчитываем NOx?
                    //if (is_NOx)
                    //{
                        //calculation.Cizm = sensorNow;

                        switch (calculationSetting.TypeNOx)
                        {
                            case TypeNOxConc.OnlyNO:
                                if (GlobalStaticSettingsASK.SensorRange.NO.Is_ppm)
                                {
                                    calculation.C_NOx = Math.Round(calculation.Izm_NO * 1.05 * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);
                                    calculation.C_NO = Math.Round(calculation.Izm_NO * calculation.rNO * (calculation.a / calculation.a_cast), 3);

                                    calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                    calculation.M_NO = Math.Round(calculation.C_NO * calculation.Vdry * 0.001, 3);
                                    calculation.M_NO2 = Math.Round(calculation.M_NOx * 0.8, 3);
                                }
                                else
                                {
                                    calculation.C_NOx = Math.Round((calculation.Izm_NO / calculation.rNO) * 1.05 * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);
                                    calculation.C_NO = Math.Round(calculation.Izm_NO * (calculation.a / calculation.a_cast), 3);

                                    calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                    calculation.M_NO = Math.Round(calculation.C_NO * calculation.Vdry * 0.001, 3);
                                    calculation.M_NO2 = Math.Round(calculation.M_NOx * 0.8, 3);
                                }
                                break;

                            case TypeNOxConc.NO_and_NO2:

                                if (GlobalStaticSettingsASK.SensorRange.NO.Is_ppm)
                                {
                                    if(GlobalStaticSettingsASK.SensorRange.NO2.Is_ppm)
                                    {
                                        calculation.C_NOx = Math.Round((calculation.Izm_NO + calculation.Izm_NO2) * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO = Math.Round(calculation.Izm_NO * calculation.rNO * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO2 = Math.Round(calculation.Izm_NO2 * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);

                                        calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO = Math.Round(calculation.C_NO * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO2 = Math.Round(calculation.C_NO2 * calculation.Vdry * 0.001, 3);
                                    }
                                    else
                                    {
                                        calculation.C_NOx = Math.Round((calculation.Izm_NO + (calculation.Izm_NO2 / calculation.rNO2)) * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO = Math.Round(calculation.Izm_NO * calculation.rNO * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO2 = Math.Round(calculation.Izm_NO2 * (calculation.a / calculation.a_cast), 3);

                                        calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO = Math.Round(calculation.C_NO * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO2 = Math.Round(calculation.C_NO2 * calculation.Vdry * 0.001, 3);
                                    }
                                }
                                else
                                {
                                    if(GlobalStaticSettingsASK.SensorRange.NO2.Is_ppm)
                                    {
                                        calculation.C_NOx = Math.Round(((calculation.Izm_NO / calculation.rNO) + calculation.Izm_NO2) * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO = Math.Round(calculation.Izm_NO * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO2 = Math.Round(calculation.Izm_NO2 * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);

                                        calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO = Math.Round(calculation.C_NO * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO2 = Math.Round(calculation.C_NO2 * calculation.Vdry * 0.001, 3);
                                    }
                                    else
                                    {
                                        calculation.C_NOx = Math.Round(((calculation.Izm_NO / calculation.rNO) + (calculation.Izm_NO2 / calculation.rNO2)) * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO = Math.Round(calculation.Izm_NO * (calculation.a / calculation.a_cast), 3);
                                        calculation.C_NO2 = Math.Round(calculation.Izm_NO2 * (calculation.a / calculation.a_cast), 3);

                                        calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO = Math.Round(calculation.C_NO * calculation.Vdry * 0.001, 3);
                                        calculation.M_NO2 = Math.Round(calculation.C_NO2 * calculation.Vdry * 0.001, 3);
                                    }
                                }
                                break;

                            case TypeNOxConc.Convertor:
                                if (GlobalStaticSettingsASK.SensorRange.NO.Is_ppm)
                                {
                                    calculation.C_NOx = Math.Round(calculation.Izm_NO * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);

                                    calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                    calculation.M_NO = Math.Round(calculation.M_NOx * 0.13, 3);
                                    calculation.M_NO2 = Math.Round(calculation.M_NOx * 0.8, 3);
                                }
                                else
                                {
                                    calculation.C_NOx = Math.Round((calculation.Izm_NO / calculation.rNO) * calculation.rNO2 * (calculation.a / calculation.a_cast), 3);

                                    calculation.M_NOx = Math.Round(calculation.C_NOx * calculation.Vdry * 0.001, 3);
                                    calculation.M_NO = Math.Round(calculation.M_NOx * 0.13, 3);
                                    calculation.M_NO2 = Math.Round(calculation.M_NOx * 0.8, 3);
                                }
                                break;
                        }
                    //}
                    //else
                    //{
                        //Рассчитывем пыль?
                        //if(is_dust)
                        //{
                            if(GlobalStaticSettingsASK.CalculationSetting.Is_NormalizationDust_H2O) //если нужно приводить пыль к по влаге 
                                calculation.Dust_k = calculation.k;

                            if(GlobalStaticSettingsASK.CalculationSetting.Is_O2Dust)                //если нужно приводить пыль к по кислороду
                                calculation.Dust_a = calculation.a;
                            else
                                calculation.Dust_a_cast = 1;

                            if (calculationSetting.Is_NormalizationDust)                            //Приводить gskm  к н.у.
                                calculation.Normalization_Dust = Math.Round(((273 + calculation.tg) * 101.3) / (273 * calculation.Pa), 3);
                  


                            switch (calculationSetting.TypeDust)
                            {
                                case TypeDustConc.DustHunter:
                                    calculation.C_Dust = Math.Round(calculation.Izm_Dust * calculation.Normalization_Dust / calculation.Dust_k * (calculation.Dust_a / calculation.Dust_a_cast), 3);
                                    
                                    calculation.M_Dust = Math.Round(calculation.C_Dust * calculation.Vdry * 0.001, 3);
                                    break;

                                case TypeDustConc.CodelACEMT:
                                    calculation.Dust_E = Math.Round(Math.Log10(100/(100 - calculation.Dust_Op)), 3);
                                    calculation.C_Dust = Math.Round(calculation.Dust_DF * calculation.Dust_E * calculation.Normalization_Dust / calculation.Dust_k * (calculation.Dust_a / calculation.Dust_a_cast), 3);

                                    calculation.M_Dust = Math.Round(calculation.C_Dust * calculation.Vdry * 0.001, 3);
                                    break;

                                case TypeDustConc.Ecomer:

                                    calculation.Dust_E = calculation.Izm_Dust;
                                    calculation.C_Dust = Math.Round(calculation.Dust_DF * calculation.Dust_E * calculation.Normalization_Dust / calculation.Dust_k * (calculation.Dust_a / calculation.Dust_a_cast), 3);

                                    calculation.M_Dust = Math.Round(calculation.C_Dust * calculation.Vdry * 0.001, 3);
                                    break;
                            }
                        //}
                        //else
                        //{

                            
                            if(GlobalStaticSettingsASK.CalculationSetting.Is_Normalization_H2O) //если нужно приводить по влаге концентрация
                                С_k = calculation.k;

                            //Ситуация где к = 0 ? 

                            calculation.Cizm = sensorNow;
                            calculation.C = Math.Round((calculation.Cizm * calculation.r * calculation.Normalization) /С_k * (calculation.a / calculation.a_cast), 3);

                            calculation.M = Math.Round(calculation.C * calculation.Vdry * 0.001, 3);
                        //}
                    //}
                }
            }

            return calculation;
        }
    }
}
