using System;

namespace YJC.Toolkit.Sys
{
    public interface IPageData : IInputData
    {
        string Title { get; }

        Uri PageUrl { get; }

        string PageExtension { get; }

        bool IsMobileDevice { get; }
    }
}