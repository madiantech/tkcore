using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ContentXmlConfig(NamespaceType = NamespaceType.Toolkit, RegName = WorkflowWebConst.APPROVE, Author = "YJC",
        CreateDate = "2018-04-04", Description = "给配置的ContentXml添加审批的模块")]
    internal class ApproveContentConfig : IConfigCreator<ContentXml>
    {
        #region IConfigCreator<ContentXml> 成员

        public ContentXml CreateObject(params object[] args)
        {
            ContentXml xml = Content != null ? Content.CreateObject() : null;
            return new ApproveContentXml(xml);
        }

        #endregion IConfigCreator<ContentXml> 成员

        [DynamicElement(ContentXmlConfigFactory.REG_NAME)]
        public IConfigCreator<ContentXml> Content { get; private set; }
    }
}