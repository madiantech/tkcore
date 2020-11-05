using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public static class MetaDataUtil
    {
        public static bool CanUseMetaData(object data, IPageStyle style)
        {
            if (data == null)
                return false;
            TkDebug.AssertArgumentNull(style, "style", null);

            ISupportMetaData supportMeta = data as ISupportMetaData;
            if (supportMeta != null)
                return supportMeta.CanUseMetaData(style);

            return false;
        }

        public static void SetMetaData(object data, IPageStyle style, IMetaData metaData)
        {
            if (metaData == null || data == null)
                return;
            TkDebug.AssertArgumentNull(style, "style", null);

            ISupportMetaData supportMeta = data as ISupportMetaData;
            if (supportMeta != null)
            {
                bool useIt = supportMeta.CanUseMetaData(style);
                if (useIt)
                    supportMeta.SetMetaData(style, metaData);
            }
        }

        public static ITableScheme ConvertToTableScheme(ITableSchemeEx scheme)
        {
            if (scheme == null)
                return null;
            ITableScheme result = scheme as ITableScheme;
            return result ?? new InternalTableScheme(scheme);
        }

        public static ITableSchemeEx ConvertToTableSchemeEx(ITableScheme scheme)
        {
            return ConvertToTableSchemeEx(scheme, null);
        }

        public static ITableSchemeEx ConvertToTableSchemeEx(ITableScheme scheme,
            Func<IFieldInfo, IFieldInfoEx> converter)
        {
            if (scheme == null)
                return null;
            ITableSchemeEx result = scheme as ITableSchemeEx;
            return result ?? new InternalTableSchemeEx(scheme, converter);
        }

        public static bool Equals(IPageStyle style1, IPageStyle style2)
        {
            if (style1 == null && style2 == null)
                return true;
            if (style1 == null || style2 == null)
                return false;

            switch (style1.Style)
            {
                case PageStyle.Custom:
                    return style1.Operation == style2.Operation;

                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Delete:
                case PageStyle.Detail:
                case PageStyle.List:
                    return style1.Style == style2.Style;

                default:
                    return false;
            }
        }

        public static bool StartsWith(IPageStyle style, string value)
        {
            if (style == null || string.IsNullOrEmpty(value))
                return false;

            if (style.Style == PageStyle.Custom)
            {
                if (!string.IsNullOrEmpty(style.Operation))
                    return style.Operation.StartsWith(value);
            }
            return false;
        }

        public static Type ConvertDataTypeToType(TkDataType type)
        {
            switch (type)
            {
                case TkDataType.Binary:
                case TkDataType.Blob:
                    return typeof(byte[]);

                case TkDataType.Bit:
                    return typeof(bool);

                case TkDataType.Byte:
                    return typeof(byte);

                case TkDataType.Date:
                case TkDataType.DateTime:
                    return typeof(DateTime);

                case TkDataType.Decimal:
                case TkDataType.Money:
                    return typeof(decimal);

                case TkDataType.Double:
                    return typeof(double);

                case TkDataType.Guid:
                    return typeof(Guid);

                case TkDataType.Int:
                    return typeof(int);

                case TkDataType.Long:
                    return typeof(long);

                case TkDataType.Short:
                    return typeof(short);

                case TkDataType.String:
                case TkDataType.Text:
                case TkDataType.Xml:
                    return typeof(string);

                default:
                    return typeof(string);
            }
        }

        public static TkDataType ConvertTypeToDataType(Type type)
        {
            TypeCode code = Type.GetTypeCode(type);
            switch (code)
            {
                case TypeCode.Boolean:
                    return TkDataType.Bit;

                case TypeCode.Char:
                    return TkDataType.String;

                case TypeCode.SByte:
                case TypeCode.Byte:
                    return TkDataType.Byte;

                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return TkDataType.Short;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return TkDataType.Int;

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return TkDataType.Long;

                case TypeCode.Single:
                case TypeCode.Double:
                    return TkDataType.Double;

                case TypeCode.Decimal:
                    return TkDataType.Decimal;

                case TypeCode.DateTime:
                    return TkDataType.DateTime;

                case TypeCode.String:
                    return TkDataType.String;

                case TypeCode.Object:
                    if (type == typeof(byte[]))
                        return TkDataType.Blob;
                    else
                        return TkDataType.String;

                default:
                    return TkDataType.String;
            }
        }

        private static ResourceTableScheme GetInternalDataXml(Stream xmlFile)
        {
            ResourceTableScheme xml = new ResourceTableScheme();
            xml.ReadFromStream("Xml", null, xmlFile, ObjectUtil.ReadSettings, QName.Toolkit);
            //xml.LoadFromReader(XmlUtil.GetXmlReader(xmlFile));
            return xml;
        }

        public static ITableScheme CreateTableScheme(Stream xmlFile)
        {
            TkDebug.AssertArgumentNull(xmlFile, "xmlFile", null);

            ResourceTableScheme xml = GetInternalDataXml(xmlFile);
            return xml;
        }

        public static ITableScheme CreateTableScheme(Stream xmlFile, string tableName)
        {
            TkDebug.AssertArgumentNull(xmlFile, "xmlFile", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            ResourceTableScheme xml = GetInternalDataXml(xmlFile);
            xml.Table.TableName = tableName;
            return xml;
        }

        private static Stream GetResourceStream(Assembly assembly, string partName)
        {
            Stream stream = ResourceUtil.GetEmbeddedResource(assembly, partName);
            TkDebug.AssertNotNull(stream, string.Format(ObjectUtil.SysCulture,
                "程序集{0}中没有名称包含{1}的资源", assembly, partName), null);
            return stream;
        }

        public static ITableScheme CreateTableScheme(Assembly assembly, string partName)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", null);
            TkDebug.AssertArgumentNullOrEmpty(partName, "partName", null);

            Stream stream = GetResourceStream(assembly, partName);
            return CreateTableScheme(stream);
        }

        public static ITableScheme CreateTableScheme(Assembly assembly, string partName, string tableName)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", null);
            TkDebug.AssertArgumentNullOrEmpty(partName, "partName", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            Stream stream = GetResourceStream(assembly, partName);
            return CreateTableScheme(stream, tableName);
        }

        public static ITableScheme CreateTableScheme(string partName)
        {
            TkDebug.AssertArgumentNullOrEmpty(partName, "partName", null);

            Assembly assembly = Assembly.GetCallingAssembly();
            return CreateTableScheme(assembly, partName);
        }

        public static ITableScheme CreateTableScheme(string partName, string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(partName, "partName", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            Assembly assembly = Assembly.GetCallingAssembly();
            return CreateTableScheme(assembly, partName, tableName);
        }

        public static TypeTableScheme CreateTypeTableScheme(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            TkDebug.ThrowIfNoGlobalVariable();
            TypeSchemeTypeFactory factory = BaseGlobalVariable.Current.FactoryManager.
                GetCodeFactory(TypeSchemeTypeFactory.REG_NAME).Convert<TypeSchemeTypeFactory>();
            Type type = factory.GetType(regName);
            if (type == null)
                return null;

            return TypeTableScheme.Create(type);
        }

        internal static IFieldInfoEx GetNameField(IEnumerable<IFieldInfoEx> fieldList)
        {
            var name = (from item in fieldList
                        where item.NickName.EndsWith("Name", StringComparison.Ordinal)
                        select item).FirstOrDefault();
            return name;
        }
    }
}