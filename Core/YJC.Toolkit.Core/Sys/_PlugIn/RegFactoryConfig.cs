namespace YJC.Toolkit.Sys
{
    public abstract class RegFactoryConfig<T> : IConfigCreator<T> where T : class
    {
        private readonly string fFactoryName;

        protected RegFactoryConfig(string factoryName)
        {
            fFactoryName = factoryName;
        }

        #region IConfigCreator<T> 成员

        public T CreateObject(params object[] args)
        {
            return PlugInFactoryManager.CreateInstance<T>(fFactoryName, Content);
        }

        #endregion

        [TextContent(Required = true)]
        public string Content { get; protected set; }
    }
}
