using System;
using System.Collections;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SimpleElementAttribute : NamedAttribute, IOrder
    {
        public SimpleElementAttribute()
        {
        }

        public SimpleElementAttribute(NamespaceType namespaceType)
        {
            NamespaceType = namespaceType;
        }

        public virtual bool IsMultiple { get; set; }

        public Type CollectionType { get; set; }

        public int Order { get; set; }

        public bool UseCData { get; set; }

        public bool UseSourceType { get; set; }

        public bool AutoTrim { get; set; }

        internal bool UseJsonObject { get; set; }

        protected override void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.ReadElement(this, reader, receiver, settings, info, serializerData);
        }

        protected override void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            serializer.WriteElement(this, writer, value, settings, info, serializerData);
        }

        public override Type GetObjType(Type propertyType, string name)
        {
            if (IsMultiple)
            {
                if (ObjectType != null)
                    return ObjectType;
                else
                    return ObjectUtil.GetListValueType(propertyType, name, this);
            }
            else
                return base.GetObjType(propertyType, name);
        }

        internal IList GetList(object receiver, ObjectPropertyInfo info)
        {
            object value = ObjectUtil.GetPropertyValue(receiver, CollectionType, info);
            IList list = value.Convert<IList>();

            return list;
        }
    }
}