using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-11-27", Description = "标准操作符配置")]
    internal sealed class StandardOperatorsConfig : IConfigCreator<IOperatorsConfig>, IOperatorsConfig
    {
        private IEnumerable<OperatorConfig> fOperators;

        #region IConfigCreator<IOperatorsConfig> 成员

        public IOperatorsConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        #region IOperatorsConfig 成员

        [DynamicElement(OperateRightConfigFactory.REG_NAME)]
        public IConfigCreator<IOperateRight> Right { get; set; }

        IEnumerable<OperatorConfig> IOperatorsConfig.Operators
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

        //[ObjectElement(NamespaceType.Toolkit, IsMultiple = true,
        //    LocalName = "Operator", UseConstructor = true)]
        [DynamicElement(OperatorConfigFactory.REG_NAME, IsMultiple = true)]
        internal List<IConfigCreator<OperatorConfig>> Operators { get; set; }
    }
}
