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

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using nako.core.Api.Binding;

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
        public async Task<IActionResult> Send([ModelBinder(BinderType = typeof(RawStringModelBinder))] string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                // http://stackoverflow.com/questions/9454811/which-http-status-code-to-use-for-required-parameters-not-provided
                return new StatusCodeResult(422);
            }

            var trx = data;

            var ret = await this.commandHandler.SendTransaction(trx);
            var response = new OkObjectResult(ret);

            return response;
        }
        
        #endregion
    }
}
