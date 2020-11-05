using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class CodeTabSheetConfig
    {
        [SimpleAttribute]
        public string CodeRegName { get; private set; }

        [SimpleAttribute]
        public string NickName { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool NeedAllTab { get; private set; }
    }
}
