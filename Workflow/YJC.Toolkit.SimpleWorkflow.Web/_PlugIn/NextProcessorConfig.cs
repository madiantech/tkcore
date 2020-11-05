using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-09",
        Description = "生成下一步的Processor")]
    internal class NextProcessorConfig : IConfigCreator<Processor>
    {
        #region IConfigCreator<Processor> 成员

        public Processor CreateObject(params object[] args)
        {
            return new NextProcessor();
        }

        #endregion IConfigCreator<Processor> 成员
    }
}