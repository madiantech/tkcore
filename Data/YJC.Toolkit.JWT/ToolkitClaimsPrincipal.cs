using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ToolkitClaimsPrincipal : ClaimsPrincipal
    {
        public ToolkitClaimsPrincipal(JWTUserInfo info)
        {
            UserInfo = info;
        }

        public JWTUserInfo UserInfo { get; }
    }
}