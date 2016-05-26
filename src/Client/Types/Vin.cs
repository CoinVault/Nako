
namespace Nako.Client.Types
{
    public class Vin
    {
        #region Public Properties

        public ScriptSig ScriptSig { get; set; }

        public long Sequence { get; set; }

        public string CoinBase { get; set; }

        public string TxId { get; set; }

        public int VOut { get; set; }

        #endregion
    }
}