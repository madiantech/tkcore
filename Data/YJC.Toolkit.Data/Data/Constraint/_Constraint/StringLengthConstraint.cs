using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class StringLengthConstraint : BaseConstraint
    {
        private readonly string fMessage;

        public StringLengthConstraint(IFieldInfo field, int length)
            : base(field)
        {
            TkDebug.AssertArgument(length > 0, "length",
                string.Format(ObjectUtil.SysCulture, "length参数的值必须大于0，现在值为{0}",
                length), null);

            Length = length;
            fMessage = string.Format(ObjectUtil.SysCulture, TkWebApp.StringLengthCMsg,
                Field.DisplayName, length);
        }

        public int Length { get; private set; }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            int length = GetLength(value);
            if (length > Length)
                return CreateErrorObject(fMessage);
            return null;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}]的长度不能超过{1}的约束",
                Field.DisplayName, Length);
        }

        internal static int GetLength(string value)
        {
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding("GB2312");
            }
            catch
            {
                encoding = Encoding.Default;
            }
            return encoding.GetByteCount(value);
        }
    }
}