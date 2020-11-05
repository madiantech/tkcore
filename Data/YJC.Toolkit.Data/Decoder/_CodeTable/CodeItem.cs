using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class CodeItem : IRegName, IDecoderItem
    {
        protected CodeItem()
        {
        }

        public CodeItem(string value, string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(value, "value", null);
            TkDebug.AssertArgumentNull(name, "name", null);

            Value = value;
            Name = name;
        }

        public CodeItem(IDecoderItem item)
        {
            TkDebug.AssertArgumentNull(item, "item", null);

            Value = item.Value;
            Name = item.Name;
        }

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return Value;
            }
        }

        #endregion

        #region IDecoderItem 成员

        [SimpleAttribute]
        public string Value { get; protected set; }

        [SimpleAttribute]
        public string Name { get; protected set; }

        [SimpleAttribute]
        public virtual string DisplayName
        {
            get
            {
                return Name;
            }
            set
            {
            }
        }

        public string this[string name]
        {
            get
            {
                return null;
            }
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
