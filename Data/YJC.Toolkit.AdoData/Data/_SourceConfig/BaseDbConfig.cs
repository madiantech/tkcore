using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    abstract internal class BaseDbConfig : IConfigCreator<ISource>, IBaseDbConfig
    {
        protected BaseDbConfig()
        {
        }

        #region IConfigCreator<ISource> 成员

        public abstract ISource CreateObject(params object[] args);

        #endregion IConfigCreator<ISource> 成员

        #region IBaseDbConfig 成员

        [SimpleAttribute]
        public bool SupportData { get; protected set; }

        [SimpleAttribute]
        public string Context { get; protected set; }

        [DynamicElement(DataRightConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IDataRight> DataRight { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public FunctionRightConfig FunctionRight { get; protected set; }

        #endregion IBaseDbConfig 成员
    }
}