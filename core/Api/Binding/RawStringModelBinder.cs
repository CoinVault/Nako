// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiModule.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nako.core.Api.Binding
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    #endregion

    public class RawStringModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            using (var memoryStream = new MemoryStream())
            {
                bindingContext.HttpContext.Request.Body.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var rdr = new StreamReader(memoryStream))
                {
                    var resut = rdr.ReadToEnd();
                    bindingContext.Result = ModelBindingResult.Success(resut);
                    return TaskCache.CompletedTask;
                }
            }
        }
    }
}