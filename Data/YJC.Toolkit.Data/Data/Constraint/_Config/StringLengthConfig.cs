using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Description = "字符串长度不大于指定长度的校验", Author = "YJC")]
    [ObjectContext]
    internal sealed class StringLengthConfig : IConfigCreator<BaseConstraint>
    {
        [SimpleAttribute(Required = true)]
        public int Length { get; private set; }

        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new StringLengthConstraint(ConstraintUtil.GetFieldInfo(args), Length);
        }

        #endregion
    }
}
