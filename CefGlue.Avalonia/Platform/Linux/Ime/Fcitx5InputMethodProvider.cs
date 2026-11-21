namespace Xilium.CefGlue.Avalonia.Platform.Linux.Ime
{
    /// <summary>
    /// Linux IME provider for Fcitx5 (also matches Fcitx 4, which uses the same "fcitx" GTK IM module name).
    /// </summary>
    internal sealed class Fcitx5InputMethodProvider : LinuxInputMethodProvider
    {
        private static readonly string[] Names = { "fcitx", "fcitx5" };

        public override string GtkImModule => "fcitx";

        protected override string[] ModuleNames => Names;
    }
}
