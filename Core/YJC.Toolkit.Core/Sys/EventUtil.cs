using System;
using System.ComponentModel;

namespace YJC.Toolkit.Sys
{
    public static class EventUtil
    {
        public static void ExecuteEventHandler<T>(EventHandlerList eventHandlers,
            string key, object sender, T e) where T : EventArgs
        {
            TkDebug.AssertArgumentNull(e, "e", sender);

            EventHandler<T> handler = eventHandlers[key] as EventHandler<T>;
            if (handler != null)
                handler(sender, e);
        }
    }
}
