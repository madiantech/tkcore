using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ChildTableInfoConfig : IRegName
    {
        #region IRegName 成员

        public string RegName
        {
            get
            {
                if (Relation != null)
                    return Relation.Name;

                return string.Empty;
            }
        }

        #endregion IRegName 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public TableRelationConfig Relation { get; protected set; }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> Operators { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public StatConfigItem Stat { get; protected set; }

        [SimpleAttribute(DefaultValue = UpdateMode.Merge)]
        public UpdateMode UpdateMode { get; protected set; }

        [SimpleAttribute]
        public bool IsNewEmptyRow { get; protected set; }
    }
}