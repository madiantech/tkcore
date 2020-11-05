using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class ToolkitRazorProject : TkRazorProject
    {
        private readonly static string Extension = FileSystemRazorProject.DefaultExtension;

        private readonly static IFileProvider fRazorFileProvider;
        private readonly static IFileProvider fTemplateFileProvider;
        private readonly static string fRazorRoot;
        private readonly static string fTemplateRoot;

        static ToolkitRazorProject()
        {
            TkDebug.ThrowIfNoAppSetting();

            fRazorRoot = Path.Combine(BaseAppSetting.Current.XmlPath, "razor");
            if (!Directory.Exists(fRazorRoot))
                Directory.CreateDirectory(fRazorRoot);
            fRazorFileProvider = new PhysicalFileProvider(fRazorRoot);
            fTemplateRoot = Path.Combine(BaseAppSetting.Current.XmlPath, "razortemplate");
            if (!Directory.Exists(fTemplateRoot))
                Directory.CreateDirectory(fTemplateRoot);
            fTemplateFileProvider = new PhysicalFileProvider(fTemplateRoot);
        }

        public override Task<TkRazorProjectItem> GetItemAsync(string templateKey)
        {
            TkDebug.AssertArgumentNullOrEmpty(templateKey, nameof(templateKey), this);

            if (!templateKey.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
            {
                templateKey += Extension;
            }
            var sourceKey = templateKey;
            bool isTemplate = RazorUtil.IsTemplate(templateKey);
            if (isTemplate)
                templateKey = templateKey.Substring(1);

            string absolutePath = NormalizeKey(isTemplate ? fTemplateRoot : fRazorRoot, templateKey);
            var item = new ToolkitRazorProjectItem(sourceKey, new FileInfo(absolutePath));

            if (item.Exists)
            {
                //item.ExpirationToken = (isTemplate ? fTemplateFileProvider : fRazorFileProvider).Watch(templateKey);
            }

            return Task.FromResult((TkRazorProjectItem)item);
        }

        private string NormalizeKey(string root, string templateKey)
        {
            TkDebug.AssertArgumentNullOrEmpty(templateKey, nameof(templateKey), this);

            var absolutePath = templateKey;
            if (!absolutePath.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                if (templateKey[0] == '/' || templateKey[0] == '\\')
                {
                    templateKey = templateKey.Substring(1);
                }

                absolutePath = Path.Combine(root, templateKey);
            }

            absolutePath = absolutePath.Replace('\\', '/');

            return absolutePath;
        }

        public override Task<IEnumerable<TkRazorProjectItem>> GetImportsAsync(string templateKey)
        {
            return Task.FromResult(Enumerable.Empty<TkRazorProjectItem>());
        }
    }
}