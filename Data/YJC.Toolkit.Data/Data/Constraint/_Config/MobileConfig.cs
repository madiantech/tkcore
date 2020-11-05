using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15", Author = "YJC",
        Description = "移动电话号码约束，在Default.xml中的tk:Simple中配置MobileConfig可以重载默认的校验")]
    internal sealed class MobileConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new MobileConstraint(ConstraintUtil.GetFieldInfo(args));
        }

        #endregion IConfigCreator<BaseConstraint> 成员
    }
}