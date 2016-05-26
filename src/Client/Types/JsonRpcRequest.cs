// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonRpcRequest.cs" company="SoftChains">
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

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Class containing data sent to the Bitcoin wallet as a JSON RPC call.
    /// </summary>
    public class JsonRpcRequest
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcRequest"/> class. 
        /// Create a new JSON RPC request with the given id, method and optionally parameters.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public JsonRpcRequest(int id, string method, params object[] parameters)
        {
            this.Id = id;
            this.Method = method;

            if (parameters != null)
            {
                this.Parameters = parameters.ToList();
            }
            else
            {
                this.Parameters = new List<object>();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Id of the RPC call. This id will be returned in the response.
        /// </summary>
        [JsonProperty(PropertyName = "id", Order = 2)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the method to call on the Bitcoin wallet.
        /// </summary>
        [JsonProperty(PropertyName = "method", Order = 0)]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets A list of parameters to pass to the method.
        /// </summary>
        [JsonProperty(PropertyName = "params", Order = 1)]
        public IList<object> Parameters { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get the bytes of the JSON representation of this object.
        /// </summary>
        /// <returns>
        /// The request bytes.
        /// </returns>
        public byte[] GetBytes()
        {
            string json = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(json);
        }

        #endregion
    }
}
