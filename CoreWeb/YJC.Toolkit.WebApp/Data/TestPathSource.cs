using System;
using System.Web;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [Source]
    [XmlPageMaker]
    public class TestPathSource : ISource
    {
        public class PathInfo
        {
            [SimpleAttribute]
            public string LocalPath { get; set; }

            [SimpleAttribute]
            public string ServerPath { get; set; }
        }
        public OutputData DoAction(IInputData input)
        {
            var request = WebGlobalVariable.Request;
            PathInfo info = new PathInfo();
            info.LocalPath = request.Path.ToString();
            info.ServerPath = request.PathBase.ToString();
            return OutputData.CreateToolkitObject(info);
        }
    }
}