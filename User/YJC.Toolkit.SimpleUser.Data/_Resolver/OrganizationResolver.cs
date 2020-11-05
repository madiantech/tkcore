using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [Resolver(Author = "YJC", CreateDate = "2014-09-02",
        Description = "组织机构表(SYS_ORGANIZATION)的数据访问对象")]
    internal class OrganizationResolver : Tk5TreeTableResolver
    {
        public const string DATAXML = "UserManager/Organization.xml";

        public OrganizationResolver(IDbDataSource source)
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
                    e.Row["CreateId"] = e.Row["UpdateId"] = BaseGlobalVariable.UserId;
                    e.Row["CreateDate"] = e.Row["UpdateDate"] = DateTime.Now;
                    e.Row["Active"] = 1;
                    break;
                case UpdateKind.Update:
                    e.Row["UpdateId"] = BaseGlobalVariable.UserId;
                    e.Row["UpdateDate"] = DateTime.Now;
                    break;
                default:
                    break;
            }
        }
    }
}
