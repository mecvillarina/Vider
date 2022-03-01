using System.Diagnostics.CodeAnalysis;

namespace Client.App
{
    [ExcludeFromCodeCoverage]
    public static class AppConstants
    {
        public static string AppName { get; } = "vider";
        public static string AppLogo { get; } = "assets/app_logo.png";
        public static string AppLogoDark { get; } = "assets/app_logo_dark.png";
        public static string LogoIcon { get; } = "assets/logo_icon.png";
        public static string AuthBackground { get; } = "assets/auth_background.jpg";
        public static string XRPLogo { get; } = "assets/xrp_logo.png";

    }
}