using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    internal class ConditionDisplayItemConfig
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MarcoConfigItem Condition { get; private set; }

        [DynamicElement(CoreDisplayConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<IDisplay> Display { get; private set; }

        public bool IsFitFor(object value, object rowValue)
        {
            string expression = Expression.Execute(Condition);
            //value = HrefDisplayConfig.ResolveRowValue(rowValue, value, value);
            return EvaluatorUtil.Execute<bool>(expression,
                ("value", value), ("row", rowValue));
        }

        public override string ToString()
        {
            return Condition == null ? base.ToString() : Condition.ToString();
        }
    }
}