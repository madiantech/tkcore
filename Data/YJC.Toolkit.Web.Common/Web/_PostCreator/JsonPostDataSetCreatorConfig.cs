using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PostObjectConfig(Description = "提交数据为Json格式，转换为DataSet",
       NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-12")]
    internal class JsonPostDataSetCreatorConfig : IConfigCreator<IPostObjectCreator>
    {
        #region IConfigCreator<IPostObjectCreator> 成员

        public IPostObjectCreator CreateObject(params object[] args)
        {
            return JsonPostDataSetObjectCreator.Creator;
        }

        #endregion
    }
}
