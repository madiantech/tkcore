using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class XmlPlugInAttribute : Attribute
    {
        public XmlPlugInAttribute(string xmlPath, Type xmlConfigType)
        {
            XmlConfigType = xmlConfigType;
            XmlPath = xmlPath;
            ConfigInfo = new XmlConfigInfo(xmlConfigType, true);
        }

        public Type XmlConfigType { get; private set; }

        public string XmlPath { get; private set; }

        public string SearchPattern { get; set; }

        internal XmlConfigInfo ConfigInfo { get; private set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(XmlPath) || XmlConfigType == null ? base.ToString() :
                string.Format(ObjectUtil.SysCulture,
                "Xml路径为{0}，扫描类型为{1}", XmlPath, XmlConfigType);
        }
    }
}
