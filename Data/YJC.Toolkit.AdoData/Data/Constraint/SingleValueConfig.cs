using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-26",
        Author = "YJC", Description = "在数据表中该字段必须唯一的校验")]
    [ObjectContext]
    internal class SingleValueConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        public BaseConstraint CreateObject(params object[] args)
        {
            SingleValueConstraint result = new SingleValueConstraint(ConstraintUtil.GetFieldInfo(args));
            if (Message != null)
                result.Message = Message.ToString();

            return result;
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Message { get; private set; }
    }
}
