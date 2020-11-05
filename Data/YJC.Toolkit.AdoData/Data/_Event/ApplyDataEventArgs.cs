using System;
using System.Transactions;

namespace YJC.Toolkit.Data
{
    public sealed class ApplyDataEventArgs : EventArgs
    {
        public ApplyDataEventArgs(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Transaction Transaction { get; private set; }
    }
}
