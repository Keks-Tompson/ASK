using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ASK.BLL.Models
{
    public class ExcelReport_Model
    {
        public Color colorTMPFooter;    //Просто переменная для хранения цвета 

        // Необходим для работы со стилями Excel (можно задавть только через буква-цифру адрес ячейки)
        //                                        0    1    2    3    4    5    6    7    8    9    10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27    28    29    30    31    32    33    34    35    36    37    38    39    40    41    42    43    44    45    46    47    48    49    50    51    52    53    54    55    56    57    58    59    60    61    62    63    64    65    66    67    68    69    70    71    72    73    74    75    76    77    78    79    80    81    82    83    84    85    86    87    88    89    90    91    92    93    94    95    96    97    98    99    100   101   102   103   104     
        public string[] masExcelCell = { "A", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ" };

        //Работа с таблицей
        public int startRow = 4; //Первая строчка таблицы
        public int row = 4; //Ручной номер колонки
        public int col = 1; //Столбец

        //Таблица 20м
        public Report_Model ReportDay = new ReportDay_Model();

        //Доп. переменные
        public string[] mode_ASK_String = new string[3] { "Работа", "Простой", "Останов" };
    }
}
