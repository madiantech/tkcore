using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [InstancePlugIn, AlwaysCache]
    [AppRightBuilder(Author = "YJC", CreateDate = "2019-10-04", Description = "创建空的权限")]
    internal class EmptyAppRightBuilder : IAppRightBuilder
    {
        public static readonly IAppRightBuilder Instance = new EmptyAppRightBuilder();

        private EmptyAppRightBuilder()
        {
        }

        public IFunctionRight CreateFunctionRight()
        {
            return new EmptyFunctionRight();
        }

        public ILogOnRight CreateLogOnRight()
        {
            return new EmptyLogOnRight();
        }

        public IMenuScriptBuilder CreateScriptBuilder()
        {
            return new EmptyMenuScriptBuilder();
        }
    }
}