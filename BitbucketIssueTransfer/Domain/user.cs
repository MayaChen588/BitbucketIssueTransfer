using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer.Domain
{
    public class User
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
    }
}
