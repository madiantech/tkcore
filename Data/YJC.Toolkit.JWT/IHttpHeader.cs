using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Web
{
    public interface IHttpHeader
    {
        string HeaderName { get; }

        bool UseRequestHeader { get; }

        string Token { get; }
    }
}