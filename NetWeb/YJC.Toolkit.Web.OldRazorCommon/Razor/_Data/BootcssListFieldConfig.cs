using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class BootcssListFieldConfig
    {
        internal BootcssListFieldConfig()
        {
        }

        internal BootcssListFieldConfig(string nickName, int col, string @class)
        {
            NickName = nickName;
            Col = col;
            Class = @class;
        }

        [SimpleAttribute]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public int Col { get; private set; }

        [SimpleAttribute]
        public string Class { get; private set; }
    }
}
