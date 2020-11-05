using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class CountInfo
    {
        public CountInfo(int totalCount, int currentPage, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 1;
            TotalCount = totalCount;
            PageSize = pageSize;
            int page = totalCount / pageSize;
            if (page > 0 && (totalCount % pageSize) == 0)
                --page;
            TotalPage = page;
            CurrentPage = TotalPage > currentPage ? currentPage : TotalPage;
        }

        internal CountInfo(int totalCount, int totalPage, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            TotalPage = totalPage;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        [SimpleAttribute]
        public int TotalCount { get; private set; }

        [SimpleAttribute]
        public int TotalPage { get; private set; }

        [SimpleAttribute]
        public int CurrentPage { get; private set; }

        [SimpleAttribute]
        public int PageSize { get; private set; }
    }
}