using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class SavePathConfigItem
    {
        [SimpleAttribute(DefaultValue = FilePathPosition.Xml)]
        public FilePathPosition Position { get; private set; }

        [SimpleAttribute]
        public string RelativePath { get; private set; }
    }
}