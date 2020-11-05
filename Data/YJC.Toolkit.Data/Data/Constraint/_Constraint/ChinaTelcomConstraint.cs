using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class ChinaTelcomConstraint : BaseMobileConstraint
    {
        public ChinaTelcomConstraint(IFieldInfo field)
            : base(field, TkWebApp.ChinaTelcom,
            "ChinaTelcomConfig", @"^1(33|49|53|7[3,7]|8[0-1,9]|99)\d{8}$")
        {
        }
    }
}