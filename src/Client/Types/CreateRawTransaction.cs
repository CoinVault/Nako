// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateRawTransaction.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Client.Types
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class CreateRawTransaction
    {
        #region Constructors and Destructors

        public CreateRawTransaction()
        {
            this.Inputs = new List<CreateRawTransactionInput>();
            this.Outputs = new Dictionary<string, decimal>();
        }

        #endregion

        #region Public Properties

        public List<CreateRawTransactionInput> Inputs { get; set; }

        public Dictionary<string, decimal> Outputs { get; set; }

        #endregion

        #region Public Methods and Operators

        public void AddInput(string transactionId, int output)
        {
            this.Inputs.Add(new CreateRawTransactionInput { TransactionId = transactionId, Output = output });
        }

        public void AddOutput(string address, decimal amount)
        {
            this.Outputs.Add(address, amount);
        }

        public void ReduceFeeFromAddress(string address, decimal fee)
        {
            var output = this.Outputs.First(f => f.Key == address);
            var amont = output.Value;
            this.Outputs[output.Key] = amont - fee;
        }

        #endregion
    }
}
