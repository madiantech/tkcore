using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal class RedisInitialization : IInitialization
    {
        public void AppEnd(object application)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            RedisUtil.CreateCache();
        }

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }
    }
}