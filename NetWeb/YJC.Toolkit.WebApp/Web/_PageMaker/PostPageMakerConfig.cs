using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-12-28",
        Description = "专门处理数据提交的PageMaker。当结果数据为KeyData时，根据配置得到返回地址。如果是错误信息直接输出。")]
    internal class PostPageMakerConfig : BasePostPageMakerConfig, IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            PostPageMaker pageMaker = new PostPageMaker(DataType, DestUrl, CustomUrl)
            {
                UseRetUrlFirst = UseRetUrlFirst
            };
            if (AlertMessage != null)
                pageMaker.AlertMessage = AlertMessage.ToString();
            return pageMaker;
        }

        #endregion IConfigCreator<IPageMaker> 成员
    }
}