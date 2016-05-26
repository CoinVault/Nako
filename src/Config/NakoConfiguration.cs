
namespace Nako.Config
{
    public class NakoConfiguration
    {
        #region Public Properties

        public int BlockStoreInterval { get; set; }

        public int BlockSyncerInterval { get; set; }

        public string ClientLocation { get; set; }

        public string CoinTag { get; set; }

        public string RpcPassword { get; set; }

        public int BlockFinderInterval { get; set; }

        public int DetailedTrace { get; set; }

        public int MaxItemsInQueue { get; set; }

        public int ParallelRequestsToTransactionRpc { get; set; }

        public int SyncApiPort { get; set; }

        public int ParalleleTableStorageBatchCount { get; set; }

        public int RpcAccessPort { get; set; }

        public bool RpcSecure { get; set; }

        public string RpcDomain { get; set; }

        public bool SyncBlockchain { get; set; }

        public long StartBlockIndex { get; set; }

        public bool SyncMemoryPool { get; set; }

        public string RpcUser { get; set; }

        public string NotifyUrl { get; set; }

        public int NotifyBatchCount { get; set; }

        #endregion
    }
}
