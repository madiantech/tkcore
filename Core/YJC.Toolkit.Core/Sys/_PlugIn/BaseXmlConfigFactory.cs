using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseXmlConfigFactory : BasePlugInFactory
    {
        public class VersionRegName
        {
            public VersionRegName(string regName, string version)
            {
                RegName = regName;
                Version = version;
            }

            [SimpleAttribute]
            public string RegName { get; private set; }

            [SimpleAttribute]
            public string Version { get; private set; }
        }

        private readonly IConfigFactoryData fConfigData;

        protected BaseXmlConfigFactory(string name, string description)
            : this(name, description, false)
        {
        }

        protected BaseXmlConfigFactory(string name, string description, bool supportVersion)
            : base(name, description)
        {
            SupportVersion = supportVersion;
            if (supportVersion)
                fConfigData = new VersionConfigFactoryData();
            else
                fConfigData = new ConfigFactoryData();
        }

        internal IConfigFactoryData ConfigData
        {
            get
            {
                return fConfigData;
            }
        }

        public bool SupportVersion { get; private set; }

        public string DefaultVersion { get; set; }

        private string GetVersion(BaseObjectElementAttribute attribute)
        {
            return string.IsNullOrEmpty(attribute.Version) ? DefaultVersion : attribute.Version;
        }

        //internal void AddElements(ObjectElementInfo elements, Type type,
        //    PropertyInfo property, string modelName, DynamicElementAttribute dynamic)
        //{
        //    EnumableCodePlugIn((regName, regType, attr) =>
        //    {
        //        ObjectElementAttribute objectAttr = attr.Convert<BaseObjectElementAttribute>()
        //            .ConvertTo(regName, regType, dynamic);
        //        elements.Add(type, property, objectAttr, modelName);
        //    });
        //}

        //internal void AddElements(Dictionary<string, ObjectElementAttribute> dictionary, string factoryName)
        //{
        //    DynamicElementAttribute dynamic = new DynamicElementAttribute(factoryName);
        //    EnumableCodePlugIn((regName, regType, attr) =>
        //    {
        //        ObjectElementAttribute objectAttr = attr.Convert<BaseObjectElementAttribute>()
        //            .ConvertTo(regName, regType, dynamic);
        //        dictionary.Add(regName, objectAttr);
        //    });
        //}

        protected override bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            BaseObjectElementAttribute elementAttr = attribute.Convert<BaseObjectElementAttribute>();
            string newRegName;
            if (SupportVersion)
            {
                VersionRegName version = new VersionRegName(regName, GetVersion(elementAttr));
                newRegName = version.WriteJson();
            }
            else
                newRegName = regName;
            var result = base.Add(newRegName, attribute, type);

            if (result)
                fConfigData.Add(this, regName, elementAttr, type);
            return result;
        }
    }
}