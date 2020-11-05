using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace YJC.Toolkit.Sys
{
    /// <remarks>Class<c>ResourceUtil</c>：资源文件操作工具类。密封类，不能被继承，提供一系列处理资源文件的静态方法
    /// </remarks>
    /// <summary>资源文件操作工具类</summary>
    public static class ResourceUtil
    {
        /// <summary>
        /// 获得资源文件的ResourceManager
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="partResName">资源文件的节点名称</param>
        /// <returns>资源文件的ResourceManager</returns>
        public static ResourceManager GetResourceManager(Assembly assembly, string partResName)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", null);
            TkDebug.AssertArgumentNullOrEmpty(partResName, "partResName", null);

            string[] resNames = assembly.GetManifestResourceNames();
            string resName = null;
            foreach (string name in resNames)
            {
                if (name.IndexOf(partResName, StringComparison.Ordinal) != -1)
                {
                    resName = name;
                    break;
                }
            }
            if (resName == null)
                return null;
            resName = resName.Substring(0, resName.Length - 10); // remove ".resources"
            return new ResourceManager(resName, assembly);
        }

        /// <summary>
        /// 获得嵌入程序集的资源
        /// </summary>
        /// <param name="assembly">程序集名称</param>
        /// <param name="partName">资源文件名</param>
        /// <returns>嵌入程序集的资源</returns>
        public static Stream GetEmbeddedResource(Assembly assembly, string partName)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", null);
            TkDebug.AssertArgumentNullOrEmpty(partName, "partName", null);

            string[] names = assembly.GetManifestResourceNames();
            partName = "." + partName.ToLower();

            foreach (string name in names)
            {
                if (name.ToLower().IndexOf(
                    partName, StringComparison.Ordinal) != -1)
                    return assembly.GetManifestResourceStream(name);
            }
            return null;
        }

        public static Stream GetEmbeddedResource(string partName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return GetEmbeddedResource(assembly, partName);
        }
    }
}
