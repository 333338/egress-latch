using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IpGuard {
    internal sealed class ClaudeDesktopLifetime {
        const uint OwnerWindow = 4;
        const int ExtendedStyle = -20;
        const int ToolWindow = 0x80;
        const int AppWindow = 0x40000;
        static readonly TimeSpan CloseDelay = TimeSpan.FromSeconds(2);
        delegate bool WindowCallback(IntPtr window, IntPtr parameter);
        [DllImport("user32.dll")] static extern bool EnumWindows(WindowCallback callback, IntPtr parameter);
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr window, out uint pid);
        [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr window);
        [DllImport("user32.dll")] static extern IntPtr GetWindow(IntPtr window, uint command);
        [DllImport("user32.dll", EntryPoint="GetWindowLongW")] static extern int GetWindowLong(IntPtr window, int index);

        readonly uint pid;
        readonly Stopwatch missing = new Stopwatch();
        bool seen;

        public ClaudeDesktopLifetime(uint pid) { this.pid = pid; }
        public bool Closed { get; private set; }

        public bool CheckClosed() {
            bool visible = false;
            bool scanned = EnumWindows((window, parameter) => {
                uint ownerPid;
                if(!visible && GetWindowThreadProcessId(window, out ownerPid)!=0 && ownerPid==pid && IsWindowVisible(window)) {
                    int style=GetWindowLong(window, ExtendedStyle);
                    visible=(style & ToolWindow)==0 && ((style & AppWindow)!=0 || GetWindow(window, OwnerWindow)==IntPtr.Zero);
                }
                return true;
            }, IntPtr.Zero);

            if(!scanned || visible) {
                seen=seen || visible;
                missing.Reset();
            } else if(seen) {
                missing.Start();
                Closed=missing.Elapsed>=CloseDelay;
            }
            return Closed;
        }
    }
}
