
namespace Nako.Client.Types
{
    #region Using Directives

    using Newtonsoft.Json;

    #endregion

    public class TransactionInfoDetails
    {
        #region Public Properties

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        #endregion
    }
}