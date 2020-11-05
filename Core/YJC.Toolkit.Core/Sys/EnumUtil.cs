using System.Collections;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public static class EnumUtil
    {
        public static IEnumerable<T> Convert<T>(T value)
        {
            TkDebug.AssertArgumentNull(value, nameof(value), null);

            yield return value;
        }

        public static IEnumerable<T> Convert<T>(T value, IEnumerable<T> values)
        {
            TkDebug.AssertArgumentNull(value, nameof(value), null);

            yield return value;
            if (values != null)
                foreach (T item in values)
                    yield return item;
        }

        public static IEnumerable<T> Convert<T>(params IEnumerable<T>[] values)
        {
            TkDebug.AssertArgumentNull(values, nameof(values), null);

            foreach (var items in values)
                if (items != null)
                {
                    foreach (T item in items)
                        if (item != null)
                            yield return item;
                }
        }

        private static IEnumerable<object> InternalConvert(IEnumerable list)
        {
            foreach (var item in list)
                yield return item;
        }

        public static IEnumerable<object> Convert(IEnumerable list)
        {
            TkDebug.AssertArgumentNull(list, nameof(list), null);

            if (list is IEnumerable<object> result)
                return result;
            return InternalConvert(list);
        }
    }
}