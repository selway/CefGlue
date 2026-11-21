using System;

namespace Xilium.CefGlue.Avalonia.Platform.Linux.Ime
{
    /// <summary>
    /// A Linux Input Method Editor (IME) framework whose GTK IM module Chromium's GtkUi can load.
    /// </summary>
    internal abstract class LinuxInputMethodProvider
    {
        /// <summary>
        /// The GTK IM module identifier expected by Chromium's GtkUi (e.g. "ibus", "fcitx").
        /// </summary>
        public abstract string GtkImModule { get; }

        /// <summary>
        /// Names this framework uses in GTK_IM_MODULE, QT_IM_MODULE and XMODIFIERS (@im=name).
        /// </summary>
        protected virtual string[] ModuleNames => new[] { GtkImModule };

        public bool Matches(string moduleName)
        {
            moduleName = moduleName?.Trim();
            return !string.IsNullOrEmpty(moduleName) &&
                Array.Exists(ModuleNames, name => string.Equals(name, moduleName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Fills in the process environment variables Chromium's GTK IM context needs, keeping any the user set.
        /// </summary>
        public virtual void ConfigureEnvironment(Func<string, string> getEnv, Action<string, string> setEnv)
        {
            if (string.IsNullOrWhiteSpace(getEnv("GTK_IM_MODULE")))
            {
                setEnv("GTK_IM_MODULE", GtkImModule);
            }

            if (string.IsNullOrWhiteSpace(getEnv("XMODIFIERS")))
            {
                setEnv("XMODIFIERS", "@im=" + GtkImModule);
            }
        }
    }
}
