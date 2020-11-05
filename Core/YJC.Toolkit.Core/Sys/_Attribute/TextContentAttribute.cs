using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class TextContentAttribute : BaseObjectAttribute
    {
        public bool UseSourceType { get; set; }

        public bool AutoTrim { get; set; }

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadTextContent(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteTextContent(this, writer, value, settings, info, serializerData);
        }
    }
}