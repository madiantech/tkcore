using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.UserIdentity
{
    public class UserIdentityResult
    {
        [SimpleElement]
        public string UserId { get; set; }

        [SimpleElement]
        public string DeviceId { get; set; }

        [SimpleElement]
        public string OpenId { get; set; }
    }
}