using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class FieldItem
    {
        public FieldItem()
        {
        }

        internal FieldItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [SimpleAttribute]
        internal string Key { get; private set; }

        [TextContent]
        internal string Value { get; private set; }
    }
}