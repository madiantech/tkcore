using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;


namespace YJC.Toolkit.LogOn
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-07-25", Description = "用户修改密码的元数据")]
    [RegType(Author = "YJC", CreateDate = "2014-11-18", Description = "用户修改密码的数据")]
    public class PasswordData : BasePasswordData
    {
        public PasswordData()
            : base(BaseGlobalVariable.UserId == null ? string.Empty : BaseGlobalVariable.UserId.ToString(),
            BaseGlobalVariable.UserName)
        {
        }

        [SimpleAttribute]
        [DisplayName("原密码")]
        [FieldControl(ControlType.Password, Order = 30)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(Length = 30)]
        public string OldPassword { get; private set; }
    }
}
