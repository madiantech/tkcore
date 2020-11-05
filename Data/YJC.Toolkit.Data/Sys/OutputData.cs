using System.Data;
using System.Xml;

namespace YJC.Toolkit.Sys
{
    public sealed class OutputData
    {
        private OutputData(SourceOutputType outputType, object data)
        {
            OutputType = outputType;
            Data = data;
        }

        public SourceOutputType OutputType { get; private set; }

        public object Data { get; private set; }

        public static OutputData Create(string data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            return new OutputData(SourceOutputType.String, data);
        }

        public static OutputData Create(XmlReader data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            return new OutputData(SourceOutputType.XmlReader, data);
        }

        public static OutputData Create(DataSet data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            return new OutputData(SourceOutputType.DataSet, data);
        }

        public static OutputData Create(FileContent data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            return new OutputData(SourceOutputType.FileContent, data);
        }

        public static OutputData CreateToolkitObject(object data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            return new OutputData(SourceOutputType.ToolkitObject, data);
        }

        public static OutputData CreateObject(object data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            return new OutputData(SourceOutputType.Object, data);
        }
    }
}
