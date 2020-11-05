using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    [TkTypeConverter(typeof(QuoteStringListTypeConverter))]
    public class QuoteStringList
    {
        private readonly HashSet<string> fHashSet;

        public QuoteStringList()
        {
            fHashSet = new HashSet<string>();
        }

        public int Count
        {
            get
            {
                return fHashSet.Count;
            }
        }

        private void ReadFromString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            string[] array = text.Split(',');
            foreach (string item in array)
            {
                string value = item.Trim('"', ' ', '\t');
                Add(value);
            }
        }

        public bool Contains(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return fHashSet.Contains(text);
        }

        public List<T> ConvertToList<T>()
        {
            if (fHashSet.Count == 0)
                return null;

            List<T> list = new List<T>(fHashSet.Count);
            foreach (string item in fHashSet)
                list.Add(item.Value<T>());

            return list;
        }

        public void Add(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            fHashSet.Add(value);
        }

        public void Add(IEnumerable<string> values)
        {
            if (values == null)
                return;

            foreach (var item in values)
                Add(item);
        }

        public IEnumerable<string> CreateEnumerable()
        {
            var result = from item in fHashSet
                         orderby item
                         select item;
            return result;
        }

        public override string ToString()
        {
            var result = from item in fHashSet
                         orderby item
                         select GetQuoteId(item);

            return string.Join(",", result);
        }

        public static string GetQuoteId(string id)
        {
            return string.Format(ObjectUtil.SysCulture, "\"{0}\"", id);
        }

        public static QuoteStringList FromString(string text)
        {
            QuoteStringList result = new QuoteStringList();
            result.ReadFromString(text);

            return result;
        }

        public static explicit operator QuoteStringList(string text)
        {
            return FromString(text);
        }
    }
}