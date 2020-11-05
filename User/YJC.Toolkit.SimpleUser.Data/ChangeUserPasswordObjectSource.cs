using YJC.Toolkit.Data;
using YJC.Toolkit.LogOn;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [ObjectSource(Author = "YJC", CreateDate = "2014-12-03", Description = "修改用户的密码")]
    class ChangeUserPasswordObjectSource : BasePasswordUpdateSource
    {
        public override object Query(IInputData input, string id)
        {
            return new BasePasswordData(input.QueryString["UserId"], input.QueryString["UserName"]);
        }

        protected override OutputData ChangePasswd(UserResolver resolver, BasePasswordData passwd)
        {
            resolver.ChangePassword(passwd.UserId,
                passwd.GetNewPassword("PasswordData"), null);
            return OutputData.CreateToolkitObject(resolver.CreateKeyData());
        }
    }
}
