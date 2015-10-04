using System;
using System.Runtime.InteropServices;
using Gtk;

namespace SFEditor
{
    public class Gtk3Wrapper
    {
        public const string gtklibpath = "libgtk-3.so.0";

        [DllImport (gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_header_bar_new ();

        [DllImport (gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_header_bar_set_subtitle (IntPtr handle, string text);

        [DllImport (gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern string gtk_header_bar_get_subtitle (IntPtr handle);

        [DllImport (gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_header_bar_set_show_close_button (IntPtr handle, bool show);

        [DllImport (gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_header_bar_get_show_close_button (IntPtr handle);

        [DllImport (gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_window_set_titlebar (IntPtr window, IntPtr widget);
    }

    public class HeaderBar : Container
    {
        public string Subtitle
        {
            get
            {
                return Gtk3Wrapper.gtk_header_bar_get_subtitle(this.Handle);
            }
            set
            {
                Gtk3Wrapper.gtk_header_bar_set_subtitle(this.Handle, value);
            }
        }

        public bool ShowCloseButton
        {
            get
            {
                return Gtk3Wrapper.gtk_header_bar_get_show_close_button(this.Handle);
            }
            set
            {
                Gtk3Wrapper.gtk_header_bar_set_show_close_button(this.Handle, value);
            }
        }

        public HeaderBar() : base(Gtk3Wrapper.gtk_header_bar_new()) { }

        public HeaderBar(IntPtr handle) : base(handle) { }

        public void AttachToWindow(Window window)
        {
            Gtk3Wrapper.gtk_window_set_titlebar(window.Handle, this.Handle);
        }
    }
}

