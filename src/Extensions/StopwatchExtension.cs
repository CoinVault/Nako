// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopwatchExtension.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Extensions
{
    #region Using Directives

    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The stopwatch extension.
    /// </summary>
    public class StopwatchExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create and start.
        /// </summary>
        /// <returns>
        /// The <see cref="Stopwatch"/>.
        /// </returns>
        public static Stopwatch CreateAndStart()
        {
            var stoper = new Stopwatch();
            stoper.Start();
            return stoper;
        }

        #endregion
    }
}
