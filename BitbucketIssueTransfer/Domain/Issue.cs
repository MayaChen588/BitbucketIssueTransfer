using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    public class Issue
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("priority")]
        public string Priority { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("reporter")]
        public string Reporter { get; set; }
        [JsonProperty("component")]
        public string Component { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("assignee")]
        public string Assignee { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("milestone")]
        public string Milestone { get; set; }
        [JsonProperty("created_on")]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        public string Resolveder { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public string Closeder { get; set; }
        public DateTime? ClosedOn { get; set; }
    }
}
