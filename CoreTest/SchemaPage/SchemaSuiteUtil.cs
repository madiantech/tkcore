using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Toolkit.SchemaSuite.Schema;

namespace Toolkit.SchemaSuite
{
    public static class SchemaSuiteUtil
    {
        private static readonly HashSet<XmlTypeCode> NUMERICE_TYPE = new HashSet<XmlTypeCode>
        {
            XmlTypeCode.Boolean,
            XmlTypeCode.Byte,
            XmlTypeCode.Double,
            XmlTypeCode.Float,
            XmlTypeCode.Int,
            XmlTypeCode.Integer,
            XmlTypeCode.Long,
            XmlTypeCode.NegativeInteger,
            XmlTypeCode.NonNegativeInteger,
            XmlTypeCode.NonPositiveInteger,
            XmlTypeCode.PositiveInteger,
            XmlTypeCode.Short,
            XmlTypeCode.UnsignedByte,
            XmlTypeCode.UnsignedInt,
            XmlTypeCode.UnsignedLong,
            XmlTypeCode.UnsignedShort
        };

        private static readonly string[] TYPE =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, "string", "bool", "decimal",
            "float", "double", string.Empty, "DateTime", "DateTime",
            "DateTime", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "byte[]", "byte[]", "string", "string",
            "string", "string","string", "string","string",
            "string", "string","string", "string","string",
            "int", "int", "int", "long", "int",
            "short", "byte", "int", "long", "int",
            "short", "byte", "int", string.Empty, string.Empty
        };

        private static readonly string[] JAVA_TYPE =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, "String", "boolean", "java.math.BigDecimal",
            "float", "double", string.Empty, "java.util.Date", "java.util.Date",
            "java.util.Date", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "byte[]", "byte[]", "String", "String",
            "String", "String","String", "String","String",
            "String", "String","String", "String","String",
            "int", "int", "int", "long", "int",
            "short", "byte", "int", "long", "int",
            "short", "byte", "int", string.Empty, string.Empty
        };

        internal static void GetName(XmlQualifiedName name, out string realName,
            out string pascalName, out string camelName)
        {
            realName = name.Name;
            char firstChar = realName[0];
            if (char.IsUpper(firstChar))
            {
                pascalName = realName;
                camelName = char.ToLower(firstChar) + realName.Substring(1);
            }
            else
            {
                camelName = realName;
                pascalName = char.ToUpper(firstChar) + realName.Substring(1);
            }
        }

        public static string ConvertTypeCode(XmlTypeCode code)
        {
            return TYPE[(int)code];
        }

        //public static string GetJavaType(AttributeNode node, Dictionary<string, RegElementConfigItem> dictionary)
        //{
        //    return GetType(node, JAVA_TYPE, dictionary);
        //}

        //public static string GetJavaType(ElementNode node, Dictionary<string, RegElementConfigItem> dictionary)
        //{
        //    return GetType(node, JAVA_TYPE, dictionary);
        //}

        //private static string GetType(AttributeNode node, string[] typeArray,
        //    Dictionary<string, RegElementConfigItem> dictionary)
        //{
        //    string typeName = node.TypeName;
        //    if (string.IsNullOrEmpty(typeName))
        //        return typeArray[(int)node.TypeCode];
        //    else if (dictionary.ContainsKey(typeName))
        //        return dictionary[typeName].ObjectType;
        //    return typeName;
        //}

        //public static string GetType(AttributeNode node, Dictionary<string, RegElementConfigItem> dictionary)
        //{
        //    return GetType(node, TYPE, dictionary);
        //}

        //private static string GetType(ElementNode node, string[] typeArray,
        //    Dictionary<string, RegElementConfigItem> dictionary)
        //{
        //    string typeName = node.TypeName;
        //    if (node.SimpleType)
        //    {
        //        if (string.IsNullOrEmpty(typeName))
        //            return typeArray[(int)node.TypeCode];
        //        return typeName;
        //    }
        //    if (dictionary.ContainsKey(typeName))
        //        return dictionary[typeName].ObjectType;
        //    return typeName;
        //}

        //public static string GetType(ElementNode node, Dictionary<string, RegElementConfigItem> dictionary)
        //{
        //    return GetType(node, TYPE, dictionary);
        //}

        public static bool IsNumericType(XmlTypeCode code)
        {
            return NUMERICE_TYPE.Contains(code);
        }

        public static string GetType(bool isEnumType, EnumNode enumNode, XmlTypeCode typeCode)
        {
            if (isEnumType)
            {
                return "枚举 " + string.Join('|', enumNode.Items);
            }
            else
                return ConvertTypeCode(typeCode);
        }

        public static void AppendLineFrontMatter(StringBuilder sb)
        {
            sb.AppendLine("---");
            //sb.AppendLine("sidebarDepth: 2");
            sb.AppendLine("sidebar: auto");
            sb.AppendLine("author: schema2md");
            sb.AppendLine("---");
            sb.AppendLine("");
        }

        public static int GetLevel(int level)
        {
            if (level >= 6)
                return 6;
            return level;
        }
    }
}