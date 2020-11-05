using YJC.Toolkit.Excel;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将结果内容输出成Excel格式的文件",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-01-23")]
    internal class ExportExcelPageMakerConfig : IConfigCreator<IPageMaker>, IReadObjectCallBack
    {
        public IPageMaker CreateObject(params object[] args)
        {
            return new ExportExcelPageMaker(this);
        }

        [SimpleAttribute(DefaultValue = true)]
        public bool UserBorder { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(HeaderFormat))]
        public ExcelContentFormat Header { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(ContentFormat))]
        public ExcelContentFormat Content { get; private set; }

        public void OnReadObject()
        {
            if (Header == null)
                Header = ExcelContentFormat.DefaultHeader;
            if (Content == null)
                Content = ExcelContentFormat.DefaultContent;
        }
    }
}
