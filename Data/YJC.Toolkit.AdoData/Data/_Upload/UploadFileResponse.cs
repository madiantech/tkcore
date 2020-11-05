using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class UploadFileResponse
    {
        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public long Size { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Type { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Error { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string DeleteUrl { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string DeleteType { get; set; }
    }
}
