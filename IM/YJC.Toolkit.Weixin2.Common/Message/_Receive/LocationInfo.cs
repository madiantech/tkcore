using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class LocationInfo
    {
        [SimpleElement(LocalName = "Location_X", Order = 10)]
        public double LocationX { get; private set; }

        [SimpleElement(LocalName = "Location_Y", Order = 20)]
        public double LocationY { get; private set; }

        [SimpleElement(Order = 30)]
        public int Scale { get; private set; }

        [SimpleElement(Order = 40)]
        public string Label { get; private set; }

        [SimpleElement(LocalName = "Poiname", Order = 50)]
        public string PoiName { get; private set; }
    }
}
