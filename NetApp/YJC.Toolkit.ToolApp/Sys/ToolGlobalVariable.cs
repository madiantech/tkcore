namespace YJC.Toolkit.Sys
{
    internal sealed class ToolGlobalVariable : BaseGlobalVariable
    {
        private IUserInfo fUserInfo;

        public ToolGlobalVariable()
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