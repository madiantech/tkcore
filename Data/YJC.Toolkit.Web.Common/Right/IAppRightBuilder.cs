namespace YJC.Toolkit.Right
{
    public interface IAppRightBuilder
    {
        ILogOnRight CreateLogOnRight();

        IFunctionRight CreateFunctionRight();

        IMenuScriptBuilder CreateScriptBuilder();
    }
}