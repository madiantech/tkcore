using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class InternalParamBuilder : IParamBuilder
    {
        public InternalParamBuilder()
        {
            Sql = string.Empty;
            Parameters = new DbParameterList();
        }

        public InternalParamBuilder(string sql, params DbParameter[] parameters)
        {
            Sql = sql;
            Parameters = new DbParameterList();
            Parameters.AddRange(parameters);
        }

        public InternalParamBuilder(string sql, DbParameterList parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public DbParameterList Parameters { get; private set; }

        public string Sql { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Sql) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "Sql为{0}的IParamBuilder", Sql);
        }
    }
}