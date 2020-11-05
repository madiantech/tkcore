namespace YJC.Toolkit.Sys.Evaluator
{
    internal class OpToken : Token
    {
        public OpToken()
        {
            IsOperator = true;
            ArgCount = 0;
        }
    }
}