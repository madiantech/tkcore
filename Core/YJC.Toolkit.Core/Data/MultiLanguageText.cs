using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultiLanguageText : IReadObjectCallBack
    {
        private readonly Dictionary<CultureInfo, LanguageText> fDictionary;

        public MultiLanguageText()
        {
            fDictionary = new Dictionary<CultureInfo, LanguageText>();
        }

        public MultiLanguageText(string content)
            : this()
        {
            TkDebug.AssertArgumentNull(content, "content", null);

            Content = content;
        }

        [SimpleElement(NamespaceType.Toolkit)]
        protected string Content { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Lang")]
        protected List<LanguageText> Languages { get; set; }

        public string ToString(CultureInfo info)
        {
            TkDebug.AssertArgumentNull(info, "info", this);

            if (Languages == null)
                return Content;
            else
            {
                LanguageText lang;
                if (fDictionary.TryGetValue(info, out lang))
                    return lang.Value;
                lang = (from item in Languages
                        where item.IsFitFor(info)
                        select item).FirstOrDefault();
                if (lang != null)
                    return lang.Value;
                else
                    return string.Empty;
            }
        }

        public override string ToString()
        {
            return ToString(ObjectUtil.SysCulture);
        }

        void IReadObjectCallBack.OnReadObject()
        {
            fDictionary.Clear();
            if (Languages != null)
                foreach (var item in Languages)
                    fDictionary.Add(item.Culture, item);
        }

        public static string ToString(MultiLanguageText text)
        {
            return text == null ? string.Empty : text.ToString();
        }
    }
}
