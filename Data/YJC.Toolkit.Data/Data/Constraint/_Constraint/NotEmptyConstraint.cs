using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class NotEmptyConstraint : BaseConstraint
    {
        private readonly string fMessage;

        public NotEmptyConstraint(IFieldInfo field)
            : base(field)
        {
            AutoTrim = true;
            fMessage = field.DisplayName + TkWebApp.NotEmptyCMsg;
        }

        public bool ForceCheck
        {
            get
            {
                return CoerceCheck;
            }
            set
            {
                CoerceCheck = value;
            }
        }

        public bool AutoTrim { get; set; }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (AutoTrim && value != null)
                value = value.Trim();
            if (string.IsNullOrEmpty(value))
                return CreateErrorObject(fMessage);
            return null;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}]不能为空的约束", Field.DisplayName);
        }
    }
}
