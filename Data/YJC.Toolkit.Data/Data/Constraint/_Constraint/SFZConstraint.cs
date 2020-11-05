using System.Text.RegularExpressions;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class SFZConstraint : RegexConstraint
    {
        public SFZConstraint(IFieldInfo field)
            : base(field, field.DisplayName +
            TkWebApp.SFZCMsg, @"^\d{15}$|^\d{17}[0-9x]$", RegexOptions.IgnoreCase)
        {
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "[{0}]必须是身份证号码的约束", Field.DisplayName);
        }
    }
}
