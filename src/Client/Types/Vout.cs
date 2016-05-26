
namespace Nako.Client.Types
{
    public class Vout
    {
        #region Public Properties

        public int N { get; set; }

        public ScriptPubKey ScriptPubKey { get; set; }

        public decimal Value { get; set; }

        #endregion
    }
}