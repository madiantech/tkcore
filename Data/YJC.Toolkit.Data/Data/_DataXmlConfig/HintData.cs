using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class HintData : MultiLanguageText
    {
        [SimpleAttribute]
        public HintPosition Position { get; set; }
    }
}
