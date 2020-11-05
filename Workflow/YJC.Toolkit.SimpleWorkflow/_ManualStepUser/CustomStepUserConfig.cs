using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Custom", Author = "YJC",
       CreateDate = "2018-04-21", Description = "步骤用户是由自定义表达式计算得到的")]
    internal class CustomStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new CustomStepUser(Expression)
            {
                Format = Format
            };
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [SimpleAttribute(DefaultValue = CustomFormat.SingleUser)]
        public CustomFormat Format { get; internal set; }

        [SimpleAttribute(Required = true)]
        public string Expression { get; internal set; }
    }
}