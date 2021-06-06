using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZMapper
{
    static class Win32
    {
        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr
        public static IntPtr SetWindowLongPtr(HWnd hWnd, int nIndex, IntPtr dwNewLong) {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }
        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr
        public static IntPtr GetWindowLongPtr(HWnd hWnd, int nIndex) {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetWindowLong32(hWnd, nIndex));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HWnd hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern int GetWindowLong32(HWnd hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HWnd hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(HWnd hWnd, int nIndex);

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public static IntPtr SetExStyle(HWnd window, int styleBits, bool state) {
            var style = (long)GetWindowLongPtr(window, GWL_EXSTYLE);
            if (state) {
                style |= (uint)styleBits;
            } else {
                style &= (long)(uint)(~styleBits);
            }
            var newStyle = new IntPtr(style);
            return SetWindowLongPtr(window, GWL_EXSTYLE, newStyle);
        }
    }

    struct HWnd
    {
        public IntPtr Handle;
        public HWnd(IntPtr val) { this.Handle = val; }
        public static implicit operator HWnd(IntPtr value) { return new HWnd(value); }
        public static implicit operator IntPtr(HWnd value) { return value.Handle; }
    }

    

}
