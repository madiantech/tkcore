using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal class SubFuncOperateRight : IOperateRight
    {
        public SubFuncOperateRight(string functionKey)
        {
            FunctionKey = functionKey;
        }

        public IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            if (WebGlobalVariable.SessionGbl.AppRight.FunctionRight is UserFunctionRight functionRight)
            {
                return functionRight.GetSubFunctions(FunctionKey);
            }
            else
                return Enumerable.Empty<string>();
        }

        public string FunctionKey { get; }
    }
}