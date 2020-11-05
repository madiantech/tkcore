using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AutoProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-12-21",
        Description = "通过注册名创建AutoProcessor")]
    internal class CompositeAutoProcessorConfig : IConfigCreator<AutoProcessor>
    {
        #region IConfigCreator<AutoProcessor> 成员

        public AutoProcessor CreateObject(params object[] args)
        {
            CompositeAutoProcessor result = new CompositeAutoProcessor { FillMode = FillMode };
            if (AutoProcessors != null)
            {
                foreach (var item in AutoProcessors)
                {
                    var processor = item.CreateObject();
                    result.Add(processor);
                }
            }
            return result;
        }

        #endregion IConfigCreator<AutoProcessor> 成员

        [SimpleAttribute(DefaultValue = FillContentMode.MainOnly)]
        public FillContentMode FillMode { get; private set; }

        [DynamicElement(AutoProcessorConfigFactory.REG_NAME, IsMultiple = true)]
        public List<IConfigCreator<AutoProcessor>> AutoProcessors { get; private set; }
    }
}