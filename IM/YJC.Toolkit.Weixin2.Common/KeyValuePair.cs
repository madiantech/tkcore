using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin
{
    public class KeyValuePair
    {
        public KeyValuePair(string key, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(key, "key", null);
            TkDebug.AssertArgumentNullOrEmpty(value, "value", null);

            Key = key;
            Value = value;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Key { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Value { get; set; }
    }
}