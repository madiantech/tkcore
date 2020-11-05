using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class HtmlAttributeBuilder
    {
        private readonly RegNameList<HtmlAttribute> fAttrs;

        public HtmlAttributeBuilder()
        {
            fAttrs = new RegNameList<HtmlAttribute>();
        }

        internal HtmlAttribute this[string name]
        {
            get
            {
                return fAttrs[name];
            }
        }

        private void AddAttribute(HtmlAttribute attr)
        {
            if (attr == null)
                return;

            if (fAttrs.Contains(attr))
            {
                HtmlAttribute curAttr = fAttrs[attr.Name];
                curAttr.Value = attr.Value;
            }
            else
                fAttrs.Add(attr);
        }

        public void Add(string name, object value)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            AddAttribute(new HtmlAttribute(name, value));
        }

        public void Add(HtmlAttribute attribute)
        {
            AddAttribute(attribute);
        }

        public bool Remove(HtmlAttribute attribute)
        {
            TkDebug.AssertArgumentNull(attribute, "attribute", null);

            return fAttrs.Remove(attribute);
        }

        public bool Remove(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            return fAttrs.Remove((HtmlAttribute)name);
        }

        public void AddRange(IEnumerable<HtmlAttribute> attributes)
        {
            if (attributes == null)
                return;

            foreach (var item in attributes)
                AddAttribute(item);
        }

        public void Clear()
        {
            fAttrs.Clear();
        }

        public string CreateAttribute()
        {
            return string.Join(" ", fAttrs);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{{Count={0}}}", fAttrs.Count);
        }
    }
}
