namespace YJC.Toolkit.Xml
{
    internal enum XmlReaderState
    {
        Initial,
        Root,
        Row,
        Field,
        FieldValue,
        EndField,
        EndRow,
        EndRoot,
        Eof,
        Close
    }
}
