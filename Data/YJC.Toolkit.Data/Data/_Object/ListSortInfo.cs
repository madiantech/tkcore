using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ListSortInfo
    {
        public ListSortInfo(IInputData input)
        {
        }

        //[SimpleAttribute]
        //public int SortField { get; internal set; }

        //[SimpleAttribute]
        //public string Order { get; internal set; }

        [SimpleAttribute]
        public string SqlCon { get; internal set; }

        [SimpleAttribute]
        public string JsonOrder { get; internal set; }

        //[SimpleAttribute]
        //public int Tab { get; set; }
    }
}