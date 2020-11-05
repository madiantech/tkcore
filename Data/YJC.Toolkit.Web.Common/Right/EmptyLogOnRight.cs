using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [Serializable]
    public class EmptyLogOnRight : ILogOnRight
    {
        #region ILogOnRight 成员

        public virtual void CheckLogOn(object userId, Uri url)
        {
        }

        public virtual void Initialize(IUserInfo user)
        {
        }

        #endregion
    }
}
