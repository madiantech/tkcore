using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class AbstractStoredProc : IDisposable
    {
        private IDbDataAdapter fAdapter;
        private IDbConnection fConnection;
        private bool fUseContext;

        protected AbstractStoredProc(string procName)
        {
            TkDebug.AssertArgumentNullOrEmpty(procName, "procName", null);

            ProcName = procName;
        }

        protected AbstractStoredProc(string procName, IDbConnection connection)
            : this(procName)
        {
            TkDebug.AssertArgumentNull(connection, "connection", null);

            Connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the StoredProc class.
        /// </summary>
        protected AbstractStoredProc(string procName, TkDbContext context)
            : this(procName)
        {
            TkDebug.AssertArgumentNull(context, "context", null);

            Context = context;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public string ProcName { get; private set; }

        public TkDbContext Context { get; set; }

        public IDbConnection Connection { get; set; }

        protected IDbCommand Command { get; private set; }

        private void CreateStoredProc()
        {
            if (Command == null)
            {
                if (Context != null)
                {
                    fUseContext = true;
                    fConnection = Context.DbConnection;
                    Command = Context.CreateCommand();
                }
                else if (Connection != null)
                {
                    fConnection = Connection;
                    Command = Connection.CreateCommand();
                    fUseContext = false;
                }
                else
                    TkDebug.ThrowToolkitException(
                        string.Format(ObjectUtil.SysCulture,
                        "存储过程{0}没有设定DbConnection或者DbContext，请检查", ProcName), this);

                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = ProcName;

                PrepareParameters();
            }
            else
                SetInputValues();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Command.DisposeObject();
                fAdapter.DisposeObject();
            }
        }

        protected abstract void PrepareParameters();

        protected abstract void SetInputValues();

        protected abstract void SetOutputValues();

        internal StoredProcParameter Add(string name, ParameterDirection direction, TkDataType type)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            StoredProcParameter param = new StoredProcParameter(name, direction, type);
            return param;
        }

        internal IDbDataParameter CreateDataParameter(StoredProcParameter param)
        {
            IDbDataParameter result;
            if (fUseContext)
                result = Context.CreateParameter(param.Type);
            else
            {
                result = Command.CreateParameter();
                result.DbType = DataSetUtil.ConvertTkDataTypeToDbType(param.Type);
            }
            param.FillDataParameter(result);

            return result;
        }

        public void Execute()
        {
            CreateStoredProc();
            DbConnectionStatus item = new DbConnectionStatus(fConnection);
            item.OpenDbConnection();
            try
            {
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "执行存储过程:{0}出错", Command.CommandText), ex, null);
            }
            finally
            {
                item.CloseDbConnection();
            }
            SetOutputValues();
        }

        public void Execute(DataSet dataSet)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);

            CreateStoredProc();

            TkDebug.Assert(fUseContext, string.Format(ObjectUtil.SysCulture,
                "存储过程{0}没有设置Context，只设置了DbConnection，这种模式不支持从数据库选取数据集",
                ProcName), this);
            TkDebug.Assert(fConnection.State == ConnectionState.Closed,
                string.Format(ObjectUtil.SysCulture,
                "存储过程{0}在填充模式下，数据库连接必须是关闭的，请检查数据库连接的状态",
                ProcName), this);

            if (fAdapter == null)
            {
                fAdapter = Context.CreateDataAdapter();
                fAdapter.SelectCommand = Command;
            }
            DbUtil.FillDataSet(this, fAdapter, dataSet, ProcName);
            SetOutputValues();
        }

        internal void InternalExecute(ISqlDataAdapter selector)
        {
            TkDebug.AssertArgumentNull(selector, "selector", this);
            CreateStoredProc();

            TkDebug.Assert(fConnection.State == ConnectionState.Closed,
                string.Format(ObjectUtil.SysCulture,
                "存储过程{0}在填充模式下，数据库连接必须是关闭的，请检查数据库连接的状态",
                ProcName), this);

            selector.DataAdapter.SelectCommand = Command;
            DbUtil.FillDataSet(this, selector.DataAdapter, selector.DataSet, ProcName);
            SetOutputValues();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void Execute(TableSelector selector)
        {
            InternalExecute(selector);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void Execute(SqlSelector selector)
        {
            InternalExecute(selector);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(ProcName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture,
                "名称为{0}的存储过程", ProcName);
        }
    }
}
