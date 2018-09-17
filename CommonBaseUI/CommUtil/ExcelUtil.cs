using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

namespace CommonBaseUI.CommUtil
{
    public class ExcelUtil
    {
        ///// <summary>
        ///// 将Base64字符串转换为图片
        ///// </summary>
        ///// <param name="workbook"></param>
        ///// <param name="base64">图片base64码</param>
        ///// <param name="tempPath">模板路径</param>
        ///// <param name="outputPath">输出路径</param>
        ///// <param name="col1">图片起始列</param>
        ///// <param name="row1">图片起始行</param>
        ///// <param name="col2">图片结束列</param>
        ///// <param name="row2">图片结束行</param>
        ///// <returns></returns>
        //public static string AddImageToExcel(HSSFWorkbook workbook, string base64, string tempPath, string outputPath, int col1, int row1, int col2, int row2)
        //{
        //    byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
        //    int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
        //    HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);
        //    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

        //    //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
        //    HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, col1, row1, col2, row2);
        //    HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

        //    string outFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    string resPath = outputPath + outFileName + ".xls";
        //    return resPath;
        //}        
        
        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="base64">图片base64码</param>
        /// <param name="tempPath">模板路径</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="col1">图片起始列</param>
        /// <param name="row1">图片起始行</param>
        /// <param name="col2">图片结束列</param>
        /// <param name="row2">图片结束行</param>
        /// <returns></returns>
        public static string AddImageToExcel(IWorkbook workbook, string base64, string tempPath, string outputPath, int col1, int row1, int col2, int row2)
        {
            byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
            ISheet sheet = workbook.GetSheetAt(0);
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

            //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, col1, row1, col2, row2);
            HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

            string outFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string resPath = outputPath + outFileName + ".xls";
            return resPath;
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="bmp">图片</param>
        /// <param name="col1">图片起始列</param>
        /// <param name="row1">图片起始行</param>
        /// <param name="col2">图片结束列</param>
        /// <param name="row2">图片结束行</param>
        /// <returns></returns>
        public static void AddImageToExcel(IWorkbook workbook, int sheetIndex, Bitmap bmp, int col1, int row1, int col2, int row2)
        {
            string base64 = ImgToBase64String(bmp);
            byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();

            //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            XSSFClientAnchor anchor = new XSSFClientAnchor(100*10000, 0, 100, 100, col1, row1, col2, row2);
            patriarch.CreatePicture(anchor, pictureIdx);
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="bmp">图片</param>
        /// <param name="col1">图片起始列</param>
        /// <param name="row1">图片起始行</param>
        /// <param name="col2">图片结束列</param>
        /// <param name="row2">图片结束行</param>
        /// <returns></returns>
        public static void AddImageToExcel(ISheet sheet, Bitmap bmp, int col1, int row1, int col2, int row2)
        {
            string base64 = ImgToBase64String(bmp);
            byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            int pictureIdx = sheet.Workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
            XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();

            //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            XSSFClientAnchor anchor = new XSSFClientAnchor(100 * 10000, 0, 100, 100, col1, row1, col2, row2);
            patriarch.CreatePicture(anchor, pictureIdx);
        }

        //图片转为base64编码的字符串  
        private static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }  

        /// <summary>  
        /// 转换excel 成PDF文档  
        /// </summary>  
        /// <param name="excelFile">原文件路径</param>  
        /// <param name="pdfFile">pdf文件输出路径</param>  
        /// <returns>true 成功</returns>  
        public static bool ExcelToPdf(string excelFile, string pdfFile)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(excelFile);
            workbook.SaveToFile(pdfFile, FileFormat.PDF);
            return true;
        }

        /// <summary>
        /// 将DataTable数组导入Excel
        /// </summary>
        /// <param name="dts"></param>
        /// <returns></returns>
        public static IWorkbook DataTableToExcel(DataTable[] dts)
        {
            ISheet sheet = null;
            IWorkbook workbook = new XSSFWorkbook();
            try
            {
                int rowNum = 0;
                foreach (DataTable dt in dts)
                {
                    sheet = workbook.CreateSheet(dt.TableName);
                    IRow row = sheet.CreateRow(rowNum);

                    for (int j = 0; j < dt.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                        sheet.AutoSizeColumn(j);
                    }

                    rowNum++;

                    for (int i = 0; i < dt.Rows.Count; ++i)
                    {
                        row = sheet.CreateRow(rowNum);
                        for (int j = 0; j < dt.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                            sheet.AutoSizeColumn(j);
                        }
                        rowNum++;
                    }
                }

                return workbook;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 将DataTable数组导入Excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sheetName"></param>
        /// <param name="useDesc">是否使用注释做列头</param>
        /// <returns></returns>
        public static IWorkbook ListToExcel<T>(List<T> list, string sheetName, bool useDesc = true)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            if (list != null && list.Count > 0)
            {
                var mo = list[0];
                //获得该类的Type
                Type t = mo.GetType();
                var props = t.GetProperties();
                int rowNum = 0;

                IRow row = sheet.CreateRow(rowNum);
                for (int i = 0; i < props.Length; i++)
                {
                    string colName = "";
                    if (useDesc)
                    {
                        object[] objs = props[i].GetCustomAttributes(typeof(DescriptionAttribute), true);
                        colName = ((DescriptionAttribute)objs[0]).Description;
                    }
                    else
                    {
                        colName = props[i].Name;
                    }
                    row.CreateCell(i).SetCellValue(colName);
                    sheet.AutoSizeColumn(i);
                }
                rowNum++;

                foreach (T model in list)
                {
                    row = sheet.CreateRow(rowNum);
                    for (int i = 0; i < props.Length; i++)
                    {
                        row.CreateCell(i).SetCellValue(props[i].GetValue(model).ToStr());
                        sheet.AutoSizeColumn(i);
                    }
                    rowNum++;
                }
            }

            return workbook;
        }
    }
}
