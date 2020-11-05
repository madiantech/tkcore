using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Xml;
using System.Threading;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "XElement格式转换器", Author = "YJC", CreateDate = "2013-09-30")]
    internal sealed class XElementObjectSerializer : IObjectSerializer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static readonly IObjectSerializer Instance = new XElementObjectSerializer();

        private XElementObjectSerializer()
        {
        }

        #region IObjectSerializer 成员

        public object CreateReader(string input, ReadSettings settings)
        {
            XDocument doc = XDocument.Parse(input);
            XElementData data = new XElementData { Root = doc, Current = doc.Root };
            return data;
        }

        public object CreateReader(Stream stream, ReadSettings settings)
        {
            XDocument doc = XDocument.Load(stream);
            stream.Close();
            return new XElementData { Root = doc, Current = doc.Root };
        }

        public object CreateCustomReader(object customObject)
        {
            XDocument doc = customObject as XDocument;
            if (doc != null)
                return new XElementData { Root = doc, Current = doc.Root };

            throw new NotSupportedException();
        }

        public object CreateWriter(Stream stream, WriteSettings settings)
        {
            XDocument doc = new XDocument();
            if (!settings.OmitHead)
            {
                XDeclaration decl = new XDeclaration("1.0", settings.Encoding.WebName, "yes");
                doc.Declaration = decl;
            }
            return new XElementData { Root = doc, Stream = stream };
        }

        public object CreateCustomWriter(object customObject)
        {
            if (customObject is XDocument)
                return customObject;

            throw new NotSupportedException();
        }

        public bool ReadToRoot(object reader, QName root)
        {
            XElementData data = reader.Convert<XElementData>();
            XName rootName = root.ToXName();
            if (data.Current.Name == rootName)
                return true;
            else
            {
                var element = (from item in data.Root.Descendants()
                               where item.Name == rootName
                               select item).FirstOrDefault();
                if (element == null)
                    return false;
                data.Current = element;
                return true;
            }
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElement current = XElementData.GetCurrent(reader);

            XName name = info.QName.ToXName();
            XAttribute attr = current.Attribute(name);
            string value = attr == null ? null : attr.Value;

            SerializerUtil.CheckRequiredAttribute(attribute, receiver, info, value);
            SerializerUtil.SetObjectValue(receiver, settings, info, value, attribute.AutoTrim);
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElement current = XElementData.GetCurrent(reader);

            string value = current.Value;
            object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info,
                value, attribute.AutoTrim);

            SerializerUtil.AddElementValue(attribute, receiver, info, objValue);
        }

        public void ReadTagElement(TagElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElementData data = reader.Convert<XElementData>();
            //XElementData next = new XElementData { Current = data.Current.Elements().First(), Root = data.Root };
            InternalReadElement(data, receiver, settings, info.QName, attribute.ChildElements, info.ModelName);
            SerializerUtil.CheckElementRequired(attribute.ChildElements, receiver);
        }

        public void ReadObjectElement(ObjectElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            Type objectType = info.ObjectType;
            object subObject = attribute.UseConstructor ? ObjectUtil.CreateObjectWithCtor(objectType)
                : ObjectUtil.CreateObject(objectType);
            Read(reader, subObject, info.ModelName, settings, info.QName, null);
            SerializerUtil.SetParent(receiver, subObject);

            SerializerUtil.AddElementValue(attribute, receiver, info, subObject);
        }

        public void ReadComplexElement(SimpleComplexElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElement current = XElementData.GetCurrent(reader);

            string value = current.HasElements ? current.FirstNode.ToString() : null;
            object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info, value, false);
            SerializerUtil.AddElementValue(attribute, receiver, info, objValue);
        }

        public void ReadTextContent(TextContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElement current = XElementData.GetCurrent(reader);

            string value = current.Value;
            SerializerUtil.CheckRequiredContent(attribute, receiver, info, value);
            SerializerUtil.SetObjectValue(receiver, settings, info, value, attribute.AutoTrim);
        }

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElement current = XElementData.GetCurrent(reader);

            string value = current.HasElements ? current.FirstNode.ToString() : null;
            SerializerUtil.CheckRequiredContent(attribute, receiver, info, value);
            SerializerUtil.SetObjectValue(receiver, settings, info, value, false);
        }

        public void ReadDictionary(DictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElement current = XElementData.GetCurrent(reader);
            IDictionary dict = attribute.GetDictionary(receiver, info);
            Type objectType = info.ObjectType;

            foreach (XElement child in current.Elements())
            {
                string nodeName = child.Name.LocalName;
                string nodeValue = child.Value;
                if (attribute.AutoTrim && nodeValue != null)
                    nodeValue = nodeValue.Trim();
                object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info, nodeValue, objectType);
                dict[nodeName] = objValue;
            }
        }

        public void ReadObjectDictionary(ObjectDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElementData currentData = reader.Convert<XElementData>();
            XElement current = currentData.Current;
            IDictionary dict = attribute.GetDictionary(receiver, info);
            Type objectType = info.ObjectType;

            foreach (XElement child in current.Elements())
            {
                string nodeName = child.Name.LocalName;
                object subObject = attribute.UseConstructor ?
                    ObjectUtil.CreateObjectWithCtor(objectType) : ObjectUtil.CreateObject(objectType);

                XElementData next = new XElementData { Current = child, Root = currentData.Root };
                Read(next, subObject, info.ModelName, settings, QName.Get(nodeName), null);
                SerializerUtil.SetParent(receiver, subObject);
                dict[nodeName] = subObject;
            }
        }

        public void ReadDynamicDictionary(DynamicDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XElementData currentData = reader.Convert<XElementData>();
            XElement current = currentData.Current;
            IDictionary dict = attribute.GetDictionary(receiver, info);
            var configData = attribute.PlugInFactory.ConfigData;
            //var propertyInfo = info.Convert<ReflectorObjectPropertyInfo>().Property;
            var elementReader = new ConfigFactoryElementReader(attribute, info, info.ModelName);

            foreach (XElement child in current.Elements())
            {
                string nodeName = child.Name.LocalName;
                QName qName = child.Name.ToQName();
                ObjectPropertyInfo subPropertyInfo;
                if (elementReader.SupportVersion)
                {
                    var verAttr = child.Attribute(ToolkitConst.VERSION);
                    string version = verAttr != null ? verAttr.Value : string.Empty;
                    subPropertyInfo = elementReader[nodeName, version];
                }
                else
                    subPropertyInfo = elementReader[nodeName];
                if (subPropertyInfo != null)
                {
                    ObjectElementAttribute elementAttr = subPropertyInfo.Attribute.Convert<ObjectElementAttribute>();
                    Type objectType = elementAttr.ObjectType;
                    object subObject = elementAttr.UseConstructor ? ObjectUtil.CreateObjectWithCtor(objectType)
                        : ObjectUtil.CreateObject(objectType);
                    XElementData next = new XElementData { Current = child, Root = currentData.Root };

                    Read(next, subObject, info.ModelName, settings, QName.Get(nodeName), null);
                    SerializerUtil.SetParent(receiver, subObject);
                    dict[nodeName] = subObject;
                }
            }
        }

        public void Read(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, BaseObjectAttribute attribute)
        {
            SerializerUtil.ReadObject(this, reader, receiver, modelName, settings,
                root, attribute, null, SerializerOptions.XmlOptions);
        }

        public void ReadObject(object reader, object receiver, string modelName, ReadSettings settings,
            QName root, object serializerData)
        {
            TkDebug.AssertArgumentNull(receiver, "receiver", this);
            TkDebug.AssertArgumentNull(settings, "settings", this);
            TkDebug.AssertArgumentNull(root, "root", this);

            XElementData currentData = reader.Convert<XElementData>();
            XElement current = currentData.Current;
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            foreach (var item in info.Attributes)
                item.Attribute.ReadObject(this, reader, receiver, settings, item, serializerData);

            if (current.IsEmpty)
            {
                SerializerUtil.ReadObjectCallBack(receiver);
                if (info.IsObjectContext)
                {
                    BaseGlobalVariable.Current.ObjectContext.Pop();
                }
                return;
            }

            bool readElement = false;
            if (info.Content != null)
                info.Content.Attribute.ReadObject(this, reader, receiver, settings,
                    info.Content, serializerData);
            else
            {
                readElement = true;
                InternalReadElement(currentData, receiver, settings, root, info.Elements, modelName);
            }

            SerializerUtil.ReadObjectCallBack(receiver);
            if (readElement)
                SerializerUtil.CheckElementRequired(info.Elements, receiver);
            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
        }

        public void ReadDictionary(object reader, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            attribute.ReadObject(this, reader, receiver, settings, info, serializerData);
        }

        public void ReadList(object reader, IList receiver, SimpleElementAttribute attribute,
            string modelName, ReadSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute);
            attribute.ReadObject(this, reader, receiver, settings, info, serializerData);
        }

        public void WriteAttribute(SimpleAttributeAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (ObjectUtil.IsDefaultValue(attribute.DefaultValue, value))
                return;

            XName name = info.QName.ToXName();
            XAttribute attr = new XAttribute(name, ObjectUtil.ToString(info.Converter, value, settings));
            XElement current = serializerData.Convert<XElement>();
            current.Add(attr);
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (!attribute.IsMultiple && ObjectUtil.IsDefaultValue(attribute.DefaultValue, value))
                return;

            var current = serializerData.Convert<XElement>();
            XElement element = new XElement(info.QName.ToXName(),
                ObjectUtil.ToString(info.Converter, value, settings));
            current.Add(element);
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            XElement element = new XElement(info.QName.ToXName());
            current.Add(element);
            SerializerUtil.WriteChildElement(this, writer, attribute.ChildElements, settings,
                value, (objValue, propInfo) => value, element);
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            XElement element = new XElement(info.QName.ToXName());
            current.Add(element);
            Write(writer, value, info.ModelName, settings, info.QName, element, null);
            //InternalWriteXml(writer, value, settings, element, info.ModelName);
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            string xmlValue = ObjectUtil.ToString(info.Converter, value, settings);
            XElement child = string.IsNullOrEmpty(xmlValue) ? null : XElement.Parse(xmlValue);
            XElement element = new XElement(info.QName.ToXName(), child);
            current.Add(element);
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            current.Value = ObjectUtil.ToString(info.Converter, value, settings);
        }

        public void WriteComplexContent(ComplexContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            string xmlValue = ObjectUtil.ToString(info.Converter, value, settings);
            if (!string.IsNullOrEmpty(xmlValue))
            {
                XElement child = XElement.Parse(xmlValue);
                current.Add(child);
            }
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            //XElement element = new XElement(info.QName.ToXName());
            //current.Add(element);
            IDictionary dict = value as IDictionary;
            if (dict != null)
            {
                foreach (DictionaryEntry item in dict)
                {
                    XElement subItem = new XElement(item.Key.ToString(),
                        ObjectUtil.ToString(info.Converter, item.Value, settings));
                    current.Add(subItem);
                }
            }
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            //XElement element = new XElement(info.QName.ToXName());
            //current.Add(element);
            IDictionary dict = value as IDictionary;
            if (dict != null)
            {
                foreach (DictionaryEntry item in dict)
                {
                    XElement subItem = new XElement(item.Key.ToString());
                    current.Add(subItem);
                    Write(writer, item.Value, info.ModelName, settings, QName.Get(item.Key.ToString()), subItem, null);
                    //InternalWriteXml(writer, item.Value, settings, element, info.ModelName);
                }
            }
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            var current = serializerData.Convert<XElement>();
            //XElement element = new XElement(info.QName.ToXName());
            //current.Add(element);
            IDictionary dict = value as IDictionary;
            if (dict != null)
            {
                var configData = attribute.PlugInFactory.ConfigData;
                foreach (DictionaryEntry item in dict)
                {
                    var itemAttr = configData[item.Value.GetType()];
                    QName itemName = itemAttr.GetQName(itemAttr.LocalName);
                    XElement subItem = new XElement(itemName.ToXName());
                    current.Add(subItem);
                    Write(writer, item.Value, info.ModelName, settings, itemName, subItem, null);
                    //InternalWriteXml(writer, item.Value, settings, element, info.ModelName);
                }
            }
        }

        public void Write(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData, BaseObjectAttribute attribute)
        {
            SerializerUtil.WriteObject(this, writer, receiver, modelName, settings,
                root, attribute, serializerData, SerializerOptions.XmlOptions);
        }

        public void WriteObject(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData)
        {
            var rootElement = serializerData.Convert<XElement>();
            InternalWriteXml(writer, receiver, settings, rootElement, modelName);
        }

        public void WriteList(object writer, IEnumerable receiver, SimpleElementAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            attribute.WriteObject(this, writer, receiver, settings, info, serializerData);
        }

        public void WriteDictionary(object writer, IDictionary receiver, BaseDictionaryAttribute attribute,
            string modelName, WriteSettings settings, QName root, object serializerData)
        {
            var info = new DictionaryListObjectPropertyInfo(receiver, attribute, root);
            attribute.WriteObject(this, writer, receiver, settings, info, serializerData);
        }

        public object BeginWrite(object writer, object receiver, WriteSettings settings, QName root)
        {
            XElementData doc = writer.Convert<XElementData>();
            XElement rootElement = new XElement(root.ToXName());
            doc.Root.Add(rootElement);
            return rootElement;
        }

        public void EndWrite(object writer, object receiver, WriteSettings settings)
        {
            XElementData doc = writer.Convert<XElementData>();
            if (doc.Stream != null)
            {
                StreamWriter sw = new StreamWriter(doc.Stream, settings.Encoding);
                using (sw)
                {
                    sw.Write(doc.Root.ToString());
                }
            }
        }

        #endregion IObjectSerializer 成员

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "root")]
        private void InternalReadElement(XElementData reader, object receiver, ReadSettings settings,

            QName root, ObjectElementInfo elements, string modelName)
        {
            foreach (var item in reader.Current.Elements())
            {
                QName name = item.Name.ToQName();
                ObjectPropertyInfo element = elements.GetObjectPerpertyInfo(name,
                    () => reader.Current.Attribute(ToolkitConst.VERSION).Value);
                if (element != null)
                {
                    XElementData next = new XElementData { Current = item, Root = reader.Root };
                    element.Attribute.ReadObject(this, next, receiver, settings, element, null);
                }
                else
                {
                    element = SerializerUtil.CustomRead(receiver, name.LocalName, modelName,
                        () => reader.Current.Attribute(ToolkitConst.VERSION).Value);
                    if (element != null)
                        element.Attribute.ReadObject(this, reader, receiver, settings, element, null);
                }
            }
        }

        private void InternalWriteXml(object writer, object receiver, WriteSettings settings,
            XElement current, string modelName)
        {
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            foreach (var attr in info.Attributes)
                SerializerUtil.WritePropertyInfo(this, writer, receiver, settings, attr, current);
            if (info.Content != null)
                SerializerUtil.WritePropertyInfo(this, writer, receiver,
                    settings, info.Content, current);
            else
            {
                SerializerUtil.WriteChildElement(this, writer, info.Elements, settings, receiver,
                    (objValue, propInfo) => propInfo.GetValue(objValue), current);
            }

            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
        }

        public override string ToString()
        {
            return "XElement格式对象转换器";
        }
    }
}