using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal class JsonField : IRegName
    {
        public JsonField(Tk5FieldInfoEx field)
        {
            Name = field.NickName;
            Type = field.JsControlName;
        }

        public JsonField(string nickName, Tk5FieldInfoEx field)
        {
            Name = nickName;
            Type = field.JsControlName;
        }

        public JsonField(string nickName, string controlType)
        {
            Name = nickName;
            Type = controlType;
        }

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return Name;
            }
        }

        #endregion IRegName 成员

        [SimpleAttribute]
        public string Name { get; private set; }

        [SimpleAttribute]
        public string Type { get; private set; }
    }
}