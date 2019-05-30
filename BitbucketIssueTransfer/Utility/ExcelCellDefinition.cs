using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Utility
{
    /// <summary>
    /// Excel欄位定義類別
    /// </summary>
    /// <typeparam name="T">來源資料型別</typeparam>
    public sealed class ExcelCellDefinition<T>
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="name">欄位名稱</param>
        /// <param name="getValueDelegate">取得欄位值處理函式</param>
        public ExcelCellDefinition(string name, Func<T, string> getValueDelegate)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (getValueDelegate == null)
            {
                throw new ArgumentNullException(nameof(getValueDelegate));
            }

            #endregion

            // Arguments
            this.Name = name;
            this.GetValue = getValueDelegate;
        }


        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 取得欄位值處理函式
        /// </summary>
        public Func<T, string> GetValue { get; private set; }
    }
}
