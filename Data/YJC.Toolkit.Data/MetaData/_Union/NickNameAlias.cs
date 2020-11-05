using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class NickNameAlias : IRegName
    {
        [SimpleAttribute(Required = true)]
        public string SrcNickName { get; set; }

        [SimpleAttribute(Required = true)]
        public string NewNickName { get; set; }

        public string RegName
        {
            get
            {
                return SrcNickName;
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{0}转换为{1}", NewNickName, SrcNickName);
        }
    }
}