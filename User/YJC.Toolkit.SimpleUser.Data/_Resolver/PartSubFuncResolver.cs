using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [Resolver(Author = "YJC", CreateDate = "2015-07-30",
        Description = "角色子功能表(SYS_PART_SUB_FUNC)的数据访问对象")]
    internal class PartSubFuncResolver : Tk5TableResolver
    {
        private const string DATAXML = "UserManager/PartSubFunc.xml";

        public PartSubFuncResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}
