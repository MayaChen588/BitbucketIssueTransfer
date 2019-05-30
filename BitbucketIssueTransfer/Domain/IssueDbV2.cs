using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    public class IssueDbV2
    {
        [JsonProperty("issues")]
        public List<IssueV2> Issues { get; set; }
        [JsonProperty("comments")]
        public List<CommentV2> Comments { get; set; }
        [JsonProperty("logs")]
        public List<LogV2> Logs { get; set; }
    }
}
