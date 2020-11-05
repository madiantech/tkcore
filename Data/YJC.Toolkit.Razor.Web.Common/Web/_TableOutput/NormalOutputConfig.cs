using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [TableOutputConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2020-06-30",
        Description = "单记录正常排列字段")]
    internal class NormalOutputConfig : IConfigCreator<ITableOutput>
    {
        [SimpleAttribute(DefaultValue = 3)]
        public int ColumnCount { get; private set; }

        public ITableOutput CreateObject(params object[] args)
        {
            return new NormalOutput
            {
                ColumnCount = ColumnCount
            };
        }
    }
}