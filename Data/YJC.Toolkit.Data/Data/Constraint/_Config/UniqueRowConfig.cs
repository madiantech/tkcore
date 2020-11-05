using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-07-21", Author = "YJC",
        Description = "该字段在提交的记录中都是唯一的校验")]
    internal sealed class UniqueRowConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new UniqueRowConstraint(ConstraintUtil.GetFieldInfo(args));
        }

        #endregion
    }
}
