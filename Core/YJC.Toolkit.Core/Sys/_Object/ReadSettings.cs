using System.Text;

namespace YJC.Toolkit.Sys
{
    public sealed class ReadSettings
    {
        public static readonly ReadSettings Default = new ReadSettings();

        public ReadSettings()
        {
            ReadRoot = true;
            //DateFormat = ToolkitConst.DATE_FMT_STR;
            DateTimeFormat = ToolkitConst.DATETIME_FMT_STR;
            Encoding = ToolkitConst.UTF8;
        }

        [SimpleAttribute(DefaultValue = true)]
        public bool ReadRoot { get; set; }

        //[SimpleAttribute]
        //public string DateFormat { get; set; }

        [SimpleAttribute(DefaultValue = ToolkitConst.DATETIME_FMT_STR)]
        public string DateTimeFormat { get; set; }

        [SimpleAttribute(DefaultValue = "utf-8")]
        public Encoding Encoding { get; set; }
    }
}
