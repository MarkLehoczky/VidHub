using VidHub.Core.Models;

namespace VidHub.Core.Data
{
    public class ExternalLicenseData
    {
        public static ExternalLicense WinUI => new()
        {
            Module = "WinUI 3",
            License = "MIT License",
            Hyperlink = "https://github.com/microsoft/microsoft-ui-xaml?tab=MIT-1-ov-file#",
        };
        public static ExternalLicense DotNet => new()
        {
            Module = ".NET 8.0",
            License = "MIT License",
            Hyperlink = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };
        public static ExternalLicense Blake3 => new()
        {
            Module = "Blake3",
            License = "BSD-2-Clause License",
            Hyperlink = "https://github.com/xoofx/Blake3.NET?tab=License-1-ov-file#",
        };
        public static ExternalLicense CommunityToolkit_Mvvm => new()
        {
            Module = "CommunityToolkit.Mvvm",
            License = "MIT License",
            Hyperlink = "https://github.com/CommunityToolkit/dotnet?tab=License-1-ov-file#",
        };
        public static ExternalLicense Microsoft_Extensions_DependencyInjection => new()
        {
            Module = "Microsoft.Extensions.DependencyInjection",
            License = "MIT License",
            Hyperlink = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };
        public static ExternalLicense Microsoft_Extensions_Hosting => new()
        {
            Module = "Microsoft.Extensions.Hosting",
            License = "MIT License",
            Hyperlink = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };
        public static ExternalLicense Microsoft_Extensions_Logging => new()
        {
            Module = "Microsoft.Extensions.Logging",
            License = "MIT License",
            Hyperlink = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };
        public static ExternalLicense Microsoft_Toolkit_Uwp_Notifications => new()
        {
            Module = "Microsoft.Toolkit.Uwp.Notifications",
            License = "MIT License",
            Hyperlink = "https://github.com/CommunityToolkit/WindowsCommunityToolkit?tab=License-1-ov-file#",
        };
        public static ExternalLicense Microsoft_Windows_SDK_BuildTools => new()
        {
            Module = "Microsoft.Windows.SDK.BuildTools",
            License = "MICROSOFT SOFTWARE LICENSE TERMS",
            Hyperlink = "https://aka.ms/WinSDKLicenseURL",
        };
        public static ExternalLicense Microsoft_WindowsAppSDK => new()
        {
            Module = "Microsoft.WindowsAppSDK",
            License = "MICROSOFT SOFTWARE LICENSE TERMS",
            Hyperlink = "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.7.250606001/License",
        };
        public static ExternalLicense System_Drawing_Common => new()
        {
            Module = "System.Drawing.Common",
            License = "MIT License",
            Hyperlink = "https://github.com/dotnet/winforms?tab=MIT-1-ov-file#",
        };
    }
}
