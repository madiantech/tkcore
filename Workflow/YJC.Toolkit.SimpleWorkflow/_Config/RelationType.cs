using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "相对关系类型代码表")]
    public enum RelationType
    {
        [DisplayName("上级组织")]
        ParentOrg = 0,			// 上级组织

        [DisplayName("上级部门")]
        ParentDept = 1,			// 上级部门

        [DisplayName("上级岗位")]
        ParentPost = 2,         // 上级岗位

        [DisplayName("上上级组织")]
        GrandParentOrg = 3,		// 上上级组织

        [DisplayName("上上级部门")]
        GrandParentDept = 4,	// 上上级部门

        [DisplayName("上上级岗位")]
        GrandParentPost = 5,    // 上上级岗位

        [DisplayName("本级组织")]
        SameOrg = 6,			// 本级组织

        [DisplayName("本级部门")]
        SameDept = 7,			// 本级部门

        [DisplayName("本级岗位")]
        SamePost = 8,           // 本级岗位

        [DisplayName("本人")]
        SamePerson = 9			// 本人
    }
}