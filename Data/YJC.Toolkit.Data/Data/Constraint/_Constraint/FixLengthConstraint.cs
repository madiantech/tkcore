using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class FixLengthConstraint : BaseConstraint
    {
        private readonly string fMessage;

        public FixLengthConstraint(IFieldInfo field, int length)
            : base(field)
        {
            Length = length;
            TkDebug.AssertArgument(length > 0, "length", string.Format(
                ObjectUtil.SysCulture, "length参数的值必须大于0，现在值为{0}", length), null);

            fMessage = string.Format(ObjectUtil.SysCulture,
                TkWebApp.FixLengthCMsg, field.DisplayName, length);
        }

        public int Length { get; private set; }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            int length = StringLengthConstraint.GetLength(value);
            if (length != Length)
                return CreateErrorObject(fMessage);
            return null;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "[{0}]的长度必须是{1}的约束", Field.DisplayName, Length);
        }
    }
}