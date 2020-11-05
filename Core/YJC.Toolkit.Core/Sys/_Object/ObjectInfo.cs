using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Collections;

namespace YJC.Toolkit.Sys
{
    public sealed class ObjectInfo : ICacheDependencyCreator
    {
        internal const BindingFlags BIND_ATTRIBUTE =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public const string CONTENT_NAME = "Content";

        private readonly DictionaryList<ObjectPropertyInfo> fAttributes;
        private readonly ObjectPropertyInfo fContent;
        private readonly ObjectElementInfo fElements;

        internal ObjectInfo()
        {
            fAttributes = new DictionaryList<ObjectPropertyInfo>();
            fElements = new ObjectElementInfo();
        }

        internal ObjectInfo(object receiver, Type type, string modelName)
            : this()
        {
            IsObjectContext = Attribute.GetCustomAttribute(type,
                typeof(ObjectContextAttribute), false) != null;

            // 扫描对象的每个属性，根据属性所标注的特性，配置Attribute，Content和Element子项
            PropertyInfo[] properties = type.GetProperties(BIND_ATTRIBUTE);

            foreach (var property in properties)
            {
                object[] propertyAttributes = property.GetCustomAttributes(false);
                if (propertyAttributes.Length == 0)
                    continue;

                var attribute = (from attr in propertyAttributes
                                 where attr is SimpleAttributeAttribute
                                 select attr).FirstOrDefault();
                if (attribute != null)
                {
                    SimpleAttributeAttribute simpleAttr = attribute as SimpleAttributeAttribute;
                    simpleAttr.SetNameMode(modelName, property);
                    ObjectPropertyInfo attrInfo = new ReflectorObjectPropertyInfo(
                        property, simpleAttr, modelName);
                    fAttributes.Add(attrInfo.QName.LocalName, attrInfo);
                    continue;
                }

                attribute = (from attr in propertyAttributes
                             where (attr is TextContentAttribute || attr is ComplexContentAttribute)
                             select attr).FirstOrDefault();
                if (attribute != null)
                {
                    AssertContent(receiver, type, fContent);
                    fContent = new ReflectorObjectPropertyInfo(property,
                        attribute as BaseObjectAttribute, modelName);
                    continue;
                }

                attribute = (from attr in propertyAttributes
                             where attr is TagElementAttribute
                             select attr).FirstOrDefault();
                if (attribute != null)
                {
                    TagElementAttribute tagAttr = attribute.Convert<TagElementAttribute>();
                    tagAttr.SetNameMode(modelName, property);
                    fElements.Add(type, property, tagAttr, modelName);
                    tagAttr.Read(type, property, modelName, propertyAttributes);
                    continue;
                }

                fElements.ReadElementAttribute(type, property, modelName, propertyAttributes);
            }
        }

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            if (Elements.XmlConfigFactories == null)
                return AlwaysDependency.Dependency;
            else
            {
                if (Elements.XmlConfigFactories.Count == 1)
                    return new XmlConfigFactoryDependency(Elements.XmlConfigFactories.First().Value);
                else
                    return new XmlConfigFactoriesDependency(Elements.XmlConfigFactories.Values);
            }
        }

        #endregion ICacheDependencyCreator 成员

        public IEnumerable<ObjectPropertyInfo> Attributes
        {
            get
            {
                return fAttributes;
            }
        }

        public ObjectPropertyInfo Content
        {
            get
            {
                return fContent;
            }
        }

        public ObjectElementInfo Elements
        {
            get
            {
                return fElements;
            }
        }

        public bool IsObjectContext { get; private set; }

        public ObjectPropertyInfo GetAttribute(string localName)
        {
            if (fAttributes.Contains(localName))
                return fAttributes[localName];
            return null;
        }

        public string GetPropertyName(string localName)
        {
            ObjectPropertyInfo prop = null;
            if (fAttributes.Contains(localName))
                prop = fAttributes[localName];
            else if (fContent != null)
            {
                if (localName == CONTENT_NAME)
                    prop = fContent;
            }
            else
                prop = fElements[localName];

            if (prop != null)
                return prop.PropertyName;

            return null;
        }

        private static void AssertContent(object obj, Type configType, ObjectPropertyInfo content)
        {
            TkDebug.Assert(content == null, string.Format(ObjectUtil.SysCulture,
                "类型{0}只能配置一个Content特性，现在配置了不止一个",
                configType), obj);
        }

        private static string GetKeyName(Type type, string modeName)
        {
            if (string.IsNullOrEmpty(modeName))
                return type.FullName;
            else
                return string.Format(ObjectUtil.SysCulture, "{0}`{1}", type.FullName, modeName);
        }

        public static ObjectInfo Create(object data, string modelName)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            string key = GetKeyName(data.GetType(), modelName);
            return CacheManager.GetItem("ObjectInfo", key, data, modelName).Convert<ObjectInfo>();
        }

        public static ObjectInfo Create(Type type, string modelName)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            string key = GetKeyName(type, modelName);
            return CacheManager.GetItem("ObjectInfo", key, type, modelName).Convert<ObjectInfo>();
        }
    }
}