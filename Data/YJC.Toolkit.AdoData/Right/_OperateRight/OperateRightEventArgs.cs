using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class OperateRightEventArgs : EventArgs
    {
        public OperateRightEventArgs(IPageStyle style, string source, DataRow row)
        {
            TkDebug.AssertArgumentNull(style, nameof(style), null);
            TkDebug.AssertArgumentNullOrEmpty(source, nameof(source), null);

            Row = row;
            Source = source;
            Style = style;
        }

        public DataRow Row { get; }

        public IPageStyle Style { get; }

        public string Source { get; }
    }
}