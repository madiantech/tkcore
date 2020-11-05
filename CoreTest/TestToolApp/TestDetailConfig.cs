using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace RazorTest
{
    internal class TestDetailConfig : IDetailDbConfig, ISingleResolverConfig
    {
        public TestDetailConfig(IConfigCreator<TableResolver> resolver)
        {
            Resolver = resolver;
        }

        public IConfigCreator<TableResolver> Resolver { get; }

        public string Context => null;

        public bool SupportData => false;

        public IConfigCreator<IDataRight> DataRight => null;

        public IConfigCreator<IOperatorsConfig> DetailOperators => null;

        public bool UseMetaData => false;

        public FunctionRightConfig FunctionRight { get; }
    }
}