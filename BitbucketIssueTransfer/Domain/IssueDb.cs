using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    public class IssueDb
    {
        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }
        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }
        [JsonProperty("logs")]
        public List<Log> Logs { get; set; }
    }
}
