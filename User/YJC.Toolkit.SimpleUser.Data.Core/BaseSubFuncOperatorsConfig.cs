using System.Collections.Generic;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal abstract class BaseSubFuncOperatorsConfig : IConfigCreator<IOperatorsConfig>,
        IOperatorsConfig, IConfigCreator<IOperateRight>
    {
        private IEnumerable<OperatorConfig> fOperators;

        protected BaseSubFuncOperatorsConfig()
        {
        }

        #region IOperatorsConfig 成员

        public IEnumerable<OperatorConfig> Operators
        {
            get
            {
                if (fOperators == null)
                    fOperators = SafeGetSubOperators(Page, FunctionKey);

                return fOperators;
            }
        }

        public IConfigCreator<IOperateRight> Right
        {
            get
            {
                return this;
            }
        }

        #endregion IOperatorsConfig 成员

        #region IConfigCreator<IOperatorsConfig> 成员

        IOperatorsConfig IConfigCreator<IOperatorsConfig>.CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IOperatorsConfig> 成员

        #region IConfigCreator<IOperateRight> 成员

        public abstract IOperateRight CreateObject(params object[] args);

        #endregion IConfigCreator<IOperateRight> 成员

        [SimpleAttribute]
        public string FunctionKey { get; protected set; }

        [SimpleAttribute]
        public OperatorPage Page { get; protected set; }

        public static IEnumerable<OperatorConfig> SafeGetSubOperators(OperatorPage page, string functionKey)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            UserFunctionRight functionRight = WebGlobalVariable.SessionGbl.AppRight.FunctionRight
                as UserFunctionRight;
            if (functionRight == null)
                return null;

            return functionRight.GetSubOperators(page, functionKey);
        }
    }
}