using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.LogOn
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-07-08", Description = "用户登录的元数据")]
    [RegType(Author = "YJC", CreateDate = "2014-11-18", Description = "用户登录的数据")]
    public class LogOnData
    {
        [SimpleAttribute]
        [DisplayName("用户名")]
        [FieldControl(ControlType.Text, Order = 10)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(IsEmpty = false, IsKey = true, Length = 30)]
        public string LogOnName { get; set; }

        [SimpleAttribute]
        [DisplayName("密　码")]
        [FieldControl(ControlType.Password, Order = 20)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(Length = 30)]
        public string Password { get; set; }
    }
}
