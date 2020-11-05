using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class EmailConstraint : BaseConstraint
    {
        private readonly string fMessage;

        public EmailConstraint(IFieldInfo field)
            : base(field)
        {
            fMessage = field.DisplayName + TkWebApp.EmailCMsg;
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (value.IndexOf('@') == -1)
                return CreateErrorObject(fMessage);
            return null;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "[{0}]的值必须是email类型的约束", Field.DisplayName);
        }
    }
}
