using System.Linq.Expressions;

namespace YJC.Toolkit.Sys
{
    public abstract class ExpressionCompiler
    {
        //protected string Pstr = null;

        protected ExpressionCompiler()
        {
            TypeRegistry = new TypeRegistry();
        }

        public TypeRegistry TypeRegistry { get; private set; }

        public Expression Expression { get; set; }

        internal Parser Parser { get; set; }

        public string StringToParse
        {
            get
            {
                return Parser.StringToParse;
            }
            set
            {
                Parser.StringToParse = value;
                Expression = null;
                ClearCompiledMethod();
            }
        }

        public void RegisterDefaultTypes()
        {
            TypeRegistry.RegisterDefaultTypes();
        }

        public void RegisterType(string key, object type)
        {
            TypeRegistry.Add(key, type);
        }

        protected Expression BuildTree(Expression scopeParam, bool isCall)
        {
            return Parser.BuildTree(scopeParam, isCall);
        }

        protected abstract void ClearCompiledMethod();

        protected void Parse()
        {
            Parser.Parse();
        }

        //public void RegisterNamespace(string p)
        //{
        //}

        //public void RegisterAssembly(System.Reflection.Assembly assembly)
        //{
        //}


        protected static Expression WrapExpression(Expression source, bool castToObject)
        {
            if (source.Type != typeof(void) && castToObject)
            {
                return Expression.Convert(source, typeof(object));
            }
            return Expression.Block(source, Expression.Constant(null));
        }
    }
}