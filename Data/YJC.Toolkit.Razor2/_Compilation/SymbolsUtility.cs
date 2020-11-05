using System;
using System.Runtime.InteropServices;

namespace YJC.Toolkit.Razor
{
    internal static class SymbolsUtility
    {
        // Native pdb writer's CLSID
        private const string SymWriterGuid = "0AE2DEB0-F901-478b-BB9F-881EE8066788";

        public static bool SupportsFullPdbGeneration()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Cross-plat always produce portable pdbs.
                return false;
            }

            if (Type.GetType("Mono.Runtime") != null)
            {
                return false;
            }

            try
            {
                // Check for the pdb writer component that roslyn uses to generate pdbs
                var type = Marshal.GetTypeFromCLSID(new Guid(SymWriterGuid));
                if (type != null)
                {
                    // This line will throw if pdb generation is not supported.
                    Activator.CreateInstance(type);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
    }
}