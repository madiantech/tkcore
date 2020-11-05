using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class ChinaUnicomConstraint : BaseMobileConstraint
    {
        public ChinaUnicomConstraint(IFieldInfo field)
            : base(field, TkWebApp.ChinaUnicom,
            "ChinaUnicomConfig", @"^1(3[0-2]|45|5[5-6]|66|7[1,5-6]|8[5-6])\d{8}$")
        {
        }
    }
}