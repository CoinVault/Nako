// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandController.cs" company="SoftChains">
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

    using Nako.Api;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using System.IO;
    using System.Text;

    #endregion

    /// <summary>
    /// Controller to get some information about a coin.
    /// </summary>
    [Route("api/[controller]")]
    public class CommandController : Controller
    {
        private readonly CommandHandler commandHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandController"/> class.
        /// </summary>
        public CommandController(CommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        #region Public Methods and Operators

        [HttpPost("[action]")]
        public async Task<IActionResult> Send()
        {
            string data = null;
            
            using (StreamReader reader = new StreamReader(this.Request.Body,Encoding.UTF8))
            {
                data = reader.ReadToEnd();
            }

            var trx = data;

            var ret = await this.commandHandler.SendTransaction(trx);
            var response = new OkObjectResult(ret);

            return response;
        }
        
        #endregion
    }
}
