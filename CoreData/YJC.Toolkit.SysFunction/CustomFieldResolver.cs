using YJC.Toolkit.Data;

namespace YJC.Toolkit.SysFunction
{
    /// <summary>
    /// SYS_FUNCTION 表的数据访问对象
    /// </summary>
    [Resolver(Author = "YJC", CreateDate = "2014-10-21",
        Description = "自定义字段(SYS_CUSTOM_FIELD)的数据访问对象")]
    internal class CustomFieldResolver : Tk5TableResolver
    {
        public const string DATAXML = "SysFunction/CustomField.xml";

        public CustomFieldResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}
