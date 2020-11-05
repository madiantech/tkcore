using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    internal class BaseFieldConfig : IRegName
    {
        [SimpleAttribute(Required = true)]
        public string NickName { get; protected set; }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.IsNullOrEmpty(NickName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "Del:[{0}]", NickName);
        }
    }
}
