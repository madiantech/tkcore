using System;

namespace YJC.Toolkit.Sys
{
    public class UrlBuilder
    {
        private readonly UriBuilder fBuilder;
        private readonly QueryStringBuilder fQueryString;

        public UrlBuilder(Uri uri)
        {
            fBuilder = new UriBuilder(uri);
            fQueryString = new QueryStringBuilder(fBuilder.Query);
        }

        public string Path
        {
            get
            {
                return fBuilder.Path;
            }
            set
            {
                if (string.IsNullOrEmpty(fBuilder.Path))
                    fBuilder.Path = value;
                else
                {
                    if (string.IsNullOrEmpty(value))
                        return;
                    if (value[0] == '/')
                        value = value.Substring(1);
                    int index = value.IndexOf('?');
                    if (index > 0)
                    {
                        string queryString = value.Substring(index + 1);
                        fQueryString.AddQueryString(queryString);
                        value = value.Substring(0, index);
                    }
                    fBuilder.Path = System.IO.Path.Combine(fBuilder.Path, value);
                }
            }
        }

        public void AddQueryString(string name, string value)
        {
            fQueryString.Add(name, value);
        }

        public Uri ToUri()
        {
            fBuilder.Query = fQueryString.ToString();
            return fBuilder.Uri;
        }
    }
}