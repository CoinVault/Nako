// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonRpcResponse.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client.Types
{
    #region Using Directives

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Class containing the information contained in a JSON RPC response received from
    /// the Bitcoin wallet.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the result object. This can simply be a string like a transaction id, 
    /// or a more complex object like TransactionDetails.
    /// </typeparam>
    public class JsonRpcResponse<T>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcResponse{T}"/> class. 
        /// Create a new JSON RPC response with the given id, error and result object.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="result">
        /// The result object.
        /// </param>
        public JsonRpcResponse(int id, JsonRpcError error, T result)
        {
            this.Id = id;
            this.Error = error;
            this.Result = result;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the error returned by the wallet, if any.
        /// </summary>
        [JsonProperty(PropertyName = "error", Order = 2)]
        public JsonRpcError Error { get; set; }

        /// <summary>
        /// Gets or sets the id of the corresponding request.
        /// </summary>
        [JsonProperty(PropertyName = "id", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the result object.
        /// </summary>
        [JsonProperty(PropertyName = "result", Order = 0)]
        public T Result { get; set; }

        #endregion
    }
}
