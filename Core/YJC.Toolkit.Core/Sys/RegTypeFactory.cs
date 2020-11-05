namespace YJC.Toolkit.Sys
{
    internal class RegTypeFactory : BaseTypeFactory
    {
        public const string REG_NAME = "_tk_RegType";
        private const string DESCRIPTION = "收集注册类型(无需实例化类)的类型工厂";

        public RegTypeFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
