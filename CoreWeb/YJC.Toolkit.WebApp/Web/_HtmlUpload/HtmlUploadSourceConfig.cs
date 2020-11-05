using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class HtmlUploadSourceConfig : IConfigCreator<ISource>
    {
        [SimpleAttribute(Required = true)]
        public string UploadPath { get; private set; }

        [SimpleAttribute(Required = true)]
        public string VirtualPath { get; private set; }

        public ISource CreateObject(params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}