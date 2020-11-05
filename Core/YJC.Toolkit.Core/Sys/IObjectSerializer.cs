using System.Collections;
using System.IO;

namespace YJC.Toolkit.Sys
{
    public interface IObjectSerializer
    {
        object CreateReader(string input, ReadSettings settings);

        object CreateReader(Stream stream, ReadSettings settings);

        object CreateCustomReader(object customObject);

        object CreateWriter(Stream stream, WriteSettings settings);

        object CreateCustomWriter(object customObject);

        bool ReadToRoot(object reader, QName root);

        void ReadAttribute(SimpleAttributeAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadElement(SimpleElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadTagElement(TagElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadObjectElement(ObjectElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadComplexElement(SimpleComplexElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadTextContent(TextContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadDictionary(DictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadObjectDictionary(ObjectDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadDynamicDictionary(DynamicDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void ReadComplexContent(ComplexContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData);

        void Read(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, BaseObjectAttribute attribute);

        void ReadObject(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, object serializerData);

        void ReadList(object reader, IList receiver, SimpleElementAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData);

        void ReadDictionary(object reader, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData);

        void WriteAttribute(SimpleAttributeAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteElement(SimpleElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void WriteComplexContent(ComplexContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData);

        void Write(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData, BaseObjectAttribute attribute);

        void WriteObject(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData);

        void WriteList(object writer, IEnumerable receiver, SimpleElementAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData);

        void WriteDictionary(object writer, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData);

        object BeginWrite(object writer, object receiver, WriteSettings settings, QName root);

        void EndWrite(object writer, object receiver, WriteSettings settings);
    }
}