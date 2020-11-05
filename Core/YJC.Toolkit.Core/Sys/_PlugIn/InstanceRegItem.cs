using System;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    internal sealed class InstanceRegItem : BaseRegItem, IDisposable
    {
        private readonly object fInstance;
        private readonly Type fInstanceType;

        /// <summary>
        /// Initializes a new instance of the InstanceRegItem class.
        /// </summary>
        public InstanceRegItem(string regName, BasePlugInAttribute attribute,
            object instance, ICacheDependency dependency)
            : base(regName, attribute)
        {
            fInstance = instance;
            fInstanceType = instance.GetType();
            Dependency = dependency;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Instance.DisposeObject();
            Dependency.DisposeObject();
        }

        #endregion

        public object Instance
        {
            get
            {
                return fInstance;
            }
        }

        public Type InstanceType
        {
            get
            {
                return fInstanceType;
            }
        }

        public override T CreateInstance<T>()
        {
            return fInstance.Convert<T>();
        }

        public override T CreateInstance<T>(params object[] args)
        {
            return CreateInstance<T>();
        }

        public ICacheDependency Dependency { get; private set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(RegName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "注册名为{0}Instance单元", RegName);
        }
    }
}
