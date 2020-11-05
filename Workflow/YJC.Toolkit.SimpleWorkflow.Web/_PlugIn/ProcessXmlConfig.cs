using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-03-29",
        Description = "根据ProcessXml的配置生成相应的Processor")]
    internal class ProcessXmlConfig : IConfigCreator<Processor>
    {
        #region IConfigCreator<Processor> 成员

        public Processor CreateObject(params object[] args)
        {
            ProcessXml config = ObjectUtil.ConfirmQueryObject<ProcessXml>(this, args);
            return new XmlProcessor(config);
        }

        #endregion IConfigCreator<Processor> 成员
    }
}