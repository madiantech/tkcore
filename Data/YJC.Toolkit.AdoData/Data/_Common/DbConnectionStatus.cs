using System.Data;
using System.Data.Common;
using System.Transactions;

namespace YJC.Toolkit.Data
{
    internal sealed class DbConnectionStatus
    {
        public IDbConnection Connection { get; private set; }

        public bool IsOpen { get; private set; }

        public DbConnectionStatus(IDbConnection connection)
        {
            Connection = connection;
            IsOpen = connection.State == ConnectionState.Open;
        }

        public void OpenDbConnection()
        {
            if (!IsOpen)
                Connection.Open();
        }

        public void CloseDbConnection()
        {
            if (!IsOpen)
                Connection.Close();
        }

        public void AttachTransaction(Transaction transaction)
        {
            OpenDbConnection();
            EnlistTransaction(transaction);
        }

        public void DetachTransaction()
        {
            if (!IsOpen)
                Connection.Close();
            else
                EnlistTransaction(null);
        }

        private void EnlistTransaction(Transaction transaction)
        {
            (Connection as DbConnection).EnlistTransaction(transaction);
        }
    }
}
