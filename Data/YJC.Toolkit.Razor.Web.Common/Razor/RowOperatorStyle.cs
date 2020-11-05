using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RowOperatorStyle
    {
        private const string DEFAULT_OPERS = "Update Delete";
        private const string DEFAULT_CAPTION = "操作";

        [SimpleAttribute(DefaultValue = OperatorStyle.Tile)]
        public OperatorStyle Style { get; set; }

        [SimpleAttribute(DefaultValue = DEFAULT_CAPTION)]
        public string MenuCaption { get; set; }

        [SimpleAttribute(DefaultValue = DEFAULT_OPERS)]
        [TkTypeConverter(typeof(StringHashSetConverter))]
        public HashSet<string> FixOperatorIds { get; set; }

        public bool IsFix(string operId)
        {
            if (FixOperatorIds == null || string.IsNullOrEmpty(operId))
                return false;

            return FixOperatorIds.Contains(operId);
        }

        public static RowOperatorStyle CreateDefault()
        {
            return new RowOperatorStyle
            {
                Style = OperatorStyle.Tile,
                MenuCaption = DEFAULT_CAPTION,
                FixOperatorIds = new HashSet<string> { "Insert", "Update" }
            };
        }
    }
}