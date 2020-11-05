namespace YJC.Toolkit.Sys.Evaluator
{
    internal class MemberToken : OpToken
    {
        public MemberToken()
        {
            Value = ".";
        }

        public string Name { get; set; }
    }
}