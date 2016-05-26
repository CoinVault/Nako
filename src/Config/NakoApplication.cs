
namespace Nako.Config
{
    #region Using Directives

    using System.Threading;

    #endregion

    public class NakoApplication
    {
        public CancellationToken ApiToken { get; set; }

        public CancellationToken SyncToken { get; set; }

        public CancellationTokenSource ApiTokenSource { get; set; }

        public CancellationTokenSource SyncTokenSource { get; set; }

        public bool ExitApplication { get; set; }

        public CancellationToken CreateApiToken()
        {
            this.ApiTokenSource = new CancellationTokenSource();

            this.ApiToken = this.ApiTokenSource.Token;

            return this.ApiToken;
        }

        public CancellationToken CreateSyncToken()
        {
            this.SyncTokenSource = new CancellationTokenSource();

            this.SyncToken = this.SyncTokenSource.Token;

            return this.SyncToken;
        }
    }
}
