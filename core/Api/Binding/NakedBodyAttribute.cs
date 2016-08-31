// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiModule.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Http;

namespace Nako.Binding.Api
{
    /////// <summary>
    /////// An attribute that captures the entire content body and stores it
    /////// into the parameter of type string or byte[].
    /////// </summary>
    /////// <remarks>
    /////// The parameter marked up with this attribute should be the only parameter as it reads the
    /////// entire request body and assigns it to that parameter.    
    /////// Code taken from here 
    /////// https://weblog.west-wind.com/posts/2013/dec/13/accepting-raw-request-body-content-with-aspnet-web-api
    /////// </remarks>
    ////[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    ////public sealed class NakedBodyAttribute : ParameterBindingAttribute
    ////{
    ////    public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
    ////    {
    ////        if (parameter == null)
    ////            throw new ArgumentException("Invalid parameter");

    ////        return new NakedBodyParameterBinding(parameter);
    ////    }
    ////}
}
