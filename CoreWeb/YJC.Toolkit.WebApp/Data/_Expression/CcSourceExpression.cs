﻿using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2015-09-29",
        Description = "获取当前Url的CcSource")]
    internal sealed class CcSourceExpression : IExpression
    {
        public static IExpression Instance = new CcSourceExpression();

        private CcSourceExpression()
        {
        }

        #region IExpression 成员

        public string Execute()
        {
            try
            {
                var info = WebUtil.CreateSourceInfo(WebGlobalVariable.Context);
                return info?.Source;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion IExpression 成员
    }
}