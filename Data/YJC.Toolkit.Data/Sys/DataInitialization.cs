using System.Globalization;

namespace YJC.Toolkit.Sys
{
    internal class DataInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            TkWebApp.Culture = appsetting.Culture ?? CultureInfo.CurrentCulture;
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }

        public void AppEnd(object application)
        {
        }

        #endregion
    }
}
