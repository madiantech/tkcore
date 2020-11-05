using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class BaseSchemeEasySearch : BaseDbEasySearch, IFieldInfoIndexer
    {
        private const string EASYSEARCH_TABLE_NAME = "EasySearch";

        protected class TempSource : IDbDataSource
        {
            public TempSource(DataSet dataSet, TkDbContext context)
            {
                DataSet = dataSet;
                Context = context;
            }

            #region IDbDataSource 成员

            public DataSet DataSet { get; private set; }

            public TkDbContext Context { get; private set; }

            #endregion IDbDataSource 成员
        }

        public class TempDisplay : IDisplayObject
        {
            public TempDisplay(IDisplayObject display, ITableScheme scheme, string idField, string nameField)
            {
                Id = GetField(display.Id, scheme, idField);
                Name = GetField(display.Name, scheme, nameField);
            }

            #region IDisplayObject 成员

            public bool SupportDisplay
            {
                get
                {
                    return true;
                }
            }

            public IFieldInfo Id { get; private set; }

            public IFieldInfo Name { get; private set; }

            #endregion IDisplayObject 成员

            private static IFieldInfo GetField(IFieldInfo defaultInfo, ITableScheme scheme, string idField)
            {
                return string.IsNullOrEmpty(idField) ? defaultInfo : scheme[idField];
            }
        }

        private readonly EasySearchProxyScheme fScheme;
        private readonly IDisplayObject fDisplay;

        protected BaseSchemeEasySearch(ITableScheme scheme)
            : this(scheme, null, null)
        {
        }

        protected internal BaseSchemeEasySearch(ITableScheme scheme, string idField, string nameField)
            : this(scheme, idField, nameField, true)
        {
        }

        protected internal BaseSchemeEasySearch(ITableScheme scheme, string idField, string nameField, bool canCache)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            TkDebug.AssertArgument(scheme is IDisplayObject, "scheme",
                "scheme需要支持IDisplayObject接口", scheme);

            fDisplay = scheme.Convert<IDisplayObject>();
            TkDebug.Assert(fDisplay.SupportDisplay,
                "scheme的SupportDisplay必须为true，当前是false", scheme);
            SourceScheme = scheme;
            fDisplay = new TempDisplay(fDisplay, scheme, idField, nameField);
            if (canCache)
                canCache = idField == null && nameField == null;
            if (canCache)
                fScheme = new EasySearchProxyScheme(scheme, fDisplay);
            else
                fScheme = new NoCacheEasySearchProxyScheme(scheme, fDisplay);
            ValueField = fDisplay.Id;
            NameField = fDisplay.Name;
        }

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fScheme[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        protected ITableScheme SourceScheme { get; private set; }

        protected ITableScheme ProxyScheme
        {
            get
            {
                return fScheme;
            }
        }

        private IParamBuilder CreateRefBuilder(TkDbContext context, EasySearchRefField field)
        {
            IFieldInfo fieldInfo = SourceScheme[field.NickName];
            if (fieldInfo != null)
                return SqlParamBuilder.CreateEqualSql(context, fieldInfo, field.Value);
            return null;
        }

        protected override IParamBuilder GetRefParamBuilder(TkDbContext context, List<EasySearchRefField> refFields)
        {
            if (refFields == null || refFields.Count == 0)
                return null;

            var builders = from item in refFields
                           select CreateRefBuilder(context, item);
            return ParamBuilder.CreateParamBuilder(builders);
        }

        protected override DataRow FetchRow(string code, TkDbContext context, DataSet dataSet)
        {
            TempSource source = new TempSource(dataSet, context);
            TableSelector selector = new TableSelector(fScheme, source);
            using (selector)
            {
                return selector.TrySelectRowWithKeys(code);
            }
        }

        protected override DataTable FetchRows(TkDbContext context, DataSet dataSet,
            IParamBuilder builder, int topCount)
        {
            SqlSelector selector = new SqlSelector(context, dataSet);
            using (selector)
            {
                IParamBuilder addBuilder = CreateAdditionCondition(context, fScheme);
                builder = ParamBuilder.CreateParamBuilder(builder, addBuilder);
                string whereSql;
                if (builder == null)
                    whereSql = string.Empty;
                else
                    whereSql = "WHERE " + builder.Sql;

                TableSchemeData schemeData = TableSchemeData.Create(context, fScheme);
                var result = context.ContextConfig.GetListSql(schemeData.SelectFields,
                    GetTableName(context), schemeData.KeyFieldArray,
                    whereSql, OrderBy, 0, topCount);
                ISimpleAdapter adapter = selector;
                adapter.SetSql(result.ListSql, builder);
                context.ContextConfig.SetListData(result, adapter, dataSet, 0, TopCount, EASYSEARCH_TABLE_NAME);

                return dataSet.Tables[EASYSEARCH_TABLE_NAME];
            }
        }

        protected virtual string GetTableName(TkDbContext context)
        {
            return context.EscapeName(fScheme.TableName);
        }
    }
}