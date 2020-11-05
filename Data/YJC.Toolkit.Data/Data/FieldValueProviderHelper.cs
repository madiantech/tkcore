using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [EvaluateAddition(Author = "YJC", CreateDate = "2018-04-17",
        Description = "")]
    public static class FieldValueProviderHelper
    {
        public static object GetValue(IFieldValueProvider provider, string nickName)
        {
            return provider[nickName];
        }

        public static object GetStringValue(IFieldValueProvider provider, string nickName)
        {
            object value = provider[nickName];
            return value.ConvertToString();
        }

        public static bool IsNull(object value)
        {
            return value == DBNull.Value;
        }
    }
}