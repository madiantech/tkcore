using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class Tk5ExtensionConfig
    {
        internal Tk5ExtensionConfig()
        {
        }

        [SimpleAttribute(DefaultValue = "1")]
        public string CheckValue { get; private set; }

        [SimpleAttribute(DefaultValue = "0")]
        public string UnCheckValue { get; private set; }

        [SimpleAttribute]
        public string Expression { get; private set; }

        [SimpleAttribute]
        public string Format { get; private set; }

        [SimpleAttribute(DefaultValue = Alignment.Left)]
        public Alignment Align { get; private set; }

        [SimpleAttribute(DefaultValue = Alignment.Left)]
        public Alignment TitleAlign { get; private set; }

        [SimpleAttribute]
        public string EmptyTitle { get; private set; }

        internal void Override(Tk5FieldInfoEx item, OverrideExtensionConfig ovExt)
        {
            if (!string.IsNullOrEmpty(ovExt.CheckValue))
                CheckValue = ovExt.CheckValue;
            if (!string.IsNullOrEmpty(ovExt.UnCheckValue))
                UnCheckValue = ovExt.UnCheckValue;
            if (!string.IsNullOrEmpty(ovExt.Expression))
            {
                Expression = ovExt.Expression;
                item.ResetExpression();
            }
            if (!string.IsNullOrEmpty(ovExt.Format))
                Format = ovExt.Format;
            if (ovExt.Align.HasValue)
                Align = ovExt.Align.Value;
            if (!string.IsNullOrEmpty(ovExt.EmptyTitle))
                EmptyTitle = ovExt.EmptyTitle;
        }
    }
}