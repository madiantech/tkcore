using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [InstancePlugIn, AlwaysCache]
    [ObjectSerializer(Description = "Xml格式转换器", Author = "YJC", CreateDate = "2013-09-29")]
    internal sealed class XmlObjectSerializer : IObjectSerializer
    {
        private const int BUFFER_SIZE = 0x400;
        internal readonly static XmlReaderSettings ReadSettings = new XmlReaderSettings { CloseInput = true };

        internal readonly static XmlWriterSettings WriteSettings = new XmlWriterSettings
        {
            Encoding = ToolkitConst.UTF8,
            OmitXmlDeclaration = true
        };

        public static IObjectSerializer Instance = new XmlObjectSerializer();

        private XmlObjectSerializer()
        {
        }

        #region IObjectSerializer 成员

        public object CreateReader(string input, ReadSettings settings)
        {
            return XmlReader.Create(new StringReader(input), ReadSettings);
        }

        public object CreateReader(Stream stream, ReadSettings settings)
        {
            StreamReader reader = new StreamReader(stream, settings.Encoding, true);
            return XmlReader.Create(reader, ReadSettings);
        }

        public object CreateCustomReader(object customObject)
        {
            throw new NotSupportedException();
        }

        public bool ReadToRoot(object reader, QName root)
        {
            XmlReader xmlreader = reader.Convert<XmlReader>();
            return xmlreader.ReadToFollowing(root.LocalName, root.Namespace);
        }

        public void ReadAttribute(SimpleAttributeAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();

            string localName = info.LocalName;
            string value = null;
            switch (attribute.NamespaceType)
            {
                case NamespaceType.None:
                    value = xml.GetAttribute(localName);
                    break;

                case NamespaceType.Toolkit:
                case NamespaceType.Namespace:
                    value = xml.GetAttribute(localName, attribute.NamespaceUri);
                    break;
            }

            SerializerUtil.CheckRequiredAttribute(attribute, receiver, info, value);
            SerializerUtil.SetObjectValue(receiver, settings, info, value, attribute.AutoTrim);
        }

        public void ReadElement(SimpleElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();
            string value = ReadString(xml);
            object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info,
                value, attribute.AutoTrim);

            SerializerUtil.AddElementValue(attribute, receiver, info, objValue);
        }

        public void ReadTagElement(TagElementAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xmlReader = reader.Convert<XmlReader>();
            InternalReadElement(xmlReader, receiver, settings, info.QName,
                attribute.ChildElements, info.ModelName);
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
            XmlReader xml = reader.Convert<XmlReader>();
            string value = ReadComplexContent(xml, info.QName);
            object objValue = SerializerUtil.GetPropertyObject(receiver, settings, info, value, false);

            SerializerUtil.AddElementValue(attribute, receiver, info, objValue);
        }

        public void ReadDictionary(DictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();

            QName name = info.QName;
            IDictionary dict = attribute.GetDictionary(receiver, info);
            Type objectType = info.ObjectType;

            if (xml.IsEmptyElement)
                return;

            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement)
                {
                    QName nodeName = QName.Get(xml);
                    if (nodeName == name)
                        return;
                }

                if (xml.NodeType == XmlNodeType.Element)
                {
                    QName nodeName = QName.Get(xml);
                    string nodeValue = ReadString(xml);
                    if (attribute.AutoTrim && nodeValue != null)
                        nodeValue = nodeValue.Trim();
                    object objValue = SerializerUtil.GetPropertyObject(receiver, settings,
                        info, nodeValue, objectType);
                    dict[nodeName.LocalName] = objValue;
                }
            }
        }

        public void ReadObjectDictionary(ObjectDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();

            QName name = info.QName;
            IDictionary dict = attribute.GetDictionary(receiver, info);
            Type objectType = info.ObjectType;

            if (xml.IsEmptyElement)
                return;

            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement)
                {
                    QName nodeName = QName.Get(xml);
                    if (nodeName == name)
                        return;
                }

                if (xml.NodeType == XmlNodeType.Element)
                {
                    QName nodeName = QName.Get(xml);
                    object subObject = attribute.UseConstructor ? ObjectUtil.CreateObjectWithCtor(objectType)
                        : ObjectUtil.CreateObject(objectType);
                    Read(reader, subObject, info.ModelName, settings, nodeName, null);
                    SerializerUtil.SetParent(receiver, subObject);

                    //string nodeValue = ReadString(xml);
                    //object objValue = SerializerUtil.GetPropertyObject(receiver, settings,
                    //    info, nodeValue, objectType);
                    dict[nodeName.LocalName] = subObject;
                }
            }
        }

        public void ReadDynamicDictionary(DynamicDictionaryAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();

            QName name = info.QName;
            IDictionary dict = attribute.GetDictionary(receiver, info);

            if (xml.IsEmptyElement)
                return;
            var configData = attribute.PlugInFactory.ConfigData;
            //var propertyInfo = info.Convert<ReflectorObjectPropertyInfo>().Property;
            var elementReader = new ConfigFactoryElementReader(attribute, info, info.ModelName);

            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement)
                {
                    QName nodeName = QName.Get(xml);
                    if (nodeName == name)
                        return;
                }

                if (xml.NodeType == XmlNodeType.Element)
                {
                    QName nodeName = QName.Get(xml);
                    ObjectPropertyInfo subPropertyInfo;
                    if (elementReader.SupportVersion)
                    {
                        string version = xml.GetAttribute(ToolkitConst.VERSION);
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
                        Read(reader, subObject, info.ModelName, settings, nodeName, null);
                        SerializerUtil.SetParent(receiver, subObject);
                        dict[nodeName.LocalName] = subObject;
                    }
                }
            }
        }

        public void ReadTextContent(TextContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();

            string value = ReadString(xml);
            SerializerUtil.CheckRequiredContent(attribute, receiver, info, value);
            SerializerUtil.SetObjectValue(receiver, settings, info, value, attribute.AutoTrim);
        }

        public void ReadComplexContent(ComplexContentAttribute attribute, object reader,
            object receiver, ReadSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlReader xml = reader.Convert<XmlReader>();
            TkDebug.Assert(xml.NodeType == XmlNodeType.Element, string.Format(ObjectUtil.SysCulture,
                "ComplexContent初始要求XmlReader在Element节点上，当前状态是{0}，位置不对", xml.NodeType), this);
            QName name = QName.Get(xml);
            string value = ReadComplexContent(xml, name);

            SerializerUtil.CheckRequiredContent(attribute, receiver, info, value);
            SerializerUtil.SetObjectValue(receiver, settings, info, value, false);
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

            XmlReader xmlReader = reader.Convert<XmlReader>();
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            foreach (var item in info.Attributes)
                item.Attribute.ReadObject(this, reader, receiver, settings, item, serializerData);

            if (xmlReader.IsEmptyElement)
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
                InternalReadElement(xmlReader, receiver, settings, root, info.Elements, modelName);
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

        public object CreateWriter(Stream stream, WriteSettings settings)
        {
            XmlWriterSettings writeSettings = new XmlWriterSettings
            {
                CloseOutput = settings.CloseInput,
                OmitXmlDeclaration = settings.OmitHead
            };
            if (settings.Encoding != ToolkitConst.UTF8)
                writeSettings.Encoding = settings.Encoding;
            var writer = XmlWriter.Create(stream, writeSettings);
            if (writer is XmlTextWriter textWriter)
                textWriter.QuoteChar = settings.QuoteChar;
            return writer;
        }

        public object CreateCustomWriter(object customObject)
        {
            throw new NotSupportedException();
        }

        public void WriteAttribute(SimpleAttributeAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (ObjectUtil.IsDefaultValue(attribute.DefaultValue, value))
                return;

            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            QName name = info.QName;
            if (name.HasNamespace)
            {
                PrefixTable prefix = serializerData.Convert<PrefixTable>();
                xmlWriter.WriteAttributeString(prefix.GetPrefix(name.Namespace), name.LocalName,
                    name.Namespace, ObjectUtil.ToString(info.Converter, value, settings));
            }
            else
                xmlWriter.WriteAttributeString(name.LocalName,
                    ObjectUtil.ToString(info.Converter, value, settings));
        }

        public void WriteElement(SimpleElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            if (!attribute.IsMultiple && ObjectUtil.IsDefaultValue(attribute.DefaultValue, value))
                return;

            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();

            WriteStartElement(xmlWriter, info.QName, prefixTable);
            string data = ObjectUtil.ToString(info.Converter, value, settings);
            if (attribute.UseCData)
                xmlWriter.WriteCData(data);
            else
                xmlWriter.WriteString(data);
            xmlWriter.WriteEndElement();
        }

        public void WriteTagElement(TagElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();
            WriteStartElement(xmlWriter, info.QName, prefixTable);
            SerializerUtil.WriteChildElement(this, writer, attribute.ChildElements, settings,
                value, (objValue, propInfo) => value, prefixTable);
            xmlWriter.WriteEndElement();
        }

        public void WriteObjectElement(ObjectElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();
            Write(writer, value, info.ModelName, settings, info.QName, prefixTable, null);
            //XmlWriter xmlWriter = writer.Convert<XmlWriter>();

            //WriteStartElement(xmlWriter, info.QName, prefixTable);
            //InternalWriteXml(writer, value, info.ModelName, settings, prefixTable);
            //xmlWriter.WriteEndElement();
        }

        public void WriteComplexElement(SimpleComplexElementAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();

            WriteStartElement(xmlWriter, info.QName, prefixTable);
            string data = ObjectUtil.ToString(info.Converter, value, settings);
            WriteComplexContent(xmlWriter, data);
            xmlWriter.WriteEndElement();
        }

        public void WriteTextContent(TextContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            xmlWriter.WriteString(ObjectUtil.ToString(info.Converter, value, settings));
        }

        public void WriteDictionary(DictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();
            WriteStartElement(xmlWriter, info.QName, prefixTable);
            IDictionary dict = value as IDictionary;
            if (dict != null)
            {
                foreach (DictionaryEntry item in dict)
                {
                    xmlWriter.WriteStartElement(item.Key.ToString());
                    xmlWriter.WriteString(ObjectUtil.ToString(info.Converter, item.Value, settings));
                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();
        }

        public void WriteObjectDictionary(ObjectDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();
            WriteStartElement(xmlWriter, info.QName, prefixTable);
            IDictionary dict = value as IDictionary;
            if (dict != null)
            {
                foreach (DictionaryEntry item in dict)
                {
                    Write(writer, item.Value, info.ModelName, settings,
                        QName.Get(item.Key.ToString()), prefixTable, null);
                    //xmlWriter.WriteStartElement(item.Key.ToString());
                    //InternalWriteXml(writer, item.Value, info.ModelName, settings, prefixTable);
                    //xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();
        }

        public void WriteDynamicDictionary(DynamicDictionaryAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = serializerData.Convert<PrefixTable>();
            WriteStartElement(xmlWriter, info.QName, prefixTable);
            IDictionary dict = value as IDictionary;
            if (dict != null)
            {
                var configData = attribute.PlugInFactory.ConfigData;
                foreach (DictionaryEntry item in dict)
                {
                    var itemAttr = configData[item.Value.GetType()];
                    QName itemName = itemAttr.GetQName(itemAttr.LocalName);
                    Write(writer, item.Value, info.ModelName, settings, itemName, prefixTable, null);
                    //WriteStartElement(xmlWriter, itemName, prefixTable);
                    //InternalWriteXml(writer, item.Value, info.ModelName, settings, prefixTable);
                    //xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();
        }

        public void WriteComplexContent(ComplexContentAttribute attribute, object writer,
            object value, WriteSettings settings, ObjectPropertyInfo info, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            string text = ObjectUtil.ToString(info.Converter, value, settings);
            WriteComplexContent(xmlWriter, text);
        }

        public void Write(object writer, object receiver, string modelName, WriteSettings settings,
            QName root, object serializerData, BaseObjectAttribute attribute)
        {
            SerializerUtil.WriteObject(this, writer, receiver, modelName, settings,
                root, attribute, serializerData, SerializerOptions.XmlOptions);
        }

        public void WriteObject(object writer, object receiver, string modelName,
            WriteSettings settings, QName root, object serializerData)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            var prefixTable = serializerData.Convert<PrefixTable>();

            WriteStartElement(xmlWriter, root, prefixTable);
            ObjectInfo info = ObjectInfo.Create(receiver, modelName);
            if (info.IsObjectContext)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.ObjectContext.Push(receiver);
            }

            foreach (var attr in info.Attributes)
                SerializerUtil.WritePropertyInfo(this, writer, receiver,
                    settings, attr, prefixTable);
            if (info.Content != null)
                SerializerUtil.WritePropertyInfo(this, writer, receiver,
                    settings, info.Content, prefixTable);
            else
            {
                SerializerUtil.WriteChildElement(this, writer, info.Elements, settings, receiver,
                    (objValue, propInfo) => propInfo.GetValue(objValue), prefixTable);
            }

            if (info.IsObjectContext)
            {
                BaseGlobalVariable.Current.ObjectContext.Pop();
            }
            xmlWriter.WriteEndElement();
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
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            PrefixTable prefixTable = new PrefixTable();

            xmlWriter.WriteStartDocument();
            return prefixTable;
        }

        public void EndWrite(object writer, object receiver, WriteSettings settings)
        {
            XmlWriter xmlWriter = writer.Convert<XmlWriter>();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
        }

        #endregion IObjectSerializer 成员

        //private void InternalWriteXml(object writer, object receiver, string modelName,
        //    WriteSettings settings, PrefixTable prefixTable)
        //{
        //}

        private void InternalReadElement(XmlReader reader, object receiver, ReadSettings settings,
            QName root, ObjectElementInfo elements, string modelName)
        {
            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;
                if (nodeType == XmlNodeType.EndElement)
                {
                    QName name = QName.Get(reader);
                    if (name == root)
                        break;
                }
                if (nodeType == XmlNodeType.Element)
                {
                    QName name = QName.Get(reader);
                    ObjectPropertyInfo element = elements.GetObjectPerpertyInfo(name,
                        () => reader.GetAttribute(ToolkitConst.VERSION));
                    if (element != null)
                        element.Attribute.ReadObject(this, reader, receiver, settings, element, null);
                    else
                    {
                        element = SerializerUtil.CustomRead(receiver, name.LocalName, modelName,
                            () => reader.GetAttribute(ToolkitConst.VERSION));
                        if (element != null)
                            element.Attribute.ReadObject(this, reader, receiver, settings, element, null);
                    }
                }
            }
        }

        private static void WriteStartElement(XmlWriter writer, QName name, PrefixTable prefixTable)
        {
            if (name.HasNamespace)
                writer.WriteStartElement(prefixTable.GetPrefix(name.Namespace),
                    name.LocalName, name.Namespace);
            else
                writer.WriteStartElement(name.LocalName);
        }

        private static bool IsTextualNode(XmlNodeType nodeType)
        {
            return (0L != (0x6018 & (((int)1) << (int)nodeType)));
        }

        private static string ReadString(XmlReader reader)
        {
            if (reader.ReadState != ReadState.Interactive)
            {
                return string.Empty;
            }
            reader.MoveToElement();
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.IsEmptyElement)
                {
                    return string.Empty;
                }
                if (!reader.Read())
                {
                    TkDebug.ThrowToolkitException("错误的Xml操作", reader);
                }
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    return string.Empty;
                }
            }
            string str = string.Empty;
            while (IsTextualNode(reader.NodeType))
            {
                str = str + reader.Value;
                if (!reader.Read())
                {
                    return str;
                }
            }
            return str;
        }

        private static string ReadComplexContent(XmlReader xml, QName name)
        {
            StringBuilder text = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(text, WriteSettings);
            using (writer)
            {
                if (xml.IsEmptyElement || xml.Read())
                    WriteNode(writer, xml, name, true);
                writer.Flush();
            }
            string value = text.ToString();
            return value;
        }

        private void WriteComplexContent(XmlWriter xmlWriter, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
            string newXml = string.Format(ObjectUtil.SysCulture, ToolkitConst.TOOLKIT_XML_SKELETON, text);
            try
            {
                XmlReader reader = CreateReader(newXml, ObjectUtil.ReadSettings).Convert<XmlReader>();
                using (reader)
                {
                    if (ReadToRoot(reader, QName.Toolkit))
                    {
                        while (reader.Read())
                        {
                            xmlWriter.WriteNode(reader, true);
                        }
                    }
                    xmlWriter.Flush();
                }
            }
            catch
            {
            }
        }

        public static void WriteNode(XmlWriter writer, XmlReader reader, QName endName, bool defattr)
        {
            bool canReadValueChunk = reader.CanReadValueChunk;
            char[] writeNodeBuffer = null;

            do
            {
                if (reader.IsEmptyElement || reader.NodeType == XmlNodeType.EndElement)
                {
                    var name = QName.Get(reader);
                    if (name == endName)
                        break;
                }
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        writer.WriteAttributes(reader, defattr);
                        if (reader.IsEmptyElement)
                        {
                            writer.WriteEndElement();
                        }
                        break;

                    case XmlNodeType.Text:
                        int num2;
                        if (!canReadValueChunk)
                        {
                            writer.WriteString(reader.Value);
                            break;
                        }
                        if (writeNodeBuffer == null)
                        {
                            writeNodeBuffer = new char[BUFFER_SIZE];
                        }
                        while ((num2 = reader.ReadValueChunk(writeNodeBuffer, 0, BUFFER_SIZE)) > 0)
                        {
                            writer.WriteChars(writeNodeBuffer, 0, num2);
                        }
                        break;

                    case XmlNodeType.CDATA:
                        writer.WriteCData(reader.Value);
                        break;

                    case XmlNodeType.EntityReference:
                        writer.WriteEntityRef(reader.Name);
                        break;

                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.XmlDeclaration:
                        writer.WriteProcessingInstruction(reader.Name, reader.Value);
                        break;

                    case XmlNodeType.Comment:
                        writer.WriteComment(reader.Value);
                        break;

                    case XmlNodeType.DocumentType:
                        writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"),
                            reader.GetAttribute("SYSTEM"), reader.Value);
                        break;

                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        writer.WriteWhitespace(reader.Value);
                        break;

                    case XmlNodeType.EndElement:
                        writer.WriteFullEndElement();
                        break;
                }
            }
            while (reader.Read());
        }

        public override string ToString()
        {
            return "Xml格式对象转换器";
        }
    }
}