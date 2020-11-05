using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.LogOn
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-07-25", Description = "系统管理员修改密码的元数据")]
    [RegType(Author = "YJC", CreateDate = "2014-11-18", Description = "系统管理员修改密码的数据")]
    public class BasePasswordData
    {
        protected BasePasswordData()
        {
        }

        public BasePasswordData(string userId, string userName)
        {
            TkDebug.AssertArgumentNullOrEmpty(userId, "userId", null);
            TkDebug.AssertArgumentNullOrEmpty(userName, "userName", null);

            UserId = userId;
            UserName = userName;
        }

        [SimpleAttribute]
        [DisplayName("用户ID")]
        [FieldControl(ControlType.Hidden, Order = 10)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(IsKey = true)]
        public string UserId { get; protected set; }

        [SimpleAttribute]
        [DisplayName("用户名")]
        [FieldControl(ControlType.Label, Order = 20)]
        [FieldLayout(FieldLayout.PerLine)]
        public string UserName { get; protected set; }

        [SimpleAttribute]
        [DisplayName("新密码")]
        [FieldControl(ControlType.Password, Order = 50)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(IsEmpty = false, Length = 30)]
        public string Password { get; protected set; }

        [SimpleAttribute]
        [DisplayName("确认密码")]
        [FieldControl(ControlType.Password, Order = 60)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(IsEmpty = false, Length = 30)]
        public string ConfirmPassword { get; protected set; }

        public string GetNewPassword(string tableName)
        {
            if (Password == ConfirmPassword)
                return Password;
            const string errorMsg = "新密码与确认密码不匹配";
            FieldErrorInfo info = new FieldErrorInfo(tableName, "Password", errorMsg);
            throw new WebPostException(errorMsg, info);
        }
    }
}
