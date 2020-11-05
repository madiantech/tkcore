using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class SubFuncFieldOperateRight : FieldOperateRight
    {
        public SubFuncFieldOperateRight(string fieldName, string functionKey)
            : base(fieldName)
        {
            TkDebug.AssertArgumentNullOrEmpty(functionKey, "functionKey", null);

            FunctionKey = functionKey;
        }

        public string FunctionKey { get; private set; }

        public override IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            var result = base.GetOperator(e);
            if (result == null)
                return null;
            IFunctionRight funcRight = WebGlobalVariable.SessionGbl.AppRight.FunctionRight;
            TkDebug.AssertNotNull(funcRight, "系统没有配置功能权限", this);
            var nextResult = from item in result
                             where funcRight.IsSubFunction(item, FunctionKey)
                             select item;
            return nextResult;
        }
    }
}
