using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class PageMakerInfo
    {
        public PageMakerInfo(Func<ISource, IInputData, OutputData, bool> function,
            IPageMaker pageMaker)
        {
            TkDebug.AssertArgumentNull(function, "function", null);
            TkDebug.AssertArgumentNull(pageMaker, "pageMaker", null);

            Function = function;
            PageMaker = pageMaker;
        }

        public Func<ISource, IInputData, OutputData, bool> Function { get; private set; }

        public IPageMaker PageMaker { get; private set; }
    }
}