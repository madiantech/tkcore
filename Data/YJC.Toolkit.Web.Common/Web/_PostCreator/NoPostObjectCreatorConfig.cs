using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PostObjectConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2015-08-13",
        Description = "无Post数据，返回null", Author = "YJC")]
    internal class NoPostObjectCreatorConfig : IConfigCreator<IPostObjectCreator>
    {
        #region IConfigCreator<IPostObjectCreator> 成员

        public IPostObjectCreator CreateObject(params object[] args)
        {
            return null;
        }

        #endregion
    }
}
