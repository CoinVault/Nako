// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Sql
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Linq;

    #endregion

    /// <summary>
    /// This class defines extension methods.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Execute a stored procedure and enumerate it.
        /// </summary>
        /// <param name="storedProcedure">
        /// The stored procedure.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public static IEnumerable<SqlDataReader> EnumerateReader(this StoredProcedure storedProcedure)
        {
            // Validate
            if (storedProcedure == null)
            {
                throw new ArgumentNullException("storedProcedure");
            }

            var reader = storedProcedure.ExecuteReader();

            while (reader.Read())
            {
                yield return reader;
            }
        }

        /// <summary>
        /// Get the value of a field in a database data reader or its (default) null value.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="i">
        /// The position in the reader..
        /// </param>
        /// <typeparam name="T">
        /// The value type.
        /// </typeparam>
        /// <returns>
        /// The type instance or null.
        /// </returns>
        public static T GetFieldValueOrDefault<T>(this SqlDataReader reader, int i)
        {
            // Validate
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
           
            return reader.IsDBNull(i) ? default(T) : reader.GetFieldValue<T>(i);
        }

        /// <summary>
        /// Get the value of a field in a data reader or its (default) null value.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="i">
        /// The position in the reader..
        /// </param>
        /// <typeparam name="T">
        /// The value type.
        /// </typeparam>
        /// <returns>
        /// The type instance or null.
        /// </returns>
        public static T GetEnumValueOrDefault<T>(this SqlDataReader reader, int i) 
        {
            // Validate
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (reader.IsDBNull(i))
            {
                return default(T);
            }

            var enumType = typeof(T);

            if (!enumType.IsEnum && (enumType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                enumType = enumType.GenericTypeArguments.SingleOrDefault(s => s.IsEnum);
            }

            if (enumType == null || !enumType.IsEnum)
            {
                throw new InvalidEnumArgumentException();
            }

            return (T)Enum.ToObject(enumType, reader.GetValue(i));
        }

        #endregion
    }
}