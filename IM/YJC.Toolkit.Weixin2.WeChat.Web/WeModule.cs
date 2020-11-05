using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Weixin;

namespace YJC.Toolkit.WeChat.Web
{
    [Module(Author = "YJC", CreateDate = "2019-04-24", Description = "微信公众号回调模块")]
    internal class WeModule : IModule
    {
        #region IModule 成员

        public IMetaData CreateMetaData(IPageData pageData)
        {
            return null;
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new WeixinPageMaker();
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            return WeixinPostObjectCreator.Creator;
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            return null; 
        }

        public ISource CreateSource(IPageData pageData)
        {
            return WeChatSource.Source;
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return false;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return false;
        }

        public string Title
        {
            get
            {
                return "WeChat";
            }
        }

        #endregion
    }
}
