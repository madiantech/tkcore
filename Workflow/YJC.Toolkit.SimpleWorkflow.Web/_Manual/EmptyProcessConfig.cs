using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessXmlConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Empty", Author = "YJC",
        CreateDate = "2017-10-23", Description = "返回ProcessXml空对象")]
    internal class EmptyProcessConfig : IConfigCreator<ProcessXml>
    {
        #region IConfigCreator<ProcessXml> 成员

        public ProcessXml CreateObject(params object[] args)
        {
            return new ProcessXml(FillContentMode.MainOnly);
        }

        #endregion IConfigCreator<ProcessXml> 成员
    }
}