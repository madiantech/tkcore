using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class RowConfigItem
    {
        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string Value { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MultiLanguageText Name { get; private set; }

        public CodeItem Convert()
        {
            return new CodeItem(Value, Name.ToString());
        }
    }
}