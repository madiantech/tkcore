namespace YJC.Toolkit.Data
{
    /// <summary>
    /// IParamBuilder ��ժҪ˵����
    /// </summary>
    public interface IParamBuilder
    {
        string Sql { get; }

        DbParameterList Parameters { get; }
    }
}
