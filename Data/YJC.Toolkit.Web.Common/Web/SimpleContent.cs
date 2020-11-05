using System.Text;
using YJC.Toolkit.Sys;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YJC.Toolkit.Web
{
    public sealed class SimpleContent : IContent
    {
        private readonly string fContentType;
        private readonly string fContent;
        private readonly Encoding fEncoding;

        /// <summary>
        /// Initializes a new instance of the SimpleContent class.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public SimpleContent(string contentType, string content, Encoding encoding)
        {
            fContentType = contentType;
            fContent = content;
            fEncoding = encoding;
        }

        public SimpleContent(string contentType, string content)
            : this(contentType, content, Encoding.UTF8)
        {
        }

        public SimpleContent(string content)
            : this(ContentTypeConst.XML, content)
        {
        }

        #region IContent 成员

        string IContent.ContentType
        {
            get
            {
                return fContentType;
            }
        }

        string IContent.Content
        {
            get
            {
                return fContent;
            }
        }

        Encoding IContent.ContentEncoding
        {
            get
            {
                return fEncoding;
            }
        }

        public Dictionary<string, string> Headers { get; set; }

        Task IContent.WritePage(object response)
        {
            return null;
        }

        #endregion IContent 成员
    }
}