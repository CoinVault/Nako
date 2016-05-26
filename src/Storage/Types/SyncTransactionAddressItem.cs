
namespace Nako.Storage.Types
{
    public class SyncTransactionAddressItem
    {
        #region Public Properties

        public int Index { get; set; }

        public string Address { get; set; }

        public string Type { get; set; }

        public string TransactionHash { get; set; }

        public string SpendingTransactionHash { get; set; }

        public string ScriptHex { get; set; }

        public string CoinBase { get; set; }

        public long? BlockIndex { get; set; }

        public long? Confirmations { get; set; }

        public decimal Value { get; set; }

        public long Time { get; set; }

        #endregion
    }
}