using System;
using System.IO;
using YJC.Toolkit.Cache;
using System.Collections;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "Dtd格式转换器", Author = "YJC", CreateDate = "2013-09-30")]
    internal class DtdObjectSerializer : IObjectSerializer
    {
        public static IObjectSerializer Instance = new DtdObjectSerializer();

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
            throw new NotImplementedException();
        }

        public object CreateCustomWriter(object customObject)
        {
            throw new NotSupportedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteComplexContent(ComplexContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void Write(object writer, object receiver, string modelName,
            WriteSettings settings, QName root, object serializerData, BaseObjectAttribute attribute)
        {
            throw new NotImplementedException();
        }

        public void WriteObject(object writer, object receiver, string modelName,
            WriteSettings settings, QName root, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteList(object writer, IEnumerable receiver, SimpleElementAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            throw new NotImplementedException();
        }

        public void WriteDictionary(object writer, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            throw new NotImplementedException();
        }

        public object BeginWrite(object writer, object receiver, WriteSettings settings, QName root)
        {
            throw new NotImplementedException();
        }

        public void EndWrite(object writer, object receiver, WriteSettings settings)
        {
            throw new NotImplementedException();
        }

        #endregion IObjectSerializer 成员

        public override string ToString()
        {
            return "DTD格式对象转换器";
        }
    }
}