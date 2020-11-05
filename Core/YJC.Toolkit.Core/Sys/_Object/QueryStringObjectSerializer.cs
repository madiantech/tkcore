using System;
using System.Collections;
using System.IO;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "QueryString格式转换器", Author = "YJC", CreateDate = "2014-11-06")]
    internal class QueryStringObjectSerializer : IObjectSerializer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1823:AvoidUnusedPrivateFields")]
        private static readonly IObjectSerializer Instance = new QueryStringObjectSerializer();

        private static readonly SerializerOptions Options = new SerializerOptions
        {
            ReadObject = true,
            WriteObject = true,
            ReadList = true,
            WriteList = true,
            ReadDictionary = true,
            WriteDictionary = true,
            CheckListAttribute = true
        };

        private QueryStringObjectSerializer()
        {
        }

        #region IObjectSerializer 成员

        public object CreateReader(string input, ReadSettings settings)
        {
            QueryStringBuilder builder = new QueryStringBuilder(input);
            return builder;
        }

        public object CreateReader(Stream stream, ReadSettings settings)
        {
            using (StreamReader reader = new StreamReader(stream, settings.Encoding, true))
            {
                string text = reader.ReadToEnd();
                QueryStringBuilder builder = new QueryStringBuilder(text);
                return builder;
            }
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
            return SerializerUtil.CheckCustomObject<QueryStringBuilder>(customObject);
        }

        public bool ReadToRoot(object reader, QName root)
        {
            return true;
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SerializerUtil.SetObjectValue(receiver, settings, info,
                GetValue(reader, info.LocalName).ConvertToString(), attribute.AutoTrim);
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
            {
                QueryStringValue itemValue = GetValue(reader, info.LocalName);
                if (itemValue == null)
                    return;
                var itemValues = itemValue.Values;
                if (itemValues == null)
                    return;

                foreach (var item in itemValues)
                {
                    object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info,
                        item, attribute.AutoTrim);
                    SerializerUtil.AddElementValue(attribute, receiver, info, objValue);
                }
            }
            else
                SerializerUtil.SetObjectValue(receiver, settings, info,
                    GetValue(reader, info.LocalName).ConvertToString(), attribute.AutoTrim);
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
            SerializerUtil.SetObjectValue(receiver, settings, info,
                GetValue(reader, ObjectInfo.CONTENT_NAME).ConvertToString(), attribute.AutoTrim);
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
            throw new NotSupportedException();
        }

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void Read(object reader, object receiver, string modelName,
            ReadSettings settings, QName root, BaseObjectAttribute attribute)
        {
            SerializerUtil.ReadObject(this, reader, receiver, modelName, settings,
                root, attribute, null, Options);
        }

        public void ReadObject(object reader, object receiver, string modelName,
            ReadSettings settings, QName root, object serializerData)
        {
            QueryStringBuilder builder = reader.Convert<QueryStringBuilder>();
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);

            foreach (string key in builder.AllKeys)
                SerializerUtil.ReadProperty(this, reader, receiver, settings, info, key, modelName);
            SerializerUtil.ReadObjectCallBack(receiver);
        }

        public void ReadList(object reader, IList receiver, SimpleElementAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            attribute.ReadObject(this, reader, receiver, settings, info, serializerData);
        }

        public void ReadDictionary(object reader, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            SerializerUtil.CheckSimpleDictionary(attribute, this);

            QueryStringBuilder builder = reader.Convert<QueryStringBuilder>();
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            DictionaryAttribute dictAttr = attribute.Convert<DictionaryAttribute>();

            foreach (string key in builder.AllKeys)
            {
                var value = builder[key];
                string strValue = value.ToString();
                receiver[key] = SerializerUtil.GetPropertyObject(receiver, settings,
                    info, strValue, dictAttr.AutoTrim);
            }
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
                foreach (object itemValue in list)
                    SetValue(writer, info, info.LocalName, itemValue, settings);
            }
            else
                SetValue(writer, info, info.LocalName, value, settings);
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SetValue(writer, info, ObjectInfo.CONTENT_NAME, value, settings);
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
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
            SerializerUtil.Write(this, writer, receiver, modelName, settings);
        }

        public void WriteList(object writer, IEnumerable receiver, SimpleElementAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            attribute.WriteObject(this, writer, receiver, settings, info, serializerData);
        }

        public void WriteDictionary(object writer, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            foreach (string key in receiver.Keys)
            {
                SetValue(writer, info, key, receiver[key], settings);
            }
        }

        public object BeginWrite(object writer, object receiver, WriteSettings settings, QName root)
        {
            return null;
        }

        public void EndWrite(object writer, object receiver, WriteSettings settings)
        {
        }

        #endregion IObjectSerializer 成员

        private static QueryStringValue GetValue(object reader, string fieldName)
        {
            QueryStringBuilder builder = reader.Convert<QueryStringBuilder>();

            return builder[fieldName];
        }

        private static void SetValue(object writer, ObjectPropertyInfo info, string fieldName,
            object value, WriteSettings settings)
        {
            QueryStringBuilder builder = writer.Convert<QueryStringBuilder>();
            builder.Add(fieldName, ObjectUtil.ToString(info.Converter, value, settings));
        }

        public override string ToString()
        {
            return "QueryString格式对象转换器";
        }
    }
}