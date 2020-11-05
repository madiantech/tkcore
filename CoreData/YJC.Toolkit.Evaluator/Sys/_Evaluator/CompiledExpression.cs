using System;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys
{

    /// <summary>
    /// Creates compiled expressions with return values that are of type T
    /// </summary>
    public class CompiledExpression<TResult> : ExpressionCompiler
    {
        private Func<TResult> fCompiledMethod;
        private Action fCompiledAction;

        public CompiledExpression()
        {
            Parser = new Parser
            {
                TypeRegistry = TypeRegistry
            };
        }

        public CompiledExpression(string expression)
        {
            Parser = new Parser(expression)
            {
                TypeRegistry = TypeRegistry
            };
        }

        public Func<TResult> Compile()
        {
            if (Expression == null)
                Expression = WrapExpression(BuildTree(null, false), false);
            return Expression.Lambda<Func<TResult>>(Expression).Compile();
        }

        /// <summary>
        /// Compiles the expression to a function that returns void
        /// </summary>
        /// <returns></returns>
        public Action CompileCall()
        {
            if (Expression == null)
                Expression = BuildTree(null, true);
            return Expression.Lambda<Action>(Expression).Compile();
        }


        /// <summary>
        /// Compiles the expression to a function that takes an object as a parameter and returns an object
        /// </summary>s
        /// <returns></returns>
        public Action<T> ScopeCompileCall<T>()
        {
            var scopeParam = Expression.Parameter(typeof(T), "scope");
            if (Expression == null)
                Expression = BuildTree(scopeParam, true);
            return Expression.Lambda<Action<T>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }


        public Func<object, TResult> ScopeCompile()
        {
            var scopeParam = Expression.Parameter(typeof(object), "scope");
            if (Expression == null)
                Expression = WrapExpression(BuildTree(scopeParam, false), true);
            return Expression.Lambda<Func<dynamic, TResult>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }

        public Func<T, TResult> ScopeCompile<T>()
        {
            var scopeParam = Expression.Parameter(typeof(T), "scope");
            if (Expression == null)
                Expression = WrapExpression(BuildTree(scopeParam, false), true);
            return Expression.Lambda<Func<T, TResult>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }

        protected override void ClearCompiledMethod()
        {
            fCompiledMethod = null;
            fCompiledAction = null;
        }

        public TResult Eval()
        {
            if (fCompiledMethod == null)
                fCompiledMethod = Compile();
            return fCompiledMethod();
        }

        public void Call()
        {
            if (fCompiledAction == null)
                fCompiledAction = CompileCall();
            fCompiledAction();
        }

        public object Global
        {
            get
            {
                return Parser.Global;
            }
            set
            {
                Parser.Global = value;
            }
        }

    }

    /// <summary>
    /// Creates compiled expressions with return values that are cast to type Object 
    /// </summary>
    public class CompiledExpression : ExpressionCompiler
    {
        private Func<object> fCompiledMethod;
        private Action fCompiledAction;

        public CompiledExpression()
        {
            Parser = new Parser() { TypeRegistry = TypeRegistry };

        }

        public CompiledExpression(string expression)
        {
            Parser = new Parser(expression) { TypeRegistry = TypeRegistry };
        }

        /// <summary>
        /// Compiles the expression to a function that returns an object
        /// </summary>
        /// <returns></returns>
        public Func<object> Compile()
        {
            if (Expression == null)
                Expression = WrapExpression(BuildTree(null, false), true);
            return Expression.Lambda<Func<object>>(Expression).Compile();
        }

        /// <summary>
        /// Compiles the expression to a function that returns void
        /// </summary>
        /// <returns></returns>
        public Action CompileCall()
        {
            if (Expression == null)
                Expression = BuildTree(null, true);
            return Expression.Lambda<Action>(Expression).Compile();
        }

        /// <summary>
        /// Compiles the expression to a function that takes an object as a parameter and returns an object
        /// </summary>
        /// <returns></returns>
        public Func<object, object> ScopeCompile()
        {
            var scopeParam = Expression.Parameter(typeof(object), "scope");
            if (Expression == null)
                Expression = WrapExpression(BuildTree(scopeParam, false), true);
            return Expression.Lambda<Func<dynamic, object>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }

        /// <summary>
        /// Compiles the expression to a function that takes an object as a parameter and returns an object
        /// </summary>
        /// <returns></returns>
        public Action<object> ScopeCompileCall()
        {
            var scopeParam = Expression.Parameter(typeof(object), "scope");
            if (Expression == null)
                Expression = BuildTree(scopeParam, true);
            return Expression.Lambda<Action<dynamic>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }

        /// <summary>
        /// Compiles the expression to a function that takes an object as a parameter and returns an object
        /// </summary>s
        /// <returns></returns>
        public Action<T> ScopeCompileCall<T>()
        {
            var scopeParam = Expression.Parameter(typeof(T), "scope");
            if (Expression == null)
                Expression = BuildTree(scopeParam, false);
            return Expression.Lambda<Action<T>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }

        /// <summary>
        /// Compiles the expression to a function that takes an typed object as a parameter and returns an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Func<T, object> ScopeCompile<T>()
        {
            var scopeParam = Expression.Parameter(typeof(T), "scope");
            if (Expression == null)
                Expression = WrapExpression(BuildTree(scopeParam, false), true);
            return Expression.Lambda<Func<T, object>>(Expression,
                new ParameterExpression[] { scopeParam }).Compile();
        }

        protected override void ClearCompiledMethod()
        {
            fCompiledMethod = null;
            fCompiledAction = null;
        }

        public object Eval()
        {
            if (fCompiledMethod == null)
                fCompiledMethod = Compile();
            return fCompiledMethod();
        }

        public void Call()
        {
            if (fCompiledAction == null)
                fCompiledAction = CompileCall();
            fCompiledAction();
        }

        public object Global
        {
            get
            {
                return Parser.Global;
            }
            set
            {
                Parser.Global = value;
            }
        }
    }
}
