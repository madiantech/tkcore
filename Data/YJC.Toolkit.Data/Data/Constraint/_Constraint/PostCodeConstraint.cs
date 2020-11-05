using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class PostCodeConstraint : RegexConstraint
    {
        public PostCodeConstraint(IFieldInfo field)
            : base(field, field.DisplayName + TkWebApp.PostCodeCMsg, @"^\d{6}$")
        {
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}]必须是邮政编码的约束",
                Field.DisplayName);
        }
    }
}
