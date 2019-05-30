using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace BitbucketIssueTransfer.Utility
{
    /// <summary>
    /// Excel檔案匯出工廠類別
    /// </summary>
    public static class ExcelFactory
    {
        /// <summary>
        /// 建立ExcelPackage物件
        /// </summary>
        /// <typeparam name="T">資料欄位定義集合泛型型別</typeparam>
        /// <param name="columns">欄位定義集合</param>
        /// <param name="columnDatas">欄位資料集合</param>
        /// <param name="targetSheetName">Sheet名稱，若無指定則預設為sheet1</param>
        /// <returns>ExcelPackage物件</returns>
        public static ExcelPackage CreateExcelPackage<T>(IEnumerable<ExcelCellDefinition<T>> columns, IEnumerable<T> columnDatas, string targetSheetName)
        {
            return CreateExcelPackage(columns, columnDatas, targetSheetName, null);
        }

        /// <summary>
        /// 建立ExcelPackage物件
        /// </summary>
        /// <typeparam name="T">資料欄位定義集合泛型型別</typeparam>
        /// <param name="columns">欄位定義集合</param>
        /// <param name="columnDatas">欄位資料集合</param>
        /// <param name="targetSheetName">Sheet名稱，若無指定則預設為sheet1</param>
        /// <param name="originalExcelPackage">原始ExcelPackage，若有指定則於此ExcelPackage新增Sheet，若無指定則產生新ExcelPackage加入Sheet</param>
        /// <returns>ExcelPackage物件</returns>
        public static ExcelPackage CreateExcelPackage<T>(IEnumerable<ExcelCellDefinition<T>> columns, IEnumerable<T> columnDatas, string targetSheetName, ExcelPackage originalExcelPackage)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columnDatas == null)
            {
                throw new ArgumentNullException(nameof(columnDatas));
            }


            string sheetName = String.IsNullOrWhiteSpace(targetSheetName) ? "sheet1" : targetSheetName;

            // ExcelPackage
            var excelPackage = originalExcelPackage ?? new ExcelPackage();

            // Sheet
            var sheet = excelPackage.Workbook.Worksheets.Add(sheetName);
            sheet.Cells.Style.Numberformat.Format = "@";

            // Header
            int headerRowIndex = 1;
            int headerCellIndex = 1;
            foreach (var excelCellDefinition in columns)
            {
                sheet.Cells[headerRowIndex, headerCellIndex].Value = excelCellDefinition.Name;
                headerCellIndex++;
            }

            // Content
            int contentRowIndex = 2;
            int contentCellIndex = 1;
            foreach (var source in columnDatas)
            {
                foreach (var excelCellDefinition in columns)
                {
                    sheet.Cells[contentRowIndex, contentCellIndex].Value = excelCellDefinition.GetValue(source);
                    contentCellIndex++;
                }

                contentCellIndex = 1;
                contentRowIndex++;
            }

            return excelPackage;
        }

        /// <summary>
        /// 依照表投定義集合、資料集合，傳回Excel內容 Stream
        /// </summary>
        /// <typeparam name="T">資料欄位定義集合泛型型別</typeparam>
        /// <param name="columns">欄位定義集合</param>
        /// <param name="columnDatas">欄位資料集合</param>
        /// <returns>Stream物件</returns>
        public static Stream CreateExcelStream<T>(IEnumerable<ExcelCellDefinition<T>> columns, IEnumerable<T> columnDatas)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columnDatas == null)
            {
                throw new ArgumentNullException(nameof(columnDatas));
            }


            //var excelStream = new MemoryStream();
            // ExcelStream
            var excelStream = new MemoryStream();

            // Fill 
            using (var excel = ExcelFactory.CreateExcelPackage(columns, columnDatas, null))
            {
                excelStream = new MemoryStream();
                excel.SaveAs(excelStream);
                excelStream.Position = 0;
            }

            return excelStream;
        }

        /// <summary>
        /// 依照ExcelPackage，傳回Excel Stream
        /// </summary>
        /// <param name="excelPackage">excelPackage物件</param>
        /// <returns>Stream物件</returns>
        public static Stream CreateExcelStream(ExcelPackage excelPackage)
        {
            if (excelPackage == null)
            {
                throw new ArgumentNullException(nameof(excelPackage));
            }
            
            
            // ExcelStream
            var excelStream = new MemoryStream();

            // Fill 
            using (var excel = excelPackage)
            {
                excelStream = new MemoryStream();
                excel.SaveAs(excelStream);
                excelStream.Position = 0;
            }

            return excelStream;
        }
    }

}
