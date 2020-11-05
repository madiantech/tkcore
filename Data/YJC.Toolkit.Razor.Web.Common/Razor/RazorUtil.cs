using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public static class RazorUtil
    {
        public const string TEMPLATE_KEY = "^";
        public const string RELATIVE_KEY = "@";
        public const string TOOLKIT_ENGINE_NAME = "Toolkit";
        public const string LIST_ENGINE_NAME = "List";
        public const string MULTIEDIT_ENGINE_NAME = "MultiEdit";

        private const string BASE_TYPE = "global::YJC.Toolkit.Razor.ToolkitTemplatePage<TModel>";
        private const string BASE_LIST_TYPE = "global::YJC.Toolkit.Razor.ListTemplatePage<TModel>";
        private const string BASE_MULTI_EDIT_TYPE = "global::YJC.Toolkit.Razor.MultiEditTemplatePage<TModel>";

        public static readonly IRazorEngine ToolkitEngine;
        public static readonly IRazorEngine TextEngine;
        public static readonly IRazorEngine ListEngine;
        public static readonly IRazorEngine MultiEditEngine;

        static RazorUtil()
        {
            var builder = new RazorEngineBuilder().UseToolkitProject().UseBaseType(BASE_TYPE);
            ToolkitEngine = builder.Build();
            builder = new RazorEngineBuilder().UseToolkitProject().UseBaseType(BASE_LIST_TYPE);
            ListEngine = builder.Build();
            builder = new RazorEngineBuilder().UseToolkitProject().UseBaseType(BASE_MULTI_EDIT_TYPE);
            MultiEditEngine = builder.Build();
            TextEngine = new RazorEngineBuilder().Build();
        }

        internal static bool IsTemplate(string templateKey)
        {
            return templateKey.StartsWith(TEMPLATE_KEY);
        }

        internal static string MakeRelativePath(string baseKey, string relativeKey)
        {
            var isTemplate = IsTemplate(baseKey);
            baseKey = baseKey.Substring(1);
            string result = FileUtil.JoinPath(baseKey, relativeKey);
            if (isTemplate)
                result = TEMPLATE_KEY + result;
            return result;
        }
    }
}