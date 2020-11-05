using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2013-09-30",
        Description = "现在（格式为yyyy-MM-dd HH:mm:ss）")]
    internal sealed class NowExpression : IExpression
    {
        public static IExpression Instance = new NowExpression();

        private NowExpression()
        {
        }

        #region IExpression 成员

        string IExpression.Execute()
        {
            return DateTime.Now.ToString(ToolkitConst.DATETIME_FMT_STR, ObjectUtil.SysCulture);
        }

        #endregion
    }
}
