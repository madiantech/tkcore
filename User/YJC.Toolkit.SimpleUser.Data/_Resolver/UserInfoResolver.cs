using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [Resolver(Author = "YJC", CreateDate = "2014-07-11", Description = "当前登录用户的数据访问对象")]
    class UserInfoResolver : UserResolver
    {
        public UserInfoResolver(IDbDataSource source)
            : base(source)
        {
            if (DetailSource != null)
                DetailSource.FillingUpdateTables += DetailSource_FillingUpdateTables;
        }

        private void DetailSource_FillingUpdateTables(object sender, FillingUpdateEventArgs e)
        {
            SelectRowWithKeys(BaseGlobalVariable.UserId);
            e.Handled.SetHandled(this, true);
        }
    }
}
