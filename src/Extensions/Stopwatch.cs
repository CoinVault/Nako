// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stopwatch.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Extensions
{
    public class Stopwatch
    {
        #region Public Methods and Operators

        public static System.Diagnostics.Stopwatch Start()
        {
            var stoper = new System.Diagnostics.Stopwatch();
            stoper.Start();
            return stoper;
        }

        #endregion
    }
}
