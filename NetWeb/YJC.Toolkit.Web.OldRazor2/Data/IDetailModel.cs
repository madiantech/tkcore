using System.Collections.Generic;
using YJC.Toolkit.Right;

namespace YJC.Toolkit.Data
{
    public interface IDetailModel : IEditModel
    {
        IEnumerable<Operator> DetailOperators { get; }

        string Source { get; }
    }
}