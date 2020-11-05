using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public class StringMinLengthConstraint : BaseConstraint
    {
        private readonly string fMessage;

        public StringMinLengthConstraint(IFieldInfo field, int minLength)
            : base(field)
        {
            TkDebug.AssertArgument(minLength > 0, "minLength", "最小长度至少是1", null);
            MinLength = minLength;
            fMessage = string.Format(ObjectUtil.SysCulture, "{0}长度至少为{1}",
                field.DisplayName, minLength);
        }

        public int MinLength { get; private set; }

        public bool TrimValue { get; set; }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return CreateErrorObject(fMessage);
            if (TrimValue)
                value = value.Trim();

            int length = StringLengthConstraint.GetLength(value);
            if (MinLength > length)
                return CreateErrorObject(fMessage);

            return null;
        }
    }
}