// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncRestartException.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The sync restart exception.
    /// </summary>
    public class SyncRestartException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncRestartException"/> class.
        /// </summary>
        public SyncRestartException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncRestartException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public SyncRestartException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncRestartException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public SyncRestartException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
