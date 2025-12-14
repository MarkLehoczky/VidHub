using VidHub.Core.Models;

namespace VidHub.Core.Data
{
    public class ExternalLicenseData
    {
        public static ExternalLicense WinUI => new()
        {
            Name = "WinUI 3",
            License = "MIT License",
            Link = "https://github.com/microsoft/microsoft-ui-xaml?tab=MIT-1-ov-file#",
        };

        public static ExternalLicense DotNet => new()
        {
            Name = ".NET 8.0",
            License = "MIT License",
            Link = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };

        public static ExternalLicense Blake3 => new()
        {
            Name = "Blake3",
            License = "BSD-2-Clause License",
            Link = "https://github.com/xoofx/Blake3.NET?tab=License-1-ov-file#",
        };

        public static ExternalLicense CommunityToolkit_Mvvm => new()
        {
            Name = "CommunityToolkit.Mvvm",
            License = "MIT License",
            Link = "https://github.com/CommunityToolkit/dotnet?tab=License-1-ov-file#",
        };

        public static ExternalLicense Microsoft_Extensions_DependencyInjection => new()
        {
            Name = "Microsoft.Extensions.DependencyInjection",
            License = "MIT License",
            Link = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };

        public static ExternalLicense Microsoft_Extensions_Hosting => new()
        {
            Name = "Microsoft.Extensions.Hosting",
            License = "MIT License",
            Link = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
        };

        public static ExternalLicense Microsoft_Toolkit_Uwp_Notifications => new()
        {
            Name = "Microsoft.Toolkit.Uwp.Notifications",
            License = "MIT License",
            Link = "https://github.com/CommunityToolkit/WindowsCommunityToolkit?tab=License-1-ov-file#",
        };

        public static ExternalLicense Microsoft_Windows_SDK_BuildTools => new()
        {
            Name = "Microsoft.Windows.SDK.BuildTools",
            License = "MICROSOFT SOFTWARE LICENSE TERMS",
            Link = "https://aka.ms/WinSDKLicenseURL",
        };

        public static ExternalLicense Microsoft_WindowsAppSDK => new()
        {
            Name = "Microsoft.WindowsAppSDK",
            License = "MICROSOFT SOFTWARE LICENSE TERMS",
            Link = "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.7.250606001/License",
        };

        public static ExternalLicense System_Drawing_Common => new()
        {
            Name = "System.Drawing.Common",
            License = "MIT License",
            Link = "https://github.com/dotnet/winforms?tab=MIT-1-ov-file#",
        };
    }
}
