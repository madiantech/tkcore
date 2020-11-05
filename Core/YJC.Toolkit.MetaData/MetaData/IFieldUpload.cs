namespace YJC.Toolkit.MetaData
{
    public interface IFieldUpload
    {
        string FileNameField { get; }

        string ServerPathField { get; }

        string ContentField { get; }

        string MimeTypeField { get; }

        string SizeField { get; }
    }
}
