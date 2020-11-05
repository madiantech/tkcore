namespace YJC.Toolkit.Cache
{
    public sealed class NoDependency : ICacheDependency, IDistributeCacheDependency
    {
        internal static readonly NoDependency InternalDependency = new NoDependency();
        public static readonly ICacheDependency Dependency = InternalDependency;
        private static readonly object Config = new NoCacheConfig();

        /// <summary>
        /// Initializes a new instance of the NoDependency class.
        /// </summary>
        private NoDependency()
        {
        }

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                return true;
            }
        }

        #endregion ICacheDependency 成员

        public object CreateStoredObject()
        {
            return Config;
        }

        public override string ToString()
        {
            return "永远不会缓存的缓存依赖";
        }
    }
}