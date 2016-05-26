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

    public class JsonRpcResponse<T>
    {
        #region Constructors and Destructors

        public JsonRpcResponse(int id, JsonRpcError error, T result)
        {
            this.Id = id;
            this.Error = error;
            this.Result = result;
        }

        #endregion

        #region Public Properties

        [JsonProperty(PropertyName = "error", Order = 2)]
        public JsonRpcError Error { get; set; }

        [JsonProperty(PropertyName = "id", Order = 1)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "result", Order = 0)]
        public T Result { get; set; }

        #endregion
    }
}
