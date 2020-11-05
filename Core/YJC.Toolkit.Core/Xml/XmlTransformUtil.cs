using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    public static class XmlTransformUtil
    {
        private readonly static XmlResolver fResolver = new XmlUrlResolver();

        internal static XmlResolver Resolver
        {
            get
            {
                return fResolver;
            }
        }

        internal static XmlReader GetXmlReader(string xml)
        {
            return XmlReader.Create(new StringReader(xml), XmlObjectSerializer.ReadSettings);
        }

        //private static string Transform(XmlReader xml, XslCompiledTransform xsl)
        //{
        //    return Transform(xml, xsl, null);
        //}

        private static string Transform(XmlReader xml, XslCompiledTransform xsl, XsltArgumentList args)
        {
            using (xml)
            {
                StringBuilder html = new StringBuilder();
                InternalStringWriter htmlWriter = new InternalStringWriter(html, xsl.OutputSettings.Encoding);
                using (XmlWriter writer = XmlWriter.Create(htmlWriter, xsl.OutputSettings))
                {
                    xsl.Transform(xml, args, writer, fResolver);

                    return html.ToString();
                }

            }
        }

        private static XDocument TransformToXDoc(XmlReader xml, XslCompiledTransform xsl, XsltArgumentList args)
        {
            return TransformToXDoc(xml, xsl, args, fResolver);
        }

        private static XDocument TransformToXDoc(XmlReader xml, XslCompiledTransform xsl, XsltArgumentList args, XmlResolver resolver)
        {
            using (xml)
            {
                XDocument result = new XDocument();
                using (XmlWriter writer = result.CreateWriter())
                {
                    xsl.Transform(xml, args, writer, resolver);

                    return result;
                }
            }
        }

        private static XslCompiledTransform GetTransformFromType(Type type, TransformSetting setting)
        {
            TkDebug.ThrowIfNoAppSetting();

            XslCompiledTransform result = null;
            bool cache = setting.UseCache && BaseAppSetting.Current.UseCache;
            if (cache)
            {
                XsltTransformCacheData data = CacheManager.GetItem("XsltTransformType",
                    type.ToString(), type, setting).Convert<XsltTransformCacheData>();
                result = data.Transform;
            }
            else
            {
                result = new XslCompiledTransform();
                result.Load(type);
            }
            return result;
        }

        private static XslCompiledTransform GetTransformFromFile(string xslFile, TransformSetting setting)
        {
            TkDebug.ThrowIfNoAppSetting();

            XslCompiledTransform result = null;
            bool cache = setting.UseCache && BaseAppSetting.Current.UseCache;
            if (cache)
            {
                XsltTransformCacheData data = CacheManager.GetItem("XsltTransformFile",
                    xslFile, setting).Convert<XsltTransformCacheData>();
                result = data.Transform;
            }
            else
            {
                result = new XslCompiledTransform();
                XsltSettings xsltSetting = setting.NeedEvidence ? XsltSettings.TrustedXslt
                    : XsltSettings.Default;
                result.Load(new Uri(xslFile).ToString(), xsltSetting, fResolver);
            }

            return result;
        }

        public static string Transform(string xmlStr, string xslFile, XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNullOrEmpty(xmlStr, "xmlStr", null);
            TkDebug.AssertArgumentNullOrEmpty(xslFile, "xslFile", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XmlReader xml = GetXmlReader(xmlStr);
            XslCompiledTransform transform = GetTransformFromFile(xslFile, setting);
            return Transform(xml, transform, args);
        }

        public static string Transform(string xmlStr, string xslFile, XsltArgumentList args)
        {
            return Transform(xmlStr, xslFile, args, TransformSetting.Default);
        }

        public static string Transform(string xmlStr, string xslFile, TransformSetting setting)
        {
            return Transform(xmlStr, xslFile, null, setting);
        }

        public static string Transform(string xmlStr, string xslFile)
        {
            return Transform(xmlStr, xslFile, null, TransformSetting.Default);
        }

        public static string Transform(XmlReader reader, string xslFile, XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);
            TkDebug.AssertArgumentNullOrEmpty(xslFile, "xslFile", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XslCompiledTransform transform = GetTransformFromFile(xslFile, setting);
            return Transform(reader, transform, args);
        }

        public static string Transform(XmlReader reader, string xslFile, XsltArgumentList args)
        {
            return Transform(reader, xslFile, args, TransformSetting.Default);
        }

        public static string Transform(XmlReader reader, string xslFile, TransformSetting setting)
        {
            return Transform(reader, xslFile, null, setting);
        }

        public static string Transform(XmlReader reader, string xslFile)
        {
            return Transform(reader, xslFile, null, TransformSetting.Default);
        }

        public static string Transform(XmlReader reader, Type xsltType, XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);
            TkDebug.AssertArgumentNull(xsltType, "xsltType", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XslCompiledTransform transform = GetTransformFromType(xsltType, setting);
            return Transform(reader, transform, args);
        }

        public static string Transform(XmlReader reader, Type xsltType, XsltArgumentList args)
        {
            return Transform(reader, xsltType, args, TransformSetting.Default);
        }

        public static string Transform(XmlReader reader, Type xsltType, TransformSetting setting)
        {
            return Transform(reader, xsltType, null, setting);
        }

        public static string Transform(XmlReader reader, Type xsltType)
        {
            return Transform(reader, xsltType, null, TransformSetting.Default);
        }

        public static string Transform(string xmlStr, Type xsltType, XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNullOrEmpty(xmlStr, "xmlStr", null);
            TkDebug.AssertArgumentNull(xsltType, "xsltType", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XmlReader xml = GetXmlReader(xmlStr);
            XslCompiledTransform transform = GetTransformFromType(xsltType, setting);
            return Transform(xml, transform, args);
        }

        public static string Transform(string xmlStr, Type xsltType, XsltArgumentList args)
        {
            return Transform(xmlStr, xsltType, args, TransformSetting.Default);
        }

        public static string Transform(string xmlStr, Type xsltType, TransformSetting setting)
        {
            return Transform(xmlStr, xsltType, null, setting);
        }

        public static string Transform(string xmlStr, Type xsltType)
        {
            return Transform(xmlStr, xsltType, null, TransformSetting.Default);
        }

        public static XDocument TransformToXDoc(string xmlStr, string xslFile, XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNullOrEmpty(xmlStr, "xmlStr", null);
            TkDebug.AssertArgumentNullOrEmpty(xslFile, "xslFile", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XmlReader xml = GetXmlReader(xmlStr);
            XslCompiledTransform transform = GetTransformFromFile(xslFile, setting);
            return TransformToXDoc(xml, transform, args);
        }

        public static XDocument TransformToXDoc(string xmlStr, string xslFile, XsltArgumentList args)
        {
            return TransformToXDoc(xmlStr, xslFile, args, TransformSetting.Default);
        }

        public static XDocument TransformToXDoc(string xmlStr, string xslFile, TransformSetting setting)
        {
            return TransformToXDoc(xmlStr, xslFile, null, setting);
        }

        public static XDocument TransformToXDoc(string xmlStr, string xslFile)
        {
            return TransformToXDoc(xmlStr, xslFile, null, TransformSetting.Default);
        }

        public static XDocument TransformToXDoc(XmlReader reader, string xslFile,
            XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);
            TkDebug.AssertArgumentNullOrEmpty(xslFile, "xslFile", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XslCompiledTransform transform = GetTransformFromFile(xslFile, setting);
            return TransformToXDoc(reader, transform, args);
        }

        public static XDocument TransformToXDoc(XmlReader reader, string xslFile, XsltArgumentList args)
        {
            return TransformToXDoc(reader, xslFile, args, TransformSetting.Default);
        }

        public static XDocument TransformToXDoc(XmlReader reader, string xslFile, TransformSetting setting)
        {
            return TransformToXDoc(reader, xslFile, null, setting);
        }

        public static XDocument TransformToXDoc(XmlReader reader, string xslFile)
        {
            return TransformToXDoc(reader, xslFile, null, TransformSetting.Default);
        }

        public static XDocument TransformToXDoc(XmlReader reader, string xslFile, XsltArgumentList args,
            TransformSetting setting, XmlResolver resolver)
        {
            XslCompiledTransform transform = GetTransformFromFile(xslFile, setting);
            return TransformToXDoc(reader, transform, args, resolver);
        }

        public static XDocument TransformToXDoc(XmlReader reader, Type xsltType,
            XsltArgumentList args, TransformSetting setting)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);
            TkDebug.AssertArgumentNull(xsltType, "xsltType", null);
            TkDebug.AssertArgumentNull(setting, "setting", null);

            XslCompiledTransform transform = GetTransformFromType(xsltType, setting);
            return TransformToXDoc(reader, transform, args);
        }

        public static XDocument TransformToXDoc(XmlReader reader, Type xsltType, XsltArgumentList args)
        {
            return TransformToXDoc(reader, xsltType, args, TransformSetting.Default);
        }

        public static XDocument TransformToXDoc(XmlReader reader, Type xsltType, TransformSetting setting)
        {
            return TransformToXDoc(reader, xsltType, null, setting);
        }

        public static XDocument TransformToXDoc(XmlReader reader, Type xsltType)
        {
            return TransformToXDoc(reader, xsltType, null, TransformSetting.Default);
        }
    }
}
