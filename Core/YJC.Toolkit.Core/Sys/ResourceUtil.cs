using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace YJC.Toolkit.Sys
{
    /// <remarks>Class<c>ResourceUtil</c>����Դ�ļ����������ࡣ�ܷ��࣬���ܱ��̳У��ṩһϵ�д�����Դ�ļ��ľ�̬����
    /// </remarks>
    /// <summary>��Դ�ļ�����������</summary>
    public static class ResourceUtil
    {
        /// <summary>
        /// �����Դ�ļ���ResourceManager
        /// </summary>
        /// <param name="assembly">����</param>
        /// <param name="partResName">��Դ�ļ��Ľڵ�����</param>
        /// <returns>��Դ�ļ���ResourceManager</returns>
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
        /// ���Ƕ����򼯵���Դ
        /// </summary>
        /// <param name="assembly">��������</param>
        /// <param name="partName">��Դ�ļ���</param>
        /// <returns>Ƕ����򼯵���Դ</returns>
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
