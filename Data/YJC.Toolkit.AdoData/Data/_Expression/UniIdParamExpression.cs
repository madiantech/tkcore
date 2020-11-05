using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ParamExpression(REG_NAME, SqlInject = false, Author = "YJC",
        CreateDate = "2009-04-13", Description = "生成数据表的Unique ID(@)")]
    internal sealed class UniIdParamExpression : IParamExpression, ICustomData
    {
        internal const string REG_NAME = "@";
        private TkDbContext fContext;

        #region INeedCustomData 成员

        void ICustomData.SetData(params object[] args)
        {
            IDbDataSource data = ObjectUtil.QueryObject<IDbDataSource>(args);
            if (data != null)
                fContext = data.Context;
            if (fContext == null)
                fContext = ObjectUtil.QueryObject<TkDbContext>(args);
            TkDebug.AssertNotNull(fContext, "参数宏(@)需要DbContext对象，但是没有从外部对象中找到", this);
        }

        #endregion

        #region IParamExpression 成员

        string IParamExpression.Execute(string parameter)
        {
            return fContext.GetUniId(parameter);
        }

        #endregion
    }
}
