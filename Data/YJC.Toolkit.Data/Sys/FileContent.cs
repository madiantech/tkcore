namespace YJC.Toolkit.Sys
{
    public sealed class FileContent
    {
        private readonly byte[] fData;
        /// <summary>
        /// Initializes a new instance of the FileContent class.
        /// </summary>
        public FileContent(string contentType, string fileName, byte[] data)
            : this(contentType, data)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", this);

            FileName = fileName;
            IsWriteFileName = true;
        }

        public FileContent(string contentType, byte[] data)
        {
            TkDebug.AssertArgumentNullOrEmpty(contentType, "contentType", this);
            TkDebug.AssertArgumentNull(data, "data", this);

            ContentType = contentType;
            fData = data;
            IsWriteFileName = false;
        }

        public string ContentType { get; private set; }

        public string FileName { get; private set; }

        public bool IsWriteFileName { get; private set; }

        public byte[] GetData()
        {
            return fData;
        }
    }
}
