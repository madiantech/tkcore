using System.IO;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class FilePageMaker : IPageMaker
    {
        public FilePageMaker(string fileName)
            : this(fileName, null, null, null)
        {
        }

        public FilePageMaker(string fileName, Encoding fileEncoding,
            Encoding contentEncoding, string contentType)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            FileName = fileName;
            FileEncoding = fileEncoding ?? Encoding.UTF8;
            Encoding = contentEncoding ?? ToolkitConst.UTF8;
            contentType = contentType ?? ContentTypeConst.HTML;
        }

        internal FilePageMaker(FilePageMakerAttribute attribute)
        {
            FileName = FileUtil.GetRealFileName(attribute.FileName, attribute.Position);
            ContentType = attribute.ContentType ?? ContentTypeConst.HTML;
            Encoding = attribute.Encoding.Value<Encoding>(ToolkitConst.UTF8);
            FileEncoding = attribute.FileEncoding.Value<Encoding>(ToolkitConst.UTF8);
        }

        internal FilePageMaker(FilePageMakerConfig config)
        {
            ContentType = config.ContentType;
            Encoding = config.Encoding;
            FileEncoding = config.FileEncoding;
            FileName = FileUtil.GetRealFileName(config.FileName, config.Position);
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            string text;
            try
            {
                text = File.ReadAllText(FileName, FileEncoding);
            }
            catch
            {
                text = string.Empty;
            }
            IContent content = new SimpleContent(ContentType, text, Encoding);
            return content;
        }

        #endregion

        public string ContentType { get; private set; }

        public string FileName { get; private set; }

        public Encoding Encoding { get; private set; }

        public Encoding FileEncoding { get; private set; }
    }
}
