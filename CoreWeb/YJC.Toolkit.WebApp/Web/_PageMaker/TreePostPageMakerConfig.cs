using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "针对Tree结构，类似于PostPageMaker，但返回地址针对Tree结构进行了优化",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-09-21")]
    internal class TreePostPageMakerConfig : BasePostPageMakerConfig, IConfigCreator<IPageMaker>
    {
        public IPageMaker CreateObject(params object[] args)
        {
            TreePostPageMaker pageMaker = new TreePostPageMaker(DataType, DestUrl, CustomUrl)
            {
                UseRetUrlFirst = UseRetUrlFirst
            };
            if (AlertMessage != null)
                pageMaker.AlertMessage = AlertMessage.ToString();
            return pageMaker;
        }
    }
}