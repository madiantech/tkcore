using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-09-06", Description = "提供对多表的Insert和Update的数据源")]
    internal class MultipleDbEditSourceConfig : BaseMultipleConfig, IEditDbConfig
    {
        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(UpdatedActionConfigFactory.REG_NAME, IsMultiple = true)]
        public List<IConfigCreator<BaseUpdatedAction>> UpdatedActions { get; private set; }

        public override ISource CreateObject(params object[] args)
        {
            MultipleDbEditSource source = new MultipleDbEditSource(this);
            if (UpdatedActions != null)
                foreach (var item in UpdatedActions)
                {
                    var action = item.CreateObject(source);
                    source.AddUpdatedAction(action);
                }
            return source;
        }
    }
}