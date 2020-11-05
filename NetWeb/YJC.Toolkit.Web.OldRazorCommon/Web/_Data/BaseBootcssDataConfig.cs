using System.Collections.Generic;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    abstract class BaseBootcssDataConfig : IConfigCreator<object>
    {
        protected BaseBootcssDataConfig()
        {
        }

        #region IConfigCreator<object> 成员

        public abstract object CreateObject(params object[] args);

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true,
            LocalName = "RazorField", UseConstructor = true)]
        public List<RazorField> RazorFields { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public RazorOutputData Header { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public RazorOutputData Footer { get; protected set; }

        protected void SetRazorField(BaseBootcssData data)
        {
            if (RazorFields != null)
            {
                foreach (var item in RazorFields)
                    data.AddDisplayField(item);
            }
            data.Header = Header;
            data.Footer = Footer;
        }
    }
}
