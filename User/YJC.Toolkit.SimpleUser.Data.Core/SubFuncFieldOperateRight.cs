using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal class SubFuncFieldOperateRight : FieldOperateRight
    {
        public SubFuncFieldOperateRight(string fieldName, string functionKey)
                 : base(fieldName)
        {
            FunctionKey = functionKey;
        }

        public string FunctionKey { get; }

        public override IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            var result = base.GetOperator(e);
            if (result == null)
                return null;
            if (WebGlobalVariable.SessionGbl.AppRight.FunctionRight is UserFunctionRight functionRight)
            {
                IEnumerable<string> subFunctions = functionRight.GetSubFunctions(FunctionKey);
                var nextResult = result.Intersect(subFunctions);
                return nextResult;
            }
            else
                return result;
        }
    }
}