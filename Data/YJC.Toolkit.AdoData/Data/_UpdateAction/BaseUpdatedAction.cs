using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseUpdatedAction
    {
        protected BaseUpdatedAction()
        {
        }

        public DataSet DataSet { get; private set; }

        public DataTable Table { get; private set; }

        public DataRow Row { get; private set; }

        public TableResolver Resolver { get; private set; }

        protected abstract void Execute();

        public void DoAction(DataSet dataSet, TableResolver resolver)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", null);

            DataSet = dataSet;
            Resolver = resolver;
            if (resolver != null)
            {
                Table = dataSet.Tables[resolver.TableName];
                if (Table != null && Table.Rows.Count > 0)
                    Row = Table.Rows[0];
            }
            try
            {
                Execute();
            }
            catch (Exception e)
            {
                throw new WebPostException("保存成功,后续操作失败,原因:" + e.Message);
            }
        }
    }
}