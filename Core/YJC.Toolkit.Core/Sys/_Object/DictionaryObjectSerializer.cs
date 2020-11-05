using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "Dictionary对象格式转换器", Author = "YJC", CreateDate = "2016-05-11")]
    internal class DictionaryObjectSerializer : IObjectSerializer
    {
        private static readonly IObjectSerializer Instance = new DictionaryObjectSerializer();

        private static readonly SerializerOptions Options = new SerializerOptions
        {
            WriteObject = true
        };

        private DictionaryObjectSerializer()
        {
        }

        #region IObjectSerializer 成员

        public object CreateReader(string input, ReadSettings settings)
        {
            throw new NotSupportedException();
        }

        public object CreateReader(Stream stream, ReadSettings settings)
        {
            throw new NotSupportedException();
        }

        public object CreateCustomReader(object customObject)
        {
            throw new NotSupportedException();
        }

        public object CreateWriter(Stream stream, WriteSettings settings)
        {
            throw new NotSupportedException();
        }

        public object CreateCustomWriter(object customObject)
        {
            return SerializerUtil.CheckCustomObject<DictionaryBuilder>(customObject);
        }

        public bool ReadToRoot(object reader, QName root)
        {
            throw new NotSupportedException();
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadTagElement(TagElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadObjectElement(ObjectElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadComplexElement(SimpleComplexElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadTextContent(TextContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadDictionary(DictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadObjectDictionary(ObjectDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadDynamicDictionary(DynamicDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
        }

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void Read(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, BaseObjectAttribute attribute)
        {
            SerializerUtil.ReadObject(this, reader, receiver, modelName, settings,
                root, attribute, null, Options);
        }

        public void ReadObject(object reader, object receiver, string modelName,
            ReadSettings settings, QName root, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadDictionary(object reader, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadList(object reader, IList receiver, SimpleElementAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteAttribute(SimpleAttributeAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SetValue(writer, info, info.LocalName, value, settings);
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
            {
                IList list = value.Convert<IList>();
                List<string> strList = new List<string>(list.Count);
                foreach (object itemValue in list)
                {
                    string item = ObjectUtil.ToString(info.Converter, itemValue, settings);
                    strList.Add(item);
                }
                SetValue(writer, info.LocalName, strList);
            }
            else
                SetValue(writer, info, info.LocalName, value, settings);
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            QName name = info.QName;

            var infoArray = attribute.ChildElements.CreateOrderPropertyInfoList().ToArray();
            TkDebug.Assert(infoArray.Length == 1, string.Format(ObjectUtil.SysCulture,
                "TagElement的子节点必须有且仅有一个，当前{1}下有{0}个", infoArray.Length, name), this);
            DictionaryBuilder subBuilder = builder.CreateBuilder();

            var item = infoArray[0];
            if (item.IsSingle)
            {
                item.Content.Attribute.WriteObject(this, subBuilder, value,
                    settings, item.Content, serializerData);
                builder.Add(name.LocalName, subBuilder);
            }
            else
            {
                IList list = value as IList;
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        var property = item.Get(list[0].GetType());
                        property.Attribute.WriteObject(this, subBuilder, value, settings,
                            property, serializerData);
                        builder.Add(name.LocalName, subBuilder);
                    }
                }
                else
                {
                    var property = item.Get(value.GetType());
                    property.Attribute.WriteObject(this, subBuilder, value, settings,
                        property, serializerData);
                    builder.Add(name.LocalName, subBuilder);
                }
            }
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            QName name = info.QName;

            if (attribute.IsMultiple)
            {
                IList list = value.Convert<IList>();
                List<Dictionary<string, object>> itemList =
                    new List<Dictionary<string, object>>(list.Count);
                foreach (var itemValue in list)
                {
                    DictionaryBuilder subBuilder = builder.CreateBuilder();
                    WriteObject(subBuilder, itemValue, info.ModelName, settings, name, null);
                    itemList.Add(subBuilder.Data);
                }
                builder.Add(name.LocalName, itemList);
            }
            else
            {
                DictionaryBuilder subBuilder = builder.CreateBuilder();
                WriteObject(subBuilder, value, info.ModelName, settings, name, null);
                builder.Add(name.LocalName, subBuilder);
            }
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            WriteElement(attribute, writer, value, settings, info, serializerData);
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SetValue(writer, info, ObjectInfo.CONTENT_NAME, value, settings);
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            IDictionary dict = value.Convert<IDictionary>();
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            QName name = info.QName;

            DictionaryBuilder subDict = builder.CreateBuilder();
            foreach (DictionaryEntry item in dict)
                subDict.Add(item.Key.ToString(), ObjectUtil.ToString(info.Converter, item.Value, settings));
            builder.Add(name.LocalName, subDict);
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            IDictionary dict = value.Convert<IDictionary>();
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            QName name = info.QName;

            DictionaryBuilder subDict = builder.CreateBuilder();
            foreach (DictionaryEntry item in dict)
            {
                DictionaryBuilder subDictItem = builder.CreateBuilder();
                WriteObject(subDictItem, item.Value, info.ModelName, settings, name, null);
                subDict.Add(item.Key.ToString(), subDictItem);
            }
            builder.Add(name.LocalName, subDict);
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            IDictionary dict = value.Convert<IDictionary>();
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            QName name = info.QName;

            DictionaryBuilder subDict = builder.CreateBuilder();
            foreach (DictionaryEntry item in dict)
            {
                DictionaryBuilder subDictItem = builder.CreateBuilder();
                WriteObject(subDictItem, item.Value, info.ModelName, settings, name, null);
                subDict.Add(item.Key.ToString(), subDictItem);
            }
            builder.Add(name.LocalName, subDict);
        }

        public void WriteComplexContent(ComplexContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void Write(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData, BaseObjectAttribute attribute)
        {
            SerializerUtil.WriteObject(this, writer, receiver, modelName, settings,
                root, attribute, null, Options);
        }

        public void WriteObject(object writer, object receiver, string modelName,
            WriteSettings settings, QName root, object serializerData)
        {
            WriteAObject(writer, receiver, modelName, settings);
        }

        public void WriteList(object writer, IEnumerable receiver, SimpleElementAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteDictionary(object writer, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            throw new NotSupportedException();
        }

        public object BeginWrite(object writer, object receiver, WriteSettings settings, QName root)
        {
            return null;
        }

        public void EndWrite(object writer, object receiver, WriteSettings settings)
        {
        }

        #endregion IObjectSerializer 成员

        private static void SetValue(object writer, ObjectPropertyInfo info, string fieldName,
            object value, WriteSettings settings)
        {
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            builder.Add(fieldName, ObjectUtil.ToString(info.Converter, value, settings));
        }

        private static void SetValue(object writer, string fieldName, IList value)
        {
            DictionaryBuilder builder = writer.Convert<DictionaryBuilder>();
            builder.Add(fieldName, value);
        }

        private void WriteAObject(object writer, object receiver, string modelName,
            WriteSettings settings)
        {
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            foreach (var attr in info.Attributes)
                SerializerUtil.WritePropertyInfo(this, writer, receiver, settings, attr, null);

            SerializerUtil.WriteElements(this, info, writer, receiver, settings);
        }

        public override string ToString()
        {
            return "Dictionary类型对象转换器";
        }
    }
}