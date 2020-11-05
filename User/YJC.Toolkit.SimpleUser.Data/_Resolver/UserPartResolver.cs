using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleRight
{
    [Resolver(Author = "YJC", CreateDate = "2014-07-08",
        Description = "用户角色表(UR_USERS_PART)的数据访问对象")]
    internal class UserPartResolver : Tk5TableResolver
    {
        public const string DATAXML = "UserManager/UsersPart.xml";

        public UserPartResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}
