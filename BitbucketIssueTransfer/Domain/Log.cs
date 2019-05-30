using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    public class Log
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("issue")]
        public int Issue { get; set; }
        [JsonProperty("comment")]
        public int Comment { get; set; }
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("changed_from")]
        public string ChangedFrom { get; set; }
        [JsonProperty("changed_to")]
        public string ChangedTo { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("created_on")]
        public DateTime? CreatedOn { get; set; }
    }
}
