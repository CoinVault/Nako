// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatsController.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers
{
    #region Using Directives

    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using System.Web.Http;

    #endregion

    /// <summary>
    /// Controller to get some information about a coin.
    /// </summary>
    [RoutePrefix("api/stats")]
    public class StatsController : ApiController
    {
        private readonly StatsHandler statsHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsController"/> class.
        /// </summary>
        public StatsController(StatsHandler statsHandler)
        {
            this.statsHandler = statsHandler;
        }

        #region Public Methods and Operators

        [Route("heartbeat")]
        public HttpResponseMessage GetHeartbeat()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK, "Heartbeat", new JsonMediaTypeFormatter { Indent = true });
           
            return response;
        }

        [Route("connections")]
        public async Task<HttpResponseMessage> GetConnections()
        {
            var ret = await this.statsHandler.StatsConnection();

            var response = this.Request.CreateResponse(HttpStatusCode.OK, ret, new JsonMediaTypeFormatter { Indent = true });

            return response;
        }

        [Route("")]
        public async Task<HttpResponseMessage> Get()
        {
            var ret = await this.statsHandler.Statistics();

            var response = this.Request.CreateResponse(HttpStatusCode.OK, ret, new JsonMediaTypeFormatter { Indent = true });

            return response;
        }

        [Route("peers")]
        public async Task<HttpResponseMessage> GetPeers()
        {
            var ret = await this.statsHandler.Peers();

            var response = this.Request.CreateResponse(HttpStatusCode.OK, ret, new JsonMediaTypeFormatter { Indent = true });

            return response;
        }

        #endregion
    }
}
