using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class BaseDbCodeTable : CodeTable, IDisposable
    {
        private readonly DataSet fDataSet;
        private DataTable fDataTable;
        private bool fFilled;
        private string fNameExpression;
        private DecoderNameExpression fExpression;

        protected BaseDbCodeTable()
        {
            fDataSet = new DataSet() { Locale = ObjectUtil.SysCulture };
            NameExpression = "{Name}";
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        public string ContextName { get; set; }

        internal DecoderNameExpression InternalExpression
        {
            get
            {
                return fExpression;
            }
        }

        public string NameExpression
        {
            get
            {
                return fNameExpression;
            }
            set
            {
                if (fNameExpression != value)
                {
                    fNameExpression = value;
                    fExpression = new DecoderNameExpression(fNameExpression);
                }
            }
        }

        private void FillData(object[] args)
        {
            if (!fFilled)
            {
                TkDbContext context = ObjectUtil.QueryObject<TkDbContext>(args);
                bool createContext = false;
                if (!string.IsNullOrEmpty(ContextName))
                {
                    if (context.ContextConfig.Name != ContextName)
                    {
                        createContext = true;
                        context = DbContextUtil.CreateDbContext(ContextName);
                    }
                }
                else if (context == null)
                {
                    createContext = true;
                    context = DbContextUtil.CreateDefault();
                }

                try
                {
                    fDataTable = FillDbData(context, fDataSet);
                    DataSetUtil.SetPrimaryKey(fDataTable,
                        fDataTable.Columns[DecoderConst.CODE_NICK_NAME]);
                    fFilled = true;
                }
                finally
                {
                    if (createContext)
                        context.Dispose();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                fDataSet.Dispose();
            }
        }

        protected abstract DataTable FillDbData(TkDbContext context, DataSet dataSet);

        public override IDecoderItem Decode(string code, params object[] args)
        {
            FillData(args);

            DataRow row;
            try
            {
                row = fDataTable.Rows.Find(code);
            }
            catch
            {
                row = null;
            }
            if (row == null)
                return null;
            else
            {
                DataRowDecoderItem item = new DataRowDecoderItem(fDataTable.Columns, row,
                    DecoderConst.CODE_NICK_NAME, DecoderConst.NAME_NICK_NAME);
                return item;
            }
        }

        public override void Fill(params object[] args)
        {
            FillData(args);

            DataSet ds = ObjectUtil.QueryObject<DataSet>(args);
            if (ds != null)
            {
                string regName = RegName;
                if (!ds.Tables.Contains(regName))
                {
                    DataTable table = fDataTable.Copy();
                    ds.Tables.Add(table);
                }
                return;
            }

            CodeTableContainer container = ObjectUtil.QueryObject<CodeTableContainer>(args);
            if (container != null)
            {
                List<CodeItem> result = new List<CodeItem>(fDataTable.Rows.Count);
                foreach (DataRow row in fDataTable.Rows)
                {
                    CodeItem item = new CodeItem(row[DecoderConst.CODE_NICK_NAME].ToString(),
                        row[DecoderConst.NAME_NICK_NAME].ToString());
                    result.Add(item);
                }
                container.Add(RegName, result);
            }
        }
    }
}