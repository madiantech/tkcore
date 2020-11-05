using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class OverrideLayoutConfig
    {
        [SimpleAttribute]
        public FieldLayout? Layout { get; set; }

        [SimpleAttribute]
        public int? UnitNum { get; set; }
    }
}
