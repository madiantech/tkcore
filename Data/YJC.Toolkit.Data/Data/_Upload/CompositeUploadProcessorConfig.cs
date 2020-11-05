using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [UploadProcessorConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2019-06-06",
        Author = "YJC", Description = "组合UploadProcessor，根据条件选择适配的UploadProcessor")]
    internal class CompositeUploadProcessorConfig : IConfigCreator<IUploadProcessor2>
    {
        #region IConfigCreator<IUploadProcessor2> 成员

        public IUploadProcessor2 CreateObject(params object[] args)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    if (item.Test())
                        return item.UploadProcessor.CreateObject();
                }
            }

            return Otherwise.CreateObject();
        }

        #endregion IConfigCreator<IUploadProcessor2> 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<CompositeUploadProcessorItemConfig> Items { get; private set; }

        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(UploadProcessorConfigFactory.REG_NAME)]
        public IConfigCreator<IUploadProcessor2> Otherwise { get; private set; }
    }
}