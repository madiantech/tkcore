using System;
using System.Collections;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal static class SerializerUtil
    {
        public static void AddElementValue(SimpleElementAttribute attribute, object receiver,
            ObjectPropertyInfo info, object objValue)
        {
            if (attribute.IsMultiple)
            {
                IList list = attribute.GetList(receiver, info);
                list.Add(objValue);
            }
            else
                info.SetValue(receiver, objValue);
        }

        public static object GetPropertyObject(object receiver, ReadSettings settings,
            ObjectPropertyInfo info, string value, Type objType)
        {
            ITkTypeConverter converter = info.Converter ?? TkTypeDescriptor.GetConverter(objType);
            ObjectUtil.AssertTypeConverter(receiver, objType, converter, info.PropertyName);

            object obj = ObjectUtil.InternalGetValue(objType, value, info.Attribute.DefaultValue,
                settings, converter);
            return obj;
        }

        public static object GetPropertyObject(object receiver, ReadSettings settings,
            ObjectPropertyInfo info, string value, bool autoTrim)
        {
            Type objType = info.ObjectType;
            if (autoTrim && value != null)
                value = value.Trim();
            return GetPropertyObject(receiver, settings, info, value, objType);
        }

        internal static void SetObjectValue(object receiver, ReadSettings settings,
            ObjectPropertyInfo info, string value, bool autoTrim)
        {
            object obj = GetPropertyObject(receiver, settings, info, value, autoTrim);
            info.SetValue(receiver, obj);
        }

        private static void WriteElement(bool isMultiple, object value, IElementWriter list,
            Func<IElementWriter, object, ObjectPropertyInfo> getPropertyFunc,
            Action<ObjectPropertyInfo, object> writeAction, Action<bool> nullAction, Action<IList> listAction)
        {
            if (nullAction != null)
                nullAction(false);
            if (isMultiple)
            {
                IList listValue = value.Convert<IList>();
                if (listAction != null)
                    listAction(listValue);
                foreach (object itemValue in listValue)
                {
                    ObjectPropertyInfo property = getPropertyFunc(list, itemValue);
                    if (property != null)
                        writeAction(property, itemValue);
                }
            }
            else
            {
                ObjectPropertyInfo property = getPropertyFunc(list, value);
                if (property != null)
                    writeAction(property, value);
            }
        }

        private static void WriteChildElement(ObjectElementInfo elements, object obj,
            Func<object, IElementWriter, object> getValueFunc, Action<ObjectPropertyInfo, object> writeAction,
            Action<bool> nullAction, Action<IList> listAction)
        {
            var propertyList = elements.CreateOrderPropertyInfoList();
            foreach (var item in propertyList)
            {
                object value = getValueFunc(obj, item);
                if (value == null)
                {
                    if (nullAction != null)
                        nullAction(true);
                    continue;
                }
                ObjectPropertyInfo property;
                if (item.IsSingle)
                    property = item.Content;
                else
                {
                    if (item.IsValueMulitple)
                    {
                        IList list = value as IList;
                        if (list.Count == 0)
                            continue;
                        property = item.Get(list[0].GetType());
                    }
                    else
                        property = item.Get(value.GetType());
                }
                SimpleElementAttribute attribute = property.Attribute as SimpleElementAttribute;
                if (item.IsSingle)
                {
                    if (attribute == null)
                        writeAction(property, value);
                    else
                        WriteElement(attribute.IsMultiple, value, item,
                            (propList, objectType) => property, writeAction, nullAction, listAction);
                }
                else
                {
                    //AssertElementAttribute(property, attribute);
                    WriteElement(attribute.IsMultiple, value, item,
                        (propList, objectType) => propList.Get(objectType.GetType()),
                        writeAction, nullAction, listAction);
                }
            }
        }

        public static void WriteChildElement(IObjectSerializer serializer, object writer,
            ObjectElementInfo elements, WriteSettings settings, object obj,
            Func<object, IElementWriter, object> getValueFunc, object prefixTable)
        {
            WriteChildElement(elements, obj, getValueFunc,
                (propertyInfo, objValue) => propertyInfo.Attribute.WriteObject(serializer,
                    writer, objValue, settings, propertyInfo, prefixTable), null, null);
        }

        public static void WritePropertyInfo(IObjectSerializer serializer, object writer,
            object receiver, WriteSettings settings, ObjectPropertyInfo attr, object serializerData)
        {
            object value = attr.GetValue(receiver);
            if (value != null)
                attr.Attribute.WriteObject(serializer, writer, value, settings, attr, serializerData);
        }

        public static object CheckCustomObject<T>(object customObject)
        {
            TkDebug.AssertArgumentNull(customObject, "customObject", null);

            if (customObject is T)
                return customObject;

            throw new NotSupportedException(string.Format(ObjectUtil.SysCulture,
                "只支持{1}类型，当前类型为{0}", customObject.GetType(), typeof(T)));
        }

        public static void ReadObjectCallBack(object obj)
        {
            // 支持IReadXmlCallback接口，即允许用户在读完Xml后，做一些数据操作
            IReadObjectCallBack callBack = obj as IReadObjectCallBack;
            if (callBack != null)
                callBack.OnReadObject();
        }

        public static void SetParent(object parent, object subObject)
        {
            IParentObject intf = subObject as IParentObject;
            if (intf != null)
                intf.SetParent(parent);
        }

        public static CustomObjectPropertyInfo CustomRead(object receiver, string localName,
            string modelName, Func<string> versionFunc)
        {
            ICustomReader reader = receiver as ICustomReader;
            if (reader != null)
            {
                string version;
                if (reader.SupportVersion)
                {
                    TkDebug.AssertNotNull(versionFunc,
                        "ICustomer的SupportVersion为true，但是versionFunc传入是null", receiver);
                    version = versionFunc();
                }
                else
                    version = null;
                var result = reader.CanRead(localName, version);
                if (result != null)
                {
                    var info = new CustomObjectPropertyInfo(localName, reader, result, version, modelName);
                    return info;
                }
            }

            return null;
        }

        public static void ReadProperty(IObjectSerializer serializer, object reader,
            object receiver, ReadSettings settings, ObjectInfo info, string localName, string modelName)
        {
            ObjectPropertyInfo property = info.GetAttribute(localName);
            if (property != null)
                property.Attribute.ReadObject(serializer, reader, receiver, settings, property, null);
            else
            {
                property = info.Elements[localName];
                if (property != null)
                    property.Attribute.ReadObject(serializer, reader, receiver, settings, property, null);
                else if (info.Content != null && localName == ObjectInfo.CONTENT_NAME)
                    info.Content.Attribute.ReadObject(serializer, reader, receiver, settings, info.Content, null);
                else
                {
                    property = SerializerUtil.CustomRead(receiver, localName, modelName, () => null);
                    if (property != null)
                        property.Attribute.ReadObject(serializer, reader, receiver, settings, property, null);
                }
            }
        }

        public static void WriteElements(IObjectSerializer serializer, ObjectInfo info, object writer,
            object receiver, WriteSettings settings)
        {
            var propertyList = info.Elements.CreateOrderPropertyInfoList();
            foreach (var item in propertyList)
            {
                //object value = ObjectUtil.GetValue(item.ClassProperty, receiver);
                object value = item.GetValue(receiver);
                if (value == null)
                    continue;
                ObjectPropertyInfo property;
                if (item.IsSingle)
                {
                    property = item.Content;
                    property.Attribute.WriteObject(serializer, writer, value, settings, property, item);
                }
                else
                {
                    object currentValue;
                    if (item.IsValueMulitple)
                    {
                        IList list = value.Convert<IList>();
                        if (list.Count > 0 && list[0] != null)
                            currentValue = list[0];
                        else
                            continue;
                    }
                    else
                        currentValue = value;
                    property = item.Get(currentValue.GetType());
                    property.Attribute.WriteObject(serializer, writer, value, settings, property, item);
                }
            }
        }

        public static void Write(IObjectSerializer serializer, object writer, object receiver,
            string modelName, WriteSettings settings)
        {
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            foreach (var attr in info.Attributes)
                SerializerUtil.WritePropertyInfo(serializer, writer,
                    receiver, settings, attr, null);
            if (info.Content != null)
                SerializerUtil.WritePropertyInfo(serializer, writer,
                    receiver, settings, info.Content, null);
            else
                SerializerUtil.WriteElements(serializer, info, writer,
                    receiver, settings);
            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
        }

        public static void CheckRequiredAttribute(SimpleAttributeAttribute attribute,
            Object receiver, ObjectPropertyInfo info, String value)
        {
            if (attribute.Required && string.IsNullOrEmpty(value))
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "属性{0}标记了Required，但是对应的属性没有配置值", info.PropertyName), receiver);
        }

        public static void CheckRequiredContent(BaseObjectAttribute attribute,
                Object receiver, ObjectPropertyInfo info, String value)
        {
            if (attribute.Required && string.IsNullOrEmpty(value))
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "字段{0}标记了required，但是对应的Content没有配置值", info.PropertyName), receiver);
        }

        public static void CheckElementRequired(ObjectElementInfo elements, Object receiver)
        {
            elements.CheckRequired(receiver);
        }

        public static void CheckSimpleDictionary(BaseDictionaryAttribute attribute, object sender)
        {
            TkDebug.Assert(attribute is DictionaryAttribute, string.Format(ObjectUtil.SysCulture,
                "{0}仅支持DictionaryAttribute，即Value值为简单对象", sender), sender);
        }

        public static void WriteObject(IObjectSerializer serializer, object writer,
            object receiver, string modelName, WriteSettings settings, QName root,
            BaseObjectAttribute attribute, object serializerData, SerializerOptions options)
        {
            Type type = receiver.GetType();
            if (receiver is IDictionary)
            {
                BaseDictionaryAttribute attr = attribute as BaseDictionaryAttribute;
                options.CheckWriteDictionary(serializer, attr);
                if (attr == null)
                    attr = GetDictionaryAttribute(receiver, root);
                serializer.WriteDictionary(writer, receiver.Convert<IDictionary>(), attr,
                    modelName, settings, root, serializerData);
            }
            else if (type.IsArray || receiver is IList)
            {
                SimpleElementAttribute attr = attribute as SimpleElementAttribute;
                options.CheckWriteList(serializer, attr);
                if (attr == null)
                {
                    Type valueType;
                    if (type.IsArray)
                    {
                        Array arr = receiver as Array;
                        if (arr.Length == 0)
                            return;
                        var enumerator = arr.GetEnumerator();
                        enumerator.MoveNext();
                        valueType = enumerator.Current.GetType();
                    }
                    else
                    {
                        valueType = ObjectUtil.GetListValueType(type, "", null);
                    }
                    attr = GetElementAttribute(receiver, valueType);
                }
                serializer.WriteList(writer, receiver.Convert<IEnumerable>(), attr,
                    modelName, settings, root, serializerData);
            }
            else
            {
                options.CheckWriteObject(serializer);
                ITkTypeConverter converter = TkTypeDescriptor.GetSimpleConverter(type);
                if (converter != null)
                {
                    SimpleElementAttribute simpleAttr = new SimpleElementAttribute
                    {
                        LocalName = root.LocalName,
                        NamespaceUri = root.Namespace
                    };
                    var info = new SimpleObjectPropertyInfo(receiver, type, simpleAttr, converter);
                    serializer.WriteElement(simpleAttr, writer, receiver, settings, info, serializerData);
                }
                else
                    serializer.WriteObject(writer, receiver, modelName, settings, root, serializerData);
            }
        }

        public static void ReadObject(IObjectSerializer serializer, object reader,
            object receiver, string modelName, ReadSettings settings, QName root,
            BaseObjectAttribute attribute, object serializerData, SerializerOptions options)
        {
            if (receiver is IDictionary)
            {
                BaseDictionaryAttribute attr = attribute as BaseDictionaryAttribute;
                options.CheckReadDictionary(serializer, attr);
                if (attr == null)
                    attr = GetDictionaryAttribute(receiver, root);

                serializer.ReadDictionary(reader, receiver.Convert<IDictionary>(), attr,
                    modelName, settings, root, serializerData);
            }
            else if (receiver is IList)
            {
                SimpleElementAttribute attr = attribute as SimpleElementAttribute;
                options.CheckReadList(serializer, attr);
                if (attr == null)
                {
                    Type valueType = ObjectUtil.GetListValueType(receiver.GetType(), "", null);
                    attr = GetElementAttribute(receiver, valueType);
                }
                serializer.ReadList(reader, receiver.Convert<IList>(), attr, modelName,
                    settings, root, serializerData);
            }
            else
            {
                options.CheckReadObject(serializer);
                serializer.ReadObject(reader, receiver, modelName, settings, root, serializerData);
            }
        }

        private static SimpleElementAttribute GetElementAttribute(object receiver, Type valueType)
        {
            ITkTypeConverter converter = TkTypeDescriptor.GetSimpleConverter(valueType);
            SimpleElementAttribute attr;
            if (converter == null)
                attr = new ObjectElementAttribute
                {
                    ObjectType = valueType,
                    IsMultiple = true
                };
            else
                attr = new SimpleElementAttribute
                {
                    ObjectType = valueType,
                    IsMultiple = true
                };
            return attr;
        }

        private static BaseDictionaryAttribute GetDictionaryAttribute(object receiver, QName root)
        {
            Type valueType = ObjectUtil.GetDictionaryValueType(receiver.GetType(), "", null);
            ITkTypeConverter converter = TkTypeDescriptor.GetSimpleConverter(valueType);
            BaseDictionaryAttribute attr;
            if (converter == null)
                attr = new ObjectDictionaryAttribute { ObjectType = valueType };
            else
                attr = new DictionaryAttribute { ObjectType = valueType };

            attr.Assign(root);
            return attr;
        }

        public static void WriteSerializer(IObjectSerializer serializer, object writer, object receiver,
            string modelName, WriteSettings settings, QName root, BaseObjectAttribute attribute)
        {
            object serializerData = serializer.BeginWrite(writer, receiver, settings, root);
            serializer.Write(writer, receiver, modelName, settings, root, serializerData, attribute);
            serializer.EndWrite(writer, receiver, settings);
        }
    }
}