using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    internal class CompositeMetaDataItemConfig
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public InputConditionConfig Condition { get; private set; }

        [DynamicElement(MetaDataConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<IMetaData> MetaData { get; private set; }

        public override string ToString()
        {
            return Condition == null ? base.ToString() : Condition.ToString();
        }
    }
}
