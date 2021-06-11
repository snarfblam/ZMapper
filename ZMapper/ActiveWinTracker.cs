using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ZMapper
{
    class ActiveWinTracker: IDisposable
    {
        WinHookFunc handler = null;
        Form owner;
        IntPtr ownerHandle;
        IntPtr hHook;
        IntPtr activeWindow;
        public event EventHandler<ActiveWindowEventArgs> ActiveWindowChanged;
        public bool IgnoreOwner { get; set; }
        Timer pollTimer = new Timer() {
            Enabled = false,
            Interval = 1000,
        };

        public ActiveWinTracker(Form owner) {
            this.owner = owner;
            this.ownerHandle = owner.Handle;

            handler = new WinHookFunc(WinEventProc);
            hHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, handler, 0, 0, WINEVENT_OUTOFCONTEXT);
            this.pollTimer.Tick += OnTimerTick;
            this.pollTimer.Start();
        }

        void OnTimerTick(object sender, EventArgs e) {
            var active = GetForegroundWindow();
            if (active != IntPtr.Zero && active != activeWindow) {
                activeWindow = active;

                var activeTitle = GetWindowTitle(active);
                var activeClass = GetWindowClass(active);
                this.ActiveWindowChanged.Raise(this, new ActiveWindowEventArgs(active, activeTitle, activeClass));
            }
        }

        delegate void WinHookFunc(IntPtr hwndEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinHookFunc lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hwndEventHook);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private string GetWindowTitle(IntPtr hwnd) {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(hwnd, Buff, nChars) > 0) {
                return Buff.ToString();
            }
            return null;
        }
        private string GetWindowClass(IntPtr hwnd) {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetClassName(hwnd, Buff, nChars) > 0) {
                return Buff.ToString();
            }
            return null;
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
            if (this.ownerHandle != hwnd || !IgnoreOwner) {
                var title = GetWindowTitle(hwnd);
                var cls = GetWindowClass(hwnd);
                activeWindow = hwnd;
                ActiveWindowChanged.Raise(this, new ActiveWindowEventArgs(hwnd, title, cls));
            }
        }


        public void Close() {
            UnhookWinEvent(hHook);
        }
        void IDisposable.Dispose() {
            Close();
        }
    }

    class ActiveWindowEventArgs:EventArgs {
        public ActiveWindowEventArgs(IntPtr hWnd, string title, string cls) {
            this.WindowTitle = title;
            this.WindowClass = cls;
            this.HWnd = hWnd;
        }
        public string WindowTitle { get; private set; }
        public string WindowClass { get; private set; }
        public IntPtr HWnd { get; private set; }
    }
}