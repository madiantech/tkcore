using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class ParameterInfo
    {
        public ParameterAttribute Attribute { get; set; }

        public PropertyInfo Property { get; set; }

        public StoredProcParameter Parameter { get; set; }

        public void SetInputValue(object value)
        {
            object propertyValue = ObjectUtil.GetValue(Property, value);
            Parameter.Value = propertyValue;
            Parameter.SetInputValue();
        }

        public void SetOutputValue(object value)
        {
            Parameter.SetOutputValue();
            ObjectUtil.SetValue(Property, value, Parameter.Value);
        }

        public override string ToString()
        {
            return Property == null ? base.ToString() : string.Format(ObjectUtil.SysCulture,
                "属性名为{0}的存储过程参数", Property.Name);
        }
    }
}
