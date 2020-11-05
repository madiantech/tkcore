using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "结合MetaData和输出的DataSet结果集，构造输出的Json数据内容，适合Vue等框架绑定数据源",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-16")]
    internal class JsonMetaDataSetPageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new JsonMetaDataSetPageMaker();
        }

        #endregion IConfigCreator<IPageMaker> 成员
    }
}