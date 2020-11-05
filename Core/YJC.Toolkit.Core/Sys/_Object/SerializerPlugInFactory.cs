namespace YJC.Toolkit.Sys
{
    public sealed class SerializerPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_Serializer";
        private const string DESCRIPTION = "对象和指定格式之间的转换器的插件工厂";

        public SerializerPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
