using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [DataRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2015-08-04", Description = "提供该用户所有下属组织机构的数据权限")]
    internal class CompanyOrgDataRightConfig : BaseFieldRightConfig
    {
        protected override IDataRight CreateDataRight(IFieldInfo fieldInfo)
        {
            return new CompanyOrgDataRight(fieldInfo);
        }
    }
}
