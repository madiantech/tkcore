namespace YJC.Toolkit.Sys
{
    public abstract class BaseDefaultValueAttribute : BasePlugInAttribute
    {
        protected BaseDefaultValueAttribute()
        {
            Suffix = "DefaultValue";
        }

        public override string FactoryName => DefaultValueTypeFactory.REG_NAME;
    }
}