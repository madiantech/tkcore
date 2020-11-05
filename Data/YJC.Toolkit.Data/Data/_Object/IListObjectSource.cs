using System;
using System.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IListObjectSource
    {
        Tuple<ListSortInfo, CountInfo, object> CreatePageInfo(IInputData input,
            int pageNumber, int pageSize);

        IEnumerable GetList(object queryContext, IInputData input, int start, int pageSize);
    }
}
