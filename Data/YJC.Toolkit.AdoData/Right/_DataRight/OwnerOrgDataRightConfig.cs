using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [DataRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-28", Description = "检查记录是否是本组织的数据权限")]
    internal class OwnerOrgDataRightConfig : BaseFieldRightConfig
    {
        protected override IDataRight CreateDataRight(IFieldInfo fieldInfo)
        {
            return new OwnerOrgDataRight(fieldInfo);
        }
    }
}
