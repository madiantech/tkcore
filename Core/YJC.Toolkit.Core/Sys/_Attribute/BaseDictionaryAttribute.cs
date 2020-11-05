using System;
using System.Collections;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseDictionaryAttribute : NamedAttribute, IOrder
    {
        protected BaseDictionaryAttribute()
        {
        }

        public Type CollectionType { get; set; }

        public int Order { get; set; }

        public IDictionary GetDictionary(object receiver, ObjectPropertyInfo info)
        {
            object value = ObjectUtil.GetPropertyValue(receiver, CollectionType, info);
            IDictionary dictionary = value.Convert<IDictionary>();

            return dictionary;
        }

        public override Type GetObjType(Type propertyType, string name)
        {
            if (ObjectType != null)
                return ObjectType;

            return ObjectUtil.GetDictionaryValueType(propertyType, name, this);
        }
    }
}