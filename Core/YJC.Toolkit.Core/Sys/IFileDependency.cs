using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public interface IFileDependency
    {
        IEnumerable<string> Files { get; }
    }
}