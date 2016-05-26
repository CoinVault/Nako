// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extension.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api
{
    #region Using Directives

    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;

    #endregion

    public static class Extension
    {
        public static HttpResponseMessage CreateOkResponse<T>(this HttpRequestMessage request, T value)
        {
            // the indentation is to format the json response so its human readable.
            return request.CreateResponse(HttpStatusCode.OK, value, new JsonMediaTypeFormatter { Indent = true });
        }
    }
}
