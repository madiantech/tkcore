using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class AbbreviateHandlerXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Abbreviate")]
        public RegNameList<AbbreviateConfigItem> AbbreviateList { get; private set; }

        public AbbreviateConfigItem FindItem(string name)
        {
            if (AbbreviateList == null)
                return null;
            return AbbreviateList[name.ToLower(ObjectUtil.SysCulture)];
        }
    }
}