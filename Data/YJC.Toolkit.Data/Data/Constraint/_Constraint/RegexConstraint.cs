using System.Text.RegularExpressions;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public class RegexConstraint : BaseConstraint
    {
        public RegexConstraint(IFieldInfo field, string msg, string pattern)
            : this(field, msg, pattern, RegexOptions.None)
        {
        }

        public RegexConstraint(IFieldInfo field, string msg, string pattern, RegexOptions options)
            : base(field)
        {
            Regex = new Regex(pattern, options);
            Message = msg;
        }

        public string Message { get; private set; }

        public Regex Regex { get; private set; }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (!Regex.IsMatch(value))
                return CreateErrorObject(Message);
            return null;
        }
    }
}
