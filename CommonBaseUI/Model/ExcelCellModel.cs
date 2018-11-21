using CommonUtils;
using NPOI.SS.UserModel;
using System;
namespace CommonBaseUI.Model
{
    public class ExcelCellModel
    {
        private const double  zIndex = 26;
        public int _RowIndex { get; set; }
        public int _ColIndex { get; set; }

        /// <summary>
        /// 通过单元格名称获取坐标
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static ExcelCellModel GetCell(string location)
        {
            location = location.ToUpper();
            int dataNum = 0;
            int dataRow = 0;
            string rowStr = "";
            string columnStr = "";
            for (int i = 0; i < location.Length; i++)
            {
                if (location[i] >= 48 && location[i] <= 57)
                {
                    columnStr += location[i];
                }
                if (location[i] >= 65 && location[i] <= 90)
                {
                    rowStr += location[i];
                }
            }
            for (int i = 0; i < rowStr.Length; i++)
            {
                char ch = rowStr[rowStr.Length - i - 1];
                dataNum = (int)(ch - 'A' + 1);
                dataNum *= Math.Pow(zIndex, i.ToDouble()).ToInt();
                dataRow += dataNum;
            }

            var model = new ExcelCellModel();
            model._RowIndex = columnStr.ToInt() - 1;
            model._ColIndex = dataRow - 1;

            return model;
        }

        /// <summary>
        /// 判断单元格数据类型并返回字符型数据
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string CellValueToString(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }
            if (cell.CellType == CellType.String)
            {
                return CommonUtil.ToStr(cell.StringCellValue);
            }
            else if (cell.CellType == CellType.Numeric)
            {
                return CommonUtil.ToStr(cell.NumericCellValue);
            }
            else if (cell.CellType == CellType.Formula)
            {
                if (cell.CachedFormulaResultType == CellType.String)
                {
                    return CommonUtil.ToStr(cell.StringCellValue);
                }
                else if (cell.CachedFormulaResultType == CellType.Numeric)
                {
                    return CommonUtil.ToStr(cell.NumericCellValue);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
