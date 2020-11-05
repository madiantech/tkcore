using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-08-05", Description = "默认查看页面的操作符配置")]
    class SimpleDetailOperatorsConfig : IConfigCreator<IOperatorsConfig>,
        IOperatorsConfig, IReadObjectCallBack
    {
        private readonly IConfigCreator<IOperateRight> fConfig;
        private OperatorConfig[] fOperators;

        public SimpleDetailOperatorsConfig()
        {
            fConfig = new EmptyOperateRightConfig();
            fOperators = new OperatorConfig[] { OperatorConfig.UpdateOperator };
        }

        #region IOperatorsConfig 成员

        public IConfigCreator<IOperateRight> Right
        {
            get
            {
                return fConfig;
            }
        }

        public IEnumerable<OperatorConfig> Operators
        {
            get
            {
                return fOperators;
            }
        }

        #endregion

        #region IConfigCreator<IOperatorsConfig> 成员

        public IOperatorsConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (IsDialog)
                fOperators = new OperatorConfig[] { OperatorConfig.UpdateDialogOperator };
        }

        #endregion

        [SimpleAttribute]
        public bool IsDialog { get; set; }

    }
}
