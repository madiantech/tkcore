using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    public static class XmlUtil
    {
        #region 通用

        internal const BindingFlags BIND_ATTRIBUTE =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static string ReadVersion(XmlReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            if (reader.ReadToFollowing("Toolkit"))
                return reader.GetAttribute("version");
            return string.Empty;
        }

        private static void WriteXmlUseXmlWriter(XmlReader reader, XmlWriter writer)
        {
            while (reader.Read())
                writer.WriteNode(reader, true);
            writer.Flush();
        }

        public static MemoryStream GetXmlStream(XmlReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            MemoryStream stream = new MemoryStream();
            XmlWriterSettings setting = new XmlWriterSettings
            {
                Encoding = ToolkitConst.UTF8,
                Indent = true
            };
            XmlWriter writer = XmlWriter.Create(stream, setting);
            using (writer)
            {
                WriteXmlUseXmlWriter(reader, writer);
            }
            return stream;
        }

        public static string GetXml(XmlReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            using (MemoryStream stream = GetXmlStream(reader))
            {
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static MemoryStream GetJsonStream(XmlReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            MemoryStream stream = new MemoryStream();
            XmlJsonWriter writer = new XmlJsonWriter(stream);
            using (writer)
            {
                WriteXmlUseXmlWriter(reader, writer);
            }
            return stream;
        }

        public static string GetJson(XmlReader reader, ActionResultData result)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            StringBuilder builder = new StringBuilder();
            using (XmlJsonWriter writer = new XmlJsonWriter(builder, result))
            {
                WriteXmlUseXmlWriter(reader, writer);
            }
            return builder.ToString();
        }

        public static string GetJson(XmlReader reader)
        {
            return GetJson(reader, ToolkitConst.QUOTE_CHAR);
        }

        public static string GetJson(XmlReader reader, char quoteChar)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            StringBuilder builder = new StringBuilder();
            using (XmlJsonWriter writer = new XmlJsonWriter(builder) { QuoteChar = quoteChar })
            {
                WriteXmlUseXmlWriter(reader, writer);
            }
            return builder.ToString();
        }

        public static void AddExceptionInfo(XElement parent, Exception ex)
        {
            TkDebug.AssertArgumentNull(parent, "parent", null);
            TkDebug.AssertArgumentNull(ex, "ex", null);

            XElement element = new XElement("Exception",
                new XElement("Message", ex.Message),
                new XElement("ErrorSource", ex.Source),
                new XElement("StackTrace", ex.StackTrace),
                new XElement("Type", ex.GetType().ToString()));
            if (ex.TargetSite != null)
            {
                XElement target = new XElement("TargetSite", ex.TargetSite.ToString());
                element.Add(target);
            }
            ToolkitException tkEx = ex as ToolkitException;
            if (tkEx != null && tkEx.ErrorObject != null)
            {
                XElement error = new XElement("ErrorObjType",
                    tkEx.ErrorObject.GetType().ToString());
                element.Add(error);
                error = new XElement("ErrorObj", tkEx.ErrorObject.ToString());
                element.Add(error);
            }
            YJC.Toolkit.Sys.ArgumentException argEx = ex as YJC.Toolkit.Sys.ArgumentException;
            if (argEx != null)
            {
                XElement error = new XElement("Argument", argEx.Argument);
                element.Add(error);
            }
            parent.Add(element);
            if (ex.InnerException != null)
                AddExceptionInfo(parent, ex.InnerException);
        }

        //public static Stream Html2Xml(TextReader input, Stream outputStream)
        //{
        //    TkDebug.AssertArgumentNull(input, "input", null);

        //    using (input)
        //    using (SgmlReader reader = new SgmlReader())
        //    using (XmlWriter writer = XmlWriter.Create(outputStream))
        //    {
        //        reader.InputStream = input;
        //        reader.Read();
        //        try
        //        {
        //            while (!reader.EOF)
        //            {
        //                writer.WriteNode(reader, false);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //        finally
        //        {
        //            writer.Flush();
        //        }
        //        return outputStream;
        //    }
        //}

        //public static string Html2Xml(Stream input, Encoding encoding)
        //{
        //    TkDebug.AssertArgumentNull(input, "input", null);
        //    TkDebug.AssertArgumentNull(encoding, "encoding", null);

        //    StreamReader reader = new StreamReader(input, encoding);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Html2Xml(reader, stream);
        //        return encoding.GetString(stream.ToArray());
        //    }
        //}

        //public static string Html2Xml(Uri url, Encoding encoding)
        //{
        //    WebClient client = new WebClient();
        //    using (client)
        //    {
        //        //Stream stream = client.OpenRead(url);
        //        byte[] data = client.DownloadData(url);
        //        return Html2Xml(new MemoryStream(data), encoding);
        //    }
        //}

        #endregion 通用
    }
}