namespace YJC.Toolkit.Sys
{
    public sealed class XmlConfigFactoryTypeConverter : BaseTypeConverter<BaseXmlConfigFactory>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return BaseGlobalVariable.Current.FactoryManager.GetConfigFactory(text);
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            BasePlugInFactory factory = value as BasePlugInFactory;
            if (factory != null)
                return factory.Name;
            return null;
        }
    }
}
