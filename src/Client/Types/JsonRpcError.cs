// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonRpcError.cs" company="SoftChains">
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

    public class JsonRpcError
    {
        #region Public Properties

        [JsonProperty(PropertyName = "code", Order = 0)]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message", Order = 1)]
        public string Message { get; set; }

        #endregion
    }
}
