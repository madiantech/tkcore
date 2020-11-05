using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2020-06-25",
        Description = "给不带有FunctionRight定义的Source外接FunctionRight的Source")]
    internal class FunctionRightSourceConfig : IConfigCreator<ISource>
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public FunctionRightConfig FunctionRight { get; private set; }

        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(SourceConfigFactory.REG_NAME)]
        public IConfigCreator<ISource> Source { get; private set; }

        public ISource CreateObject(params object[] args)
        {
            ISource source = Source.CreateObject(args);
            return new FunctionRightSource(source, FunctionRight);
        }
    }
}