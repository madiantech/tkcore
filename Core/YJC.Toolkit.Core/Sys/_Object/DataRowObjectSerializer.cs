using System;
using System.Data;
using System.IO;
using YJC.Toolkit.Cache;
using System.Collections;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "DataRow格式转换器", Author = "YJC", CreateDate = "2013-10-25")]
    internal sealed class DataRowObjectSerializer : IObjectSerializer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static readonly IObjectSerializer Instance = new DataRowObjectSerializer();

        private static readonly SerializerOptions Options = new SerializerOptions
        {
            ReadObject = true,
            WriteObject = true,
            ReadDictionary = true,
            WriteDictionary = true
        };

        private DataRowObjectSerializer()
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
            return SerializerUtil.CheckCustomObject<DataRow>(customObject);
        }

        public object CreateWriter(Stream stream, WriteSettings settings)
        {
            throw new NotSupportedException();
        }

        public object CreateCustomWriter(object customObject)
        {
            return SerializerUtil.CheckCustomObject<DataRow>(customObject);
        }

        public bool ReadToRoot(object reader, QName root)
        {
            return true;
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SerializerUtil.SetObjectValue(receiver, settings, info,
                GetValue(reader, info.LocalName), attribute.AutoTrim);
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
                throw new NotSupportedException();

            string value = GetValue(reader, info.LocalName);
            object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info,
                value, attribute.AutoTrim);

            SerializerUtil.AddElementValue(attribute, receiver, info, objValue);
        }

        public void ReadTagElement(TagElementAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void ReadObjectElement(ObjectElementAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
                throw new NotSupportedException();

            string xml = GetValue(reader, info.LocalName);
            if (string.IsNullOrEmpty(xml))
                SerializerUtil.AddElementValue(attribute, receiver, info, null);
            else
            {
                Type objectType = info.ObjectType;
                object subObject = attribute.UseConstructor ? ObjectUtil.CreateObjectWithCtor(objectType)
                    : ObjectUtil.CreateObject(objectType);
                subObject.ReadXml(xml, settings, QName.ToolkitNoNS);
                SerializerUtil.AddElementValue(attribute, receiver, info, subObject);
                SerializerUtil.SetParent(receiver, subObject);
            }
        }

        public void ReadComplexElement(SimpleComplexElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            ReadElement(attribute, reader, receiver, settings, info, serializerData);
        }

        public void ReadTextContent(TextContentAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SerializerUtil.SetObjectValue(receiver, settings, info,
                GetValue(reader, ObjectInfo.CONTENT_NAME), attribute.AutoTrim);
        }

        public void ReadDictionary(DictionaryAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
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

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
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
            DataRow row = reader.Convert<DataRow>();
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }
            DataColumnCollection cols = row.Table.Columns;

            foreach (DataColumn column in cols)
            {
                string columnName = column.ColumnName;

                SerializerUtil.ReadProperty(this, reader, receiver, settings,
                    info, columnName, modelName);
            }
            SerializerUtil.ReadObjectCallBack(receiver);
            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
        }

        public void ReadDictionary(object reader, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            SerializerUtil.CheckSimpleDictionary(attribute, this);

            DataRow row = reader.Convert<DataRow>();
            DataColumnCollection cols = row.Table.Columns;
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            DictionaryAttribute dictAttr = attribute.Convert<DictionaryAttribute>();

            foreach (DataColumn column in cols)
            {
                string columnName = column.ColumnName;

                receiver[columnName] = SerializerUtil.GetPropertyObject(receiver, settings,
                    info, GetValue(reader, columnName), dictAttr.AutoTrim);
            }
        }

        public void ReadList(object reader, IList receiver, SimpleElementAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteAttribute(SimpleAttributeAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SetValue(writer, info.LocalName, value, info, settings);
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (attribute.IsMultiple)
                throw new NotSupportedException();
            else
                SetValue(writer, info.LocalName, value, info, settings);
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            throw new NotSupportedException();
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            string xml = value.WriteXml(settings, QName.ToolkitNoNS);
            SetValue(writer, info.LocalName, xml, info, settings);
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            WriteElement(attribute, writer, value, settings, info, serializerData);
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            SetValue(writer, ObjectInfo.CONTENT_NAME, value, info, settings);
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
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
            throw new NotSupportedException();
        }

        public void WriteDictionary(object writer, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            SerializerUtil.CheckSimpleDictionary(attribute, this);

            DataRow row = writer.Convert<DataRow>();
            DataColumnCollection cols = row.Table.Columns;
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            DictionaryAttribute dictAttr = attribute.Convert<DictionaryAttribute>();

            foreach (DataColumn column in cols)
            {
                string columnName = column.ColumnName;

                SetValue(writer, columnName, receiver[columnName], info, settings);
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

        private static string GetValue(object reader, string fieldName)
        {
            DataRow row = reader.Convert<DataRow>();

            try
            {
                object value = row[fieldName];
                return value == DBNull.Value ? null : value.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static void SetValue(object writer, string fieldName, object value,
            ObjectPropertyInfo info, WriteSettings settings)
        {
            DataRow row = writer.Convert<DataRow>();
            try
            {
                row[fieldName] = ObjectUtil.ToString(info.Converter, value, settings);
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            return "DataRow格式对象转换器";
        }
    }
}