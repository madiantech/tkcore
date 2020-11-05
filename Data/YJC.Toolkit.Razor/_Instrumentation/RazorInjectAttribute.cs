using System;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RazorInjectAttribute : Attribute
    {
    }
}