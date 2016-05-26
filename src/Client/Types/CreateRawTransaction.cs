
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
