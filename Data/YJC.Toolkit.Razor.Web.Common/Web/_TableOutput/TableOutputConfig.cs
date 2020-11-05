using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [TableOutputConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2020-06-30",
        Description = "多记录以Table方式排列字段")]
    internal class TableOutputConfig : IConfigCreator<ITableOutput>
    {
        [SimpleAttribute]
        public bool IsFix { get; private set; }

        [SimpleAttribute]
        public bool DirectShowDetail { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public RazorOutputData OtherNewButton { get; protected set; }

        public ITableOutput CreateObject(params object[] args)
        {
            return new TableOutput(DirectShowDetail)
            {
                IsFix = IsFix,
                OtherNewButton = OtherNewButton
            };
        }
    }
}