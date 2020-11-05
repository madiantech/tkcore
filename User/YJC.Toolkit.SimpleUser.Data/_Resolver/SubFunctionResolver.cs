using YJC.Toolkit.Data;
using YJC.Toolkit.Data.Constraint;

namespace YJC.Toolkit.SimpleRight
{
    /// <summary>
    /// SYS_SUB_FUNC 表的数据访问对象
    /// </summary>
    [Resolver(Author = "YJC", CreateDate = "2014-07-10",
        Description = "附属功能表(SYS_SUB_FUNC)的数据访问对象")]
    internal class SubFunctionResolver : Tk5TableResolver
    {
        public const string DATAXML = "UserManager/SubFunc.xml";

        public SubFunctionResolver(IDbDataSource dataSource)
            : base(DATAXML, dataSource)
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
                    break;
                default:
                    break;
            }
        }

        protected override void SetConstraints(MetaData.IPageStyle style)
        {
            base.SetConstraints(style);

            Constraints.Add(new UniqueRowConstraint(GetFieldInfo("NameId")));
        }
    }
}
