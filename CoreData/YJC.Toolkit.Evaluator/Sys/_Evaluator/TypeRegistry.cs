using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    [Serializable]
    public class TypeRegistry : Dictionary<string, object>
    {
        public TypeRegistry()
        {
            //Add default aliases
            Add("bool", typeof(System.Boolean));
            Add("byte", typeof(System.Byte));
            Add("char", typeof(System.Char));
            Add("int", typeof(System.Int32));
            Add("decimal", typeof(System.Decimal));
            Add("double", typeof(System.Double));
            Add("float", typeof(System.Single));
            Add("object", typeof(System.Object));
            Add("string", typeof(System.String));
        }

        public void RegisterDefaultTypes()
        {
            Add("DateTime", typeof(System.DateTime));
            Add("Convert", typeof(System.Convert));
            Add("Math", typeof(System.Math));
        }
    }

}
