using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace YJC.Toolkit.Sys
{
    public static partial class ObjectExtension
    {
        public static string AppVirutalPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            if (BaseAppSetting.Current == null ||
                string.IsNullOrEmpty(BaseAppSetting.Current.AppVirtualPath))
                return path;
            return BaseAppSetting.Current.AppVirtualPath + path;
        }

        public static string ConvertToString(this object value)
        {
            return value == null ? null : value.ToString();
        }

        public static T Convert<T>(this object objValue) where T : class
        {
            TkDebug.AssertArgumentNull(objValue, "objValue", null);

            T result = objValue as T;
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "将类型{0}转换为类型{1}失败，请确认代码", objValue.GetType(), typeof(T)), objValue);
            return result;
        }

        public static bool IsNullableType(this Type type)
        {
            if (type == null)
                return false;

            return type.IsGenericType && type.FullName.StartsWith(
                "System.Nullable`1", StringComparison.Ordinal);
        }

        public static T Value<T>(this object objValue)
        {
            if (objValue == null || objValue == DBNull.Value)
                return default(T);
            Type destType = typeof(T);
            if (objValue.GetType() == destType)
                return (T)objValue;
            try
            {
                return (T)System.Convert.ChangeType(objValue, destType, ObjectUtil.SysCulture);
            }
            catch
            {
                return ObjectUtil.GetDefaultValue<T>(objValue.ToString());
            }
        }

        public static object Value(this object objValue, Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            if (objValue == null || objValue == DBNull.Value)
                return ObjectUtil.GetTypeDefaultValue(type);
            if (objValue.GetType() == type)
                return objValue;
            try
            {
                return System.Convert.ChangeType(objValue, type, ObjectUtil.SysCulture);
            }
            catch
            {
                return ObjectUtil.GetDefaultValue(objValue.ToString(), type);
            }
        }

        public static T Value<T>(this object objValue, T defaultValue)
        {
            if (objValue == null || objValue == DBNull.Value)
                return defaultValue;
            Type destType = typeof(T);
            if (objValue.GetType() == destType)
                return (T)objValue;
            try
            {
                return (T)System.Convert.ChangeType(objValue, destType, ObjectUtil.SysCulture);
            }
            catch
            {
                return ObjectUtil.GetDefaultValue<T>(objValue.ToString(), defaultValue);
            }
        }

        public static void CopyFromObject(this object dst, object src)
        {
            TkDebug.AssertArgumentNull(dst, nameof(dst), null);
            TkDebug.AssertArgumentNull(src, nameof(src), null);

            string json = src.WriteJson();
            dst.ReadJson(json);
        }

        internal static IObjectSerializer CreateSerializer(string method)
        {
            IObjectSerializer serializer = PlugInFactoryManager.CreateInstance<IObjectSerializer>(
                            SerializerPlugInFactory.REG_NAME, method);
            return serializer;
        }

        //public static void ReadFromStream(this object receiver, string method, Stream data,
        //    ReadSettings settings, QName root)
        //{
        //    ReadFromStream(receiver, method, null, data, settings, root);
        //}

        public static void ReadFromStream(this object receiver, string method, string modelName,
            Stream data, ReadSettings settings, QName root)
        {
            TkDebug.AssertArgumentNull(receiver, "receiver", null);
            TkDebug.AssertArgumentNullOrEmpty(method, "method", null);
            TkDebug.AssertArgumentNull(data, "data", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            IObjectSerializer serializer = CreateSerializer(method);
            object reader = serializer.CreateReader(data, settings);
            ReadFromReader(receiver, settings, root, serializer, reader, modelName);
        }

        private static void ReadFromReader(object receiver, ReadSettings settings, QName root,
            IObjectSerializer serializer, object reader, string modelName)
        {
            using (reader as IDisposable)
            {
                if (serializer.ReadToRoot(reader, root))
                {
                    serializer.Read(reader, receiver, modelName, settings, root, null);
                }
            }
        }

        //public static void ReadFromString(this object receiver, string method, string data,
        //    ReadSettings settings, QName root)
        //{
        //    ReadFromString(receiver, method, null, data, settings, root);
        //}

        public static void ReadFromString(this object receiver, string method, string modelName,
            string data, ReadSettings settings, QName root)
        {
            TkDebug.AssertArgumentNull(receiver, "receiver", null);
            TkDebug.AssertArgumentNullOrEmpty(method, "method", null);
            TkDebug.AssertArgumentNullOrEmpty(data, "data", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            IObjectSerializer serializer = CreateSerializer(method);
            object reader = serializer.CreateReader(data, settings);
            ReadFromReader(receiver, settings, root, serializer, reader, modelName);
        }

        //public static string WriteToString(this object receiver, string method,
        //    WriteSettings settings, QName root)
        //{
        //    return WriteToString(receiver, method, null, settings, root);
        //}

        public static string WriteToString(this object receiver, string method, string modelName,
            WriteSettings settings, QName root)
        {
            TkDebug.AssertArgumentNullOrEmpty(method, "method", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            if (receiver == null)
                return null;

            IObjectSerializer serializer = CreateSerializer(method);
            MemoryStream stream = new MemoryStream();
            object writer = serializer.CreateWriter(stream, settings);
            using (writer as IDisposable)
            {
                SerializerUtil.WriteSerializer(serializer, writer, receiver, modelName, settings, root, null);
                byte[] data = stream.ToArray();
                return settings.Encoding.GetString(data, 0, data.Length);
            }
        }

        public static void WriteToStream(this object receiver, string method, string modelName,
            Stream stream, WriteSettings settings, QName root)
        {
            TkDebug.AssertArgumentNullOrEmpty(method, "method", null);
            TkDebug.AssertArgumentNull(stream, "stream", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            if (receiver == null)
                return;

            IObjectSerializer serializer = CreateSerializer(method);
            object writer = serializer.CreateWriter(stream, settings);
            using (writer as IDisposable)
            {
                SerializerUtil.WriteSerializer(serializer, writer, receiver, modelName, settings, root, null);
            }
        }

        public static object CreateObject(this object receiver, string method, string modelName,
            WriteSettings settings, QName root)
        {
            TkDebug.AssertArgumentNullOrEmpty(method, "method", null);
            TkDebug.AssertArgumentNull(settings, "settings", null);

            if (receiver == null)
                return null;

            IObjectSerializer serializer = CreateSerializer(method);
            object writer = serializer.CreateWriter(null, settings);
            using (writer as IDisposable)
            {
                SerializerUtil.WriteSerializer(serializer, writer, receiver, modelName, settings, root, null);
                return writer;
            }
        }

        public static void ReadXml(this object receiver, string modelName, string xml,
            ReadSettings settings, QName root)
        {
            TkDebug.AssertArgumentNull(root, "root", null);

            ReadFromString(receiver, "Xml", modelName, xml, settings, root);
        }

        public static void ReadXml(this object receiver, string xml, ReadSettings settings, QName root)
        {
            TkDebug.AssertArgumentNull(root, "root", null);

            ReadFromString(receiver, "Xml", null, xml, settings, root);
        }

        public static void ReadXml(this object receiver, string modelName, string xml)
        {
            ReadFromString(receiver, "Xml", modelName, xml, ObjectUtil.ReadSettings, QName.Toolkit);
        }

        public static void ReadXml(this object receiver, string xml)
        {
            ReadFromString(receiver, "Xml", null, xml, ObjectUtil.ReadSettings, QName.Toolkit);
        }

        public static T CreateFromXmlFactory<T>(this string xml, string factoryName, params object[] args)
        {
            IConfigCreator<T> obj = ReadXmlFromFactory<IConfigCreator<T>>(xml, factoryName);
            TkDebug.AssertNotNull(obj, string.Format(ObjectUtil.SysCulture,
                "\"{0}\"在配置工厂{1}中无法读出，请确认相关参数", xml, factoryName), null);
            return obj.CreateObject(args);
        }

        public static object ReadXmlFromFactory(this string xml, string factoryName)
        {
            return ReadXmlFromFactory(xml, factoryName, ObjectUtil.ReadSettings);
        }

        public static object ReadXmlFromFactory(this string xml, string factoryName, ReadSettings settings)
        {
            TkDebug.AssertArgumentNullOrEmpty(xml, "xml", null);
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", null);

            string newXml = string.Format(ObjectUtil.SysCulture, ToolkitConst.TOOLKIT_XML_SKELETON, xml);
            ToolkitDynObject obj = new ToolkitDynObject(factoryName);
            ReadXml(obj, newXml, settings, QName.Toolkit);
            return obj.Data;
        }

        public static T CreateFromXmlFactoryUseJson<T>(this string json, string factoryName, params object[] args)
        {
            IConfigCreator<T> obj = ReadJsonFromFactory<IConfigCreator<T>>(json, factoryName);
            TkDebug.AssertNotNull(obj, string.Format(ObjectUtil.SysCulture,
                "\"{0}\"在配置工厂{1}中无法读出，请确认相关参数", json, factoryName), null);
            return obj.CreateObject(args);
        }

        public static Object ReadJsonFromFactory(this string json, string factoryName)
        {
            return ReadJsonFromFactory(json, factoryName, ObjectUtil.ReadSettings);
        }

        public static Object ReadJsonFromFactory(this string json, string factoryName, ReadSettings settings)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", null);

            ToolkitDynObject obj = new ToolkitDynObject(factoryName);
            ReadJson(obj, json, settings);
            return obj.Data;
        }

        public static T ReadJsonFromFactory<T>(this string json, string factoryName) where T : class
        {
            return ReadJsonFromFactory<T>(json, factoryName, ObjectUtil.ReadSettings);
        }

        public static T ReadJsonFromFactory<T>(this string json, string factoryName,
            ReadSettings settings) where T : class
        {
            object result = ReadJsonFromFactory(json, factoryName, settings);
            return result as T;
        }

        public static T ReadXmlFromFactory<T>(this string xml, string factoryName) where T : class
        {
            return ReadXmlFromFactory<T>(xml, factoryName, ObjectUtil.ReadSettings);
        }

        public static T ReadXmlFromFactory<T>(this string xml, string factoryName,
            ReadSettings settings) where T : class
        {
            object result = ReadXmlFromFactory(xml, factoryName, settings);
            return result as T;
        }

        public static T ReadXml<T>(this string xml, bool useConstructor)
        {
            TkDebug.AssertArgumentNullOrEmpty(xml, "xml", null);

            ToolkitXmlObject<T> obj = new ToolkitXmlObject<T>() { UseConstructor = useConstructor };
            string newXml = string.Format(ObjectUtil.SysCulture,
               "<tk:Toolkit xmlns:tk='http://www.qdocuments.net'>{0}</tk:Toolkit>", xml);
            ReadXml(obj, newXml);
            return obj.Data;
        }

        public static T ReadXml<T>(this string xml) where T : new()
        {
            return ReadXml<T>(xml, false);
        }

        public static void ReadJson(this object receiver, string modelName, string json)
        {
            ReadFromString(receiver, "Json", modelName, json, ObjectUtil.ReadSettings, QName.Toolkit);
        }

        public static void ReadJson(this object receiver, string json)
        {
            ReadFromString(receiver, "Json", null, json, ObjectUtil.ReadSettings, QName.Toolkit);
        }

        public static void ReadJson(this object receiver, string modelName,
            string json, ReadSettings settings)
        {
            ReadFromString(receiver, "Json", modelName, json, settings, QName.Toolkit);
        }

        public static void ReadJson(this object receiver, string json, ReadSettings settings)
        {
            ReadFromString(receiver, "Json", null, json, settings, QName.Toolkit);
        }

        public static T ReadJson<T>(this string json, bool useConstructor)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);

            ToolkitXmlObject<T> obj = new ToolkitXmlObject<T>() { UseConstructor = useConstructor };
            string newJson = string.Format(ObjectUtil.SysCulture, "{{\"Toolkit\":{0}}}", json);
            ReadJson(obj, newJson);
            return obj.Data;
        }

        public static T ReadJson<T>(this string json) where T : new()
        {
            return ReadJson<T>(json, false);
        }

        public static void ReadQueryString(this object receiver, string queryString)
        {
            ReadQueryString(receiver, null, queryString);
        }

        public static void ReadQueryString(this object receiver, string modelName, string queryString)
        {
            ReadFromString(receiver, "QueryString", modelName, queryString,
                ReadSettings.Default, QName.Toolkit);
        }

        public static string WriteXml(this object receiver, string modelName,
            WriteSettings settings, QName root)
        {
            TkDebug.AssertArgumentNull(root, "root", null);

            return WriteToString(receiver, "Xml", modelName, settings, root);
        }

        public static string WriteXml(this object receiver, WriteSettings settings, QName root)
        {
            return WriteXml(receiver, null, settings, root);
        }

        public static string WriteXml(this object receiver, string modelName)
        {
            return WriteToString(receiver, "Xml", modelName, ObjectUtil.WriteSettings, QName.Toolkit);
        }

        public static string WriteXml(this object receiver)
        {
            return WriteXml(receiver, null);
        }

        public static string WriteJson(this object receiver, string modelName, WriteSettings settings)
        {
            return WriteToString(receiver, "Json", modelName, settings, QName.Toolkit);
        }

        public static string WriteJson(this object receiver, WriteSettings settings)
        {
            return WriteToString(receiver, "Json", null, settings, QName.Toolkit);
        }

        public static string WriteJson(this object receiver, string modelName)
        {
            return WriteToString(receiver, "Json", modelName, ObjectUtil.WriteSettings, QName.Toolkit);
        }

        public static string WriteJson(this object receiver)
        {
            return WriteToString(receiver, "Json", null, ObjectUtil.WriteSettings, QName.Toolkit);
        }

        public static string WriteQueryString(this object receiver, string modelName)
        {
            return WriteQueryString(receiver, modelName, QueryStringOutput.Default);
        }

        public static string WriteQueryString(this object receiver)
        {
            return WriteQueryString(receiver, null, QueryStringOutput.Default);
        }

        public static string WriteQueryString(this object receiver, QueryStringOutput output)
        {
            return WriteQueryString(receiver, null, output);
        }

        public static string WriteQueryString(this object receiver,
            string modelName, QueryStringOutput output)
        {
            if (receiver == null)
                return null;

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("QueryString");
            QueryStringBuilder builder = new QueryStringBuilder(output);
            object writer = serializer.CreateCustomWriter(builder);
            SerializerUtil.WriteSerializer(serializer, writer, receiver, modelName, ObjectUtil.WriteSettings, QName.Toolkit, null);

            return builder.ToString();
        }

        public static Dictionary<string, Object> WriteDictionary(this object receiver)
        {
            return WriteDictionary(receiver, null, DictionaryOutput.Default);
        }

        public static Dictionary<string, Object> WriteDictionary(this object receiver,
            DictionaryOutput output)
        {
            return WriteDictionary(receiver, null, output);
        }

        public static Dictionary<string, Object> WriteDictionary(this object receiver,
            string modelName, DictionaryOutput output)
        {
            if (receiver == null)
                return null;

            IObjectSerializer serializer = ObjectExtension.CreateSerializer("Dictionary");
            DictionaryBuilder builder = new DictionaryBuilder(output);
            object writer = serializer.CreateCustomWriter(builder);
            SerializerUtil.WriteSerializer(serializer, writer, receiver, modelName, ObjectUtil.WriteSettings, QName.Toolkit, null);

            return builder.Data;
        }

        public static void DisposeObject(this object obj)
        {
            IDisposable dispose = obj as IDisposable;
            if (dispose != null)
                dispose.Dispose();
        }

        public static bool Config(this ConfigType configType, bool appSetting)
        {
            switch (configType)
            {
                case ConfigType.True:
                    return true;

                case ConfigType.False:
                    return false;

                case ConfigType.SystemConfiged:
                    return appSetting;

                default:
                    return false;
            }
        }
    }
}