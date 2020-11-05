using YJC.Toolkit.Data;
using YJC.Toolkit.LogOn;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [ObjectSource(Author = "YJC", CreateDate = "2014-12-03", Description = "修改自己的密码")]
    class ChangeSelfPasswordObjectSource : BasePasswordUpdateSource
    {
        public override object Query(IInputData input, string id)
        {
            return new PasswordData();
        }

        protected override OutputData ChangePasswd(UserResolver resolver, BasePasswordData passwd)
        {
            PasswordData password = passwd.Convert<PasswordData>();
            bool success = resolver.ChangePassword(passwd.UserId, passwd.GetNewPassword("PasswordData"),
                password.OldPassword);
            if (success)
                return OutputData.CreateToolkitObject(resolver.CreateKeyData());
            else
            {
                const string errorMsg = "原密码不匹配";
                FieldErrorInfo field = new FieldErrorInfo("PasswordData", "OldPassword", errorMsg);
                throw new WebPostException(errorMsg, field);
            }
        }
    }
}
