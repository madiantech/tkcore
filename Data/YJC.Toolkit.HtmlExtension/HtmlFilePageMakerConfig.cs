using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.HtmlExtension
{
    [ObjectContext]
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-23",
        Description = "自动替换html文件中的script和link中的相对路径，并将结果输出。比较适合用webpack打包好的html文件")]
    internal class HtmlFilePageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new HtmlFilePageMaker(VirtualPath, FileName)
            {
                UseCache = UseCache,
                Option = Option
            };
        }

        #endregion IConfigCreator<IPageMaker> 成员

        [SimpleAttribute]
        public string VirtualPath { get; private set; }

        [SimpleAttribute(Required = true)]
        public string FileName { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseCache { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public HtmlOption Option { get; private set; }
    }
}