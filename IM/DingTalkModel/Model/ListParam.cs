using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ListParam
    {
        public const int MAX = 200;
        public const int DEFAULT = 20;
        public const int ATTENDANCE = 10;

        internal ListParam()
        {
            Size = DEFAULT;
        }

        public ListParam(int size, int offset)
        {
            TkDebug.AssertArgument(offset >= 0, "offset",
                string.Format(ObjectUtil.SysCulture, "offset参数必须>0，当前值为{0}", offset), null);
            TkDebug.AssertArgument(size > 0 && size <= MAX, "size",
                string.Format(ObjectUtil.SysCulture, "size参数必须在0到200之间，当前值是{0}", size), null);

            Offset = offset;
            Size = size;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Size { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Offset { get; set; }
    }
}