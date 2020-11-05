using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-12-26", Description = "提供对单表的Insert和Update操作的数据源")]
    internal class SingleDbEditSourceConfig : BaseResolverConfig, IEditDbConfig, ISingleResolverConfig
    {
        public override ISource CreateObject(params object[] args)
        {
            SingleDbEditSource source = new SingleDbEditSource(this);
            source.MainResolver.UpdateMode = UpdateMode;
            if (UpdatedActions != null)
                foreach (var item in UpdatedActions)
                {
                    var action = item.CreateObject(source);
                    source.AddUpdatedAction(action);
                }
            return source;
        }

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        [SimpleAttribute(DefaultValue = UpdateMode.OneRow)]
        public UpdateMode UpdateMode { get; protected set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(UpdatedActionConfigFactory.REG_NAME, IsMultiple = true)]
        public List<IConfigCreator<BaseUpdatedAction>> UpdatedActions { get; private set; }
    }
}