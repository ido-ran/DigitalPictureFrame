using System;

namespace DigitalFrame.Core.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            EventHandler evt = handler;
            if (evt != null) evt(sender, args);
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            EventHandler<T> evt = handler;
            if (evt != null) evt(sender, args);
        }
    }
}