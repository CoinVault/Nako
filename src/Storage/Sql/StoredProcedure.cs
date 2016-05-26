// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredProcedure.cs" company="SoftChains">
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
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    #endregion

    /// <summary>
    /// NOTE:The sql functionality is not maintained.
    /// </summary>
    public class StoredProcedure : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// The internal SQL command.
        /// </summary>
        private readonly SqlCommand command;

        /// <summary>
        /// The SQL connection for the stored procedure.
        /// </summary>
        private readonly ReliableSqlConnection connection;

        /// <summary>
        /// The RetryPolicy for the Stored Procedure.
        /// </summary>
        private readonly RetryPolicy retryPolicy;

        /// <summary>
        /// Identifies if the object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedure"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="procedureName">
        /// The name of the stored procedure.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Objects are disposed of in the Dispose method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Used to pass the SQL stored procedure name.")]
        public StoredProcedure(string connectionString, string procedureName)
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultSqlConnectionRetryPolicy();

            this.connection = new ReliableSqlConnection(connectionString, this.retryPolicy);

            this.command = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = procedureName, Connection = this.connection.Current, CommandTimeout = 600 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedure"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="procedureName">
        /// The name of the stored procedure.
        /// </param>
        /// <param name="retryPolicy">
        /// The retryPolicy.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Objects are disposed of in the Dispose method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Used to pass the SQL stored procedure name.")]
        public StoredProcedure(string connectionString, string procedureName, RetryPolicy retryPolicy)
        {
            // Use the passed in retry policy
            this.retryPolicy = retryPolicy;
 
            this.connection = new ReliableSqlConnection(connectionString, retryPolicy);

            this.command = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = procedureName, Connection = this.connection.Current, CommandTimeout = 600 };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an in parameter to the stored procedure.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter.
        /// </param>
        /// <param name="dataType">
        /// The data type of the parameter.
        /// </param>
        /// <param name="value">
        /// The parameters value.
        /// </param>
        public void AddIn(string name, SqlDbType dataType, object value)
        {
            // Add the parameter.
            this.command.Parameters.Add(new SqlParameter { Direction = ParameterDirection.Input, ParameterName = name, SqlDbType = dataType, Value = value ?? Convert.DBNull });
        }

        /// <summary>
        /// The add in decimal.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="precision">
        /// The precision.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public void AddInDecimal(string name, decimal? value, byte precision, byte scale)
        {
            // Add the parameter.
            this.command.Parameters.Add(new SqlParameter { Direction = ParameterDirection.Input, ParameterName = name, SqlDbType = SqlDbType.Decimal, Value = value, Precision = precision, Scale = scale });
        }

        /// <summary>
        /// Adds an out parameter to the stored procedure.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter.
        /// </param>
        /// <param name="dataType">
        /// The data type of the parameter.
        /// </param>
        public void AddOut(string name, SqlDbType dataType)
        {
            // Add the parameter.
            this.command.Parameters.Add(new SqlParameter { Direction = ParameterDirection.Output, ParameterName = name, SqlDbType = dataType });
        }

        /// <summary>
        /// Disposes the <see cref="StoredProcedure"/> object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Runs the stored procedure.
        /// </summary>
        /// <returns>The amount of rows effected.</returns>
        public int ExecuteNonQuery()
        {
            // Check the connection state.
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }

            this.command.Connection = this.connection.Current;

            return this.command.ExecuteNonQueryWithRetry(this.retryPolicy);
        }

        /// <summary>
        /// Runs the stored procedure.
        /// </summary>
        /// <returns>The amount of rows effected.</returns>
        public async Task<int> ExecuteNonQueryAsync()
        {
            // Check the connection state.
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }

            this.command.Connection = this.connection.Current;

            return await this.retryPolicy.ExecuteAsync(async () => await this.command.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// Runs the stored procedure.
        /// </summary>
        /// <returns>The data reader for stored procedure.</returns>
        public SqlDataReader ExecuteReader()
        {
            // Check the connection state.
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }

            this.command.Connection = this.connection.Current;

            return this.command.ExecuteReaderWithRetry(this.retryPolicy);
        }

        /// <summary>
        /// Runs the stored procedure.
        /// </summary>
        /// <returns>The data reader for stored procedure.</returns>
        public async Task<SqlDataReader> ExecuteReaderAsync()
        {
            // Check the connection state.
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }

            this.command.Connection = this.connection.Current;

            return await this.retryPolicy.ExecuteAsync(async () => await this.command.ExecuteReaderAsync());
        }

        /// <summary>
        /// Gets an out parameter to the stored procedure.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter.
        /// </param>
        /// <returns>
        /// The <see cref="SqlParameter"/>.
        /// </returns>
        public SqlParameter GetOut(string name)
        {
            if (!this.command.Parameters.Contains(name))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "SqlParameterNotFound", name));
            }

            return this.command.Parameters[name];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose of the this object.
        /// </summary>
        /// <param name="disposing">
        /// Indicates if the object is being disposed.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.command != null)
                    {
                        this.command.Dispose();
                    }

                    if (this.connection != null)
                    {
                        this.connection.Dispose();
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}