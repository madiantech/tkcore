using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleRight
{
    /// <summary>
    /// SYS_FUNCTION 表的数据访问对象
    /// </summary>
    [Resolver(Author = "YJC", CreateDate = "2014-07-10",
        Description = "系统功能表(SYS_FUNCTION)的数据访问对象")]
    internal class FunctionResolver : Tk5TreeTableResolver
    {
        public const string DATAXML = "UserManager/Function.xml";

        public FunctionResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            switch (e.Status)
            {
                case UpdateKind.Delete:
                    break;
                case UpdateKind.Insert:
                    e.Row["Id"] = CreateUniId();
                    break;
                case UpdateKind.Update:
                    //if (string.IsNullOrEmpty(e.Row["FN_PARENT_ID"].ToString()))
                    //    e.Row["FN_PARENT_ID"] = RootId;
                    break;
                default:
                    break;
            }
        }
    }
}
