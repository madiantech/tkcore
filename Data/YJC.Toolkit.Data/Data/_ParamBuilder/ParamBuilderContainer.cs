using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ParamBuilderContainer : IParamBuilder
    {
        private readonly StringBuilder fSql;
        private readonly DbParameterList fParams;

        /// <summary>
        /// Initializes a new instance of the ParamBuilderContainer class.
        /// </summary>
        public ParamBuilderContainer()
        {
            fSql = new StringBuilder();
            fParams = new DbParameterList();
        }

        #region IParamBuilder 成员

        public string Sql
        {
            get
            {
                return fSql.ToString();
            }
        }

        public DbParameterList Parameters
        {
            get
            {
                return fParams;
            }
        }

        #endregion

        public bool IsEmpty
        {
            get
            {
                return fSql.Length == 0;
            }
        }

        public void Add(IParamBuilder builder)
        {
            if (builder == null)
                return;
            string sql = builder.Sql;
            TkDebug.AssertNotNullOrEmpty(sql,
                "传入参数builder生成的Sql为空，这是不允许的", builder);

            AddSql(sql);
            DbParameterList parameters = builder.Parameters;
            TkDebug.AssertNotNull(parameters,
                "传入参数builder调用GetParameters返回为空，这是不允许的", builder);

            fParams.Add(parameters);
        }

        private void AddSql(string sql)
        {
            if (fSql.Length == 0)
                fSql.Append("(").Append(sql).Append(")");
            else
                fSql.Append(" AND (").Append(sql).Append(")");
        }

        public void Add(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return;
            AddSql(sql);
        }

        public ParamBuilderContainer Copy()
        {
            ParamBuilderContainer result = new ParamBuilderContainer();
            result.fSql.Append(fSql.ToString());
            result.fParams.Add(fParams);
            return result;
        }
    }
}
