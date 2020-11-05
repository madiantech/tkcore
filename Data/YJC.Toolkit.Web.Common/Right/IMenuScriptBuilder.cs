using System.Data;

namespace YJC.Toolkit.Right
{
    public interface IMenuScriptBuilder
    {
        string GetMenuScript(DataSet menuDataSet);
    }
}
