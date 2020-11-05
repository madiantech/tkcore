using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DynamicElementAttribute : BaseObjectAttribute, IOrder
    {
        private bool fIsMultiple;
        private BaseXmlConfigFactory fPlugInFactory;

        public DynamicElementAttribute(string factoryName)
        {
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", null);
            FactoryName = factoryName;
        }

        public string FactoryName { get; private set; }

        public bool IsMultiple
        {
            get
            {
                return fIsMultiple;
            }
            set
            {
                fIsMultiple = value;
                if (value)
                    UseJsonObject = true;
            }
        }

        public Type CollectionType { get; set; }

        public int Order { get; set; }

        public bool UseJsonObject { get; set; }

        public BaseXmlConfigFactory PlugInFactory
        {
            get
            {
                if (fPlugInFactory == null)
                {
                    TkDebug.ThrowIfNoGlobalVariable();
                    fPlugInFactory = BaseGlobalVariable.Current.FactoryManager.GetConfigFactory(
                        FactoryName);
                }
                return fPlugInFactory;
            }
        }

        protected override void Read(IObjectSerializer serializer, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        protected override void Write(IObjectSerializer serializer, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }
    }
}
