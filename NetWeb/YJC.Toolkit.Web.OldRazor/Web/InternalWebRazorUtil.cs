using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class InternalWebRazorUtil
    {
        public static RazorPageMaker CreateDetailListPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            RazorPageMaker pageMaker = BaseCompositeRazorPageMakerConfig.CreateRazorPageMaker(
                "NormalDetailList", config, pageData);
            if (pageMaker.RazorData == null)
            {
                pageMaker.RazorData = WebRazorUtil.CreateDefaultDetailListData();
            }
            return pageMaker;
        }
    }
}