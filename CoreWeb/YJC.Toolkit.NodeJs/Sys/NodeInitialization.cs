using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    internal class NodeInitialization : IInitialization
    {
        public void AppEnd(object application)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            EsModelSettings setting = new EsModelSettings();
            setting.Initialize(globalVariable);
        }

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }
    }
}