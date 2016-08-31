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
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    #region Using Directives

    using System.Net;
    //using System.Net.Http;
    using System.Net.Http.Formatting;

    #endregion

    public static class Extension
    {
        public static IActionResult CreateOkResponse<T>(this Controller controller, T value)
        {
            return new OkObjectResult(value);

            // the indentation is to format the json response so its human readable.
            //return (request as HttpRequestMessage).CreateResponse(HttpStatusCode.OK, value, new JsonMediaTypeFormatter { Indent = true });
        }
    }
}
