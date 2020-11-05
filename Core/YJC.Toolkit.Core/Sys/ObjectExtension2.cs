using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

//using Microsoft.CSharp.RuntimeBinder;

namespace YJC.Toolkit.Sys
{
    public static partial class ObjectExtension
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            TkDebug.AssertArgumentNull(collection, "collection", null);

            var list = from item in collection.Cast<string>()
                       where !string.IsNullOrEmpty(item)
                       select new { Key = item, Value = collection[item] };
            return list.ToDictionary(p => p.Key, p => p.Value);
        }

        public static void ReadFromFile(this object receiver, string method, string modelName,
            string fileName, ReadSettings settings, QName root)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            fileName = Path.GetFullPath(fileName);
            FileStream stream = new FileStream(fileName, FileMode.Open,
                FileAccess.Read, FileShare.Read);
            receiver.ReadFromStream(method, modelName, stream, settings, root);
            IDataFile file = receiver as IDataFile;
            if (file != null)
                file.FullPath = fileName;
        }

        public static void WriteToFile(this object receiver, string method, string modelName,
            string fileName, WriteSettings settings, QName root)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            FileStream stream = new FileStream(fileName, FileMode.Create,
                FileAccess.Write, FileShare.Write);
            receiver.WriteToStream(method, modelName, stream, settings, root);
        }

        public static void ReadXmlFromFile(this object receiver, string fileName,
            ReadSettings settings, QName root)
        {
            ReadXmlFromFile(receiver, null, fileName, settings, root);
        }

        public static void ReadXmlFromFile(this object receiver, string modelName, string fileName,
            ReadSettings settings, QName root)
        {
            TkDebug.AssertArgumentNull(root, "root", null);

            ReadFromFile(receiver, "Xml", modelName, fileName, settings, root);
        }

        public static void ReadXmlFromFile(this object receiver, string fileName)
        {
            ReadXmlFromFile(receiver, null, fileName);
        }

        public static void ReadXmlFromFile(this object receiver, string modelName, string fileName)
        {
            ReadFromFile(receiver, "Xml", modelName, fileName, ObjectUtil.ReadSettings, QName.Toolkit);
        }

        public static XDocument CreateXDocument(this object receiver, string modelName,
            WriteSettings settings, QName root)
        {
            return receiver.CreateObject("XElement", modelName, settings, root).Convert<XElementData>().Root;
        }

        public static XDocument CreateXDocument(this object receiver)
        {
            return CreateXDocument(receiver, null, ObjectUtil.WriteSettings, QName.Toolkit);
        }

        private static void CreateTableStructure(ObjectInfo info, DataTable table)
        {
            DataColumnCollection columns = table.Columns;
            foreach (var item in info.Attributes)
                columns.Add(item.LocalName);

            if (info.Content != null)
                columns.Add(ObjectInfo.CONTENT_NAME);
            else
            {
                var elements = info.Elements.CreateOrderPropertyInfoList();
                foreach (var item in elements)
                {
                    if (item.IsSingle)
                    {
                        string localName = item.Content.LocalName;
                        columns.Add(localName);
                    }
                }
            }
        }

        public static List<T> CreateListFromTable<T>(this DataTable table) where T : new()
        {
            return CreateListFromTable<T>(table, null, ObjectUtil.ReadSettings);
        }

        public static List<T> CreateListFromTable<T>(this DataTable table,
            ReadSettings settings) where T : new()
        {
            return CreateListFromTable<T>(table, null, settings);
        }

        public static List<T> CreateListFromTable<T>(this DataTable table, Func<T> createFunc)
        {
            return CreateListFromTable(table, null, ObjectUtil.ReadSettings, createFunc);
        }

        public static List<T> CreateListFromTable<T>(this DataTable table, ReadSettings settings,
            Func<T> createFunc)
        {
            return CreateListFromTable(table, null, settings, createFunc);
        }

        public static List<T> CreateListFromTable<T>(this DataTable table, string modelName,
            ReadSettings settings, Func<T> createFunc)
        {
            if (table == null)
                return null;
            TkDebug.AssertArgumentNull(settings, "settings", null);
            TkDebug.AssertArgumentNull(createFunc, "createFunc", null);

            List<T> result = new List<T>(table.Rows.Count);

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("DataRow");

            foreach (DataRow row in table.Rows)
            {
                T obj = createFunc();
                object reader = serializer.CreateCustomReader(row);
                serializer.ReadObject(reader, obj, modelName, settings, QName.Toolkit, null);
                result.Add(obj);
            }
            return result;
        }

        public static List<T> CreateListFromTable<T>(this DataTable table, string modelName,
            ReadSettings settings) where T : new()
        {
            return CreateListFromTable(table, modelName, settings, () => new T());
        }

        public static DataTable CreateTable(this IEnumerable list, string tableName)
        {
            return CreateTable(list, tableName, ObjectUtil.WriteSettings);
        }

        public static DataTable CreateTable(this IEnumerable list, string tableName,
            WriteSettings settings)
        {
            return CreateTable(list, tableName, null, settings);
        }

        public static DataTable CreateTable(this IEnumerable list, string tableName,
            string modelName, WriteSettings settings)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            if (list == null)
                return null;

            object value = list.Cast<object>().FirstOrDefault();
            if (value == null)
                return null;

            ObjectInfo info = ObjectInfo.Create(value, modelName);
            DataTable table = new DataTable(tableName) { Locale = ObjectUtil.SysCulture };

            CreateTableStructure(info, table);
            IObjectSerializer serializer = ObjectExtension.CreateSerializer("DataRow");
            foreach (var item in list)
            {
                DataRow row = table.NewRow();
                object writer = serializer.CreateCustomWriter(row);
                serializer.WriteObject(writer, item, null, settings, QName.Toolkit, null);
                table.Rows.Add(row);
            }
            return table;
        }

        public static void AddToDataRow(this object receiver, DataRow row)
        {
            AddToDataRow(receiver, row, null, ObjectUtil.WriteSettings);
        }

        public static void AddToDataRow(this object receiver, DataRow row, string modelName)
        {
            AddToDataRow(receiver, row, modelName, ObjectUtil.WriteSettings);
        }

        public static void AddToDataRow(this object receiver, DataRow row, WriteSettings settings)
        {
            AddToDataRow(receiver, row, null, settings);
        }

        public static void AddToDataRow(this object receiver, DataRow row,
            string modelName, WriteSettings settings)
        {
            TkDebug.AssertArgumentNull(receiver, "receiver", null);
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("DataRow");
            object writer = serializer.CreateCustomWriter(row);
            serializer.WriteObject(writer, receiver, modelName, settings, QName.Toolkit, null);
        }

        public static T ReadFromDataRow<T>(this DataRow row) where T : new()
        {
            T result = new T();
            ReadFromDataRow(result, row);
            return result;
        }

        public static void ReadFromDataRow(this object receiver, DataRow row)
        {
            ReadFromDataRow(receiver, row, null, ObjectUtil.ReadSettings);
        }

        public static void ReadFromDataRow(this object receiver, DataRow row, string modelName)
        {
            ReadFromDataRow(receiver, row, modelName, ObjectUtil.ReadSettings);
        }

        public static void ReadFromDataRow(this object receiver, DataRow row, ReadSettings settings)
        {
            ReadFromDataRow(receiver, row, null, settings);
        }

        public static void ReadFromDataRow(this object receiver, DataRow row, string modelName,
            ReadSettings settings)
        {
            TkDebug.AssertArgumentNull(receiver, "receiver", null);
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("DataRow");
            object reader = serializer.CreateCustomReader(row);
            serializer.ReadObject(reader, receiver, modelName, settings, QName.Toolkit, null);
            SerializerUtil.ReadObjectCallBack(receiver);
        }

        public static void ReadDictionaryFromDataRow(this IDictionary dictionary,
            DataRow row, ReadSettings settings)
        {
            TkDebug.AssertArgumentNull(dictionary, "dictionary", null);
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("DataRow");
            object reader = serializer.CreateCustomReader(row);
            DictionaryAttribute attr = new DictionaryAttribute { AutoTrim = true };
            serializer.ReadDictionary(reader, dictionary, attr, null, settings, null, null);
        }

        public static void AddToDataSet(this object receiver, DataSet dataSet)
        {
            AddToDataSet(receiver, dataSet, null, ObjectUtil.WriteSettings);
        }

        public static void AddToDataSet(this object receiver, DataSet dataSet, WriteSettings settings)
        {
            AddToDataSet(receiver, dataSet, null, settings);
        }

        public static void AddToDataSet(this object receiver, DataSet dataSet,
            string modelName, WriteSettings settings)
        {
            if (receiver == null)
                return;

            TkDebug.AssertArgumentNull(dataSet, "dataSet", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("DataSet");
            object writer = serializer.CreateCustomWriter(dataSet);
            serializer.WriteObject(writer, receiver, modelName, settings, QName.Toolkit, null);
        }

        public static DataTable CreateTable(this Dictionary<string, string> dictionary, string tableName)
        {
            return CreateTable(dictionary, tableName, null, ObjectUtil.WriteSettings);
        }

        public static DataTable CreateTable(this Dictionary<string, string> dictionary, string tableName,
            string modelName, WriteSettings settings)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            if (dictionary == null)
                return null;

            using (DataSet ds = new DataSet() { Locale = ObjectUtil.SysCulture })
            {
                TempDictionaryObject temp = new TempDictionaryObject { Temp = dictionary };
                AddToDataSet(temp, ds, modelName, settings);
                DataTable result = ds.Tables["Temp"].Copy();
                result.TableName = tableName;
                return result;
            }
        }

        public static object MemberValue(this object receiver, string propertyName)
        {
            if (receiver == null)
                return null;
            if (string.IsNullOrEmpty(propertyName))
                return null;

            var func = EvaluatorExtension.Extension.BuildDynamicGetter(receiver.GetType(), propertyName);
            return func(receiver);
        }
    }
}