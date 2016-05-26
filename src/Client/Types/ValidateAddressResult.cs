
namespace Nako.Client.Types
{
    public class ValidateAddressResult
    {
        #region Public Properties

        public string Account { get; set; }

        public string Address { get; set; }

        public bool IsMine { get; set; }

        public bool IsScript { get; set; }

        public bool IsValid { get; set; }


        public string Pubkey { get; set; }

        #endregion
    }
}
