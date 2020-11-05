using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class CurrentDateConstraint : BaseConstraint
    {
        public CurrentDateConstraint(IFieldInfo field)
            : base(field)
        {
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            try
            {
                DateTime time = DateTime.Parse(value, ObjectUtil.SysCulture);
                if (time > DateTime.Today)
                    return CreateErrorObject(Field.DisplayName + TkWebApp.CurrentDateCMsg);
                return null;
            }
            catch
            {
                return CreateErrorObject(Field.DisplayName + TkWebApp.DateCMsg);
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "日期[{0}]不大于当前日期的约束", Field.DisplayName);
        }
    }
}
