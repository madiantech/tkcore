using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class DataRightEventArgs : ListDataRightEventArgs
    {
        public DataRightEventArgs(TkDbContext context, IUserInfo user,
            IFieldInfoIndexer fieldIndexer, IPageStyle style, DataRow row)
            : base(context, user, fieldIndexer)
        {
            Style = style;
            Row = row;
        }

        public IPageStyle Style { get; private set; }

        public DataRow Row { get; private set; }
    }
}
