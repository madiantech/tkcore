using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.WeCorp.Model.UserIdentity;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IIdentityVerificationService
    {
        //获取访问用户身份
        [ApiMethod("/user/getuserinfo")]
        UserIdentityResult GetUserInfo([ApiParameter()]string code);
    }
}