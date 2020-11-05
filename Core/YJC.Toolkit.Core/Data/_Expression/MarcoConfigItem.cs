using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Sys.Converter;

namespace YJC.Toolkit.Data
{
    public class MarcoConfigItem
    {
        public MarcoConfigItem()
        {
        }

        public MarcoConfigItem(bool needParse, bool sqlInject, string value)
            : this(needParse, sqlInject, value, null)
        {
        }

        public MarcoConfigItem(bool needParse, bool sqlInject, string value, string emptyMarco)
        {
            NeedParse = needParse;
            SqlInject = sqlInject;
            Value = value;
            EmptyMarco = (string[])StringArrayConverter.Converter.ConvertFromString(
                emptyMarco, ObjectUtil.ReadSettings);
        }

        [SimpleAttribute]
        public bool NeedParse { get; protected set; }

        [SimpleAttribute(LocalName = "SQLInject", DefaultValue = true)]
        public bool SqlInject { get; protected set; }

        [SimpleAttribute]
        private string[] EmptyMarco { get; set; }

        [TextContent]
        public string Value { get; protected set; }

        public IEnumerable<string> EmptyMarcoes
        {
            get
            {
                return EmptyMarco;
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Value) ? base.ToString() :
                "宏值为：" + Value;
        }
    }
}
