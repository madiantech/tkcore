using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.HtmlExtension
{
    [ModuleCreator(Author = "YJC", CreateDate = "2018-05-06", RegName = "h",
        Description = "自动创建在html目录下html文件的Module包装")]
    [AlwaysCache, InstancePlugIn]
    internal class HtmlModuleCreator : IModuleCreator
    {
        public static readonly IModuleCreator Instance = new HtmlModuleCreator();

        private HtmlModuleCreator()
        {
        }

        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            return new HtmlModule(source);
        }

        #endregion IModuleCreator 成员
    }
}