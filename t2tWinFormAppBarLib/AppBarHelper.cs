using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace tip2tail.WinFormAppBarLib
{

    /// <summary>
    /// Edge to dock AppBar form to (or None for no docking)
    /// </summary>
    public enum AppBarEdge : int
    {
        Left = 0,
        Top,
        Right,
        Bottom,
        None
    }

    /// <summary>
    /// Helper library for controling AppBar forms
    /// </summary>
    public static class AppBarHelper
    {

        /// <summary>
        /// A unique identifying message that relates to this AppBar form
        /// </summary>
        public static string AppBarMessage { get; set; } = string.Empty;

        private static RECT BackupRc;
        private static bool ConfirmedInLocation = false;
        private static Dictionary<Form, RegisterInfo> RegisterFormInfo = new Dictionary<Form, RegisterInfo>();

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public override string ToString()
            {
                return string.Format("Left: {0}, Top: {1}, Right: {2}, Bottom: {3}", left, top, right, bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        private enum AppBarMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }

        private enum AppBarNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }

        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);

        private delegate void MoveAndResizeWindowDelegate(IntPtr formHandle, RECT rc);

        private class RegisterInfo : IMessageFilter
        {
            public int CallbackId { get; set; }
            public bool IsRegistered { get; set; }
            public Form WinForm { get; set; }
            public AppBarEdge Edge { get; set; }
            public FormBorderStyle OrigBorderStyle { get; set; }
            public Point OrigPosition { get; set; }
            public Size OrigSize { get; set; }
            public bool OrigOnTop { get; set; }

            public bool PreFilterMessage(ref Message m)
            {
                bool isOK = false;
                if (m.Msg == CallbackId)
                {
                    DebugLog("PreFilterMessage()", m.ToString());
                    if (m.WParam.ToInt32() == (int)AppBarNotify.ABN_POSCHANGED)
                    {
                        DebugLog("PreFilterMessage()", "Calling Set Position");
                        AppBarSetPosition(Edge, WinForm);
                        isOK = true;
                    }
                    m.Result = IntPtr.Zero;
                }
                m.Result = IntPtr.Zero;

                // Ensure the position is held...
                if (ConfirmedInLocation)
                {
                    if (WinForm.Top != BackupRc.top)
                    {
                        MoveAndResizeWindow(WinForm.Handle, BackupRc);
                    }
                }

                return isOK;
            }

        }

        #region PRIVATE METHODS

        private static void DebugLog(string type, string message)
        {
#if DEBUG
            Debug.WriteLine(message, type);
#endif
        }

        private static RegisterInfo GetRegisterInfo(Form appBarForm)
        {
            RegisterInfo reg;
            if (RegisterFormInfo.ContainsKey(appBarForm))
            {
                reg = RegisterFormInfo[appBarForm];
            }
            else
            {
                reg = new RegisterInfo()
                {
                    CallbackId = 0,
                    IsRegistered = false,
                    WinForm = appBarForm,
                    Edge = AppBarEdge.None,
                    OrigBorderStyle = appBarForm.FormBorderStyle,
                    OrigPosition = appBarForm.Location,
                    OrigSize = appBarForm.Size,
                    OrigOnTop = appBarForm.TopMost,
                };
                RegisterFormInfo.Add(appBarForm, reg);
            }
            return reg;
        }

        private static void RestoreWindow(Form appBarForm)
        {
            RegisterInfo formInfo = GetRegisterInfo(appBarForm);
            appBarForm.FormBorderStyle = formInfo.OrigBorderStyle;
            appBarForm.TopMost = formInfo.OrigOnTop;
            RECT rc = new RECT
            {
                top = formInfo.OrigPosition.Y,
                left = formInfo.OrigPosition.X,
                right = (formInfo.OrigPosition.X + formInfo.OrigSize.Width),
                bottom = (formInfo.OrigPosition.Y + formInfo.OrigSize.Height)
            };
            appBarForm.BeginInvoke(new MoveAndResizeWindowDelegate(RestoreAndResizeWindow), appBarForm.Handle, rc);
        }

        private static void RestoreAndResizeWindow(IntPtr formHandle, RECT rc)
        {
            DebugLog("RestoreAndResizeWindow()", "New Location: " + rc.ToString());
            MoveWindow(formHandle, rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, true);
            ConfirmedInLocation = false;
        }

        private static Rectangle GetScreenBounds(Control control)
        {
            return Screen.FromControl(control).Bounds;
        }

        private static void AppBarSetPosition(AppBarEdge edge, Form appBarForm)
        {

            var regInfo = GetRegisterInfo(appBarForm);
            var screenRect = GetScreenBounds(appBarForm);

            APPBARDATA barData = new APPBARDATA();
            barData.cbSize = Marshal.SizeOf(barData);
            barData.hWnd = appBarForm.Handle;
            barData.uEdge = (int)edge;

            DebugLog("AppBarSetPosition()", "Starting...");

            if (edge == AppBarEdge.Left || edge == AppBarEdge.Right)
            {
                // Bar should be the full available height of the screen
                barData.rc.top = 0;
                barData.rc.bottom = screenRect.Height;

                if (edge == AppBarEdge.Left)
                {
                    // Setup proposed location
                    barData.rc.left = 0;
                    barData.rc.right = barData.rc.left + regInfo.OrigSize.Width;
                }
                else if (edge == AppBarEdge.Right)
                {
                    // Setup proposed location
                    barData.rc.right = screenRect.Width;
                    barData.rc.left = barData.rc.right - regInfo.OrigSize.Width;
                }
            }
            else
            {
                // Bar should be the full available width of the screen
                barData.rc.left = 0;
                barData.rc.right = screenRect.Width;

                if (edge == AppBarEdge.Top)
                {
                    // Setup proposed location
                    barData.rc.top = 0;
                    barData.rc.bottom = barData.rc.top + regInfo.OrigSize.Height;
                }
                else if (edge == AppBarEdge.Bottom)
                {
                    // Setup proposed location
                    barData.rc.bottom = screenRect.Height;
                    barData.rc.top = barData.rc.bottom - regInfo.OrigSize.Height;
                }
            }

            // Send the query message to find out the other bars on the screen at that edge...
            DebugLog("AppBarSetPosition()", "barData.rc before ABM_QUERYPOS: " + barData.rc.ToString());
            SHAppBarMessage((int)AppBarMsg.ABM_QUERYPOS, ref barData);
            DebugLog("AppBarSetPosition()", "barData.rc after ABM_QUERYPOS: " + barData.rc.ToString());

            // Now we need to use that returned info and resize the barData.rc structure...
            if (edge == AppBarEdge.Top)
            {
                barData.rc.bottom = barData.rc.top + regInfo.OrigSize.Height;
            }
            else if (edge == AppBarEdge.Bottom)
            {
                barData.rc.top = barData.rc.bottom - regInfo.OrigSize.Height;
            }
            else if (edge == AppBarEdge.Left)
            {
                barData.rc.right = barData.rc.left + regInfo.OrigSize.Width;
            }
            else if (edge == AppBarEdge.Right)
            {
                barData.rc.left = barData.rc.right - regInfo.OrigSize.Width;
            }

            // Send the set position message...
            DebugLog("AppBarSetPosition()", "barData.rc before ABM_SETPOS: " + barData.rc.ToString());
            SHAppBarMessage((int)AppBarMsg.ABM_SETPOS, ref barData);
            DebugLog("AppBarSetPosition()", "barData.rc after ABM_SETPOS: " + barData.rc.ToString());

            // Sleep 0.5 seconds
            System.Threading.Thread.Sleep(500);

            // Finally move the window into mosition...
            appBarForm.BeginInvoke(new MoveAndResizeWindowDelegate(MoveAndResizeWindow), barData.hWnd, barData.rc);
        }

        private static void MoveAndResizeWindow(IntPtr formHandle, RECT rc)
        {
            DebugLog("MoveAndResizeWindow()", "New Location: " + rc.ToString());
            BackupRc = rc;
            MoveWindow(formHandle, rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, true);
            ConfirmedInLocation = true;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Called to set/unset a Form as an AppBar
        /// </summary>
        /// <param name="appBarForm">Form to set/unset as AppBar</param>
        /// <param name="edge">Edge to dock to (or None to undock)</param>
        /// <exception cref="System.Exception">Thrown when AppBarMessage is not set</exception>
        public static void SetAppBar(Form appBarForm, AppBarEdge edge)
        {

            if (string.IsNullOrEmpty(AppBarMessage))
            {
                throw new Exception("The AppBarMessage property must be set before calling SetAppBar()");
            }

            // Start of a new app bar movement
            ConfirmedInLocation = false;

            RegisterInfo info = GetRegisterInfo(appBarForm);

            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = appBarForm.Handle;

            do
            {

                // Restore to a non-app bar window state
                if (edge == AppBarEdge.None)
                {
                    if (info.IsRegistered)
                    {
                        SHAppBarMessage((int)AppBarMsg.ABM_REMOVE, ref abd);
                        info.IsRegistered = false;
                    }
                    RestoreWindow(appBarForm);
                    break;
                }

                // Register as a new app bar
                if (!info.IsRegistered)
                {
                    info.CallbackId = RegisterWindowMessage(AppBarMessage);
                    abd.uCallbackMessage = info.CallbackId;

                    uint ret = SHAppBarMessage((int)AppBarMsg.ABM_NEW, ref abd);
                    Application.AddMessageFilter(info);

                    info.IsRegistered = true;
                }

                // Set the app bar position
                appBarForm.TopMost = false;
                appBarForm.FormBorderStyle = FormBorderStyle.None;
                AppBarSetPosition(edge, appBarForm);

            } while (false);

            info.Edge = edge;

        }

        #endregion

    }
}
