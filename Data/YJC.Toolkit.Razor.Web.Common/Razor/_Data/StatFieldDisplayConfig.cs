using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class StatFieldDisplayConfig : IRegName
    {
        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion IRegName 成员

        [SimpleAttribute(Required = true)]
        public string NickName { get; private set; }

        [DynamicElement(CoreDisplayConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<IDisplay> Display { get; private set; }
    }
}