namespace YJC.Toolkit.Data
{
    /// <summary>
    /// IParamBuilder 的摘要说明。
    /// </summary>
    public interface IParamBuilder
    {
        string Sql { get; }

        DbParameterList Parameters { get; }
    }
}
