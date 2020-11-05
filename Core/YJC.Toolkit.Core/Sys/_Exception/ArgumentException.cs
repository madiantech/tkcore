namespace YJC.Toolkit.Sys
{
    public class ArgumentException : AssertException
    {
        protected ArgumentException()
        {
        }

        public ArgumentException(string argument, string message, object errorObject)
            : base(message, errorObject)
        {
            TkDebug.AssertArgumentNullOrEmpty(argument, "argument", null);

            Argument = argument;
        }

        public string Argument { get; set; }

        public override void FillExceptionInfo(ExceptionInfo info)
        {
            base.FillExceptionInfo(info);

            info.Argument = Argument;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Argument))
                return base.ToString();
            else
                return string.Format(ObjectUtil.SysCulture, "参数{0}的例外", Argument);
        }
    }
}
