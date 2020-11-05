using YJC.Toolkit.Data;

namespace YJC.Toolkit.SysFunction
{
    /// <summary>
    /// SYS_FUNCTION 表的数据访问对象
    /// </summary>
    [Resolver(Author = "YJC", CreateDate = "2014-10-21",
        Description = "自定义表输入(SYS_CUSTOM_TABLE)的数据访问对象")]
    internal class CustomTableResolver : Tk5TableResolver
    {
        public const string DATAXML = "SysFunction/CustomTable.xml";

        public CustomTableResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}
