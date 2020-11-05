using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class DateConstraint : BaseConstraint
    {
        private readonly string fMessage;

        public DateConstraint(IFieldInfo field)
            : base(field)
        {
            IsFirstCheck = true;
            fMessage = field.DisplayName + TkWebApp.DateCMsg;
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            try
            {
                DateTime.Parse(value, ObjectUtil.SysCulture);
                return null;
            }
            catch
            {
                return CreateErrorObject(fMessage);
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "[{0}]的值必须是日期类型的约束", Field.DisplayName);
        }
    }
}
