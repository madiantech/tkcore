using System;

namespace YJC.Toolkit.IM
{
    public interface IIMPlatform
    {
        Uri BaseUri { get; }

        string QueryStringName { get; }

        string AccessToken { get; }
    }
}