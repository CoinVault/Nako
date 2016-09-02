// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BitcoinClientException.cs" company="SoftChains">
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
    using System.Net;

    #endregion

    /// <summary>
    /// The bit net client exception.
    /// </summary>
    public class BitcoinClientException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinClientException"/> class.
        /// </summary>
        public BitcoinClientException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinClientException"/> class.
        /// </summary>
        public BitcoinClientException(string message)
            : base(message)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("StatusCode='{0}' Error={1}", this.StatusCode, base.Message);
            }
        }

        public string RawMessage { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return string.Format("StatusCode = {0} Error = {1} {2}", this.StatusCode, this.RawMessage, base.ToString());
        }

        public bool IsTransactionNotFound()
        {
            if (this.ErrorCode == -5)
            {
                if (this.ErrorMessage == "No information available about transaction")
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}