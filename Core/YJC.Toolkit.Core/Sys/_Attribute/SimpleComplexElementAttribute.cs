using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SimpleComplexElementAttribute : SimpleElementAttribute
    {
        public SimpleComplexElementAttribute()
        {
        }

        public SimpleComplexElementAttribute(NamespaceType namespaceType)
            : base(namespaceType)
        {
        }

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadComplexElement(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteComplexElement(this, writer, value, settings, info, serializerData);
        }
    }
}