
namespace Nako.Storage.Types
{
    public class SyncTransactionItemInput
    {
        #region Public Properties

        public string InputCoinBase { get; set; }

        public int PreviousIndex { get; set; }

        public string PreviousTransactionHash { get; set; }

        #endregion
    }
}