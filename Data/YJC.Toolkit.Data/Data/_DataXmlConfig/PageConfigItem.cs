using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class PageConfigItem
    {
        [SimpleAttribute(Required = true)]
        public PageStyleClass Style { get; private set; }

        [SimpleAttribute(Required = true)]
        public ControlType Control { get; private set; }

        [SimpleAttribute]
        public int Order { get; private set; }

        [SimpleAttribute]
        public string CustomControl { get; private set; }

        [SimpleAttribute]
        public string CustomControlData { get; private set; }
    }
}