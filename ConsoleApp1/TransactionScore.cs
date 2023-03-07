using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreReader
{
    public class TransactionScore
    {
        [JsonProperty("userId")]
        public string UserID { get; set; }

        [JsonProperty("externalAccountID")]
        public string ExternalAccountID { get; set; }

        [JsonProperty("transactionID")]
        public string TransactionID { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }
    }
}
