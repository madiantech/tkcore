using YJC.Toolkit.Properties;

namespace YJC.Toolkit.Sys
{
    public class ArgumentNullException : ArgumentException
    {
        protected ArgumentNullException()
        {
        }

        public ArgumentNullException(string argument, object errorObject)
            : base(argument, string.Format(ObjectUtil.SysCulture,
            TkCore.ArgumentNull, argument), errorObject)
        {
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Argument))
                return base.ToString();
            else
                return string.Format(ObjectUtil.SysCulture, "参数{0}为NULL的例外", Argument);
        }
    }
}