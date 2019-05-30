using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Utility
{
    /// <summary>
    /// Excel欄位定義擴充類別.
    /// </summary>
    public static class ExcelCellDefinitionListExtensions
    {
        /// <summary>
        /// 加入欄位定義
        /// </summary>
        /// <typeparam name="T">欄位資料來源型別</typeparam>
        /// <param name="columns">欄位集合</param>
        /// <param name="name">欄位名稱</param>
        /// <param name="getValueDelegate">取得欄位值處理函式</param>
        public static void Add<T>(this List<ExcelCellDefinition<T>> columns, string name, Func<T, string> getValueDelegate)
        {
            columns.Add(new ExcelCellDefinition<T>(name, getValueDelegate));
        }
    }
}
