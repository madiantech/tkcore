using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal abstract class BaseMultipleConfig : BaseDbConfig, IMultipleResolverConfig
    {
        protected BaseMultipleConfig()
        {
        }

        #region IMultipleResolverConfig 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> MainResolver { get; protected set; }

        IEnumerable<ChildTableInfoConfig> IMultipleResolverConfig.ChildResolvers
        {
            get
            {
                return ChildTables;
            }
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ChildTable")]
        public List<ChildTableInfoConfig> ChildTables { get; protected set; }
    }
}
