using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BaseObjectPageMaker : CompositePageMaker
    {
        protected BaseObjectPageMaker()
        {
        }

        public bool IsGZip { get; set; }

        public bool IsEncrypt { get; set; }

        protected override IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            IContent content = base.WritePage(source, pageData, outputData);
            if (!IsGZip && !IsEncrypt)
                return content;
            byte[] data = content.ContentEncoding.GetBytes(content.Content);
            string contentType = content.ContentType;
            if (IsGZip)
            {
                MemoryStream stream = GZipUtil.GZipCompress(data);
                using (stream)
                {
                    data = stream.ToArray();
                }
                contentType = ContentTypeConst.GZIP;
            }
            if (IsEncrypt)
            {
                data = CryptoUtil.Encrypt(data);
                contentType = ContentTypeConst.BINARY;
            }
            return new WebFileContent(new FileContent(contentType, data));
        }

        internal void SetFormat(IObjectFormat format)
        {
            TkDebug.ThrowIfNoAppSetting();

            IsGZip = format.GZip.Config(WebAppSetting.WebCurrent.OutputGZip);
            IsEncrypt = format.Encrypt.Config(WebAppSetting.WebCurrent.OutputEncrypt);
        }
    }
}
