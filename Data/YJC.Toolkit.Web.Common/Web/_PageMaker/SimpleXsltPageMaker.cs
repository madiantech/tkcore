using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    public class SimpleXsltPageMaker : IPageMaker, ISupportMetaData
    {
        private IMetaData fMetaData;

        //public SimpleXsltPageMaker()
        //    : this(string.Empty)
        //{
        //}

        public SimpleXsltPageMaker(string xsltFile)
            : this(xsltFile, false)
        {
        }

        public SimpleXsltPageMaker(string xsltFile, bool useXsltArgs)
            : this(xsltFile, useXsltArgs, ContentTypeConst.HTML)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SimpleXsltPageMaker class.
        /// </summary>
        public SimpleXsltPageMaker(string xsltFile, bool useXsltArgs,
            string contentType)
        {
            TkDebug.AssertArgumentNull(xsltFile, "xsltFile", null);
            TkDebug.AssertArgumentNullOrEmpty(contentType, "contentType", null);

            UseXsltArgs = useXsltArgs;
            ContentType = contentType;
            XsltFile = xsltFile;
        }

        public SimpleXsltPageMaker(string xsltFile, bool useXsltArgs,
            string contentType, Encoding encoding)
            : this(xsltFile, useXsltArgs, contentType)
        {
            TkDebug.AssertArgumentNull(encoding, "encoding", null);

            Encoding = encoding;
        }

        #region IPageMaker 成员

        IContent IPageMaker.WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            return WritePage(pageData, outputData);
        }

        #endregion

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return UseXsltArgs;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            if (UseXsltArgs)
                fMetaData = metaData;
        }

        #endregion

        public bool UseXsltArgs { get; private set; }

        public string ContentType { get; private set; }

        public string XsltFile { get; set; }

        public Encoding Encoding { get; set; }

        protected static internal object Prepare()
        {
            return null;
        }

        protected internal virtual IContent CreateContent(IPageData pageData,
            string content, Encoding encoding)
        {
            return new SimpleContent(ContentType, content, encoding);
        }

        protected virtual void AddXsltParam(XsltArgumentList args, IPageData pageData)
        {
            args.AddParam("IsPost", string.Empty, pageData.IsPost);
            if (fMetaData != null)
            {
                object tkObj = fMetaData.ToToolkitObject();
                if (tkObj != null)
                {
                    XDocument element = tkObj.CreateXDocument(null, ObjectUtil.WriteSettings, QName.ToolkitNoNS);
                    args.AddParam("MetaData", string.Empty, element.CreateNavigator().Select("/Toolkit"));
                }
            }
        }

        protected internal IContent WritePage(IPageData pageData, OutputData outputData)
        {
            PageMakerUtil.AssertType(this, outputData, SourceOutputType.XmlReader, SourceOutputType.String,
                SourceOutputType.DataSet, SourceOutputType.ToolkitObject);

            string xsltFile = GetRealXsltFile(pageData); // Path.Combine(AppSetting.Current.XmlPath, XsltFile);
            TkDebug.AssertNotNullOrEmpty(xsltFile, "没有设置XsltFile属性，该值为空", this);
            TkDebug.Assert(File.Exists(xsltFile), string.Format(ObjectUtil.SysCulture,
                "系统中并不存在文件名为{0}的文件，请检查路径！", xsltFile), this);

            XsltArgumentList args = null;
            if (UseXsltArgs)
            {
                args = new XsltArgumentList();
                AddXsltParam(args, pageData);
            }
            string content = string.Empty;
            XmlReader reader = null;
            switch (outputData.OutputType)
            {
                case SourceOutputType.XmlReader:
                    reader = outputData.Data.Convert<XmlReader>();
                    break;
                case SourceOutputType.String:
                    reader = XmlTransformUtil.GetXmlReader(outputData.Data.Convert<string>());
                    break;
                case SourceOutputType.ToolkitObject:
                    XDocument doc = outputData.Data.CreateXDocument(null, ObjectUtil.WriteSettings, QName.ToolkitNoNS);
                    reader = doc.CreateReader();
                    break;
                case SourceOutputType.DataSet:
                    reader = new XmlDataSetReader(outputData.Data.Convert<DataSet>());
                    break;
                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }
            content = XmlTransformUtil.Transform(reader, xsltFile, args, TransformSetting.All);
            Encoding encoding = Encoding ?? (pageData.IsPost ? Encoding.UTF8 : null);

            return CreateContent(pageData, content, encoding);
        }

        protected virtual string GetRealXsltFile(IPageData pageData)
        {
            return XsltFile;
        }
    }
}
