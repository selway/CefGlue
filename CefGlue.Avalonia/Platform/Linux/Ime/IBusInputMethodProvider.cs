using System;

namespace Xilium.CefGlue.Avalonia.Platform.Linux.Ime
{
    /// <summary>
    /// Linux IME provider for IBus.
    /// </summary>
    internal sealed class IBusInputMethodProvider : LinuxInputMethodProvider
    {
        public override string GtkImModule => "ibus";

        public override void ConfigureEnvironment(Func<string, string> getEnv, Action<string, string> setEnv)
        {
            base.ConfigureEnvironment(getEnv, setEnv);

            // Chromium's GtkUi in CEF pumps X11EventSource via MessagePumpGlib rather than gtk_main(),
            // so im-ibus.so's default async mode drops unhandled keys (English, Space, Enter, etc.)
            // re-injected via gdk_event_put(). Enabling IBus sync mode makes gtk_im_context_filter_keypress()
            // return handled/unhandled synchronously so both Hangul composition and normal keys work.
            if (string.IsNullOrWhiteSpace(getEnv("IBUS_ENABLE_SYNC_MODE")))
            {
                setEnv("IBUS_ENABLE_SYNC_MODE", "1");
            }
        }
    }
}
