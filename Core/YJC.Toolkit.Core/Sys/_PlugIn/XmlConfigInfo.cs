using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal sealed class XmlConfigInfo
    {
        private readonly static Type PlugInType = typeof(IXmlPlugInItem);
        private readonly static string PlugInTypeName = PlugInType.FullName;

        /// <summary>
        /// Initializes a new instance of the XmlConfigInfo class.
        /// </summary>
        public XmlConfigInfo(Type xmlConfigType, bool searchConfig)
        {
            XmlConfigType = xmlConfigType;
            if (searchConfig)
                ConfigItems = SearchConfigItem(xmlConfigType);
        }

        public Type XmlConfigType { get; private set; }

        public List<XmlPlugInItemInfo> ConfigItems { get; private set; }

        [System.Diagnostics.Conditional(ToolkitConst.DEBUG)]
        private static void AssertPlugInItemType(PropertyInfo property, Type xmlConfigType)
        {
            Type type = property.PropertyType;
            TkDebug.Assert(type.IsGenericType,
                string.Format(ObjectUtil.SysCulture,
                "类型{1}的属性{0}必须是泛型类型", property.Name, xmlConfigType), null);
            Type[] gernericTypes = type.GetGenericArguments();
            TkDebug.Assert(gernericTypes != null && gernericTypes.Length == 1,
                string.Format(ObjectUtil.SysCulture,
                "类型{1}的属性{0}的泛型参数只能是一个，现在可能没有或有多个",
                property.Name, xmlConfigType), null);
            Type assertType = gernericTypes[0];
            if (assertType != typeof(IXmlPlugInItem))
            {
                Type interfaceType = assertType.GetInterface(PlugInTypeName);
                TkDebug.AssertNotNull(interfaceType,
                    string.Format(ObjectUtil.SysCulture,
                    "类型{1}的属性{0}的泛型参数必须实现IPlugInXmlItem接口，现在没有实现",
                    property.Name, xmlConfigType), null);
            }
        }

        internal static List<XmlPlugInItemInfo> SearchConfigItem(Type xmlConfigType)
        {
            PropertyInfo[] properties = xmlConfigType.GetProperties(BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance);

            var configProperties = from memberInfo in properties
                                   from attribute in memberInfo.GetCustomAttributes(false)
                                   where attribute is XmlPlugInItemAttribute
                                   select memberInfo;
            List<XmlPlugInItemInfo> fConfigItems = new List<XmlPlugInItemInfo>();
            foreach (PropertyInfo propertyInfo in configProperties)
            {
                XmlPlugInItemType type = XmlPlugInItemType.None;
                Type[] interfaceTypes = propertyInfo.PropertyType.GetInterfaces();
                foreach (Type item in interfaceTypes)
                {
                    if (item == typeof(IEnumerable))
                    {
                        AssertPlugInItemType(propertyInfo, xmlConfigType);
                        type = XmlPlugInItemType.Enumerable;
                        break;
                    }
                    if (item == typeof(IEnumerator))
                    {
                        AssertPlugInItemType(propertyInfo, xmlConfigType);
                        type = XmlPlugInItemType.Enumerator;
                        break;
                    }
                    if (item == PlugInType)
                    {
                        type = XmlPlugInItemType.Single;
                        break;
                    }
                }
                TkDebug.Assert(type != XmlPlugInItemType.None, string.Format(ObjectUtil.SysCulture,
                    "类型{1}的属性{0}虽然加注XmlPlugInItemAttribute的特性，但是它必须是IPlugInXmlItem"
                    + "或者IEnumerable<IPlugInXmlItem>或者IEnumerator<IPlugInXmlItem>类型",
                    propertyInfo.Name, xmlConfigType), null);
                fConfigItems.Add(new XmlPlugInItemInfo { PropertyInfo = propertyInfo, Type = type });
            }
            TkDebug.Assert(fConfigItems.Count > 0, string.Format(ObjectUtil.SysCulture,
                "类型{0}没有一个属性加注XmlPlugInItemAttribute特性，这样无法读取配置", xmlConfigType), null);
            return fConfigItems;
        }
    }
}
