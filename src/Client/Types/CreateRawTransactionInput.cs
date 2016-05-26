
namespace Nako.Client.Types
{
    #region Using Directives

    using Newtonsoft.Json;

    #endregion

    public class CreateRawTransactionInput
    {
        #region Public Properties

        [JsonProperty("vout")]
        public int Output { get; set; }

        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        #endregion
    }
}
