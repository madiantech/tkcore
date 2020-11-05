using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class ObjectOperateRightEventArgs : EventArgs
    {
        public ObjectOperateRightEventArgs(IPageStyle style, object mainObj)
        {
            TkDebug.AssertArgumentNull(style, "style", null);

            MainObj = mainObj;
            Style = style;
        }

        public object MainObj { get; private set; }

        public IPageStyle Style { get; private set; }
    }
}
