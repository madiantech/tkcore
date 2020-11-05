using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public abstract class BaseOperatorConfig : IReadObjectCallBack, IConfigCreator<OperatorConfig>
    {
        private readonly string fDefaultIconClass;
        private readonly string fDefaultCaption;

        protected BaseOperatorConfig(string defaultCaption, string defaultIconClass)
        {
            fDefaultIconClass = defaultIconClass;
            fDefaultCaption = defaultCaption;
        }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(IconClass))
                IconClass = fDefaultIconClass;
            OperatorCaption = Caption == null ? fDefaultCaption : Caption.ToString();
        }

        #endregion

        #region IConfigCreator<OperatorConfig> 成员

        public abstract OperatorConfig CreateObject(params object[] args);

        #endregion

        protected string OperatorCaption { get; set; }

        [SimpleAttribute]
        public string IconClass { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Caption { get; protected set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(OperatorCaption))
                return base.ToString();
            return OperatorCaption;
        }
    }
}
