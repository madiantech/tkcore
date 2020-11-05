using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class ObjectElementAttribute : SimpleElementAttribute
    {
        public ObjectElementAttribute()
        {
        }

        public ObjectElementAttribute(NamespaceType namespaceType)
            : base(namespaceType)
        {
        }

        public bool UseConstructor { get; set; }

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadObjectElement(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteObjectElement(this, writer, value, settings, info, serializerData);
        }
    }
}