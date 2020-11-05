using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Author = "YJC", Description = "格式必须是邮政编码的校验")]
    internal sealed class PostCodeConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new PostCodeConstraint(ConstraintUtil.GetFieldInfo(args));
        }

        #endregion
    }
}
