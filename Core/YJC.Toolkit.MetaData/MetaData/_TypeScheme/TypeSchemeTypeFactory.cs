using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal class TypeSchemeTypeFactory : BaseTypeFactory
    {
        public const string REG_NAME = "_tk_MetaScheme";
        private const string DESCRIPTION = "将数据类型标记为元数据类型的类型工厂";

        public TypeSchemeTypeFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
