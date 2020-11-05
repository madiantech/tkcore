using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Sys
{
    internal interface IEvaluatorExtension
    {
        object Execute(string expression, params (string Name, object Value)[] additionObjs);

        Func<object, object> BuildDynamicGetter(Type targetType, string propertyName);
    }
}