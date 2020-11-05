namespace YJC.Toolkit.Sys
{
    public sealed class ConsoleGlobalVariable : BaseGlobalVariable
    {
        private IUserInfo fUserInfo;

        internal ConsoleGlobalVariable()
        {
            Current = this;
            fUserInfo = new SimpleUserInfo();
        }

        public override IUserInfo UserInfo
        {
            get
            {
                return fUserInfo;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                fUserInfo = value;
            }
        }
    }
}