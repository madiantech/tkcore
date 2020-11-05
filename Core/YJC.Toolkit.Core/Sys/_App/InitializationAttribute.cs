using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class InitializationAttribute : Attribute
    {
        public InitializationAttribute(Type initClassType)
            : this(initClassType, InitPriority.Normal)
        {
        }

        public InitializationAttribute(Type initClassType, InitPriority priority)
        {
            InitClassType = initClassType;
            Priority = priority;
        }

        public Type InitClassType { get; private set; }

        public InitPriority Priority { get; private set; }
    }
}
