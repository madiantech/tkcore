using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public sealed class ObjectElementInfo //: IEnumerable<ObjectPropertyInfo>
    {
        //private readonly Dictionary<QName, ObjectPropertyInfo> fElements;
        //private readonly Dictionary<string, ObjectPropertyInfo> fLocalElements;
        //private readonly Dictionary<PropertyInfo, ObjectPropertyInfoList> fProperties;

        private Dictionary<string, BaseXmlConfigFactory> fXmlConfigFactories;
        private readonly Dictionary<QName, ObjectPropertyInfo> fSingleElements;
        private readonly Dictionary<string, ObjectPropertyInfo> fSingleLocalElements;
        private readonly List<IMultipleElementReader> fMultiElements;
        private readonly List<IElementWriter> fElementWriter;

        public ObjectElementInfo()
        {
            //fElements = new Dictionary<QName, ObjectPropertyInfo>();
            //fLocalElements = new Dictionary<string, ObjectPropertyInfo>();
            //fProperties = new Dictionary<PropertyInfo, ObjectPropertyInfoList>();

            fSingleElements = new Dictionary<QName, ObjectPropertyInfo>();
            fSingleLocalElements = new Dictionary<string, ObjectPropertyInfo>();
            fMultiElements = new List<IMultipleElementReader>();
            fElementWriter = new List<IElementWriter>();
        }

        //#region IEnumerable<ObjectPropertyInfo> 成员

        //public IEnumerator<ObjectPropertyInfo> GetEnumerator()
        //{
        //    return fElements.Values.GetEnumerator();
        //}

        //#endregion IEnumerable<ObjectPropertyInfo> 成员

        //#region IEnumerable 成员

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //#endregion IEnumerable 成员

        //private void InternalAdd(Type type, PropertyInfo property,
        //    NamedAttribute elementAttribute, string modelName)
        //{
        //    elementAttribute.SetNameMode(modelName, property);
        //    ObjectPropertyInfo item = new ReflectorObjectPropertyInfo(property, elementAttribute, modelName);
        //    QName key = elementAttribute.GetQName(item.LocalName);
        //    try
        //    {
        //        fElements.Add(key, item);
        //        fLocalElements.Add(key.LocalName, item);
        //        ObjectPropertyInfoList list;
        //        if (fProperties.ContainsKey(property))
        //            list = fProperties[property];
        //        else
        //        {
        //            list = new ObjectPropertyInfoList(property, elementAttribute.Required);
        //            fProperties.Add(property, list);
        //        }
        //        list.Add(type, item);
        //    }
        //    catch (Exception ex)
        //    {
        //        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
        //            "类型{0}的属性{1}中的XmlElementAttribute或者MultipleFlagAttribute的Name(值为{2})已存在，请检查",
        //            type, property.Name, key), ex, type);
        //    }
        //}

        //private void AddAttribute(SimpleElementAttribute elementAttribute, Type type,
        //    PropertyInfo property, string modelName)
        //{
        //    //ElementAttribute attr = elementAttribute as ElementAttribute;
        //    //bool addDefault = attr.DefaultValue != null && !attr.IsMultiple &&
        //    //    attr.GetType() == typeof(XmlElementAttribute);
        //    InternalAdd(type, property, elementAttribute, modelName);
        //}

        private void AddSingleAttrible(Type type, PropertyInfo property,
            NamedAttribute elementAttribute, string modelName)
        {
            elementAttribute.SetNameMode(modelName, property);
            ObjectPropertyInfo item = new ReflectorObjectPropertyInfo(property, elementAttribute, modelName);
            QName key = elementAttribute.GetQName(item.LocalName);
            try
            {
                fSingleElements.Add(key, item);
                fSingleLocalElements.Add(key.LocalName, item);
                fElementWriter.Add(new SingleElementWriter(item));
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}的属性{1}中的XmlElementAttribute或者MultipleFlagAttribute的Name(值为{2})已存在，请检查",
                    type, property.Name, key), ex, type);
            }
        }

        internal void ReadElementAttribute(Type type, PropertyInfo property, string modelName,
            object[] propertyAttributes)
        {
            object dynamicAttribute = (from attr in propertyAttributes
                                       where attr is DynamicElementAttribute
                                       select attr).FirstOrDefault();
            if (dynamicAttribute != null)
            {
                DynamicElementAttribute attribute = dynamicAttribute as DynamicElementAttribute;
                BaseXmlConfigFactory factory = attribute.PlugInFactory;
                if (fXmlConfigFactories == null)
                    fXmlConfigFactories = new Dictionary<string, BaseXmlConfigFactory>();
                if (!fXmlConfigFactories.ContainsKey(factory.Name))
                    fXmlConfigFactories.Add(factory.Name, factory);
                //factory.AddElements(this, type, property, modelName, attribute);
                ReflectorObjectPropertyInfo objInfo = new ReflectorObjectPropertyInfo(property, attribute, modelName);
                var dyn = new ConfigFactoryElementReader(attribute, objInfo, modelName);
                fMultiElements.Add(dyn);
                fElementWriter.Add(dyn);
            }
            else
            {
                //foreach (var attr in propertyAttributes)
                //{
                //    if (attr is BaseDictionaryAttribute)
                //        InternalAdd(type, property, attr as NamedAttribute, modelName);
                //    else
                //    {
                //        SimpleElementAttribute eleAttr = attr as SimpleElementAttribute;
                //        if (eleAttr != null)
                //            AddAttribute(eleAttr, type, property, modelName);
                //    }
                //}

                var objElemAttrs = (from item in propertyAttributes
                                    where item is ObjectElementAttribute
                                    select (ObjectElementAttribute)item).ToArray();
                if (objElemAttrs.Length == 0)
                {
                    foreach (var attr in propertyAttributes)
                    {
                        if (attr is BaseDictionaryAttribute)
                            AddSingleAttrible(type, property, attr as NamedAttribute, modelName);
                        else
                        {
                            SimpleElementAttribute eleAttr = attr as SimpleElementAttribute;
                            if (eleAttr != null)
                                AddSingleAttrible(type, property, eleAttr, modelName);
                        }
                    }
                }
                else if (objElemAttrs.Length == 1)
                {
                    AddSingleAttrible(type, property, objElemAttrs[0], modelName);
                }
                else
                {
                    var multi = new MultipleElementReader(property, modelName, objElemAttrs);
                    fMultiElements.Add(multi);
                    fElementWriter.Add(multi);
                }
            }
        }

        internal void Add(Type type, PropertyInfo property,
            NamedAttribute elementAttribute, string modelName)
        {
            //InternalAdd(type, property, elementAttribute, modelName);
            AddSingleAttrible(type, property, elementAttribute, modelName);
        }

        public ObjectPropertyInfo this[QName name]
        {
            get
            {
                //ObjectPropertyInfo result;
                //if (fElements.TryGetValue(name, out result))
                //    return result;
                //return null;
                ObjectPropertyInfo result;
                if (fSingleElements.TryGetValue(name, out result))
                    return result;
                foreach (var item in fMultiElements)
                {
                    result = item[name];
                    if (result != null)
                        return result;
                }
                return null;
            }
        }

        public ObjectPropertyInfo GetObjectPerpertyInfo(QName name, Func<string> versionFunc)
        {
            ObjectPropertyInfo result;
            if (fSingleElements.TryGetValue(name, out result))
                return result;
            string version = null;
            bool execVersion = false;
            foreach (var item in fMultiElements)
            {
                if (item.SupportVersion)
                {
                    if (!execVersion)
                    {
                        version = versionFunc();
                        execVersion = true;
                    }
                    result = item[name, version];
                }
                else
                    result = item[name];
                if (result != null)
                    return result;
            }
            return null;
        }

        public ObjectPropertyInfo this[string name]
        {
            get
            {
                //return ObjectUtil.TryGetValue(fLocalElements, name);
                ObjectPropertyInfo result;
                if (fSingleLocalElements.TryGetValue(name, out result))
                    return result;
                foreach (var item in fMultiElements)
                {
                    result = item[name];
                    if (result != null)
                        return result;
                }
                return null;
            }
        }

        internal Dictionary<string, BaseXmlConfigFactory> XmlConfigFactories
        {
            get
            {
                return fXmlConfigFactories;
            }
        }

        internal IEnumerable<IElementWriter> CreateOrderPropertyInfoList()
        {
            var result = from item in fElementWriter
                         orderby item.Order
                         select item;
            return result;
        }

        public void CheckRequired(Object receiver)
        {
            foreach (var entry in fElementWriter)
            {
                if (entry.Required)
                {
                    //var propertyInfo = entry.ClassProperty;
                    Object value = entry.GetValue(receiver); //ObjectUtil.GetValue(propertyInfo, receiver);
                    TkDebug.AssertNotNull(value, string.Format(ObjectUtil.SysCulture,
                        "字段{0}设置了required=true，但是实际为null", entry.PropertyName), receiver);
                }
            }
        }
    }
}