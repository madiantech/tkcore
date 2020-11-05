using System;
using YJC.Toolkit.Right;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Sys
{
    [Serializable]
    public sealed class SessionGlobal
    {
        private IRWLock fRWLock;

        public SessionGlobal()
        {
            Info = new SimpleUserInfo();
            TempIndentity = Guid.NewGuid();
            AppRight = new WebAppRight();
        }

        public string SessionId { get; set; }

        public IUserInfo Info { get; internal set; }

        public Guid TempIndentity { get; private set; }

        public WebAppRight AppRight { get; private set; }

        public IRWLock RWLock
        {
            get
            {
                if (fRWLock == null)
                    fRWLock = BaseGlobalVariable.Current.CreateRWLock();
                return fRWLock;
            }
        }

        public void SetGuid()
        {
            TempIndentity = Guid.NewGuid();
        }
    }
}
