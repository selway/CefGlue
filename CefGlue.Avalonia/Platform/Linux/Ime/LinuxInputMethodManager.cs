using System;
using System.Runtime.InteropServices;

namespace Xilium.CefGlue.Avalonia.Platform.Linux.Ime
{
    /// <summary>
    /// Detects the active Linux IME framework and ensures the environment variables its GTK IM module
    /// needs are populated before CEF initializes its GTK IM context.
    /// </summary>
    internal static class LinuxInputMethodManager
    {
        private const string XModifiersImPrefix = "@im=";

        [DllImport("libc.so.6", EntryPoint = "setenv")]
        private static extern int NativeSetEnv(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string value,
            int overwrite);

        private static readonly LinuxInputMethodProvider[] Providers =
        {
            new IBusInputMethodProvider(),
            new Fcitx5InputMethodProvider()
        };

        /// <returns>The configured provider, or null when no supported IME is active.</returns>
        public static LinuxInputMethodProvider ConfigureIfNeeded(
            Func<string, string> getEnv = null,
            Action<string, string> setEnv = null)
        {
            getEnv ??= Environment.GetEnvironmentVariable;
            setEnv ??= SetProcessEnvironmentVariable;

            var provider = Detect(getEnv);
            provider?.ConfigureEnvironment(getEnv, setEnv);
            return provider;
        }

        private static LinuxInputMethodProvider Detect(Func<string, string> getEnv)
        {
            // An explicit GTK_IM_MODULE is the user's choice for GTK apps (Chromium included); honour it,
            // and leave unsupported modules such as "xim" untouched.
            var gtkIm = getEnv("GTK_IM_MODULE");
            if (!string.IsNullOrWhiteSpace(gtkIm))
            {
                return Find(gtkIm);
            }

            return Find(GetXModifiersIm(getEnv("XMODIFIERS"))) ?? Find(getEnv("QT_IM_MODULE"));
        }

        private static LinuxInputMethodProvider Find(string moduleName) =>
            Array.Find(Providers, provider => provider.Matches(moduleName));

        // "@im=ibus" -> "ibus"
        private static string GetXModifiersIm(string xmodifiers)
        {
            var start = xmodifiers?.IndexOf(XModifiersImPrefix, StringComparison.OrdinalIgnoreCase) ?? -1;
            if (start < 0)
            {
                return null;
            }

            start += XModifiersImPrefix.Length;
            var end = xmodifiers.IndexOfAny(new[] { '@', ' ' }, start);
            return end < 0 ? xmodifiers.Substring(start) : xmodifiers.Substring(start, end - start);
        }

        private static void SetProcessEnvironmentVariable(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);

            // On Linux, .NET's Environment.SetEnvironmentVariable only updates the managed
            // cache and does not call libc setenv(), so native GTK/IBus/Fcitx modules calling
            // getenv()/g_getenv() would not observe the change unless we also call setenv().
            NativeSetEnv(name, value, 1);
        }
    }
}
