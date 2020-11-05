using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    abstract class BaseObjectSourceConfig<T> : IConfigCreator<ISource> where T : class
    {
        protected BaseObjectSourceConfig()
        {
        }

        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            var source = ObjectSource.CreateObjectSource().Convert<T>();
            BaseObjectSource<T> result = CreateSource(source);
            result.ObjectName = ObjectName;
            result.UseMetaData = UseMetaData;
            return result;
        }

        #endregion

        [SimpleAttribute]
        public bool UseMetaData { get; protected set; }

        [SimpleAttribute]
        public string ObjectName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ObjectSourceConfig ObjectSource { get; protected set; }

        protected abstract BaseObjectSource<T> CreateSource(T source);
    }
}
