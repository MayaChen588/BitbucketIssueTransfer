using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    public class CommentV2
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("issue")]
        public int Issue { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("created_on")]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty("updated_on")]
        public DateTime? UpdatedOn { get; set; }
    }
}
