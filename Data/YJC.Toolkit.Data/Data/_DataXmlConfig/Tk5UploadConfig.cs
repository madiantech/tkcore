using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class Tk5UploadConfig : IFieldUpload
    {
        internal Tk5UploadConfig()
        {
        }

        internal Tk5UploadConfig(FieldUploadAttribute attr)
            : this((IFieldUpload)attr)
        {
        }

        public Tk5UploadConfig(IFieldUpload fieldUpload)
        {
            TkDebug.AssertArgumentNull(fieldUpload, "fieldUpload", null);

            FileNameField = fieldUpload.FileNameField;
            ServerPathField = fieldUpload.ServerPathField;
            ContentField = fieldUpload.ContentField;
            MimeTypeField = fieldUpload.MimeTypeField;
            SizeField = fieldUpload.SizeField;
        }

        [SimpleAttribute(Required = true)]
        public string ServerPathField { get; private set; }

        [SimpleAttribute]
        public string ContentField { get; private set; }

        [SimpleAttribute(Required = true)]
        public string MimeTypeField { get; private set; }

        [SimpleAttribute(Required = true)]
        public string SizeField { get; private set; }

        [SimpleAttribute]
        public bool IsView { get; set; }

        [SimpleAttribute]
        public int MaxSize { get; set; }

        [SimpleAttribute]
        public string FileExt { get; set; }

        [DynamicElement(UploadProcessorConfigFactory.REG_NAME)]
        public IConfigCreator<IUploadProcessor2> Processor { get; set; }

        public string FileNameField { get; internal set; }
    }
}