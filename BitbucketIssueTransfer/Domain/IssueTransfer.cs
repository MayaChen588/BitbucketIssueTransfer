using BitbucketIssueTransfer.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    /// <summary>
    /// Bitbucket issue資料轉換類別
    /// </summary>
    public class IssueTransfer
    {
        /// <summary>
        /// 匯出Issue資料為Excel檔案
        /// </summary>
        /// <param name="sourceFilePath">issue來源檔案路徑檔名</param>
        /// <param name="resultFilePath">匯出檔案路徑檔名</param>
        public void Export(string sourceFilePath, string resultFilePath)
        {
            if (String.IsNullOrEmpty(sourceFilePath))
            {
                throw new ArgumentNullException(nameof(sourceFilePath));
            }

            if (!File.Exists(sourceFilePath))
            {
                throw new InvalidOperationException($"File not found.{sourceFilePath}");
            }


            Console.WriteLine("");
            Console.WriteLine($"Issue檔格式: v1");
            Console.WriteLine($"載入來源檔案: {sourceFilePath}");

            IssueDb issueData = JsonConvert.DeserializeObject<IssueDb>(File.ReadAllText(sourceFilePath));

            Console.WriteLine($"載入來源檔案完成，開始資料解析...");

            if (issueData.Issues != null &&
                issueData.Logs != null)
            {
                foreach (var issue in issueData.Issues.Where(x => x.Status == "resolved" || x.Status == "closed"))
                {
                    var resolvedLog = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "resolved")
                        .OrderBy(y => y.CreatedOn).LastOrDefault();

                    if (resolvedLog != null)
                    {
                        issue.Resolveder = resolvedLog.User;
                        issue.ResolvedOn = resolvedLog.CreatedOn;
                    }

                    var closedLog = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "closed")
                        .OrderBy(y => y.CreatedOn).LastOrDefault();

                    if (closedLog != null)
                    {
                        issue.Closeder = closedLog.User;
                        issue.ClosedOn = closedLog.CreatedOn;
                    }
                }

                foreach (var issue in issueData.Issues.Where(x => x.Status == "invalid"))
                {
                    issue.ResolvedOn = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "invalid")
                        .OrderBy(y => y.CreatedOn).LastOrDefault()?.CreatedOn;
                }

                foreach (var issue in issueData.Issues.Where(x => x.Status == "duplicate"))
                {
                    issue.ResolvedOn = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "duplicate")
                        .OrderBy(y => y.CreatedOn).LastOrDefault()?.CreatedOn;
                }
            }


            // ExcelDefinition
            // Issue
            var excelIssueColumns = new List<ExcelCellDefinition<Issue>>();
            excelIssueColumns.Add("Issue編號", item => item.Id.ToString());
            excelIssueColumns.Add("重要程度", item => item.Priority ?? "");
            excelIssueColumns.Add("種類", item => item.Kind ?? "");
            excelIssueColumns.Add("模組", item => item.Component ?? "");
            excelIssueColumns.Add("標題", item => item.Title ?? "");
            excelIssueColumns.Add("問題描述", item => item.Content ?? "");
            excelIssueColumns.Add("起案人", item => item.Reporter ?? "");
            excelIssueColumns.Add("處理人", item => item.Assignee ?? "");
            excelIssueColumns.Add("狀態", item => item.Status ?? "");
            excelIssueColumns.Add("QA版號", item => item.Version ?? "");
            excelIssueColumns.Add("建立日期", item => item.CreatedOn.HasValue ? item.CreatedOn.Value.ToString("yyyy-MM-dd") : "");
            excelIssueColumns.Add("更新日期", item => item.UpdatedOn.HasValue ? item.UpdatedOn.Value.ToString("yyyy-MM-dd") : "");
            excelIssueColumns.Add("修正日期", item => item.ResolvedOn.HasValue ? item.ResolvedOn.Value.ToString("yyyy-MM-dd") : "");
            excelIssueColumns.Add("結案日期", item => item.ClosedOn.HasValue ? item.ClosedOn.Value.ToString("yyyy-MM-dd") : "");

            var excel = ExcelFactory.CreateExcelPackage(excelIssueColumns, issueData.Issues.OrderBy(x => x.Id), "Issue", null);

            //  Comment
            var excelCommentColumns = new List<ExcelCellDefinition<Comment>>();
            excelCommentColumns.Add("編號", item => item.Id.ToString());
            excelCommentColumns.Add("Issue編號", item => item.Issue.ToString());
            excelCommentColumns.Add("備註內容", item => item.Content ?? "");
            excelCommentColumns.Add("人員", item => item.User ?? "");
            excelCommentColumns.Add("建立日期", item => item.CreatedOn.HasValue ? item.CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
            excelCommentColumns.Add("更新日期", item => item.UpdatedOn.HasValue ? item.UpdatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");

            excel = ExcelFactory.CreateExcelPackage(excelCommentColumns, issueData.Comments.OrderBy(x => x.Issue).ThenBy(x => x.CreatedOn), "Comment", excel);

            Console.WriteLine($"匯出結果檔案: {resultFilePath}");
            File.WriteAllBytes(resultFilePath, excel.GetAsByteArray());
            Console.WriteLine($"匯出檔案完成...");

            excel = null;
        }


        /// <summary>
        /// 匯出Issue資料為Excel檔案
        /// </summary>
        /// <param name="sourceFilePath">issue來源檔案路徑檔名</param>
        /// <param name="resultFilePath">匯出檔案路徑檔名</param>
        public void ExportV2(string sourceFilePath, string resultFilePath)
        {
            if (String.IsNullOrEmpty(sourceFilePath))
            {
                throw new ArgumentNullException(nameof(sourceFilePath));
            }

            if (!File.Exists(sourceFilePath))
            {
                throw new InvalidOperationException($"File not found.{sourceFilePath}");
            }


            Console.WriteLine("");
            Console.WriteLine($"Issue檔格式: v2");
            Console.WriteLine($"載入來源檔案: {sourceFilePath}");

            IssueDbV2 issueData = JsonConvert.DeserializeObject<IssueDbV2>(File.ReadAllText(sourceFilePath));

            Console.WriteLine($"載入來源檔案完成，開始資料解析...");

            if (issueData.Issues != null &&
                issueData.Logs != null)
            {
                foreach (var issue in issueData.Issues.Where(x => x.Status == "resolved" || x.Status == "closed"))
                {
                    var resolvedLog = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "resolved")
                        .OrderBy(y => y.CreatedOn).LastOrDefault();

                    if (resolvedLog != null)
                    {
                        issue.Resolveder = resolvedLog.User?.DisplayName;
                        issue.ResolvedOn = resolvedLog.CreatedOn;
                    }

                    var closedLog = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "closed")
                        .OrderBy(y => y.CreatedOn).LastOrDefault();

                    if (closedLog != null)
                    {
                        issue.Closeder = closedLog.User?.DisplayName;
                        issue.ClosedOn = closedLog.CreatedOn;
                    }
                }

                foreach (var issue in issueData.Issues.Where(x => x.Status == "invalid"))
                {
                    issue.ResolvedOn = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "invalid")
                        .OrderBy(y => y.CreatedOn).LastOrDefault()?.CreatedOn;
                }

                foreach (var issue in issueData.Issues.Where(x => x.Status == "duplicate"))
                {
                    issue.ResolvedOn = issueData.Logs.Where(y => y.Issue == issue.Id && y.Field == "status" && y.ChangedTo == "duplicate")
                        .OrderBy(y => y.CreatedOn).LastOrDefault()?.CreatedOn;
                }
            }


            // ExcelDefinition
            // Issue
            var excelIssueColumns = new List<ExcelCellDefinition<IssueV2>>();
            excelIssueColumns.Add("Issue編號", item => item.Id.ToString());
            excelIssueColumns.Add("重要程度", item => item.Priority ?? "");
            excelIssueColumns.Add("種類", item => item.Kind ?? "");
            excelIssueColumns.Add("模組", item => item.Component ?? "");
            excelIssueColumns.Add("標題", item => item.Title ?? "");
            excelIssueColumns.Add("問題描述", item => item.Content ?? "");
            excelIssueColumns.Add("起案人", item => item.Reporter == null ? "" : item.Reporter.DisplayName ?? "");
            excelIssueColumns.Add("處理人", item => item.Assignee == null ? "" : item.Assignee.DisplayName ?? "");
            excelIssueColumns.Add("狀態", item => item.Status ?? "");
            excelIssueColumns.Add("QA版號", item => item.Version ?? "");
            excelIssueColumns.Add("建立日期", item => item.CreatedOn.HasValue ? item.CreatedOn.Value.ToString("yyyy-MM-dd") : "");
            excelIssueColumns.Add("更新日期", item => item.UpdatedOn.HasValue ? item.UpdatedOn.Value.ToString("yyyy-MM-dd") : "");
            excelIssueColumns.Add("修正人員", item => item.Resolveder ?? "");
            excelIssueColumns.Add("修正日期", item => item.ResolvedOn.HasValue ? item.ResolvedOn.Value.ToString("yyyy-MM-dd") : "");
            excelIssueColumns.Add("結案人員", item => item.Closeder ?? "");
            excelIssueColumns.Add("結案日期", item => item.ClosedOn.HasValue ? item.ClosedOn.Value.ToString("yyyy-MM-dd") : "");

            var excel = ExcelFactory.CreateExcelPackage(excelIssueColumns, issueData.Issues.OrderBy(x => x.Id), "Issue", null);

            //  Comment
            var excelCommentColumns = new List<ExcelCellDefinition<CommentV2>>();
            excelCommentColumns.Add("編號", item => item.Id.ToString());
            excelCommentColumns.Add("Issue編號", item => item.Issue.ToString());
            excelCommentColumns.Add("備註內容", item => item.Content ?? "");
            excelCommentColumns.Add("人員", item => item.User.DisplayName ?? "");
            excelCommentColumns.Add("建立日期", item => item.CreatedOn.HasValue ? item.CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
            excelCommentColumns.Add("更新日期", item => item.UpdatedOn.HasValue ? item.UpdatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");

            excel = ExcelFactory.CreateExcelPackage(excelCommentColumns, issueData.Comments.OrderBy(x => x.Issue).ThenBy(x => x.CreatedOn), "Comment", excel);

            Console.WriteLine($"匯出結果檔案: {resultFilePath}");
            File.WriteAllBytes(resultFilePath, excel.GetAsByteArray());
            Console.WriteLine($"匯出檔案完成...");

            excel = null;
        }
    }
}
