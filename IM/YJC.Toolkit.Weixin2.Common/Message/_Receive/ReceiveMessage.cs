using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class ReceiveMessage : BaseWeChatMessage
    {
        [SimpleElement(Order = 50)]
        public string Content { get; private set; }

        [SimpleElement(Order = 60)]
        public string PicUrl { get; private set; }

        [SimpleElement(Order = 70)]
        public string MediaId { get; private set; }

        [SimpleElement(Order = 80)]
        public string Format { get; private set; }

        [SimpleElement(Order = 90)]
        public string ThumbMediaId { get; private set; }

        [SimpleElement(LocalName = "Location_X", Order = 100)]
        public double LocationX { get; private set; }

        [SimpleElement(LocalName = "Location_Y", Order = 110)]
        public double LocationY { get; private set; }

        [SimpleElement(Order = 120)]
        public int Scale { get; private set; }

        [SimpleElement(Order = 130)]
        public string Label { get; private set; }

        [SimpleElement(Order = 140)]
        public string Title { get; private set; }

        [SimpleElement(Order = 150)]
        public string Description { get; private set; }

        [SimpleElement(Order = 160)]
        public string Url { get; private set; }

        [SimpleElement(Order = 170)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        public EventType Event { get; private set; }

        [SimpleElement(Order = 180)]
        public string EventKey { get; private set; }

        [SimpleElement(Order = 190)]
        public string Ticket { get; private set; }

        [SimpleElement(Order = 200)]
        public double Latitude { get; private set; }

        [SimpleElement(Order = 210)]
        public double Longitude { get; private set; }

        [SimpleElement(Order = 220)]
        public double Precision { get; private set; }

        [SimpleElement(Order = 230)]
        public string Recognition { get; private set; }

        [ObjectElement(Order = 240)]
        public ScanCodeInfo ScanCodeInfo { get; private set; }

        [ObjectElement(Order = 250)]
        public PicsInfo SendPicsInfo { get; private set; }

        [ObjectElement(Order = 260)]
        public LocationInfo SendLocationInfo { get; private set; }

        [SimpleElement(Order = 290, LocalName = "AgentID", DefaultValue = -1)]
        public int AgentId { get; private set; }

        [SimpleElement(Order = 300)]
        public long MsgId { get; protected set; }

        [SimpleElement(Order = 330)]
        public string Status { get; private set; }

        [SimpleElement(Order = 340)]
        public int TotalCount { get; private set; }

        [SimpleElement(Order = 350)]
        public int FilterCount { get; private set; }

        [SimpleElement(Order = 360)]
        public int SentCount { get; private set; }

        [SimpleElement(Order = 370)]
        public int ErrorCount { get; private set; }
    }
}