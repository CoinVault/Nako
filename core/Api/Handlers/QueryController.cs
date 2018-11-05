﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryController.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Cors;

namespace Nako.Api.Handlers
{
    using Microsoft.AspNetCore.Mvc;
    #region Using Directives

    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    #endregion

    /// <summary>
    /// Controller to expose an api that queries the blockchain.
    /// </summary>
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        private readonly QueryHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryController"/> class.
        /// </summary>
        public QueryController(QueryHandler queryHandler)
        {
            this.handler = queryHandler;
        }

        #region Public Methods and Operators

        [Route("address/{address}/confirmations/{confirmations:long=0}/transactions")]
        public IActionResult GetAddressTransactions(string address, long confirmations)
        {
            var ret = this.handler.GetAddressTransactions(address, confirmations);

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [Route("address/{address}/confirmations/{confirmations:long=0}")]
        public IActionResult GetAddress(string address, long confirmations)
        {
            var ret = this.handler.GetAddress(address, confirmations);

            var response = this.CreateOkResponse(ret);
           
            return response;
        }

        [Route("address/{address}/confirmations/{confirmations:long=0}/unspent/transactions")]
        public IActionResult GetAddressUtxoTransactions(string address, long confirmations)
        {
            var ret = this.handler.GetAddressUtxoTransactions(address, confirmations);

            var response = this.CreateOkResponse(ret);
           
            return response;
        }

        [Route("address/{address}/confirmations/{confirmations:long=0}/unspent")]
        public IActionResult GetAddressUtxo(string address, long confirmations)
        {
            var ret = this.handler.GetAddressUtxo(address, confirmations);

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [Route("address/{address}/confirmations/{confirmations:long=0}/unspent/confirmed")]
        public IActionResult GetAddressUtxoConfirmedTransactions(string address, long confirmations)
        {
            var ret = this.handler.GetAddressUtxoConfirmedTransactions(address, confirmations);

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [Route("address/{address}/confirmations/{confirmations:long}/unspent/unconfirmed")]
        public IActionResult GetAddressUtxoUnconfirmedTransactions(string address, long confirmations)
        {
            var ret = this.handler.GetAddressUtxoUnconfirmedTransactions(address, confirmations);

            var response = this.CreateOkResponse(ret);
          
            return response;
        }

        [Route("address/{address}/unspent")]
        public IActionResult GetAddressUtxo(string address)
        {
            var ret = this.handler.GetAddressUtxo(address, 0);

            var response = this.CreateOkResponse(ret);
            
            return response;
        }

        [Route("address/{address}")]
        public IActionResult GetAddress(string address)
        {
            var ret = this.handler.GetAddress(address, 0);

            var response = this.CreateOkResponse(ret);
           
            return response;
        }

        [Route("address/{address}/unspent/transactions")]
        public IActionResult GetAddressUtxoTransactions(string address)
        {
            var ret = this.handler.GetAddressUtxoTransactions(address, 0);

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [Route("address/{address}/transactions")]
        public IActionResult GetAddressTransactions(string address)
        {
            var ret = this.handler.GetAddressTransactions(address, 0);

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [Route("block/Latest/{transactions?}")]
        public IActionResult GetBlock(string transactions = null)
        {
            var ret = this.handler.GetLastBlock(!string.IsNullOrEmpty(transactions));

            if (ret == null)
            {
                return new NotFoundResult();
            }

            var response = this.CreateOkResponse(ret);
           
            return response;
        }

        [Route("block/{blockHash}/{transactions?}")]
        public IActionResult GetBlockByHash(string blockHash, string transactions = null)
        {
            var ret = this.handler.GetBlock(blockHash, !string.IsNullOrEmpty(transactions));

            if (ret == null)
            {
                return new NotFoundResult();
            }

            var response = this.CreateOkResponse(ret);
            
            return response;
        }

        [Route("block/Index/{blockIndex}/{transactions?}")]
        public IActionResult GetBlockByHash(long blockIndex, string transactions = null)
        {
            var ret = this.handler.GetBlock(blockIndex, !string.IsNullOrEmpty(transactions));

            if (ret == null)
            {
                return new NotFoundResult();
            }

            var response = this.CreateOkResponse(ret);

            return response;
        }

        [Route("transaction/{transactionId}")]
        public IActionResult GetTransaction(string transactionId)
        {
            var ret = this.handler.GetTransaction(transactionId);

            if (ret == null)
            {
                return new NotFoundResult();
            }

            var response = this.CreateOkResponse(ret);

            return response;
        }

        #endregion
    }
}
