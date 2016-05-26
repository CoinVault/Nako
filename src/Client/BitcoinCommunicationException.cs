// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BitcoinCommunicationException.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The client communication exception.
    /// </summary>
    public class BitcoinCommunicationException : ApplicationException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinCommunicationException"/> class.
        /// </summary>
        public BitcoinCommunicationException(string message, Exception ex)
            : base(message, ex)
        {
        }

        #endregion
    }
}
