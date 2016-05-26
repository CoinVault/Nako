
namespace Nako.Storage.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class SyncTransactionAddressBalance
    {
        #region Public Properties

        public decimal Available { get; set; }

        public decimal? Received { get; set; }

        public decimal? Sent { get; set; }

        public decimal Unconfirmed { get; set; }

        public IEnumerable<SyncTransactionAddressItem> Items { get; set; }

        #endregion
    }
}
