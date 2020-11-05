using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ParamExpression(REG_NAME, SqlInject = true, Author = "YJC",
        CreateDate = "2013-10-16", Description = "通过Request.QueryString获得数据(#)")]
    [AlwaysCache, CacheInstance]
    internal sealed class QueryStringParamExpression : IParamExpression
    {
        internal const string REG_NAME = "#";

        #region IParamExpression 成员

        string IParamExpression.Execute(string parameter)
        {
            TkDebug.ThrowIfNoGlobalVariable();

            return WebGlobalVariable.Request.Query[parameter];
        }

        #endregion IParamExpression 成员
    }
}