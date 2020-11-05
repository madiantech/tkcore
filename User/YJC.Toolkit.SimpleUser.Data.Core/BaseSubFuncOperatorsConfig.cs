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

        #region IOperatorsConfig ��Ա

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

        #endregion IOperatorsConfig ��Ա

        #region IConfigCreator<IOperatorsConfig> ��Ա

        IOperatorsConfig IConfigCreator<IOperatorsConfig>.CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IOperatorsConfig> ��Ա

        #region IConfigCreator<IOperateRight> ��Ա

        public abstract IOperateRight CreateObject(params object[] args);

        #endregion IConfigCreator<IOperateRight> ��Ա

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