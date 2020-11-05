using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    abstract class BaseResolverConfig : BaseDbConfig, ISingleResolverConfig
    {
        protected BaseResolverConfig()
        {
        }

        #region ISingleResolverConfig 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }

        #endregion
    }
}
