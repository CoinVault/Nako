// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Notifier.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync.SyncTasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Security;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations.Types;
    using Nako.Storage;
    using Newtonsoft.Json;

    public class Notifier : TaskRunner<AddressNotifications>
    {
        private readonly ILogger<Notifier> log;

        private readonly NakoConfiguration configuration;

        private readonly IStorage storage;

        private readonly Lazy<HttpClient> client;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class. 
        /// </summary>
        public Notifier(IOptions<NakoConfiguration> configuration, ILogger<Notifier> logger, IStorage storage)
            : base(configuration, logger)
        {
            this.configuration = configuration.Value;
            this.log = logger;
            this.storage = storage;
            this.client = new Lazy<HttpClient>(() => new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, certificate, chain, errors) => errors == SslPolicyErrors.None || errors == SslPolicyErrors.RemoteCertificateNameMismatch }));
        }

        /// <inheritdoc />
        public override async Task<bool> OnExecute()
        {
            if (this.configuration.NotifyBatchCount == 0)
            {
                this.Abort = true;
                return true;
            }

            AddressNotifications item;
            if (this.TryDequeue(out item))
            {
                var stoper = Stopwatch.Start();

                var queue = new Queue<string>(item.Addresses);
                var total = queue.Count();
                var sendCount = 0;
                do
                {
                    var addresses = Extensions.TakeAndRemove(queue, this.configuration.NotifyBatchCount).ToList();

                    var coin = new CoinAddressInfo
                    {
                        CoinTag = this.configuration.CoinTag, 
                        Address = addresses.ToList()
                    };

                    try
                    {
                        await this.client.Value.PostAsJsonAsync(this.configuration.NotifyUrl, coin);
                        sendCount++;
                    }
                    catch (Exception ex)
                    {
                        this.log.LogError(ex, "Notifier");
                        this.Abort = true;
                        return false;
                    }
                }
                while (queue.Any());

                stoper.Stop();

                this.log.LogDebug($"Seconds = {stoper.Elapsed.TotalSeconds} - Total = {total} - Requests = {sendCount}");

                return true;
            }

            return false;
        }

        public class CoinAddressInfo
        {
            [JsonProperty("A")]
            public IEnumerable<string> Address { get; set; }

            [JsonProperty("C")]
            public string CoinTag { get; set; }
        }
    }
}
