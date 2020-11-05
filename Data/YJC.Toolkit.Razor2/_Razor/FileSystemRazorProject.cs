using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class FileSystemRazorProject : TkRazorProject
    {
        public const string DefaultExtension = ".cshtml";
        private readonly IFileProvider fFileProvider;

        public FileSystemRazorProject(string root)
            : this(root, DefaultExtension)
        {
        }

        public FileSystemRazorProject(string root, string extension)
        {
            TkDebug.AssertArgumentNullOrEmpty(extension, nameof(extension), this);
            TkDebug.AssertArgumentNullOrEmpty(root, nameof(root), this);
            TkDebug.AssertArgument(Directory.Exists(root), nameof(root),
                $"Root directory {root} not found", this);

            Extension = extension;
            Root = root;
            fFileProvider = new PhysicalFileProvider(Root);
        }

        public virtual string Extension { get; set; }

        public string Root { get; }

        public override Task<TkRazorProjectItem> GetItemAsync(string templateKey)
        {
            if (!templateKey.EndsWith(Extension))
            {
                templateKey = templateKey + Extension;
            }

            string absolutePath = NormalizeKey(templateKey);
            var item = new FileSystemRazorProjectItem(templateKey, new FileInfo(absolutePath));

            if (item.Exists)
            {
                item.ExpirationToken = fFileProvider.Watch(templateKey);
            }

            return Task.FromResult((TkRazorProjectItem)item);
        }

        protected string NormalizeKey(string templateKey)
        {
            TkDebug.AssertArgumentNullOrEmpty(templateKey, nameof(templateKey), this);

            var absolutePath = templateKey;
            if (!absolutePath.StartsWith(Root, StringComparison.OrdinalIgnoreCase))
            {
                if (templateKey[0] == '/' || templateKey[0] == '\\')
                {
                    templateKey = templateKey.Substring(1);
                }

                absolutePath = Path.Combine(Root, templateKey);
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