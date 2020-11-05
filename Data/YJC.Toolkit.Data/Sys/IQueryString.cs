using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public interface IQueryString : IEnumerable<KeyValuePair<string, string>>
    {
        string this[string key] { get; }

        IEnumerable<string> AllKeys { get; }

        int Count { get; }
    }
}