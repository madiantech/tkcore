namespace YJC.Toolkit.Sys
{
    internal sealed class TestGlobalVariable : BaseGlobalVariable
    {
        private IUserInfo fUserInfo;

        public TestGlobalVariable()
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