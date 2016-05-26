
namespace Nako.Client.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class DecodedRawTransaction
    {
        #region Public Properties

        public string Hex { get; set; }

        public long Locktime { get; set; }

        public string TxId { get; set; }

        public List<Vin> VIn { get; set; }

        public List<Vout> VOut { get; set; }

        public int Version { get; set; }

        #endregion
    }
}
