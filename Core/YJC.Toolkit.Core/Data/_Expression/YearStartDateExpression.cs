using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2013-09-30",
        Description = "当年开始日期(yyyy-01-01)")]
    internal sealed class YearStartDateExpression : IExpression
    {
        public static IExpression Instance = new YearStartDateExpression();

        private YearStartDateExpression()
        {
        }


        #region IExpression 成员

        string IExpression.Execute()
        {
            string year = DateTime.Today.Year.ToString(ObjectUtil.SysCulture);
            return year + "-01-01";
        }

        #endregion
    }
}
