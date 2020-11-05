using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FieldUploadAttribute : Attribute, IFieldUpload
    {
        public FieldUploadAttribute()
            : this(null, null, null, null, null)
        {
        }

        public FieldUploadAttribute(string fileNameField, string serverPathField, 
            string contentField, string mimeTypeField, string sizeField)
        {
            FileNameField = string.IsNullOrEmpty(fileNameField) ? "FileName" : fileNameField;
            ServerPathField = string.IsNullOrEmpty(serverPathField) ? "ServerPath" : serverPathField;
            ContentField = string.IsNullOrEmpty(contentField) ? "Content" : contentField;
            MimeTypeField = string.IsNullOrEmpty(mimeTypeField) ? "ContentType" : mimeTypeField;
            SizeField = string.IsNullOrEmpty(sizeField) ? "Size" : sizeField;
        }
        #region IFieldUpload 成员

        public string FileNameField { get; private set; }

        public string ServerPathField { get; private set; }

        public string ContentField { get; private set; }

        public string MimeTypeField { get; private set; }

        public string SizeField { get; private set; }

        #endregion
    }
}
