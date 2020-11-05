using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn, AlwaysCache]
    [Expression(SqlInject = false, Author = "YJC", CreateDate = "2015-09-29", Description = "获取当前Url的Source")]
    internal sealed class SourceExpression : IExpression
    {
        public static IExpression Instance = new SourceExpression();

        private SourceExpression()
        {
        }

        #region IExpression 成员

        public string Execute()
        {
            try
            {
                var info = WebUtil.CreateSourceInfo(WebGlobalVariable.Request);
                return info.Source;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
