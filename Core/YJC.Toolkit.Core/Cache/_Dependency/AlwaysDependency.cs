namespace YJC.Toolkit.Cache
{
    public sealed class AlwaysDependency : ICacheDependency, IDistributeCacheDependency
    {
        internal static readonly AlwaysDependency InternalDependency = new AlwaysDependency();
        public static readonly ICacheDependency Dependency = InternalDependency;
        private static readonly object Config = new AlwaysConfig();

        /// <summary>
        /// Initializes a new instance of the ObjectDependency class.
        /// </summary>
        private AlwaysDependency()
        {
        }

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                return false;
            }
        }

        #endregion ICacheDependency 成员

        public object CreateStoredObject()
        {
            return Config;
        }

        public override string ToString()
        {
            return "永远存在的缓存依赖";
        }
    }
}