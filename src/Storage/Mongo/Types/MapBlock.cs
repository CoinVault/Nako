
namespace Nako.Storage.Mongo.Types
{
    public class MapBlock
    {
        #region Public Properties

        public string BlockHash { get; set; }

        public long BlockIndex { get; set; }

        public long BlockSize { get; set; }

        public long BlockTime { get; set; }

        public string NextBlockHash { get; set; }

        public string PreviousBlockHash { get; set; }

        public bool SyncComplete { get; set; }

        public int TransactionCount { get; set; }

        #endregion
    }
}