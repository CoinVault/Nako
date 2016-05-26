// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BitcoinClient.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Nako.Client.Types;

    using Newtonsoft.Json;

    #endregion

    public class BitcoinClient : IDisposable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="BitcoinClient"/> class.
        /// </summary>
        static BitcoinClient()
        {
            // The certificate is self signed and for some reason the name does not match the local certificate.
            // This needs some further investigation on how in create the certificate correctly and install it in the local store.
            // For now we allow requests with certificates that have a name mismatch.
            ServicePointManager.ServerCertificateValidationCallback = CertificateHandler.ValidateCertificate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinClient"/> class.
        /// </summary>
        public BitcoinClient()
        {
            this.Client = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinClient"/> class.
        /// </summary>
        public BitcoinClient(string uri)
        {
            this.Url = new Uri(uri);
            this.Client = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinClient"/> class.
        /// </summary>
        public BitcoinClient(string uri, NetworkCredential credentials)
        {
            this.Url = new Uri(uri);
            this.Credentials = credentials;
            this.Client = new HttpClient();

            // Set basic authentication.
            var token = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", this.Credentials.UserName, this.Credentials.Password));
            var base64Token = Convert.ToBase64String(token);
            this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Token);
            this.Client.DefaultRequestHeaders.Add("x-crypto", new[] { "true" });
            this.Client.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public Uri Url { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        private HttpClient Client { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// A static method to create a client.
        /// </summary>
        public static BitcoinClient Create(string connection, int port, string user, string pass, bool secure)
        {
            var schema = secure ? "https" : "http";
            return new BitcoinClient(string.Format("{0}://{1}:{2}", schema, connection, port), new NetworkCredential(user, pass));
        }

        /// <inheritdoc />
        public async Task BackupWalletAsync(string destination)
        {
            await this.Call("backupwallet", destination);
        }

        /// <inheritdoc />
        public async Task<string> CreateRawTransactionAsync(CreateRawTransaction rawTransaction)
        {
            return await this.Call<string>("createrawtransaction", rawTransaction.Inputs, rawTransaction.Outputs);
        }

        /// <inheritdoc />
        public async Task<DecodedRawTransaction> DecodeRawTransactionAsync(string rawTransactionHex)
        {
            var res = await this.Call<DecodedRawTransaction>("decoderawtransaction", rawTransactionHex);
            return res;
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }

        /// <inheritdoc />
        public async Task<string> DumpPrivkeyAsync(string address)
        {
            return await this.Call<string>("dumpprivkey", address);
        }

        /// <inheritdoc />
        public async Task EncryptWalletAsync(string passphrase)
        {
            await this.Call("encryptwallet", passphrase);
        }

        /// <inheritdoc />
        public async Task<string> GetAccountAddressAsync(string account)
        {
            return await this.Call<string>("getaccountaddress", account);
        }

        /// <inheritdoc />
        public async Task<string> GetAccountAsync(string address)
        {
            return await this.Call<string>("getaccount", address);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetAddressesByAccountAsync(string account)
        {
            return await this.Call<IEnumerable<string>>("getaddressesbyaccount", account);
        }

        /// <inheritdoc />
        public async Task<decimal> GetBalanceAsync(string account = null, int minconf = 1)
        {
            if (account == null)
            {
                return await this.Call<decimal>("getbalance");
            }

            return await this.Call<decimal>("getbalance", account, minconf);
        }

        /// <inheritdoc />
        public async Task<BlockInfo> GetBlockAsync(string hash)
        {
            return await this.Call<BlockInfo>("getblock", hash);
        }

        /// <inheritdoc />
        public async Task<int> GetBlockCountAsync()
        {
            return await this.Call<int>("getblockcount");
        }

        /// <inheritdoc />
        public async Task<string> GetblockHashAsync(long index)
        {
            return await this.Call<string>("getblockhash", index);
        }

        /// <inheritdoc />
        public async Task<int> GetConnectionCountAsync()
        {
            return await this.Call<int>("getconnectioncount");
        }

        /// <inheritdoc />
        public async Task<decimal> GetDifficultyAsync()
        {
            return await this.Call<decimal>("getdifficulty");
        }

        /// <inheritdoc />
        public async Task<bool> GetGenerateAsync()
        {
            return await this.Call<bool>("getgenerate");
        }

        /// <inheritdoc />
        public async Task<decimal> GetHashesPerSecAsync()
        {
            return await this.Call<decimal>("gethashespersec");
        }

        /// <inheritdoc />
        public async Task<ClientInfo> GetInfoAsync()
        {
            return await this.Call<ClientInfo>("getinfo");
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PeerInfo>> GetPeerInfo()
        {
            return await this.Call<IEnumerable<PeerInfo>>("getpeerinfo");
        }

        /// <inheritdoc />
        public async Task<string> GetNewAddressAsync(string account)
        {
            return await this.Call<string>("getnewaddress", account);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetRawMemPoolAsync()
        {
            return await this.Call<IEnumerable<string>>("getrawmempool");
        }

        /// <inheritdoc />
        public async Task<DecodedRawTransaction> GetRawTransactionAsync(string txid, int verbose = 0)
        {
            if (verbose == 0)
            {
                var hex = await this.Call<string>("getrawtransaction", txid, verbose);
                return new DecodedRawTransaction { Hex = hex };
            }

            var res = await this.Call<DecodedRawTransaction>("getrawtransaction", txid, verbose);

            return res;
        }

        /// <inheritdoc />
        public async Task<decimal> GetReceivedByAccountAsync(string account, int minconf = 1)
        {
            return await this.Call<decimal>("getreceivedbyaccount", account, minconf);
        }

        /// <inheritdoc />
        public async Task<decimal> GetReceivedByAddressAsync(string address, int minconf = 1)
        {
            return await this.Call<decimal>("getreceivedbyaddress", address, minconf);
        }

        /// <inheritdoc />
        public async Task<TransactionInfo> GetTransactionAsync(string txid)
        {
            return await this.Call<TransactionInfo>("gettransaction", txid);
        }

        /// <inheritdoc />
        public async Task<TransactionOutputInfo> GetTxOutAsync(string txid, int outputIndex, bool includemempool = true)
        {
            return await this.Call<TransactionOutputInfo>("gettxout", txid, outputIndex, includemempool);
        }

        /// <inheritdoc />
        public async Task<WorkInfo> GetWorkAsync()
        {
            return await this.Call<WorkInfo>("getwork");
        }

        /// <inheritdoc />
        public async Task<bool> GetWorkAsync(string data)
        {
            return await this.Call<bool>("getwork", data);
        }

        /// <inheritdoc />
        public async Task<string> HelpAsync(string command = "")
        {
            return await this.Call<string>("help", command);
        }

        /// <inheritdoc />
        public async Task ImportPrivkeyAsync(string bitcoinprivkey, string label, bool rescan = true)
        {
            await this.Call("importprivkey", bitcoinprivkey, label, rescan);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionAccountInfo>> ListAccountsAsync(int minconf = 1)
        {
            return await this.Call<IEnumerable<TransactionAccountInfo>>("listaccounts", minconf);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionInfo>> ListReceivedByAccountAsync(int minconf = 1, bool includeEmpty = false)
        {
            return await this.Call<IEnumerable<TransactionInfo>>("listreceivedbyaccount", minconf, includeEmpty);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionInfo>> ListReceivedByAddressAsync(int minconf = 1, bool includeEmpty = false)
        {
            return await this.Call<IEnumerable<TransactionInfo>>("listreceivedbyaddress", minconf, includeEmpty);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionAccountInfo>> ListTransactionsAsync(string account, int count = 10)
        {
            return await this.Call<IEnumerable<TransactionAccountInfo>>("listtransactions", account, count);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionUnspentInfo>> ListUnspent(int minconf = 1, int maxconf = 999999)
        {
            return await this.Call<IEnumerable<TransactionUnspentInfo>>("listunspent", minconf, maxconf);
        }

        /// <inheritdoc />
        public async Task<bool> MoveAsync(string fromAccount, string toAccount, decimal amount, int minconf = 1, string comment = "")
        {
            return await this.Call<bool>("move", fromAccount, toAccount, amount, minconf, comment);
        }
        

        /// <inheritdoc />
        public async Task<string> SendFromAsync(string fromAccount, string toAddress, decimal amount, int minconf = 1, string comment = "", string commentTo = "")
        {
            return await this.Call<string>("sendfrom", fromAccount, toAddress, amount, minconf, comment, commentTo);
        }

        /// <inheritdoc />
        public async Task<string> SendToAddressAsync(string address, decimal amount, string comment, string commentTo)
        {
            return await this.Call<string>("sendtoaddress", address, amount, comment, commentTo);
        }

        /// <inheritdoc />
        public async Task<string> SentRawTransactionAsync(string hexString)
        {
            return await this.Call<string>("sendrawtransaction", hexString);
        }

        /// <inheritdoc />
        public async Task SetAccountAsync(string address, string account)
        {
            await this.Call("setaccount", address, account);
        }

        /// <inheritdoc />
        public async Task SetGenerateAsync(bool generate, int genproclimit = 1)
        {
            await this.Call("setgenerate", generate, genproclimit);
        }

        /// <inheritdoc />
        public async Task SetTxFeeAsync(decimal amount)
        {
            await this.Call("settxfee", amount);
        }

        /// <inheritdoc />
        public async Task<SignedRawTransaction> SignRawTransactionAsync(SignRawTransaction rawTransaction)
        {
            var hex = rawTransaction.RawTransactionHex;
            var inputs = rawTransaction.Inputs.Any() ? rawTransaction.Inputs : null;
            var privateKeys = rawTransaction.PrivateKeys.Any() ? rawTransaction.PrivateKeys : null;

            var res = await this.Call<SignedRawTransaction>("signrawtransaction", hex, inputs, privateKeys);
            return res;
        }

        /// <inheritdoc />
        public async Task StopAsync()
        {
            await this.Call("stop");
        }

        /// <inheritdoc />
        public async Task<ValidateAddressResult> ValidateAddressAsync(string address)
        {
            return await this.Call<ValidateAddressResult>("validateaddress", address);
        }

        /// <inheritdoc />
        public async Task WalletLockAsync()
        {
            await this.Call("walletlock");
        }

        /// <inheritdoc />
        public async Task WalletPassphraseAsync(string passphrase, int sectimeout)
        {
            await this.Call("walletpassphrase", passphrase, sectimeout);
        }

        /// <inheritdoc />
        public async Task WalletPassphraseChangeAsync(string passphrase, string newPassphrase)
        {
            await this.Call("walletpassphrasechange", passphrase, newPassphrase);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a crypto client exception.
        /// </summary>
        private static BitcoinClientException CreateException(HttpResponseMessage response, int code, string msg)
        {
            return new BitcoinClientException(string.Format("{0} ({1})", msg, code))
                {
                    StatusCode = response.StatusCode, 
                    RawMessage = response.Content.ReadAsStringAsync().Result, 
                    ErrorCode = code, 
                    ErrorMessage = msg
                };
        }

        /// <summary>
        /// Send the request and wrap any exception.
        /// </summary>
        private static async Task<HttpResponseMessage> Send(HttpClient client, Uri url, HttpContent content)
        {
            try
            {
                return await client.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                throw new BitcoinCommunicationException(string.Format("Bitcoin Failed Url = '{0}'", url), ex);
            }
        }

        /// <summary>
        /// Make a call to crypto API.
        /// </summary>
        private async Task<T> Call<T>(string method, params object[] parameters)
        {
            var rpcReq = new JsonRpcRequest(1, method, parameters);

            var serialized = JsonConvert.SerializeObject(rpcReq);

            // serialize json for the request
            var byteArray = Encoding.UTF8.GetBytes(serialized);

            using (var request = new StreamContent(new MemoryStream(byteArray)))
            {
                request.Headers.ContentType = new MediaTypeHeaderValue("application/json-rpc");

                var response = await Send(this.Client, this.Url, request);
                var ret = await this.CheckResponseOk<T>(response);

                return ret;
            }
        }

        /// <summary>
        /// Make a call to crypto API.
        /// </summary>
        private async Task Call(string method, params object[] parameters)
        {
            var rpcReq = new JsonRpcRequest(1, method, parameters);

            var s = JsonConvert.SerializeObject(rpcReq);

            // serialize json for the request
            var byteArray = Encoding.UTF8.GetBytes(s);

            using (var request = new StreamContent(new MemoryStream(byteArray)))
            {
                request.Headers.ContentType = new MediaTypeHeaderValue("application/json-rpc");

                var response = await Send(this.Client, this.Url, request);
                await this.CheckResponseOk<string>(response);
            }
        }

        /// <summary>
        /// Check the crypto client response is ok.
        /// </summary>
        private async Task<T> CheckResponseOk<T>(HttpResponseMessage response)
        {
            try
            {
                using (var jsonStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var jsonStreamReader = new StreamReader(jsonStream))
                    {
                        var ret = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(await jsonStreamReader.ReadToEndAsync());

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            var code = ret != null && ret.Error != null ? ret.Error.Code : 0;
                            var msg = ret != null && ret.Error != null ? ret.Error.Message : "Error";

                            throw CreateException(response, code, msg);
                        }

                        return ret.Result;
                    }
                }
            }
            catch (BitcoinClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BitcoinClientException(string.Format("Failed parsing the result, StatusCode={0}, row message={1}", response.StatusCode, response.Content.ReadAsStringAsync().Result), ex);
            }
        }

        #endregion
    }
}