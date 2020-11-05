using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    internal class UserListParam : ListParam
    {
        public new const int MAX = 100;

        public UserListParam()
        {
        }

        public UserListParam(string userId, int offset, int size)
        {
            TkDebug.AssertArgument(size > 0 && size <= MAX, "size",
                   string.Format(ObjectUtil.SysCulture, "size参数必须在0到100之间，当前值是{0}", size), null);

            this.UserId = userId;
            this.Offset = offset;
            this.Size = size;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }
    }
}