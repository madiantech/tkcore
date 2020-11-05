using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal interface IElementWriter
    {
        int Order { get; }

        bool IsSingle { get; }

        bool IsValueMulitple { get; }

        ObjectPropertyInfo Content { get; }

        bool Required { get; }

        ObjectPropertyInfo Get(Type type);

        string PropertyName { get; }

        object GetValue(object receiver);
    }
}