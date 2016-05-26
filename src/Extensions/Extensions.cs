// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="SoftChains">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    #endregion

    /// <summary>
    /// This class defines extension methods.
    /// </summary>
    [DebuggerStepThrough]
    public static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The unix time stamp to date time.
        /// </summary>
        /// <param name="unixTimeStamp">
        /// The unix time stamp.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return UnixUtils.UnixTimestampToDate(unixTimeStamp);
        }

        /// <summary>
        /// Batch a collection in to fixed size batches.
        /// </summary>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="batchSize">
        /// The batch size.
        /// </param>
        /// <returns>
        /// The batch.
        /// </returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            //// consider using the System.Collections.Concurrent.Partitioner
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, batchSize - 1);
                }
            }
        }

        /// <summary>
        /// Implemented a foreach extension for an enumerable object.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The first type.
        /// </typeparam>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            // Validate
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Implemented a foreach extension for an enumerable object.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            // Validate
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var index = 0;
            foreach (var item in source)
            {
                action(item, index);
                index++;
            }
        }

        /// <summary>
        /// Split a collection in to batches of collections.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object in the source.
        /// </typeparam>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="parts">
        /// The number of parts.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Intended design.")]
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int parts)
        {
            // Validate
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            parts = parts == 0 ? 1 : parts;
            var i = 0;
            var splits = from item in source group item by i++ % parts into part select part.AsEnumerable();
            return splits;
        }

        /// <summary>
        /// The take and remove.
        /// </summary>
        /// <param name="queue">
        /// The queue.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <typeparam name="T">
        /// The item type.
        /// </typeparam>
        /// <returns>
        /// The collection.
        /// </returns>
        public static IEnumerable<T> TakeAndRemove<T>(Queue<T> queue, int count)
        {
            var queuecount = Math.Min(queue.Count, count);
            for (var i = 0; i < queuecount; i++)
            {
                yield return queue.Dequeue();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inner method to batch a collection in to fixed size batches.
        /// </summary>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="batchSize">
        /// The batch size.
        /// </param>
        /// <returns>
        /// The batch.
        /// </returns>
        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
            {
                yield return source.Current;
            }
        }

        #endregion
    }
}