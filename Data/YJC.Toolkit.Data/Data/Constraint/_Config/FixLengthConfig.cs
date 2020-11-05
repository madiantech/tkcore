using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Author = "YJC", Description = "长度必须是指定长度的校验")]
    [ObjectContext]
    internal sealed class FixLengthConfig : IConfigCreator<BaseConstraint>
    {
        [SimpleAttribute]
        public int Length { get; private set; }

        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new FixLengthConstraint(ConstraintUtil.GetFieldInfo(args), Length);
        }

        #endregion
    }
}
