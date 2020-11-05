using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys.Json;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "Json格式转换器", Author = "YJC", CreateDate = "2013-09-29")]
    internal sealed class JsonObjectSerializer : IObjectSerializer
    {
        public static IObjectSerializer Instance = new JsonObjectSerializer();
        private const string CONTENT_NAME = "Content";

        private JsonObjectSerializer()
        {
        }

        #region IObjectSerializer 成员

        public object CreateReader(string input, ReadSettings settings)
        {
            StringReader reader = new StringReader(input);
            return new JsonTextReader(reader);
        }

        public object CreateReader(Stream stream, ReadSettings settings)
        {
            StreamReader reader = new StreamReader(stream, settings.Encoding, true);
            return new JsonTextReader(reader);
        }

        public object CreateCustomReader(object customObject)
        {
            throw new NotSupportedException();
        }

        public object CreateWriter(Stream stream, WriteSettings settings)
        {
            StreamWriter writer = new StreamWriter(stream, settings.Encoding);
            return new JsonTextWriter(writer) { QuoteChar = settings.QuoteChar };
        }

        public object CreateCustomWriter(object customObject)
        {
            throw new NotSupportedException();
        }

        public bool ReadToRoot(object reader, QName root)
        {
            return true;
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            object value = ReadSimpleValue(reader, receiver, settings, info, attribute.AutoTrim);
            SerializerUtil.CheckRequiredAttribute(attribute, receiver, info,
                value.ConvertToString());
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
            {
                JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
                IList list = attribute.GetList(receiver, info);
                // 考虑支持多层次Array读取，外层Object可能读取过，所以这里做判断对此进行屏蔽 2019.6.24
                if (jsonReader.TokenType != JsonToken.StartArray)
                    AssertRead(jsonReader);
                AssertReadState(jsonReader, JsonToken.StartArray, info.PropertyName);
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.EndArray)
                        break;

                    object value = GetSimpleValue(jsonReader, receiver, settings,
                        info, attribute.AutoTrim);
                    list.Add(value);
                }
            }
            else
                ReadSimpleValue(reader, receiver, settings, info, attribute.AutoTrim);
        }

        public void ReadTagElement(TagElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            AssertRead(jsonReader);
            AssertReadState(jsonReader, JsonToken.StartObject, info.PropertyName);
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                    break;

                AssertReadState(jsonReader, JsonToken.PropertyName, info.PropertyName);
                string localName = jsonReader.Value.ToString();
                ObjectPropertyInfo childInfo = attribute.ChildElements[localName];
                TkDebug.AssertNotNull(childInfo, string.Format(ObjectUtil.SysCulture,
                    "{0}没有在TagElement类型中声明，无法读取", localName), this);

                childInfo.Attribute.ReadObject(this, reader, receiver, settings, childInfo, serializerData);
            }

            SerializerUtil.CheckElementRequired(attribute.ChildElements, receiver);
        }

        public void ReadObjectElement(ObjectElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            Type objectType = info.ObjectType;
            // 考虑支持多层次Array读取，外层Object可能读取过，所以这里做判断对此进行屏蔽 2019.6.24
            if (jsonReader.TokenType != JsonToken.StartArray)
                AssertRead(jsonReader);

            if (attribute.IsMultiple)
            {
                IList list = attribute.GetList(receiver, info);
                if (attribute.UseJsonObject)
                {
                    bool useArray = jsonReader.TokenType == JsonToken.StartArray;
                    if (useArray)
                    {
                        while (jsonReader.Read())
                        {
                            if (jsonReader.TokenType == JsonToken.EndArray)
                                break;
                            object subObject = ReadObject(attribute.UseConstructor, settings,
                                jsonReader, objectType, receiver, info.ModelName);
                            list.Add(subObject);
                        }
                    }
                    else
                    {
                        object subObject = ReadObject(attribute.UseConstructor, settings,
                            jsonReader, objectType, receiver, info.ModelName);
                        list.Add(subObject);
                    }
                }
                else
                {
                    AssertReadState(jsonReader, JsonToken.StartArray, info.PropertyName);
                    while (jsonReader.Read())
                    {
                        if (jsonReader.TokenType == JsonToken.EndArray)
                            break;
                        object subObject = ReadObject(attribute.UseConstructor, settings,
                            jsonReader, objectType, receiver, info.ModelName);
                        list.Add(subObject);
                    }
                }
            }
            else
            {
                AssertReadState(jsonReader, JsonToken.StartObject, info.PropertyName);

                object subObject = ReadObject(attribute.UseConstructor, settings, jsonReader,
                    objectType, receiver, info.ModelName);
                info.SetValue(receiver, subObject);
            }
        }

        public void ReadComplexElement(SimpleComplexElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
            {
                JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
                IList list = attribute.GetList(receiver, info);
                AssertRead(jsonReader);
                AssertReadState(jsonReader, JsonToken.StartArray, info.PropertyName);
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.EndArray)
                        break;

                    object value = ReadComplexValue(jsonReader, receiver, settings, info);
                    list.Add(value);
                }
            }
            else
                ReadComplexValue(reader, receiver, settings, info);
        }

        public void ReadTextContent(TextContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            object value = ReadSimpleValue(reader, receiver, settings, info, attribute.AutoTrim);
            SerializerUtil.CheckRequiredContent(attribute, receiver, info,
                value.ConvertToString());
        }

        public void ReadDictionary(DictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            // 考虑支持多层次Dictionary读取，外层Object可能读取过，所以这里做判断对此进行屏蔽 2019.6.24
            if (jsonReader.TokenType != JsonToken.StartObject)
                AssertRead(jsonReader);
            AssertReadState(jsonReader, JsonToken.StartObject, info.PropertyName);
            IDictionary dict = attribute.GetDictionary(receiver, info);
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                    break;
                AssertReadState(jsonReader, JsonToken.PropertyName, info.PropertyName);
                string name = jsonReader.Value.ToString();
                AssertRead(jsonReader);
                object value = GetSimpleValue(jsonReader, receiver, settings,
                    info, attribute.AutoTrim);
                dict[name] = value;
            }
        }

        public void ReadObjectDictionary(ObjectDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            AssertRead(jsonReader);
            AssertReadState(jsonReader, JsonToken.StartObject, info.PropertyName);
            IDictionary dict = attribute.GetDictionary(receiver, info);
            Type objectType = info.ObjectType;

            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                    break;
                AssertReadState(jsonReader, JsonToken.PropertyName, info.PropertyName);
                string name = jsonReader.Value.ToString();
                AssertRead(jsonReader);
                // 支持内嵌Json数组
                AssertReadState(jsonReader, JsonToken.StartObject, JsonToken.StartArray);
                object subObject = ReadObject(attribute.UseConstructor, settings, jsonReader,
                    objectType, receiver, info.ModelName);
                dict[name] = subObject;
                //dict.Add(name, value);
            }
        }

        public void ReadDynamicDictionary(DynamicDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            AssertRead(jsonReader);
            AssertReadState(jsonReader, JsonToken.StartObject, info.PropertyName);
            IDictionary dict = attribute.GetDictionary(receiver, info);

            var configData = attribute.PlugInFactory.ConfigData;
            //var propertyInfo = info.Convert<ReflectorObjectPropertyInfo>().Property;
            var elementReader = new ConfigFactoryElementReader(attribute, info, info.ModelName);

            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                    break;
                AssertReadState(jsonReader, JsonToken.PropertyName, info.PropertyName);
                string name = jsonReader.Value.ToString();
                AssertRead(jsonReader);

                AssertReadState(jsonReader, JsonToken.StartObject, info.PropertyName);
                ObjectPropertyInfo subPropertyInfo = elementReader[name];
                Type objectType = subPropertyInfo.ObjectType;
                bool useConstructor = subPropertyInfo.Attribute.Convert<ObjectElementAttribute>().UseConstructor;
                object subObject = ReadObject(useConstructor, settings, jsonReader,
                    objectType, receiver, info.ModelName);
                dict[name] = subObject;
                //dict.Add(name, value);
            }
        }

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            TkDebug.ThrowToolkitException("Json不支持ComplexContent节点", this);
        }

        public void Read(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, BaseObjectAttribute attribute)
        {
            SerializerUtil.ReadObject(this, reader, receiver, modelName, settings,
                root, attribute, null, SerializerOptions.All);
        }

        public void ReadObject(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, object serializerData)
        {
            TkDebug.AssertArgumentNull(receiver, "receiver", this);

            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            // 由于支持了递归调用，这里不再是首层结构，所以，只有首层才能出去下列代码。递归调用时，该代码不再触动。2019.6.24
            if (jsonReader.TokenType == JsonToken.None)
            {
                AssertRead(jsonReader);
                AssertReadState(jsonReader, JsonToken.StartObject, null);
            }

            InternalRead(jsonReader, receiver, modelName, settings);
        }

        public void ReadList(object reader, IList receiver, SimpleElementAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute);
            attribute.ReadObject(this, reader, receiver, settings, info, serializerData);
        }

        public void ReadDictionary(object reader, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            attribute.ReadObject(this, reader, receiver, settings, info, serializerData);
        }

        public void WriteAttribute(SimpleAttributeAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info,
            object serializerData)
        {
            WriteSimpleElement(attribute, writer, value, settings, info, attribute.UseSourceType);
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
            {
                var list = value.Convert<IEnumerable>();
                JsonWriter jsonWriter = writer.Convert<JsonWriter>();
                QName name = info.QName;
                if (info.WriteMode == SerializerWriteMode.WriteName)
                    jsonWriter.WritePropertyName(name.LocalName);
                jsonWriter.WriteStartArray();
                foreach (var itemValue in list)
                    WriteSimpleJsonValue(jsonWriter, itemValue, settings, info, attribute.UseSourceType);
                //jsonWriter.WriteValue(ObjectUtil.ToString(info, itemValue, settings));
                jsonWriter.WriteEndArray();
            }
            else
                WriteSimpleElement(attribute, writer, value, settings, info, attribute.UseSourceType);
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonWriter jsonWriter = writer.Convert<JsonWriter>();
            QName name = info.QName;
            jsonWriter.WritePropertyName(name.LocalName);
            jsonWriter.WriteStartObject();
            var infoArray = attribute.ChildElements.CreateOrderPropertyInfoList().ToArray();
            TkDebug.Assert(infoArray.Length == 1, string.Format(ObjectUtil.SysCulture,
                "TagElement的子节点必须有且仅有一个，当前{1}下有{0}个", infoArray.Length, name), this);
            var item = infoArray[0];
            if (item.IsSingle)
            {
                item.Content.Attribute.WriteObject(this, writer, value,
                    settings, item.Content, item);
            }
            else
            {
                IList list = value as IList;
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        var property = item.Get(list[0].GetType());
                        property.Attribute.WriteObject(this, writer, value, settings,
                            property, item);
                    }
                }
                else
                {
                    var property = item.Get(value.GetType());
                    property.Attribute.WriteObject(this, writer, value, settings,
                        property, item);
                }
            }

            jsonWriter.WriteEndObject();
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonTextWriter jsonWriter = writer.Convert<JsonTextWriter>();
            QName name = info.QName;

            if (attribute.IsMultiple)
            {
                var list = value.Convert<IEnumerable>();
                if (attribute.UseJsonObject)
                {
                    var valueList = EnumUtil.Convert(list);
                    IElementWriter elementWriter = serializerData.Convert<IElementWriter>();
                    var itemGroup = from item in valueList
                                    where item != null
                                    let propInfo = elementWriter.Get(item.GetType())
                                    group new { Item = item, Info = propInfo } by propInfo.LocalName;
                    foreach (var group in itemGroup)
                    {
                        jsonWriter.WritePropertyName(group.Key);
                        jsonWriter.WriteStartArray();
                        foreach (var item in group)
                        {
                            WriteAObject(writer, item.Item, item.Info.ModelName, settings, jsonWriter);
                        }
                        jsonWriter.WriteEndArray();
                    }
                    //foreach (object itemValue in list)
                    //{
                    //    if (itemValue == null)
                    //        continue;
                    //    var subPropertyInfo = elementWriter.Get(itemValue.GetType());
                    //    jsonWriter.WritePropertyName(subPropertyInfo.LocalName);
                    //    WriteAObject(writer, itemValue, subPropertyInfo.ModelName, settings, jsonWriter);
                    //}
                }
                else
                {
                    if (info.WriteMode == SerializerWriteMode.WriteName)
                        jsonWriter.WritePropertyName(name.LocalName);
                    jsonWriter.WriteStartArray();
                    foreach (var itemValue in list)
                        Write(writer, itemValue, info.ModelName, settings, name, serializerData, null);
                    jsonWriter.WriteEndArray();
                }
            }
            else
            {
                jsonWriter.WritePropertyName(name.LocalName);
                Write(writer, value, info.ModelName, settings, name, serializerData, null);
            }
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
            {
                IList list = value.Convert<IList>();
                JsonWriter jsonWriter = writer.Convert<JsonWriter>();
                QName name = info.QName;
                jsonWriter.WritePropertyName(name.LocalName);
                jsonWriter.WriteStartArray();
                foreach (var itemValue in list)
                    WriteComplexContent(jsonWriter, itemValue.ConvertToString());
                //jsonWriter.WriteValue(ObjectUtil.ToString(info, itemValue, settings));
                jsonWriter.WriteEndArray();
            }
            else
                WriteComplexElement(attribute, writer, value, settings, info);
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            JsonWriter jsonWriter = writer.Convert<JsonWriter>();
            jsonWriter.WritePropertyName(CONTENT_NAME);
            WriteSimpleJsonValue(jsonWriter, value, settings, info, attribute.UseSourceType);
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            IDictionary dict = value.Convert<IDictionary>();
            JsonWriter jsonWriter = writer.Convert<JsonWriter>();
            if (info.WriteMode == SerializerWriteMode.WriteName)
            {
                QName name = info.QName;
                jsonWriter.WritePropertyName(name.LocalName);
            }
            WriteDictionary(dict, jsonWriter, info.Converter, settings);
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            IDictionary dict = value.Convert<IDictionary>();
            JsonTextWriter jsonWriter = writer.Convert<JsonTextWriter>();
            QName name = info.QName;
            if (info.WriteMode == SerializerWriteMode.WriteName)
            {
                jsonWriter.WritePropertyName(name.LocalName);
            }
            jsonWriter.WriteStartObject();
            foreach (DictionaryEntry item in dict)
            {
                jsonWriter.WritePropertyName(item.Key.ToString());
                Write(writer, item.Value, info.ModelName, settings, name, serializerData, null);
            }
            jsonWriter.WriteEndObject();
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            IDictionary dict = value.Convert<IDictionary>();
            JsonTextWriter jsonWriter = writer.Convert<JsonTextWriter>();

            if (info.WriteMode == SerializerWriteMode.WriteName)
            {
                QName name = info.QName;
                jsonWriter.WritePropertyName(name.LocalName);
            }
            jsonWriter.WriteStartObject();
            foreach (DictionaryEntry item in dict)
            {
                jsonWriter.WritePropertyName(item.Key.ToString());
                WriteAObject(writer, item.Value, info.ModelName, settings, jsonWriter);
            }
            jsonWriter.WriteEndObject();
        }

        public void WriteComplexContent(ComplexContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            TkDebug.ThrowToolkitException("Json不支持ComplexContent节点", this);
        }

        public void Write(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData, BaseObjectAttribute attribute)
        {
            SerializerUtil.WriteObject(this, writer, receiver, modelName, settings,
                root, attribute, null, SerializerOptions.All);
        }

        public void WriteObject(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData)
        {
            JsonTextWriter jsonWriter = writer.Convert<JsonTextWriter>();

            WriteAObject(writer, receiver, modelName, settings, jsonWriter);
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
            attribute.WriteObject(this, writer, receiver, settings, info, serializerData);
        }

        public object BeginWrite(object writer, object receiver, WriteSettings settings, QName root)
        {
            return null;
        }

        public void EndWrite(object writer, object receiver, WriteSettings settings)
        {
            JsonTextWriter jsonWriter = writer.Convert<JsonTextWriter>();
            jsonWriter.Flush();
        }

        #endregion IObjectSerializer 成员

        private static void WriteDictionary(IDictionary dict, JsonWriter jsonWriter,
            ITkTypeConverter converter, WriteSettings settings)
        {
            jsonWriter.WriteStartObject();
            foreach (DictionaryEntry item in dict)
            {
                jsonWriter.WritePropertyName(item.Key.ToString());
                jsonWriter.WriteValue(ObjectUtil.ToString(converter, item.Value, settings));
            }
            jsonWriter.WriteEndObject();
        }

        private object ReadObject(bool usector, ReadSettings settings, JsonTextReader jsonReader,
            Type objectType, object receiver, string modelName)
        {
            object subObject = usector ? ObjectUtil.CreateObjectWithCtor(objectType)
                : ObjectUtil.CreateObject(objectType);
            //InternalRead(jsonReader, subObject, modelName, settings);
            //允许递归调用，内嵌的如果不是对象，是数组或者Dictionary，都支持了
            Read(jsonReader, subObject, modelName, settings, null, null);
            SerializerUtil.SetParent(receiver, subObject);
            return subObject;
        }

        private static void WriteSimpleJsonValue(JsonWriter jsonWriter, object value,
            WriteSettings settings, ObjectPropertyInfo info, bool useSourceType)
        {
            if (useSourceType)
            {
                if (value == null)
                    jsonWriter.WriteValue((string)null);
                Type type = value.GetType();
                if (type.IsNullableType())
                {
                    Type nestType = Nullable.GetUnderlyingType(type);
                    value = Convert.ChangeType(value, nestType, ObjectUtil.SysCulture);
                }
                jsonWriter.WriteValue(value);
            }
            else
                jsonWriter.WriteValue(ObjectUtil.ToString(info.Converter, value, settings));
        }

        private static void WriteSimpleElement(NamedAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, bool useSourceType)
        {
            //if (ObjectUtil.IsDefaultValue(attribute.DefaultValue, value))
            //    return;

            JsonWriter jsonWriter = writer.Convert<JsonWriter>();
            if (info.WriteMode == SerializerWriteMode.WriteName)
            {
                QName name = info.QName;
                jsonWriter.WritePropertyName(name.LocalName);
            }
            WriteSimpleJsonValue(jsonWriter, value, settings, info, useSourceType);
        }

        private void WriteComplexElement(NamedAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info)
        {
            JsonWriter jsonWriter = writer.Convert<JsonWriter>();
            if (info.WriteMode == SerializerWriteMode.WriteName)
            {
                QName name = info.QName;
                jsonWriter.WritePropertyName(name.LocalName);
            }
            WriteComplexContent(jsonWriter, value.ConvertToString());
        }

        private void InternalRead(JsonTextReader reader, object receiver, string modelName, ReadSettings settings)
        {
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            bool readElement = false;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                AssertReadState(reader, JsonToken.PropertyName, null);
                string localName = reader.Value.Convert<string>();

                ObjectPropertyInfo property = info.GetAttribute(localName);
                if (property != null)
                    property.Attribute.ReadObject(this, reader, receiver, settings, property, null);
                else
                {
                    property = info.Elements[localName];
                    if (property != null)
                    {
                        readElement = true;
                        property.Attribute.ReadObject(this, reader, receiver, settings, property, null);
                    }
                    else
                    {
                        if (localName == CONTENT_NAME && info.Content != null)
                        {
                            property = info.Content;
                            property.Attribute.ReadObject(this, reader, receiver, settings, property, null);
                        }
                        else
                        {
                            property = SerializerUtil.CustomRead(receiver, localName, modelName, () => null);
                            if (property != null)
                                property.Attribute.ReadObject(this, reader, receiver, settings, property, null);
                            else
                                SkipProperty(reader);
                        }
                    }
                }
            }
            SerializerUtil.ReadObjectCallBack(receiver);

            if (readElement)
                SerializerUtil.CheckElementRequired(info.Elements, receiver);
            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
        }

        private object GetSimpleValue(JsonTextReader reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, bool autoTrim)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                case JsonToken.Undefined:
                    return null;

                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                case JsonToken.String:
                    string value = reader.Value.ToString();
                    return SerializerUtil.GetPropertyObject(receiver, settings, info, value, autoTrim);

                case JsonToken.Date:
                case JsonToken.Bytes:
                    return reader.Value;

                default:
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "读取属性{3}时发生JSON错误。当前的状态是{0}，不是有效的值状态，在第{1}行，第{2}列",
                        reader.TokenType, reader.CurrentLineNumber, reader.CurrentLinePosition,
                        info.PropertyName), this);
                    return null;
            }
        }

        private object ReadSimpleValue(object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, bool autoTrim)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            AssertRead(jsonReader);
            object value = GetSimpleValue(jsonReader, receiver, settings, info, autoTrim);
            info.SetValue(receiver, value);

            return value;
        }

        private object ReadComplexValue(object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info)
        {
            JsonTextReader jsonReader = reader.Convert<JsonTextReader>();
            AssertRead(jsonReader);
            string strValue = ReadComplexContent(jsonReader);
            object value = SerializerUtil.GetPropertyObject(receiver, settings, info, strValue, false);
            info.SetValue(receiver, value);

            return value;
        }

        private void SkipProperty(JsonTextReader reader)
        {
            int depth = reader.Depth;
            AssertRead(reader);
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject && reader.Depth == depth)
                            break;
                    }
                    AssertReadState(reader, JsonToken.EndObject, null);
                    break;

                case JsonToken.StartArray:
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndArray && reader.Depth == depth)
                            break;
                    }
                    AssertReadState(reader, JsonToken.EndArray, null);
                    break;

                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    break;

                default:
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "当前的状态是{0}，不是有效的值状态或者对象数组的起始状态，在第{1}行，第{2}列",
                        reader.TokenType, reader.CurrentLineNumber, reader.CurrentLinePosition), this);
                    break;
            }
            //AssertRead(reader);
        }

        private void AssertRead(JsonTextReader reader)
        {
            if (!reader.Read())
                TkDebug.ThrowToolkitException(
                    "需要读取Json数据时，却无法读出，请确认数据是否完整", this);
        }

        private void AssertReadState(JsonTextReader reader, JsonToken token, string propertyName)
        {
            if (reader.TokenType != token)
            {
                string propHint = string.IsNullOrEmpty(propertyName) ? string.Empty :
                    string.Format(ObjectUtil.SysCulture, "分析属性{0}出错：", propertyName);
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "{3}Json数据的状态有问题，当前需要的状态是{0}，实际状态却是{1}，在第{2}行，第{3}列",
                    token, reader.TokenType, reader.CurrentLineNumber, reader.CurrentLinePosition,
                    propHint), this);
            }
        }

        private void AssertReadState(JsonTextReader reader, JsonToken token1, JsonToken token2)
        {
            if (reader.TokenType != token1 && reader.TokenType != token2)
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "Json数据的状态有问题，当前需要的状态是{0}或{1}，实际状态却是{2}，在第{3}行，第{4}列",
                    token1, token2, reader.TokenType, reader.CurrentLineNumber, reader.CurrentLinePosition), this);
        }

        private void WriteAObject(object writer, object receiver, string modelName,
            WriteSettings settings, JsonTextWriter jsonWriter)
        {
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }
            jsonWriter.WriteStartObject();
            foreach (var attr in info.Attributes)
                SerializerUtil.WritePropertyInfo(this, writer, receiver, settings, attr, null);

            if (info.Content != null)
                SerializerUtil.WritePropertyInfo(this, writer, receiver, settings, info.Content, null);
            else
                SerializerUtil.WriteElements(this, info, writer, receiver, settings);
            jsonWriter.WriteEndObject();
            jsonWriter.Flush();

            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
        }

        private static string ReadComplexContent(JsonTextReader reader)
        {
            StringBuilder text = new StringBuilder();
            StringWriter sw = new StringWriter(text);
            JsonTextWriter writer = new JsonTextWriter(sw);
            using (writer)
            {
                writer.WriteToken(reader);
                writer.Flush();
            }
            string value = text.ToString();
            return value;
        }

        private void WriteComplexContent(JsonWriter writer, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                writer.WriteNull();
                return;
            }
            try
            {
                JsonTextReader reader = CreateReader(text,
                    ObjectUtil.ReadSettings).Convert<JsonTextReader>();
                using (reader)
                {
                    while (reader.Read())
                    {
                        writer.WriteToken(reader);
                    }
                    writer.Flush();
                }
            }
            catch
            {
                writer.WriteNull();
            }
        }

        public override string ToString()
        {
            return "Json格式对象转换器";
        }
    }
}