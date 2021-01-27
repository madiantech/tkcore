using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public class DateAfterConstraint : BaseConstraint
    {
        public DateAfterConstraint(IFieldInfo field, IFieldInfo beforeField)
            : base(field)
        {
            BeforeField = beforeField;
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            string beforeValue = HostDataSet.Tables[TableName].Rows[position]
                [BeforeField.NickName].ToString();
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(beforeValue))
                return null;

            DateTime beforeTime = beforeValue.Value<DateTime>();
            DateTime afterTime = value.Value<DateTime>();
            if (beforeTime > afterTime)
                return CreateErrorObject(string.Format(ObjectUtil.SysCulture, "{0}必须在{1}之后！",
                    Field.DisplayName, BeforeField.DisplayName));

            return null;
        }

        public IFieldInfo BeforeField { get; private set; }
    }
}