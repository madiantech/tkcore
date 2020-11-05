using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class PageDataEventArgs : EventArgs
    {
        public PageDataEventArgs(ISource source, IInputData input, OutputData outputData, object pageData)
        {
            PageData = pageData;
            Source = source;
            Input = input;
            OutputData = outputData;
        }

        public object PageData { get; private set; }

        public ISource Source { get; private set; }

        public IInputData Input { get; private set; }

        public OutputData OutputData { get; private set; }
    }
}