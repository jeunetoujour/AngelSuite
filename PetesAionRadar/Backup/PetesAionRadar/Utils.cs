/*
    Copyright © 2009, Aion-Radar.com
    All rights reserved.
    http://www.aion-radar.com


    This file is part of Aion Radar.

    Aion Radar is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Aion Radar is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Aion Radar.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class Utils
{
    private Utils() { } // CA1053, http://msdn2.microsoft.com/library/ms182169(VS.90).aspx

    #region Platform invokes (P/Invoke)

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(String ClassName, String WindowName);

    [DllImport("user32.dll")]
    public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsZoomed(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HTCAPTION = 0x2;

    [DllImport("User32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("User32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    #endregion

    /// <summary>
    /// Returns a RegistryKey pointing to the application's registry key for the current
    /// user.  Does not provide version/build information like System.Windows.Forms.Application.
    /// UserAppDataRegistry does.
    /// </summary>
    public static Microsoft.Win32.RegistryKey AppSettingKey
    {
        get
        {
            try
            {
                return Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\" + System.Windows.Forms.Application.CompanyName + "\\" + System.Windows.Forms.Application.ProductName);
            }
            catch
            {
                Microsoft.Win32.RegistryKey key = System.Windows.Forms.Application.UserAppDataRegistry;
                string sKeyToUse = key.ToString().Replace("HKEY_CURRENT_USER\\", string.Empty);
                sKeyToUse = sKeyToUse.Substring(0, sKeyToUse.LastIndexOf("\\"));
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(sKeyToUse, true);

                return key;
            }
        }
    }

    /// <summary>
    /// Reads a string value of a registry key.  Returns 'string.Empty' if the key
    /// could not be found.
    /// </summary>
    /// <param name="sKeyName">The key in the registry to read.</param>
    public static string GetAppSetting(string sKeyName)
    {
        string sVal = string.Empty;

        try
        {
            sVal = Utils.AppSettingKey.GetValue(sKeyName, string.Empty).ToString();
        }
        catch { }

        return sVal;
    }

    /// <summary>
    /// Writes a string value for a registry key.  Will delete the key if the value
    /// is null or 'string.Empty'.
    /// </summary>
    /// <param name="sKeyName">The key in the registry to write.</param>
    /// <param name="sKeyValue">The value of the registry key.</param>
    public static void SetAppSetting(string sKeyName, string sKeyValue)
    {
        try
        {
            if (string.IsNullOrEmpty(sKeyValue))
                Utils.AppSettingKey.DeleteValue(sKeyName);
            else
                Utils.AppSettingKey.SetValue(sKeyName, sKeyValue);
        }
        catch { }

        return;
    }

    /// <summary>
    /// Provides full garbage collection, including using .Net 3.5 methods if available.
    /// </summary>
    public static void DoGarbageCollection()
    {
        try
        {
            bool bHasWaitforFullGCComplete = false;

            try
            {
                Type t = typeof(System.GC);
                System.Reflection.MemberInfo[] mi = t.GetMember("WaitForFullGCComplete");
                bHasWaitforFullGCComplete = (mi.Length > 0);
            }
            catch { }

            if (!bHasWaitforFullGCComplete)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            else
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
            }
        }
        catch { }
        return;
    }

    /// <summary>
    /// Gets the caption text of the active window.
    /// </summary>
    public static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        IntPtr handle = IntPtr.Zero;
        StringBuilder Buff = new StringBuilder(nChars);
        handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) != IntPtr.Zero)
            return Buff.ToString();
        return null;
    }

    /// <summary>
    /// Draws text to a Graphics object.
    /// </summary>
    /// <param name="g">Graphics object to draw on.</param>
    /// <param name="s">Text to draw.</param>
    /// <param name="f">Font to use.</param>
    /// <param name="b1">Foreground Brush to use.</param>
    /// <param name="b2">Background Brush to use.</param>
    /// <param name="p">Point on the Graphics object to draw.</param>
    public static void OutputString(ref Graphics g, string s, Font f, Brush b1, Brush b2, Point p)
    {
        // Draw the shading/background first
        g.DrawString(s, f, b2, new Point(p.X - 1, p.Y - 1));
        g.DrawString(s, f, b2, new Point(p.X - 1, p.Y + 1));
        g.DrawString(s, f, b2, new Point(p.X + 1, p.Y - 1));
        g.DrawString(s, f, b2, new Point(p.X + 1, p.Y + 1));

        // Then draw the foreground text on top
        g.DrawString(s, f, b1, p);
    }

    /// <summary>
    /// Loads an icon resource from the current assembly manifest.
    /// </summary>
    /// <param name="sResource">Resource name.</param>
    public static Icon LoadIconFromResource(string sResource)
    {
        Icon ico = null;

        try
        {
            string strNameSpace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
            System.IO.Stream str = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strNameSpace + "." + sResource);
            ico = new Icon(str);
        }
        catch { }

        return (ico);
    }

    /// <summary>
    /// Loads a bitmap resource from the current assembly manifest.
    /// </summary>
    /// <param name="sResource">Resource name.</param>
    public static Bitmap LoadBitmapFromResource(string sResource)
    {
        Bitmap bmp = null;

        try
        {
            string strNameSpace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
            System.IO.Stream str = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strNameSpace + "." + sResource);
            bmp = new Bitmap(str);
        }
        catch { }

        return (bmp);
    }

    /// <summary>
    /// Creates a solid color Brush object based on a hex color code string.
    /// </summary>
    /// <param name="sHex">The hex color to use.</param>
    public static Brush BrushColor(string sHex)
    {
        try
        {
            return new System.Drawing.SolidBrush(Utils.HexColor(sHex));
        }
        catch { }

        return null;
    }

    /// <summary>
    /// Creates a solid color Brush object based on a hex color code string.
    /// </summary>
    /// <param name="sHex">The hex color to use.</param>
    public static Color HexColor(string sHex)
    {
        sHex = sHex.ToLower().Trim();
        if (sHex.StartsWith("0x"))
            sHex = sHex.Substring(2);
        try
        {
            int red = Convert.ToInt32(sHex.Substring(0, 2), 16);
            int green = Convert.ToInt32(sHex.Substring(2, 2), 16);
            int blue = Convert.ToInt32(sHex.Substring(4, 2), 16);
            return Color.FromArgb(red, green, blue);
        }
        catch { }

        return Color.Black;
    }
}
