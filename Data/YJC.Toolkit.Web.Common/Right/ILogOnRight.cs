using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public interface ILogOnRight
    {
        void CheckLogOn(object userId, Uri url);

        void Initialize(IUserInfo user);
    }
}
