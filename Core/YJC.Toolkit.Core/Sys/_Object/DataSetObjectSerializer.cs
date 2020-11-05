using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "DataSet格式转换器", Author = "YJC", CreateDate = "2013-10-25")]
    internal class DataSetObjectSerializer : IObjectSerializer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static readonly IObjectSerializer Instance = new DataSetObjectSerializer();

        private static readonly SerializerOptions Options = new SerializerOptions
        {
            WriteObject = true,
        };

        private DataSetObjectSerializer()
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
            return SerializerUtil.CheckCustomObject<DataSet>(customObject);
        }

        public object CreateWriter(Stream stream, WriteSettings settings)
        {
            throw new NotSupportedException();
        }

        public object CreateCustomWriter(object customObject)
        {
            return SerializerUtil.CheckCustomObject<DataSet>(customObject);
        }

        public bool ReadToRoot(object reader, QName root)
        {
            return true;
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void ReadTagElement(TagElementAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
        }

        public void ReadObjectElement(ObjectElementAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            DataTable table = serializerData.Convert<DataTable>();
            if (table.Rows.Count == 0)
                return;

            Type objectType = info.ObjectType;
            IObjectSerializer rowSerializer = ObjectExtension.CreateSerializer("DataRow");

            if (attribute.IsMultiple)
            {
                IList list = attribute.GetList(receiver, info);
                foreach (DataRow row in table.Rows)
                {
                    object subObject = ReadObject(attribute.UseConstructor, settings, row, objectType,
                        receiver, rowSerializer, info.ModelName);
                    list.Add(subObject);
                }
            }
            else
            {
                object subObject = ReadObject(attribute.UseConstructor, settings, table.Rows[0],
                    objectType, receiver, rowSerializer, info.ModelName);
                info.SetValue(receiver, subObject);
            }
        }

        public void ReadComplexElement(SimpleComplexElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
        }

        public void ReadTextContent(TextContentAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void ReadDictionary(DictionaryAttribute attribute, object reader, object receiver,
            ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            DataTable table = serializerData.Convert<DataTable>();
            if (table.Rows.Count != 1)
                return;
            DataRow row = table.Rows[0];
            IDictionary dict = attribute.GetDictionary(receiver, info);
            foreach (DataColumn column in table.Columns)
                dict[column.ColumnName] = row[column];
        }

        public void ReadObjectDictionary(ObjectDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            DataTable table = serializerData.Convert<DataTable>();
            if (table.Rows.Count != 1)
                return;
            DataRow row = table.Rows[0];
            IDictionary dict = attribute.GetDictionary(receiver, info);
            foreach (DataColumn column in table.Columns)
            {
                //dict[column.ColumnName] = row[column];
                object rowValue = row[column];
                string xml = rowValue == DBNull.Value ? null : rowValue.ToString();
                if (string.IsNullOrEmpty(xml))
                    dict[column.ColumnName] = null;
                else
                {
                    Type objectType = info.ObjectType;
                    object subObject = attribute.UseConstructor ?
                        ObjectUtil.CreateObjectWithCtor(objectType) : ObjectUtil.CreateObject(objectType);
                    subObject.ReadXml(info.ModelName, xml, settings, QName.ToolkitNoNS);
                    dict[column.ColumnName] = subObject;
                }
            }
        }

        public void ReadDynamicDictionary(DynamicDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
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

        public void WriteAttribute(SimpleAttributeAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotImplementedException();
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            string tableName = info.LocalName;
            DataSet ds = writer.Convert<DataSet>();
            if (ds.Tables.Contains(tableName))
                return;

            IEnumerable list;
            if (attribute.IsMultiple)
                list = value as IList;
            else
                list = EnumUtil.Convert(value);

            DataTable table = list.CreateTable(tableName, info.ModelName, settings);
            if (table != null)
                ds.Tables.Add(table);
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void WriteComplexContent(ComplexContentAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            //throw new NotSupportedException();
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer, object value,
            WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            string tableName = info.LocalName;
            DataSet ds = writer.Convert<DataSet>();
            if (ds.Tables.Contains(tableName))
                return;

            IDictionary dictionary = value as IDictionary;
            if (dictionary != null)
            {
                DataTable table = new DataTable(tableName) { Locale = ObjectUtil.SysCulture };
                var cols = table.Columns;
                foreach (string key in dictionary.Keys)
                    if (!string.IsNullOrEmpty(key))
                        cols.Add(key);

                DataRow row = table.NewRow();
                row.BeginEdit();
                try
                {
                    foreach (DataColumn column in table.Columns)
                        row[column] = dictionary[column.ColumnName];
                }
                finally
                {
                    row.EndEdit();
                }
                table.Rows.Add(row);

                ds.Tables.Add(table);
            }
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            string tableName = info.LocalName;
            DataSet ds = writer.Convert<DataSet>();
            if (ds.Tables.Contains(tableName))
                return;

            IDictionary dictionary = value as IDictionary;
            if (dictionary != null)
            {
                DataTable table = new DataTable(tableName) { Locale = ObjectUtil.SysCulture };
                var cols = table.Columns;
                foreach (string key in dictionary.Keys)
                    if (!string.IsNullOrEmpty(key))
                        cols.Add(key);

                DataRow row = table.NewRow();
                row.BeginEdit();
                try
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        string xml = dictionary[column.ColumnName].WriteXml(info.ModelName,
                            settings, QName.ToolkitNoNS);
                        if (xml == null)
                            row[column] = DBNull.Value;
                        else
                            row[column] = xml;
                    }
                }
                finally
                {
                    row.EndEdit();
                }
                table.Rows.Add(row);

                ds.Tables.Add(table);
            }
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
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
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            var elements = info.Elements.CreateOrderPropertyInfoList();
            var writeElements = from item in elements
                                where item.IsSingle && IsSupport(item.Content.Attribute)
                                select item;
            foreach (var item in writeElements)
            {
                ObjectPropertyInfo property = item.Content;
                object value = property.GetValue(receiver);
                if (value == null)
                    continue;

                item.Content.Attribute.WriteObject(this, writer, value, settings,
                    item.Content, serializerData);
            }

            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
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

        private static bool IsSupport(BaseObjectAttribute attribute)
        {
            return attribute is ObjectElementAttribute || attribute is DictionaryAttribute;
        }

        private static object ReadObject(bool usector, ReadSettings settings, DataRow row,
            Type objectType, object receiver, IObjectSerializer datarowSerializer, string modelName)
        {
            object subObject = usector ? ObjectUtil.CreateObjectWithCtor(objectType)
                : ObjectUtil.CreateObject(objectType);
            object rowReader = datarowSerializer.CreateCustomReader(row);
            datarowSerializer.ReadObject(rowReader, subObject, modelName, settings, QName.Toolkit, null);
            SerializerUtil.SetParent(receiver, subObject);

            return subObject;
        }

        public override string ToString()
        {
            return "DataSet格式对象转换器";
        }
    }
}