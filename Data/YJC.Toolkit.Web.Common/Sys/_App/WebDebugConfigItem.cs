namespace YJC.Toolkit.Sys
{
    internal class WebDebugConfigItem : DebugConfigItem
    {
        [SimpleAttribute(DefaultValue = "_toolkit")]
        public string XmlQueryString { get; private set; }

        [SimpleAttribute(DefaultValue = "xml")]
        public string XmlValue { get; private set; }

        [SimpleAttribute(DefaultValue = "meta")]
        public string MetaDataValue { get; private set; }

        [SimpleAttribute(DefaultValue = "json")]
        public string JsonValue { get; private set; }

        [SimpleAttribute(DefaultValue = "excel")]
        public string ExcelValue { get; private set; }
    }
}