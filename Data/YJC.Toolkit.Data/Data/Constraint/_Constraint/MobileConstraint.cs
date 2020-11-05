using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class MobileConstraint : BaseMobileConstraint
    {
        public MobileConstraint(IFieldInfo field)
            : base(field, TkWebApp.MobileCName, "MobileConfig",
            @"^1([3-9][0-9])\d{8}$")
        {
        }
    }
}