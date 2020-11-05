using System;
using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class Tk5EditConfig
    {
        internal Tk5EditConfig()
        {
        }

        public Tk5EditConfig(List<HtmlAttribute> attributeList)
        {
            AttributeList = attributeList;
        }

        [SimpleAttribute]
        public bool ReadOnly { get; set; }

        [SimpleAttribute]
        public string Class { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem DefaultValue { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        internal UpdatingConfigItem Updating { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Attribute")]
        public List<HtmlAttribute> AttributeList { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CoreDisplayConfigFactory.REG_NAME)]
        public IConfigCreator<IDisplay> Display { get; set; }

        internal void Override(OverrideEditConfig ovEdit)
        {
            if (ovEdit.ReadOnly.HasValue)
                ReadOnly = ovEdit.ReadOnly.Value;
            if (ovEdit.Class != null)
                Class = ovEdit.Class;
            if (ovEdit.DefaultValue != null)
                DefaultValue = ovEdit.DefaultValue;
            if (ovEdit.Updating != null)
                Updating = ovEdit.Updating;
            if (ovEdit.AttributeList != null)
            {
                if (AttributeList == null)
                    AttributeList = ovEdit.AttributeList;
                else
                    AttributeList.AddRange(ovEdit.AttributeList);
            }
            if (ovEdit.Display != null)
                Display = ovEdit.Display;
        }

        public static Tk5EditConfig Clone(Tk5EditConfig config)
        {
            if (config == null)
                return null;

            Tk5EditConfig result = new Tk5EditConfig
            {
                ReadOnly = config.ReadOnly,
                Class = config.Class,
                DefaultValue = config.DefaultValue,
                Updating = config.Updating,
                AttributeList = config.AttributeList,
                Display = config.Display
            };
            return result;
        }
    }
}