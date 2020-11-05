using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig]
    internal class VueFrameSourceConfig : IConfigCreator<ISource>
    {
        [SimpleAttribute]
        public string Model { get; private set; }

        [SimpleAttribute]
        public string Template { get; private set; }

        public ISource CreateObject(params object[] args)
        {
            return new VueFrameSource(Model, Template);
        }
    }
}