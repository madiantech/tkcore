using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public class ExcelContentFormat
    {
        public static readonly ExcelContentFormat DefaultHeader 
            = new ExcelContentFormat(true, Alignment.Center);
        public static readonly ExcelContentFormat DefaultContent
            = new ExcelContentFormat(false, Alignment.Right);

        protected ExcelContentFormat()
        {
        }

        public ExcelContentFormat(bool fontBold, Alignment align)
            : this("宋体", 11, fontBold, align)
        {
        }

        public ExcelContentFormat(string fontName, short fontSize, bool fontBold, Alignment align)
        {
            FontName = fontName;
            FontSize = fontSize;
            FontBold = fontBold;
            Align = align;
        }

        [SimpleAttribute(DefaultValue = "宋体")]
        public string FontName { get; protected set; }

        [SimpleAttribute(DefaultValue = 11)]
        public short FontSize { get; protected set; }

        public bool FontBold { get; protected set; }

        public Alignment Align { get; protected set; }
    }
}
