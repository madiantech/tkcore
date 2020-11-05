using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-11-13", Description = "默认查看页面的对象操作符配置")]
    class SimpleDetailObjectOperatorsConfig : IConfigCreator<IObjectOperatorsConfig>,
        IObjectOperatorsConfig, IReadObjectCallBack
    {
        private readonly IConfigCreator<IObjectOperateRight> fConfig;
        private OperatorConfig[] fOperators;

        public SimpleDetailObjectOperatorsConfig()
        {
            fConfig = new EmptyObjectOperateRightConfig();
            fOperators = new OperatorConfig[] { OperatorConfig.UpdateOperator };
        }

        #region IObjectOperatorsConfig 成员

        public IConfigCreator<IObjectOperateRight> Right
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

        #region IConfigCreator<IObjectOperatorsConfig> 成员

        public IObjectOperatorsConfig CreateObject(params object[] args)
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
