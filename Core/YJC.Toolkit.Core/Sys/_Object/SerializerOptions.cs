using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public class SerializerOptions
    {
        public static readonly SerializerOptions All = new SerializerOptions
        {
            ReadObject = true,
            WriteObject = true,
            ReadList = true,
            WriteList = true,
            ReadDictionary = true,
            WriteDictionary = true,
            CheckListAttribute = false,
            CheckDictionaryAttribute = false
        };

        public static readonly SerializerOptions OnlyObject = new SerializerOptions
        {
            ReadObject = true,
            WriteObject = true
        };

        public static readonly SerializerOptions XmlOptions = new SerializerOptions
        {
            ReadObject = true,
            WriteObject = true,
            ReadList = true,
            WriteList = true,
            ReadDictionary = true,
            WriteDictionary = true,
            CheckListAttribute = true
        };

        public bool ReadObject { get; set; }

        public bool WriteObject { get; set; }

        public bool ReadList { get; set; }

        public bool WriteList { get; set; }

        public bool ReadDictionary { get; set; }

        public bool WriteDictionary { get; set; }

        public bool CheckListAttribute { get; set; }

        public bool CheckDictionaryAttribute { get; set; }

        public void CheckReadObject(IObjectSerializer serializer)
        {
            if (!ReadObject)
                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                    "{0}不支持读取数据存储到对象操作", serializer), serializer);
        }

        public void CheckWriteObject(IObjectSerializer serializer)
        {
            if (!WriteObject)
                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                    "{0}不支持对象内容写到指定格式数据的操作", serializer), serializer);
        }

        public void CheckReadList(IObjectSerializer serializer, SimpleElementAttribute attribute)
        {
            if (!ReadList)
                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                    "{0}不支持读取数据存储到列表的操作", serializer), serializer);
            CheckAttribute(serializer, attribute, CheckListAttribute);
        }

        public void CheckWriteList(IObjectSerializer serializer, SimpleElementAttribute attribute)
        {
            if (!WriteList)
                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                    "{0}不支持列表内容写到指定格式数据的操作", serializer), serializer);
            CheckAttribute(serializer, attribute, CheckListAttribute);
        }

        public void CheckReadDictionary(IObjectSerializer serializer, BaseDictionaryAttribute attribute)
        {
            if (!ReadDictionary)
                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                 "{0}不支持读取数据存储到字典的操作", serializer), serializer);

            CheckAttribute(serializer, attribute, CheckDictionaryAttribute);
        }

        public void CheckWriteDictionary(IObjectSerializer serializer, BaseDictionaryAttribute attribute)
        {
            if (!WriteDictionary)
                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                    "{0}不支持字典内容写到指定格式数据的操作", serializer), serializer);

            CheckAttribute(serializer, attribute, CheckDictionaryAttribute);
        }

        private static void CheckAttribute(IObjectSerializer serializer,
            NamedAttribute attribute, bool checkAttribute)
        {
            if (checkAttribute)
            {
                if (attribute == null || string.IsNullOrEmpty(attribute.LocalName))
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                        "{0}要求传入的attribute参数必须配置LocalName，当前没有配置", serializer), serializer);
            }
        }
    }
}