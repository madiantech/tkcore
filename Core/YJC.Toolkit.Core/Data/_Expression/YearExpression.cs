using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2013-09-30", Description = "当年(yyyy)")]
    internal sealed class YearExpression : IExpression
    {
        public static IExpression Instance = new YearExpression();

        private YearExpression()
        {
        }


        #region IExpression 成员

        string IExpression.Execute()
        {
            return DateTime.Today.Year.ToString(ObjectUtil.SysCulture);
        }

        #endregion
    }
}
