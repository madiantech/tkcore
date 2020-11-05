using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2013-09-30", Description = "当月(MM)")]
    internal sealed class MonthExpression : IExpression
    {
        public static IExpression Instance = new MonthExpression();

        private MonthExpression()
        {
        }

        #region IExpression 成员

        string IExpression.Execute()
        {
            return DateTime.Today.ToString("MM", ObjectUtil.SysCulture);
        }

        #endregion
    }
}
