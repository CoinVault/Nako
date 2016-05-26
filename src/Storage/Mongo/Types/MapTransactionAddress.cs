
namespace Nako.Storage.Mongo.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class MapTransactionAddress
    {
        #region Public Properties

        public string Id { get; set; }

        public int Index { get; set; }

        public List<string> Addresses { get; set; }

        public string TransactionId { get; set; }

        public string ScriptHex { get; set; }

        public double Value { get; set; }

        public string SpendingTransactionId { get; set; }

        public long BlockIndex { get; set; }

        public bool CoinBase { get; set; }

        #endregion
    }
}