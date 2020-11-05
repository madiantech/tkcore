using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "处理人类型代码表")]
    public enum ActorType  	// 处理人类型
    {
        [DisplayName("流程创建者")]
        Creator = 0,			// 流程创建者

        [DisplayName("个人")]
        SingleUser = 1,			// 个人

        [DisplayName("指定组织和部门")]
        Organization = 2,		// 指定组织和部门

        [DisplayName("指定组织和角色")]
        OrgRole = 3,			// 指定组织和角色

        [DisplayName("指定组织和岗位")]
        OrgPost = 4,            // 指定组织和岗位

        [DisplayName("相对步骤")]
        Relative = 5,			// 相对步骤

        [DisplayName("指定角色，组织待定")]
        RoleCustomOrg = 6,   	// 指定角色，组织待定

        [DisplayName("指定岗位，组织待定")]
        PostCustomOrg = 7,      // 指定岗位，组织待定

        [DisplayName("全自定义")]
        Custom = 8 				// 全自定义
    }
}