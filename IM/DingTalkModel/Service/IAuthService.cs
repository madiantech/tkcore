using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model.Company;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IAuthService
    {
        //获取通讯录权限范围 接口说明*
        [ApiMethod("/auth/scopes")]
        AuthResult GetAuthScorps();
    }
}