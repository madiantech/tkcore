using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class OverrideExtensionConfig
    {
        [SimpleAttribute]
        public string CheckValue { get; private set; }

        [SimpleAttribute]
        public string UnCheckValue { get; private set; }

        [SimpleAttribute]
        public string Expression { get; private set; }

        [SimpleAttribute]
        public string Format { get; private set; }

        [SimpleAttribute]
        public Alignment? Align { get; private set; }

        [SimpleAttribute]
        public string EmptyTitle { get; private set; }
    }
}
