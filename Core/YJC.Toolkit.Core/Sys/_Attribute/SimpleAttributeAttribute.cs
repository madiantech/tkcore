using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SimpleAttributeAttribute : NamedAttribute
    {
        public bool UseSourceType { get; set; }

        public bool AutoTrim { get; set; }

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadAttribute(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteAttribute(this, writer, value, settings, info, serializerData);
        }
    }
}