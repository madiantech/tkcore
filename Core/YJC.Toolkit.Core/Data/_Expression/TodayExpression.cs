using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2013-09-30",
        Description = "今天（格式为yyyy-MM-dd）")]
    internal sealed class TodayExpression : IExpression
    {
        public static IExpression Instance = new TodayExpression();

        private TodayExpression()
        {
        }

        #region IExpression 成员

        string IExpression.Execute()
        {
            return DateTime.Today.ToString(ToolkitConst.DATE_FMT_STR, ObjectUtil.SysCulture);
        }

        #endregion
    }
}
