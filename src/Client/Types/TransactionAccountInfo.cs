
namespace Nako.Client.Types
{
    #region Using Directives

    using Newtonsoft.Json;

    #endregion

    public class TransactionAccountInfo
    {
        #region Public Properties

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockindex")]
        public int BlockIndex { get; set; }

        [JsonProperty("blocktime")]
        public long BlockTime { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("timereceived")]
        public long TimeReceived { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        #endregion
    }
}