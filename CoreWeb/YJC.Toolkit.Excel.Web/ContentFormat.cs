using YJC.Toolkit.Excel;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ContentFormat : ExcelContentFormat
    {
        [SimpleAttribute(DefaultValue = Alignment.Right, LocalName = "Align")]
        public Alignment InternalAlign
        {
            get
            {
                return Align;
            }
            set
            {
                Align = value;
            }
        }

        [SimpleAttribute(DefaultValue = false, LocalName = "FontBold")]
        public bool InternalFontBold
        {
            get
            {
                return FontBold;
            }
            set
            {
                FontBold = value;
            }
        }
    }
}
