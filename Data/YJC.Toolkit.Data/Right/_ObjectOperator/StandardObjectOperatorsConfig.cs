using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-11-13", Description = "标准对象操作符配置")]
    internal sealed class StandardObjectOperatorsConfig : IObjectOperatorsConfig,
        IConfigCreator<IObjectOperatorsConfig>
    {
        private IEnumerable<OperatorConfig> fOperators;

        #region IConfigCreator<IObjectOperatorsConfig> 成员

        public IObjectOperatorsConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        #region IObjectOperatorsConfig 成员

        [DynamicElement(ObjectOperateRightConfigFactory.REG_NAME)]
        public IConfigCreator<IObjectOperateRight> Right { get; private set; }

        IEnumerable<OperatorConfig> IObjectOperatorsConfig.Operators
        {
            get
            {
                if (fOperators == null)
                    fOperators = (from item in Operators
                                  select item.CreateObject()).ToArray();
                return fOperators;
            }
        }

        #endregion

        [DynamicElement(OperatorConfigFactory.REG_NAME, IsMultiple = true)]
        internal List<IConfigCreator<OperatorConfig>> Operators { get; private set; }
    }
}
