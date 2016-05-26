
namespace Nako.Client.Types
{
    #region Using Directives

    using Newtonsoft.Json;

    #endregion

    public class JsonRpcError
    {
        #region Public Properties

        [JsonProperty(PropertyName = "code", Order = 0)]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message", Order = 1)]
        public string Message { get; set; }

        #endregion
    }
}
