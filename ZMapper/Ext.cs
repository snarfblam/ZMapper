using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZMapper
{
    static class Ext
    {
        public static void Raise<T>(this EventHandler<T> evt, object sender, T eventArgs) where T : EventArgs {
            if (evt != null) evt(sender, eventArgs);
        }

        public static int Clamp(this int i, int min, int max) {
            return i < min ? min : (i > max ? max : i);
        }
    }
}
