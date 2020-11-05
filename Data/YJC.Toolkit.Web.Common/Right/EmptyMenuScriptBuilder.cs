using System.Data;

namespace YJC.Toolkit.Right
{
    public class EmptyMenuScriptBuilder : IMenuScriptBuilder
    {
        #region IMenuScriptBuilder 成员

        public virtual string GetMenuScript(DataSet menuDataSet)
        {
            return string.Empty;
        }

        #endregion
    }
}
