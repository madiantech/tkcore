using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class BaseDbEasySearch : EasySearch, IDisposable
    {
        private const string EASY_TABLE = "_EasySearch";

        private ISearch fSearch;
        private readonly DataSet fDataSet;
        private string fNameExpression;
        private DecoderNameExpression fExpression;
        private readonly Dictionary<string, IDecoderItem> fValues;
        private DecoderNameExpression fDisplayExpression;
        private string fDisplayNameExpression;

        protected BaseDbEasySearch()
        {
            fDataSet = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
            fValues = new Dictionary<string, IDecoderItem>();
            NameExpression = "{Name}";
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public string ContextName { get; set; }


        public string DisplayNameExpression
        {
            get
            {
                return fDisplayNameExpression;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && fDisplayNameExpression != value)
                {
                    fDisplayNameExpression = value;
                    fDisplayExpression = new DecoderNameExpression(fDisplayNameExpression);
                }
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
                if (!string.IsNullOrEmpty(value) && fNameExpression != value)
                {
                    fNameExpression = value;
                    fExpression = new DecoderNameExpression(fNameExpression);
                }
            }
        }

        public MarcoConfigItem FilterSql { get; protected set; }

        public IDataRight DataRight { get; protected set; }

        public IActiveData ActiveData { get; protected set; }

        public string OrderBy { get; protected set; }

        public ISearch SearchMethod
        {
            get
            {
                if (fSearch == null)
                    SearchMethod = ClassicSearch.Instance;
                return fSearch;
            }
            set
            {
                if (fSearch == value)
                    return;
                fSearch = value;
                //fLinqSearch = value as ILinqSearch;
            }
        }

        private void GetContext(object[] args, out TkDbContext context, out bool createContext)
        {
            context = ObjectUtil.QueryObject<TkDbContext>(args);
            createContext = false;
            if (!string.IsNullOrEmpty(ContextName))
            {
                if (context.ContextConfig.Name != ContextName)
                {
                    createContext = true;
                    context = DbContextUtil.CreateDbContext(ContextName);
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

        protected virtual IParamBuilder CreateAdditionCondition(TkDbContext context, IFieldInfoIndexer indexer)
        {
            TkDebug.AssertArgumentNull(context, "context", this);
            TkDebug.AssertArgumentNull(indexer, "indexer", this);

            ParamBuilderContainer result = new ParamBuilderContainer();
            if (FilterSql != null)
            {
                string sql = Expression.Execute(FilterSql, context, indexer);
                result.Add(sql);
            }
            if (DataRight != null)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                IUserInfo userInfo = BaseGlobalVariable.Current.UserInfo;
                IParamBuilder builder = DataRight.GetListSql(new ListDataRightEventArgs(context, userInfo, indexer));
                result.Add(builder);
            }
            if (ActiveData != null)
            {
                IParamBuilder builder = ActiveData.CreateParamBuilder(context, indexer);
                result.Add(builder);
            }
            if (result.IsEmpty)
                return null;
            return result;
        }

        protected virtual IParamBuilder GetRefParamBuilder(TkDbContext context,
            List<EasySearchRefField> refFields)
        {
            return null;
        }

        protected abstract DataRow FetchRow(string code, TkDbContext context, DataSet dataSet);

        protected abstract DataTable FetchRows(TkDbContext context, DataSet dataSet, IParamBuilder builder, int topCount);

        public override IEnumerable<IDecoderItem> Search(SearchField field,
            string text, List<EasySearchRefField> refFields)
        {
            IFieldInfo fieldName = null;
            switch (field)
            {
                case SearchField.Value:
                case SearchField.DefaultValue:
                    fieldName = ValueField;
                    break;
                case SearchField.Name:
                    fieldName = NameField;
                    break;
                case SearchField.Pinyin:
                    fieldName = PinyinField ?? NameField;
                    break;
            }
            TkDbContext context = string.IsNullOrEmpty(ContextName) ? DbContextUtil.CreateDefault()
                : DbContextUtil.CreateDbContext(ContextName);
            using (context)
            {
                IParamBuilder builder = SearchMethod.Search(this, field, context, fieldName, text);
                if (refFields != null && refFields.Count > 0)
                {
                    IParamBuilder refBuilder = GetRefParamBuilder(context, refFields);
                    builder = ParamBuilder.CreateParamBuilder(builder, refBuilder);
                }
                DataTable table = FetchRows(context, fDataSet, builder, TopCount);
                if (table == null || table.Rows.Count == 0)
                    return null;

                List<IDecoderItem> result = new List<IDecoderItem>(table.Rows.Count);
                DataColumnCollection columns = table.Columns;
                foreach (DataRow row in table.Rows)
                {
                    result.Add(new DataRowDecoderItem(columns, row,
                        DecoderConst.CODE_NICK_NAME, fExpression, fDisplayExpression));
                }
                return result;
            }
        }

        public override IDecoderItem[] SearchByName(string name, params object[] args)
        {
            TkDbContext context;
            bool createContext;
            GetContext(args, out context, out createContext);

            try
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, NameField, name);
                DataTable table = FetchRows(context, fDataSet, builder, 2);
                if (table == null || table.Rows.Count == 0)
                    return null;
                IDecoderItem[] result = new IDecoderItem[table.Rows.Count];
                int index = 0;
                DataColumnCollection columns = table.Columns;
                foreach (DataRow row in table.Rows)
                    result[index++] = new DataRowDecoderItem(columns, row,
                        DecoderConst.CODE_NICK_NAME, fExpression, fDisplayExpression);

                return result;
            }
            finally
            {
                if (createContext)
                    context.Dispose();
            }
        }

        protected virtual IDecoderItem CreateItem(string code, params object[] args)
        {
            TkDbContext context;
            bool createContext;
            GetContext(args, out context, out createContext);
            try
            {
                DataRow row = FetchRow(code, context, fDataSet);
                if (row == null)
                    return null;
                DataColumnCollection columns = row.Table.Columns;
                DataRowDecoderItem result = new DataRowDecoderItem(columns, row,
                    DecoderConst.CODE_NICK_NAME, fExpression, fDisplayExpression);
                return result;
            }
            finally
            {
                if (createContext)
                    context.Dispose();
            }
        }

        public override IDecoderItem Decode(string code, params object[] args)
        {
            IDecoderItem result;
            if (fValues.TryGetValue(code, out result))
                return result;
            else
            {
                result = CreateItem(code, args);
                if (result == null)
                    return null;

                fValues.Add(code, result);
                return result;
            }
        }
    }
}
