using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DynamicDictionaryAttribute : BaseDictionaryAttribute
    {
        private BaseXmlConfigFactory fPlugInFactory;

        public DynamicDictionaryAttribute(string factoryName)
        {
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", null);
            FactoryName = factoryName;
            UseJsonObject = true;
        }

        public string FactoryName { get; private set; }

        public bool UseJsonObject { get; private set; }

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

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
       ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadDynamicDictionary(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteDynamicDictionary(this, writer, value, settings, info, serializerData);
        }
    }
}