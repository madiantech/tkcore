using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;

namespace TestWfData
{
    [ObjectContext]
    [AutoProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-29",
        Description = "生成会签的AutoProcessor")]
    public class CounterSignProcessorConfig : IConfigCreator<AutoProcessor>
    {
        #region IConfigCreator<AutoProcessor> 成员

        public AutoProcessor CreateObject(params object[] args)
        {
            string xml = this.WriteXml();
            Trace.Write(xml);
            return new CounterSignAutoProcessor(CounterSignUserConfig, SubWorkflowName)
            {
                TitleExpression = TitleExpression
            };
        }

        #endregion IConfigCreator<AutoProcessor> 成员

        [SimpleComplexElement(NamespaceType = NamespaceType.Toolkit, UseCData = true)]
        public string CounterSignUserConfig { get; private set; }

        [SimpleAttribute(Required = true)]
        public string SubWorkflowName { get; private set; }

        [SimpleElement(NamespaceType = NamespaceType.Toolkit)]
        public string TitleExpression { get; private set; }
    }
}