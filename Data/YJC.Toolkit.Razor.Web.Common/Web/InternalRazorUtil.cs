using System;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class InternalRazorUtil
    {
        public static Func<ISource, IInputData, OutputData, bool> IsStyle(PageStyle style)
        {
            return (source, input, output) => input.Style.Style == style;
        }

        public static Func<ISource, IInputData, OutputData, bool> IsOperation(params string[] args)
        {
            return (source, input, output) => input.Style.Style == PageStyle.Custom
                && args.Contains(input.Style.Operation);
        }

        public static Func<ISource, IInputData, OutputData, bool> IsEditStyle(bool isPost)
        {
            return (source, input, output) => input.IsPost == isPost &&
                (input.Style.Style == PageStyle.Insert || input.Style.Style == PageStyle.Update);
        }

        public static Func<ISource, IInputData, OutputData, bool> IsEditStyle(bool isPost, PageStyle style)
        {
            return (source, input, output) => input.IsPost == isPost &&
                input.Style.Style == style;
        }

        public static void SetDialogMode(object pageData, bool dialogMode)
        {
            ISupportDialog support = pageData as ISupportDialog;
            if (support != null)
                support.SetDialogMode(dialogMode);
        }
    }
}