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
    using Microsoft.AspNetCore.Mvc;
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
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class StatsController : Controller
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

        [HttpGet]
        [Route("[action]")]
        public IActionResult Heartbeat()
        {
            var response = this.CreateOkResponse("Heartbeat");

            return response;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Connections()
        {
            var ret = await this.statsHandler.StatsConnection();

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var ret = await this.statsHandler.Statistics();

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Peers()
        {
            var ret = await this.statsHandler.Peers();

            var response = this.CreateOkResponse(ret);

            return response;
        }

        #endregion
    }
}
