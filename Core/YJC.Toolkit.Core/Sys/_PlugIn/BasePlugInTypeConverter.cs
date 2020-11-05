using System;

namespace YJC.Toolkit.Sys
{
    public abstract class BasePlugInTypeConverter<T> : BaseTypeConverter<T> where T : class
    {
        protected BasePlugInTypeConverter(string factoryName)
        {
            FactoryName = factoryName;
        }

        public string FactoryName { get; private set; }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            return PlugInFactoryManager.CreateInstance<T>(FactoryName, text);
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            if (value == null)
                return null;
            Attribute attribute = Attribute.GetCustomAttribute(value.GetType(),
                typeof(BasePlugInAttribute));
            if (attribute == null)
                return base.ConvertToString(value, settings);
            else
                return ((BasePlugInAttribute)attribute).GetRegName(value.GetType());
        }
    }
}
