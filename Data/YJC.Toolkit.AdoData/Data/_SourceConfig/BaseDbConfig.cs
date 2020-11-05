using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    abstract internal class BaseDbConfig : IConfigCreator<ISource>, IBaseDbConfig
    {
        protected BaseDbConfig()
        {
        }

        #region IConfigCreator<ISource> ��Ա

        public abstract ISource CreateObject(params object[] args);

        #endregion IConfigCreator<ISource> ��Ա

        #region IBaseDbConfig ��Ա

        [SimpleAttribute]
        public bool SupportData { get; protected set; }

        [SimpleAttribute]
        public string Context { get; protected set; }

        [DynamicElement(DataRightConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IDataRight> DataRight { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public FunctionRightConfig FunctionRight { get; protected set; }

        #endregion IBaseDbConfig ��Ա
    }
}