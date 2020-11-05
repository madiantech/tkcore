using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;

namespace ZyWorkOrder
{
    [ObjectContext]
    [AutoProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YK", CreateDate = "2020-02-18",
        Description = "单表数据更新的AutoProcessor")]
    public class SingleDataUpdateProcessorConfig : IConfigCreator<AutoProcessor>
    {
        #region IConfigCreator<AutoProcessor> 成员

        public AutoProcessor CreateObject(params object[] args)
        {
            string xml = this.WriteXml();
            Trace.Write(xml);
            return new SingleDataUpdateAutoProcessor(this);
        }

        #endregion IConfigCreator<AutoProcessor> 成员

        [SimpleAttribute(Required = true)]
        public string DataXml { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, Required = true, LocalName = "Fields")]
        public List<KeyValueItem> FieldList { get; private set; }
    }


    public class KeyValueItem
    {
        [SimpleAttribute(Required = true)]
        public string Key { get; private set; }

        [SimpleAttribute(Required = true)]
        public string Value { get; private set; }
    }
}