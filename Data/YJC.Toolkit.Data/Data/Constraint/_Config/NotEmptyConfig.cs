using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Author = "YJC", Description = "不能为空的校验")]
    [ObjectContext]
    internal sealed class NotEmptyConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new NotEmptyConstraint(ConstraintUtil.GetFieldInfo(args))
            {
                ForceCheck = ForceCheck,
                AutoTrim = AutoTrim
            };
        }

        #endregion

        [SimpleAttribute]
        public bool ForceCheck { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool AutoTrim { get; private set; }
    }
}
