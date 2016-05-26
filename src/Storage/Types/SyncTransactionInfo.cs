
namespace Nako.Storage.Types
{
    public class SyncTransactionInfo
    {
        #region Public Properties

        public string BlockHash { get; set; }

        public long BlockIndex { get; set; }

        public long Timestamp { get; set; }

        public string TransactionHash { get; set; }

        public long Confirmations { get; set; }

        #endregion
    }
}