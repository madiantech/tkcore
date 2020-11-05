using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class BaseSqlEasySearch : BaseSchemeEasySearch
    {
        private const string TABLE_NAME = "__TOOLKIT_SQL";
        private readonly string fSql;
        private readonly string fSelectFields;

        private class SqlProxyScheme : ITableScheme, IDisplayObject
        {
            private readonly ITableScheme fScheme;
            private readonly string fIdField;
            private readonly string fNameField;

            public SqlProxyScheme(ITableScheme scheme, string idField, string nameField)
            {
                fNameField = nameField;
                fIdField = idField;
                fScheme = scheme;
            }

            #region ITableScheme 成员

            public string TableName
            {
                get
                {
                    return fScheme.TableName;
                }
            }

            public IEnumerable<IFieldInfo> Fields
            {
                get
                {
                    return fScheme.Fields;
                }
            }

            public IEnumerable<IFieldInfo> AllFields
            {
                get
                {
                    return fScheme.AllFields;
                }
            }

            #endregion ITableScheme 成员

            #region IFieldInfoIndexer 成员

            public IFieldInfo this[string nickName]
            {
                get
                {
                    return fScheme[nickName];
                }
            }

            #endregion IFieldInfoIndexer 成员

            #region IDisplayObject 成员

            public bool SupportDisplay
            {
                get
                {
                    return true;
                }
            }

            public IFieldInfo Id
            {
                get
                {
                    return fScheme[fIdField];
                }
            }

            public IFieldInfo Name
            {
                get
                {
                    return fScheme[fNameField];
                }
            }

            #endregion IDisplayObject 成员
        }

        protected internal BaseSqlEasySearch(string sql, string idField, string nameField,
            TkDbContext context)
            : base(new SqlProxyScheme(DbUtil.CreateSqlTableScheme(sql, TABLE_NAME,
                idField, context), idField, nameField),
            StringUtil.GetNickName(idField), StringUtil.GetNickName(nameField), false)
        {
            fSql = sql;
            TableSchemeData schemeData = TableSchemeData.Create(context, ProxyScheme);
            fSelectFields = schemeData.SelectFields;
        }

        protected override DataRow FetchRow(string code, TkDbContext context, DataSet dataSet)
        {
            SqlSelector selector = new SqlSelector(context, dataSet);
            using (selector)
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, ValueField, code);
                String sql = string.Format("SELECT {0} FROM ({1}) {2}", fSelectFields, fSql, TABLE_NAME);
                return selector.TrySelectRow(TABLE_NAME, sql, builder);
            }
        }

        protected override string GetTableName(TkDbContext context)
        {
            return string.Format(ObjectUtil.SysCulture, "({0}) {1}", fSql, TABLE_NAME);
        }
    }
}