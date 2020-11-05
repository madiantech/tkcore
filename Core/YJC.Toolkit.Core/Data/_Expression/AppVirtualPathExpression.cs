using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2014-06-27",
        Description = "AppSetting.AppVirtualPath")]
    internal sealed class AppVirtualPathExpression : IExpression
    {
        public static IExpression Instance = new AppVirtualPathExpression();

        private AppVirtualPathExpression()
        {
        }

        #region IExpression 成员

        public string Execute()
        {
            if (BaseAppSetting.Current == null)
                return string.Empty;
            return BaseAppSetting.Current.AppVirtualPath;
        }

        #endregion
    }
}
