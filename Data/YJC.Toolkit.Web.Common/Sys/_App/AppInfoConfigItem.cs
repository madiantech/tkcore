using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    internal class AppInfoConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText FullName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText ShortName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Description { get; private set; }
    }
}
