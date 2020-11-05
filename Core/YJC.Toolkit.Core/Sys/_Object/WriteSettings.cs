using System.Text;

namespace YJC.Toolkit.Sys
{
    public sealed class WriteSettings
    {
        public static readonly WriteSettings Default = new WriteSettings();

        public WriteSettings()
        {
            Encoding = ToolkitConst.UTF8;
            CloseInput = true;
            //DateFormat = ToolkitConst.DATE_FMT_STR;
            DateTimeFormat = ToolkitConst.DATETIME_FMT_STR;
            QuoteChar = ToolkitConst.QUOTE_CHAR;
        }

        //[SimpleAttribute]
        //public string DateFormat { get; set; }

        [SimpleAttribute(DefaultValue = ToolkitConst.DATETIME_FMT_STR)]
        public string DateTimeFormat { get; set; }

        [SimpleAttribute(DefaultValue = "utf-8")]
        public Encoding Encoding { get; set; }

        [SimpleAttribute]
        public bool OmitHead { get; set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool CloseInput { get; set; }

        [SimpleAttribute(DefaultValue = ToolkitConst.QUOTE_CHAR)]
        public char QuoteChar { get; set; }
    }
}