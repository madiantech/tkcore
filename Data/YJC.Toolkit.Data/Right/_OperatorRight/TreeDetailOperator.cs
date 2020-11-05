using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class TreeDetailOperator
    {
        [SimpleAttribute]
        public TreeOperatorPosition Position { get; private set; }

        [SimpleAttribute(DefaultValue = UpdateKind.Update)]
        public UpdateKind Button { get; private set; }

        [DynamicElement(OperatorConfigFactory.REG_NAME)]
        internal IConfigCreator<OperatorConfig> Operator { get; private set; }

        public OperatorConfig OperatorConfig { get; private set; }

        public void CreateOperator()
        {
            OperatorConfig = Operator.CreateObject();
        }
    }
}
