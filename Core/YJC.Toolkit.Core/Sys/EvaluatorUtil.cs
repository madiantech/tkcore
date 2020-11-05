using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public static class EvaluatorUtil
    {
        private static readonly Dictionary<string, object> fAdditionObj = new Dictionary<string, object>();

        internal static Dictionary<string, object> AdditionObj
        {
            get => fAdditionObj;
        }

        public static object Execute(string expression)
        {
            return Execute(expression, ((string Name, object Value)[])null);
        }

        public static T Execute<T>(string expression)
        {
            return Execute<T>(expression, ((string Name, object Value)[])null);
        }

        public static object Execute(string expression, params (string Name, object Value)[] additionObjs)
        {
            TkDebug.AssertArgumentNullOrEmpty(expression, "expression", null);

            return EvaluatorExtension.Extension.Execute(expression, additionObjs);
        }

        public static T Execute<T>(string expression, params (string Name, object Value)[] additionObjs)
        {
            object result = Execute(expression, additionObjs);
            return (T)result;
        }

        internal static void AddAdditionObj((string Name, object Value) item)
        {
            fAdditionObj.Add(item.Name, item.Value);
        }
    }
}