using System;
using System.Reflection;
using System.Threading;

namespace YJC.Toolkit.Sys
{
    internal static class SingletonApp
    {
        private static Mutex fMutex;

        public static bool Run()
        {
            bool first = IsFirstInstance();
            if (first)
                AppDomain.CurrentDomain.ProcessExit += OnExit;
            return first;
        }

        private static bool IsFirstInstance()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string name = assembly.FullName;

            fMutex = new Mutex(false, name);
            bool owned = fMutex.WaitOne(TimeSpan.Zero, false);
            return owned;
        }

        private static void OnExit(object sender, EventArgs args)
        {
            fMutex.ReleaseMutex();
            fMutex.Close();
        }
    }
}