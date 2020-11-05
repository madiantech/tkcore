using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace YJC.Toolkit.Sys
{
    internal static class SingletonApp
    {
        static Mutex fMutex;

        public static bool Run()
        {
            bool first = IsFirstInstance();
            if (first)
                Application.ApplicationExit += OnExit;
            return first;
        }

        static bool IsFirstInstance()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string name = assembly.FullName;

            fMutex = new Mutex(false, name);
            bool owned = fMutex.WaitOne(TimeSpan.Zero, false);
            return owned;
        }

        static void OnExit(object sender, EventArgs args)
        {
            fMutex.ReleaseMutex();
            fMutex.Close();
        }
    }
}