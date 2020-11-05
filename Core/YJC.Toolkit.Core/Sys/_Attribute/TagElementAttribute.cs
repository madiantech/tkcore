using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class TagElementAttribute : NamedAttribute, IOrder
    {
        private readonly ObjectElementInfo fChildElements;

        public TagElementAttribute()
        {
            fChildElements = new ObjectElementInfo();
        }

        public TagElementAttribute(NamespaceType namespaceType)
            : this()
        {
            NamespaceType = namespaceType;
        }

        public ObjectElementInfo ChildElements
        {
            get
            {
                return fChildElements;
            }
        }

        public int Order { get; set; }

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadTagElement(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteTagElement(this, writer, value, settings, info, serializerData);
        }

        public override Type GetObjType(Type propertyType, string name)
        {
            return base.GetObjType(propertyType, name);
        }

        internal void Read(Type type, PropertyInfo property, string modelName,
            object[] propertyAttributes)
        {
            fChildElements.ReadElementAttribute(type, property, modelName, propertyAttributes);
        }
    }
}