
namespace Nako.Storage.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class SyncTransactionItems
    {
        #region Public Properties

        public List<SyncTransactionItemInput> Inputs { get; set; }

        public List<SyncTransactionItemOutput> Outputs { get; set; }

        #endregion
    }
}