using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    public class Tk55Parser
    {
        public Tk55Parser(string parser, string moduleCreator, IPageStyle style, string source)
        {
            Parser = parser;
            ModuleCreator = moduleCreator;
            Style = style;
            Source = source;
        }

        public string Parser { get; }

        public string ModuleCreator { get; }

        public IPageStyle Style { get; }

        public string Source { get; }

        public static Tk55Parser Parse(string url)
        {
            IEnumerable<string> GetSourcePart(string[] part)
            {
                for (int i = 4; i < part.Length; ++i)
                {
                    string s = part[i];
                    if (s.Contains('?'))
                    {
                        s = s.Substring(0, s.IndexOf('?'));
                    }
                    yield return s;
                }
            }

            if (string.IsNullOrEmpty(url))
                return null;
            url = BaseGlobalVariable.Current.ResolveUrl(url);
            var data = url.Split('/');
            if (data.Length >= 5)
            {
                return new Tk55Parser(data[1], data[2], data[3].Value<PageStyleClass>(),
                    string.Join("/", GetSourcePart(data)));
            }
            return null;
        }

        internal static Tk55Parser Create(FunctionItem function, SubFuncClass subFunc)
        {
            if (string.IsNullOrEmpty(subFunc.Content))
            {
                if (function.Parser == null)
                    return null;
                IPageStyle style = subFunc.NameId.Value<PageStyleClass>();
                return new Tk55Parser(function.Parser.Parser, function.Parser.ModuleCreator,
                    style, function.Parser.Source);
            }
            else
                return Parse(subFunc.Content);
        }
    }
}