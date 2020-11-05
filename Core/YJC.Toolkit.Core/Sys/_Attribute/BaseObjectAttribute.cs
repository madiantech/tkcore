using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseObjectAttribute : Attribute
    {
        protected BaseObjectAttribute()
        {
        }

        public object DefaultValue { get; set; }

        public Type ObjectType { get; set; }

        public bool Required { get; set; }

        protected internal Type GetRealType(PropertyInfo info)
        {
            return ObjectType ?? info.PropertyType;
        }

        protected abstract void Read(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        protected abstract void Write(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        public void ReadObject(IObjectSerializer serializer, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            Read(serializer, reader, receiver, settings, info, serializerData);
        }

        public void WriteObject(IObjectSerializer serializer, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            Write(serializer, writer, value, settings, info, serializerData);
        }

        public virtual Type GetObjType(Type propertyType, string name)
        {
            return ObjectType ?? propertyType;
        }
    }
}