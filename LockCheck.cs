using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SelfControl
{
    static class LockCheck
    {
        // Used to check if the workstation is locked
        [DllImport("user32", SetLastError = true)]
        private static extern IntPtr OpenDesktop(string lpszDesktop,
                                                 uint dwFlags,
                                                 bool fInherit,
                                                 uint dwDesiredAccess);

        [DllImport("user32", SetLastError = true)]
        private static extern IntPtr OpenInputDesktop(uint dwFlags,
                                                      bool fInherit,
                                                      uint dwDesiredAccess);

        [DllImport("user32", SetLastError = true)]
        private static extern IntPtr CloseDesktop(IntPtr hDesktop);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SwitchDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        // Check if the workstation has been locked.
        public static bool IsWorkstationLocked()
        {
            const int DESKTOP_SWITCHDESKTOP = 256;
            IntPtr hwnd = OpenInputDesktop(0, false, DESKTOP_SWITCHDESKTOP);

            if (hwnd == IntPtr.Zero)
            {
                // Could not get the input desktop, might be locked already?
                hwnd = OpenDesktop("Default", 0, false, DESKTOP_SWITCHDESKTOP);
            }

            // Can we switch the desktop?
            if (hwnd != IntPtr.Zero)
            {
                if (SwitchDesktop(hwnd))
                {
                    // Workstation is NOT LOCKED.
                    CloseDesktop(hwnd);
                }
                else
                {
                    CloseDesktop(hwnd);
                    // Workstation is LOCKED.
                    return true;
                }
            }

            return false;
        }
    }
}
