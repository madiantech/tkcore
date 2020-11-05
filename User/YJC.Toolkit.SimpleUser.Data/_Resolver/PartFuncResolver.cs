using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleRight
{
    [Resolver(Author = "YJC", CreateDate = "2014-08-08",
        Description = "角色功能表(SYS_PART_FUNC)的数据访问对象")]
    internal class PartFuncResolver : Tk5TableResolver
    {
        public const string DATAXML = "UserManager/PartFunc.xml";

        public PartFuncResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}
