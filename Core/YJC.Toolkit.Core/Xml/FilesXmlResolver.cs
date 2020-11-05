using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace YJC.Toolkit.Xml
{
    internal class FilesXmlResolver : XmlUrlResolver
    {
        private readonly List<string> fFileList;

        public FilesXmlResolver()
        {
            fFileList = new List<string>();
        }

        public string[] GetFileNames()
        {
            IEnumerable<string> files = fFileList.Distinct();
            List<string> result = new List<string>(files);
            return result.ToArray();
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            fFileList.Add(absoluteUri.LocalPath);

            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}
