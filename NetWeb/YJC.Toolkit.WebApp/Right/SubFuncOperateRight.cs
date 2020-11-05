using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class SubFuncOperateRight : IOperateRight
    {
        /// <summary>
        /// Initializes a new instance of the SubFuncOperateRight class.
        /// </summary>
        /// <param name="functionKey"></param>
        public SubFuncOperateRight(string functionKey)
        {
            TkDebug.AssertArgumentNullOrEmpty(functionKey, "functionKey", null);

            FunctionKey = functionKey;
        }

        #region IOperateRight 成员

        public IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            IFunctionRight funcRight = WebGlobalVariable.SessionGbl.AppRight.FunctionRight;
            TkDebug.AssertNotNull(funcRight, "系统没有配置功能权限", this);

            return funcRight.GetSubFunctions(FunctionKey);
        }

        #endregion

        public string FunctionKey { get; private set; }
    }
}
