
namespace Nako.Storage.Types
{
    public class SyncBlockInfo
    {
        #region Public Properties

        public string BlockHash { get; set; }

        public long BlockIndex { get; set; }

        public long BlockSize { get; set; }

        public long BlockTime { get; set; }

        public string NextBlockHash { get; set; }

        public string PreviousBlockHash { get; set; }

        public string ReveresedBlockIndex { get; set; }

        public bool SyncComplete { get; set; }

        public int TransactionCount { get; set; }

        public string ETag { get; set; }

        #endregion
    }
}