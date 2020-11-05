using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-03-31",
        Author = "YJC", Description = "在指定字段日期之后的校验")]
    [ObjectContext]
    internal class DateAfterConstraintConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        public BaseConstraint CreateObject(params object[] args)
        {
            IFieldInfo beforeField = ConstraintUtil.GetFieldInfo(BeforeNickName, args);
            return new DateAfterConstraint(ConstraintUtil.GetFieldInfo(args), beforeField);
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string BeforeNickName { get; private set; }
    }
}
