// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnixUtils.cs" company="SoftChains">
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

    using System;

    #endregion

    /// <summary>
    /// Internal class providing certain utility functions to other classes.
    /// </summary>
    internal sealed class UnixUtils
    {
        #region Static Fields

        /// <summary>
        /// The Unix start date.
        /// </summary>
        private static readonly DateTime UnixStartDate = new DateTime(1970, 1, 1, 0, 0, 0);

        #endregion

        #region Methods

        /// <summary>
        /// Converts a <see cref="DateTime"/> object into a unix timestamp number.
        /// </summary>
        /// <param name="date">
        /// The date to convert.
        /// </param>
        /// <returns>
        /// A long for the number of seconds since 1st January 1970, as per unix specification.
        /// </returns>
        internal static long DateToUnixTimestamp(DateTime date)
        {
            var ts = date - UnixStartDate;
            return (long)ts.TotalSeconds;
        }

        /// <summary>
        /// Converts a string, representing a unix timestamp number into a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="timestamp">
        /// The timestamp, as a string.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/> object the time represents.
        /// </returns>
        internal static DateTime UnixTimestampToDate(string timestamp)
        {
            if (string.IsNullOrEmpty(timestamp))
            {
                return DateTime.MinValue;
            }

            return UnixTimestampToDate(long.Parse(timestamp));
        }

        /// <summary>
        /// Converts a <see cref="long"/>, representing a unix timestamp number into a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="timestamp">
        /// The unix timestamp.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/> object the time represents.
        /// </returns>
        internal static DateTime UnixTimestampToDate(long timestamp)
        {
            return UnixStartDate.AddSeconds(timestamp);
        }

        #endregion
    }
}