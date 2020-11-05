using System.Collections.Specialized;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Sys
{
    public interface IInputData
    {
        IPageStyle Style { get; }

        bool IsPost { get; }

        IQueryString QueryString { get; }

        object PostObject { get; }

        ICallerInfo CallerInfo { get; }

        string QueryStringText { get; }

        PageSourceInfo SourceInfo { get; }
    }
}