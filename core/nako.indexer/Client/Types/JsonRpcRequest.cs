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

    public class JsonRpcRequest
    {
        #region Constructors and Destructors

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

        [JsonProperty(PropertyName = "id", Order = 2)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "method", Order = 0)]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params", Order = 1)]
        public IList<object> Parameters { get; set; }

        #endregion

        #region Public Methods and Operators

        public byte[] GetBytes()
        {
            string json = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(json);
        }

        #endregion
    }
}
