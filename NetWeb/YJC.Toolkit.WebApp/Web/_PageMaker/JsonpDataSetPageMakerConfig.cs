using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将DataSet或者类DataSet的Xml输出为Jsonp，允许跨域访问",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-14")]
    class JsonpDataSetPageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new JsonpDataSetPageMaker();
        }

        #endregion
    }
}
