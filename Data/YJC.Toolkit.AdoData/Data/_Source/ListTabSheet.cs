using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ListTabSheet : IRegName
    {
        public ListTabSheet(string id, string caption, IParamBuilder paramBuilder)
        {
            Id = id;
            Caption = caption;
            ParamBuilder = paramBuilder;
            Selected = false;
        }

        [SimpleAttribute]
        public string Id { get; private set; }

        [SimpleAttribute]
        public string Caption { get; private set; }

        [SimpleAttribute]
        public bool Selected { get; set; }

        public IParamBuilder ParamBuilder { get; private set; }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Id;
            }
        }

        #endregion
    }
}
