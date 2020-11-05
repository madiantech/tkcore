using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [NotifyActionConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-06-01", Author = "YJC",
        Description = "根据配置的注册名实例化相应的NotifyAction")]
    internal class RegNameNotifyActionConfig : RegFactoryConfig<INotifyAction>
    {
        public RegNameNotifyActionConfig()
            : base(NotifyActionPlugInFactory.REG_NAME)
        {
        }
    }
}