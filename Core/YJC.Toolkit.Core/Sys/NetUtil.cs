using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class NetUtil
    {
        private readonly static Dictionary<string, string> MimeMapping = CreateMimeMapping();

        private static Dictionary<string, string> CreateMimeMapping()
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            #region 添加Mapping，从MimeMapping类复制过来的

            mapping.Add(".323", "text/h323");

            mapping.Add(".asx", "video/x-ms-asf");
            mapping.Add(".acx", "application/internet-property-stream");
            mapping.Add(".ai", "application/postscript");
            mapping.Add(".aif", "audio/x-aiff");
            mapping.Add(".aiff", "audio/aiff");
            mapping.Add(".axs", "application/olescript");
            mapping.Add(".aifc", "audio/aiff");
            mapping.Add(".asr", "video/x-ms-asf");
            mapping.Add(".avi", "video/x-msvideo");
            mapping.Add(".asf", "video/x-ms-asf");
            mapping.Add(".au", "audio/basic");
            mapping.Add(".application", "application/x-ms-application");

            mapping.Add(".bin", "application/octet-stream");
            mapping.Add(".bas", "text/plain");
            mapping.Add(".bcpio", "application/x-bcpio");
            mapping.Add(".bmp", "image/bmp");

            mapping.Add(".cdf", "application/x-cdf");
            mapping.Add(".cat", "application/vndms-pkiseccat");
            mapping.Add(".crt", "application/x-x509-ca-cert");
            mapping.Add(".c", "text/plain");
            mapping.Add(".css", "text/css");
            mapping.Add(".cer", "application/x-x509-ca-cert");
            mapping.Add(".crl", "application/pkix-crl");
            mapping.Add(".cmx", "image/x-cmx");
            mapping.Add(".csh", "application/x-csh");
            mapping.Add(".cod", "image/cis-cod");
            mapping.Add(".cpio", "application/x-cpio");
            mapping.Add(".clp", "application/x-msclip");
            mapping.Add(".crd", "application/x-mscardfile");

            mapping.Add(".deploy", "application/octet-stream");
            mapping.Add(".dll", "application/x-msdownload");
            mapping.Add(".dot", "application/msword");
            mapping.Add(".doc", "application/msword");
            mapping.Add(".dvi", "application/x-dvi");
            mapping.Add(".dir", "application/x-director");
            mapping.Add(".dxr", "application/x-director");
            mapping.Add(".der", "application/x-x509-ca-cert");
            mapping.Add(".dib", "image/bmp");
            mapping.Add(".dcr", "application/x-director");
            mapping.Add(".disco", "text/xml");

            mapping.Add(".exe", "application/octet-stream");
            mapping.Add(".etx", "text/x-setext");
            mapping.Add(".evy", "application/envoy");
            mapping.Add(".eml", "message/rfc822");
            mapping.Add(".eps", "application/postscript");

            mapping.Add(".flr", "x-world/x-vrml");
            mapping.Add(".fif", "application/fractals");

            mapping.Add(".gtar", "application/x-gtar");
            mapping.Add(".gif", "image/gif");
            mapping.Add(".gz", "application/x-gzip");

            mapping.Add(".hta", "application/hta");
            mapping.Add(".htc", "text/x-component");
            mapping.Add(".htt", "text/webviewhtml");
            mapping.Add(".h", "text/plain");
            mapping.Add(".hdf", "application/x-hdf");
            mapping.Add(".hlp", "application/winhlp");
            mapping.Add(".html", "text/html");
            mapping.Add(".htm", "text/html");
            mapping.Add(".hqx", "application/mac-binhex40");

            mapping.Add(".isp", "application/x-internet-signup");
            mapping.Add(".iii", "application/x-iphone");
            mapping.Add(".ief", "image/ief");
            mapping.Add(".ivf", "video/x-ivf");
            mapping.Add(".ins", "application/x-internet-signup");
            mapping.Add(".ico", "image/x-icon");

            mapping.Add(".jpg", "image/jpeg");
            mapping.Add(".jfif", "image/pjpeg");
            mapping.Add(".jpe", "image/jpeg");
            mapping.Add(".jpeg", "image/jpeg");
            mapping.Add(".js", "application/x-javascript");

            mapping.Add(".lsx", "video/x-la-asf");
            mapping.Add(".latex", "application/x-latex");
            mapping.Add(".lsf", "video/x-la-asf");

            mapping.Add(".manifest", "application/x-ms-manifest");
            mapping.Add(".mhtml", "message/rfc822");
            mapping.Add(".mny", "application/x-msmoney");
            mapping.Add(".mht", "message/rfc822");
            mapping.Add(".mid", "audio/mid");
            mapping.Add(".mpv2", "video/mpeg");
            mapping.Add(".man", "application/x-troff-man");
            mapping.Add(".mvb", "application/x-msmediaview");
            mapping.Add(".mpeg", "video/mpeg");
            mapping.Add(".m3u", "audio/x-mpegurl");
            mapping.Add(".mdb", "application/x-msaccess");
            mapping.Add(".mpp", "application/vnd.ms-project");
            mapping.Add(".m1v", "video/mpeg");
            mapping.Add(".mpa", "video/mpeg");
            mapping.Add(".me", "application/x-troff-me");
            mapping.Add(".m13", "application/x-msmediaview");
            mapping.Add(".movie", "video/x-sgi-movie");
            mapping.Add(".m14", "application/x-msmediaview");
            mapping.Add(".mpe", "video/mpeg");
            mapping.Add(".mp2", "video/mpeg");
            mapping.Add(".mov", "video/quicktime");
            mapping.Add(".mp3", "audio/mpeg");
            mapping.Add(".mpg", "video/mpeg");
            mapping.Add(".ms", "application/x-troff-ms");

            mapping.Add(".nc", "application/x-netcdf");
            mapping.Add(".nws", "message/rfc822");

            mapping.Add(".oda", "application/oda");
            mapping.Add(".ods", "application/oleobject");

            mapping.Add(".pmc", "application/x-perfmon");
            mapping.Add(".p7r", "application/x-pkcs7-certreqresp");
            mapping.Add(".p7b", "application/x-pkcs7-certificates");
            mapping.Add(".p7s", "application/pkcs7-signature");
            mapping.Add(".pmw", "application/x-perfmon");
            mapping.Add(".ps", "application/postscript");
            mapping.Add(".p7c", "application/pkcs7-mime");
            mapping.Add(".pbm", "image/x-portable-bitmap");
            mapping.Add(".ppm", "image/x-portable-pixmap");
            mapping.Add(".pub", "application/x-mspublisher");
            mapping.Add(".pnm", "image/x-portable-anymap");
            mapping.Add(".pml", "application/x-perfmon");
            mapping.Add(".p10", "application/pkcs10");
            mapping.Add(".pfx", "application/x-pkcs12");
            mapping.Add(".p12", "application/x-pkcs12");
            mapping.Add(".pdf", "application/pdf");
            mapping.Add(".pps", "application/vnd.ms-powerpoint");
            mapping.Add(".p7m", "application/pkcs7-mime");
            mapping.Add(".pko", "application/vndms-pkipko");
            mapping.Add(".ppt", "application/vnd.ms-powerpoint");
            mapping.Add(".pmr", "application/x-perfmon");
            mapping.Add(".pma", "application/x-perfmon");
            mapping.Add(".pot", "application/vnd.ms-powerpoint");
            mapping.Add(".prf", "application/pics-rules");
            mapping.Add(".pgm", "image/x-portable-graymap");

            mapping.Add(".qt", "video/quicktime");

            mapping.Add(".ra", "audio/x-pn-realaudio");
            mapping.Add(".rgb", "image/x-rgb");
            mapping.Add(".ram", "audio/x-pn-realaudio");
            mapping.Add(".rmi", "audio/mid");
            mapping.Add(".ras", "image/x-cmu-raster");
            mapping.Add(".roff", "application/x-troff");
            mapping.Add(".rtf", "application/rtf");
            mapping.Add(".rtx", "text/richtext");

            mapping.Add(".sv4crc", "application/x-sv4crc");
            mapping.Add(".spc", "application/x-pkcs7-certificates");
            mapping.Add(".setreg", "application/set-registration-initiation");
            mapping.Add(".snd", "audio/basic");
            mapping.Add(".stl", "application/vndms-pkistl");
            mapping.Add(".setpay", "application/set-payment-initiation");
            mapping.Add(".stm", "text/html");
            mapping.Add(".shar", "application/x-shar");
            mapping.Add(".sh", "application/x-sh");
            mapping.Add(".sit", "application/x-stuffit");
            mapping.Add(".spl", "application/futuresplash");
            mapping.Add(".sct", "text/scriptlet");
            mapping.Add(".scd", "application/x-msschedule");
            mapping.Add(".sst", "application/vndms-pkicertstore");
            mapping.Add(".src", "application/x-wais-source");
            mapping.Add(".sv4cpio", "application/x-sv4cpio");

            mapping.Add(".tex", "application/x-tex");
            mapping.Add(".tgz", "application/x-compressed");
            mapping.Add(".t", "application/x-troff");
            mapping.Add(".tar", "application/x-tar");
            mapping.Add(".tr", "application/x-troff");
            mapping.Add(".tif", "image/tiff");
            mapping.Add(".txt", "text/plain");
            mapping.Add(".texinfo", "application/x-texinfo");
            mapping.Add(".trm", "application/x-msterminal");
            mapping.Add(".tiff", "image/tiff");
            mapping.Add(".tcl", "application/x-tcl");
            mapping.Add(".texi", "application/x-texinfo");
            mapping.Add(".tsv", "text/tab-separated-values");

            mapping.Add(".ustar", "application/x-ustar");
            mapping.Add(".uls", "text/iuls");

            mapping.Add(".vcf", "text/x-vcard");

            mapping.Add(".wps", "application/vnd.ms-works");
            mapping.Add(".wav", "audio/wav");
            mapping.Add(".wrz", "x-world/x-vrml");
            mapping.Add(".wri", "application/x-mswrite");
            mapping.Add(".wks", "application/vnd.ms-works");
            mapping.Add(".wmf", "application/x-msmetafile");
            mapping.Add(".wcm", "application/vnd.ms-works");
            mapping.Add(".wrl", "x-world/x-vrml");
            mapping.Add(".wdb", "application/vnd.ms-works");
            mapping.Add(".wsdl", "text/xml");

            mapping.Add(".xml", "text/xml");
            mapping.Add(".xlm", "application/vnd.ms-excel");
            mapping.Add(".xaf", "x-world/x-vrml");
            mapping.Add(".xla", "application/vnd.ms-excel");
            mapping.Add(".xls", "application/vnd.ms-excel");
            mapping.Add(".xof", "x-world/x-vrml");
            mapping.Add(".xlt", "application/vnd.ms-excel");
            mapping.Add(".xlc", "application/vnd.ms-excel");
            mapping.Add(".xsl", "text/xml");
            mapping.Add(".xbm", "image/x-xbitmap");
            mapping.Add(".xlw", "application/vnd.ms-excel");
            mapping.Add(".xpm", "image/x-xpixmap");
            mapping.Add(".xwd", "image/x-xwindowdump");
            mapping.Add(".xsd", "text/xml");

            mapping.Add(".z", "application/x-compress");
            mapping.Add(".zip", "application/x-zip-compressed");

            mapping.Add(".*", "application/octet-stream");

            #endregion 添加Mapping，从MimeMapping类复制过来的

            return mapping;
        }

        public static string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower(ObjectUtil.SysCulture);
            string contentType;
            if (MimeMapping.TryGetValue(extension, out contentType))
                return contentType;

            return MimeMapping[".*"];
        }

        public static WebResponse HttpGet(Uri uri)
        {
            return HttpGet(uri, string.Empty);
        }

        public static WebResponse HttpGet(Uri uri, string contentType)
        {
            TkDebug.AssertArgumentNull(uri, "uri", null);

            WebRequest request = WebRequest.Create(uri);
            TkTrace.LogInfo(uri.ToString());

            try
            {
                WebResponse response = request.GetResponse();
                if (!string.IsNullOrEmpty(contentType))
                {
                    TkDebug.Assert(response.ContentType.Contains(contentType), string.Format(
                        ObjectUtil.SysCulture, "{0}返回的ContentType是{1}，而目标是{2}，两者不兼容",
                        uri, response.ContentType, contentType), null);
                }
                return response;
            }
            catch (WebException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(
                    ObjectUtil.SysCulture, "访问{0}时出错", uri), ex, null);
                return null;
            }
        }

        public static WebResponse HttpPost(Uri uri, string postData)
        {
            return HttpPost(uri, Encoding.UTF8.GetBytes(postData), "application/x-www-form-urlencoded");
        }

        public static WebResponse HttpPost(Uri uri, string postData, string contentType)
        {
            return HttpPost(uri, Encoding.UTF8.GetBytes(postData), contentType);
        }

        public static WebResponse HttpPost(Uri uri, byte[] postData, string contentType)
        {
            WebRequest request = WebRequest.Create(uri);
            TkTrace.LogInfo(uri.ToString());

            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = postData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(postData, 0, postData.Length);
            }

            WebResponse response = request.GetResponse();
            return response;
        }

        public static WebResponse FormUploadFile(Uri url, string controlName, string fileName,
            params KeyValuePair<string, string>[] formDatas)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            byte[] data = File.ReadAllBytes(fileName);

            return FormUploadFile(url, controlName, fileName, data, formDatas);
        }

        public static T ReadObjectFromResponse<T>(WebResponse response, string modelName, T obj)
        {
            TkDebug.AssertArgumentNull(obj, "obj", null);

            if (response != null)
            {
                using (response)
                using (Stream stream = response.GetResponseStream())
                {
                    obj.ReadFromStream("Json", modelName, stream, ObjectUtil.ReadSettings, QName.Toolkit);
                }
            }

            return obj;
        }

        public static T HttpGetReadJson<T>(Uri uri) where T : new()
        {
            T obj = new T();
            return HttpGetReadJson(uri, null, obj);
        }

        public static T HttpGetReadJson<T>(Uri uri, string modelName) where T : new()
        {
            T obj = new T();
            return HttpGetReadJson(uri, modelName, obj);
        }

        public static T HttpGetReadJson<T>(Uri uri, T obj)
        {
            return HttpGetReadJson(uri, null, obj);
        }

        public static T HttpGetReadJson<T>(Uri uri, string modelName, T obj)
        {
            TkDebug.AssertArgumentNull(uri, "uri", null);

            var response = HttpGet(uri);
            return ReadObjectFromResponse(response, modelName, obj);
        }

        public static WebResponse FormUploadFile(Uri url, string controlName,
            string fileName, byte[] fileData, params KeyValuePair<string, string>[] formDatas)
        {
            TkDebug.AssertArgumentNull(url, "url", null);
            TkDebug.AssertArgumentNullOrEmpty(controlName, "controlName", null);
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            TkDebug.AssertArgumentNull(fileData, "fileData", null);

            string formDataBoundary = string.Format(ObjectUtil.SysCulture,
                "----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(formDataBoundary, controlName,
                fileName, fileData, formDatas);

            return PostForm(url, contentType, formData);
        }

        public static byte[] GetResponseData(WebResponse response)
        {
            using (response)
            using (MemoryStream output = new MemoryStream())
            {
                Stream input = response.GetResponseStream();
                using (input)
                {
                    FileUtil.CopyStream(input, output);

                    return output.ToArray();
                }
            }
        }

        public static string GetResponseData(WebResponse response, Encoding encoding)
        {
            using (response)
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, encoding);
                using (reader)
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static WebResponse PostForm(Uri postUrl, string contentType, byte[] formData)
        {
            WebRequest request = WebRequest.Create(postUrl);

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            //request.UserAgent = userAgent;
            //request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            // You could add authentication here as well if needed:
            // request.PreAuthenticate = true;
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
            }

            return request.GetResponse();
        }

        private static byte[] GetMultipartFormData(string boundary, string controlName,
            string fileName, byte[] fileData, KeyValuePair<string, string>[] formDatas)
        {
            Encoding encoding = Encoding.UTF8;
            MemoryStream formDataStream = new MemoryStream();
            using (formDataStream)
            {
                bool needsCLRF = false;
                if (needsCLRF)
                {
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
                }
                needsCLRF = true;
                string header = string.Format(ObjectUtil.SysCulture,
                    "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                    boundary, controlName, Path.GetFileName(fileName), GetContentType(fileName));
                formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                formDataStream.Write(fileData, 0, fileData.Length);

                if (formDatas != null)
                {
                    foreach (var item in formDatas)
                    {
                        header = string.Format(ObjectUtil.SysCulture,
                            "\r\n--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n",
                            boundary, item.Key);
                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                        byte[] desData = encoding.GetBytes(item.Value);
                        formDataStream.Write(desData, 0, desData.Length);
                    }
                }

                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
                return formDataStream.ToArray();
            }
        }

        //private static byte[] GetMultipartFormData(string boundary, string controlName,
        //    string fileName, byte[] fileData)
        //{
        //    Encoding encoding = Encoding.UTF8;
        //    MemoryStream formDataStream = new MemoryStream();
        //    using (formDataStream)
        //    {
        //        bool needsCLRF = false;

        //        // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
        //        // Skip it on the first parameter, add it to subsequent parameters.
        //        if (needsCLRF)
        //            formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

        //        needsCLRF = true;

        //        // Add just the first part of this param, since we will write the file data directly to the Stream
        //        string header = string.Format(ObjectUtil.SysCulture,
        //            "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
        //            boundary, controlName, Path.GetFileName(fileName), GetContentType(fileName));

        //        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

        //        // Write the file data directly to the Stream, rather than serializing it to a string.
        //        formDataStream.Write(fileData, 0, fileData.Length);

        //        //string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
        //        //    boundary,
        //        //    param.Key,
        //        //    param.Value);

        //        //formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));

        //        // Add the end of the request.  Start with a newline
        //        string footer = "\r\n--" + boundary + "--\r\n";
        //        formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

        //        return formDataStream.ToArray();
        //    }
        //}
    }
}