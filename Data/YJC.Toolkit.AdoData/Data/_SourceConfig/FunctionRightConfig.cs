using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class FunctionRightConfig
    {
        [SimpleAttribute]
        public string FunctionKey { get; private set; }

        [SimpleAttribute(Required = true)]
        public FunctionRightType FunctionType { get; private set; }
    }
}