using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ContentXmlConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Empty", Author = "YJC",
        CreateDate = "2018-02-05", Description = "返回ContentXml空对象")]
    internal class EmptyContentConfig : IConfigCreator<ContentXml>
    {
        public ContentXml CreateObject(params object[] args)
        {
            return new ContentXml();
        }
    }
}