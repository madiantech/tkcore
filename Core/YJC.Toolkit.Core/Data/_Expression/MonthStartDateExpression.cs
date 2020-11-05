using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2013-09-30",
        Description = "当月开始日期(yyyy-MM-01)")]
    internal sealed class MonthStartDateExpression : IExpression
    {
        public static IExpression Instance = new MonthStartDateExpression();

        private MonthStartDateExpression()
        {
        }

        internal const string REG_NAME = "MonthStartDate";

        #region IExpression 成员

        string IExpression.Execute()
        {
            DateTime today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1).ToString(ToolkitConst.DATE_FMT_STR,
                ObjectUtil.SysCulture);
        }

        #endregion
    }
}
