using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ParamExpression(REG_NAME, SqlInject = true, CreateDate = "2014-04-30", Author = "YJC",
        Description = "获取AppSetting中配置的Host值，如果存在，将会确保最后的一位为/")]
    internal class HostParamExpression : IParamExpression
    {
        internal const string REG_NAME = "%";

        #region IParamExpression 成员

        public string Execute(string parameter)
        {
            TkDebug.ThrowIfNoAppSetting();

            if (string.IsNullOrEmpty(parameter))
                return string.Empty;
            Uri result = BaseAppSetting.Current.GetHost(parameter);
            if (result == null)
                return string.Empty;
            string url = result.ToString();
            if (url.EndsWith("/", StringComparison.Ordinal))
                return url;
            return url + "/";
        }

        #endregion
    }
}
