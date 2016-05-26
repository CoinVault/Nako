
namespace Nako.Client.Types
{
    #region Using Directives

    using System.Collections.Generic;

    using Newtonsoft.Json;

    #endregion

    public class TransactionInfo
    {
        #region Public Properties

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockindex")]
        public int BlockIndex { get; set; }

        [JsonProperty("blocktime")]
        public long BlockTime { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        public IEnumerable<TransactionInfoDetails> Details { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("timereceived")]
        public long TimeReceived { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        #endregion
    }
}