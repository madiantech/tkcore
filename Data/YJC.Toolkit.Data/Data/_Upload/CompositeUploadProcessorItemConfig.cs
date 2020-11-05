using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class CompositeUploadProcessorItemConfig
    {
        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string Condition { get; private set; }

        [DynamicElement(UploadProcessorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<IUploadProcessor2> UploadProcessor { get; private set; }

        public bool Test()
        {
            return EvaluatorUtil.Execute<bool>(Condition);
        }
    }
}