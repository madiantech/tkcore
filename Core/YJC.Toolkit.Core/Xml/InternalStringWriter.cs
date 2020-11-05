using System.IO;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    internal class InternalStringWriter : StringWriter
    {
        private readonly Encoding fEncoding;

        public InternalStringWriter(StringBuilder builder, Encoding encoding)
            : base(builder, ObjectUtil.SysCulture)
        {
            fEncoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return fEncoding ?? base.Encoding;
            }
        }
    }
}
