using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class EasySearchRefConfig
    {
        internal EasySearchRefConfig()
        {
        }

        public EasySearchRefConfig(string field, string refField)
        {
            TkDebug.AssertArgumentNullOrEmpty(field, "field", null);
            TkDebug.AssertArgumentNullOrEmpty(refField, "refField", null);

            Field = field;
            RefField = refField;
        }

        [SimpleAttribute(Required = true)]
        public string Field { get; private set; }

        [SimpleAttribute(Required = true)]
        public string RefField { get; private set; }

        [SimpleAttribute]
        internal string CtrlType { get; set; }
    }
}