using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class ChinaMobileConstraint : BaseMobileConstraint
    {
        public ChinaMobileConstraint(IFieldInfo field)
            : base(field, TkWebApp.ChinaMobile,
            "ChinaMobileConfig", @"^1(3[4-9]|47|5[0-2,7-9]|7[2,8]|8[2-4,7-8]|98)\d{8}$")
        {
        }
    }
}