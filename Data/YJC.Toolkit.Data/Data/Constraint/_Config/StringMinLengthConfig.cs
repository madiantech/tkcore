using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-22", Author = "YJC",
        Description = "输入字符串必须有一定长度的校验")]
    [ObjectContext]
    class StringMinLengthConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        public BaseConstraint CreateObject(params object[] args)
        {
            return
                new StringMinLengthConstraint(ConstraintUtil.GetFieldInfo(args), MinLength)
                {
                    TrimValue = TrimValue
                };
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public int MinLength { get; private set; }

        [SimpleAttribute]
        public bool TrimValue { get; private set; }
    }
}
