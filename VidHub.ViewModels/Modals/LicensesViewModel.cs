using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Models;

namespace VidHub.ViewModels.Modals
{
    public partial class LicensesViewModel : ObservableRecipient
    {
        public string License => "MIT License";
        public string Copyright => "Copyright © 2025 Mark Lehoczky";
        public string LicenseText => "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\r\n\r\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\r\n\r\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

        public IList<ExternalLicense> ExternalLicenses { get; set; } = [
            new ExternalLicense
            {
                Name = "WinUI 3",
                License = "MIT License",
                Link = "https://github.com/microsoft/microsoft-ui-xaml?tab=MIT-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = ".NET 8.0",
                License = "MIT License",
                Link = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = "Blake3",
                License = "BSD-2-Clause License",
                Link = "https://github.com/xoofx/Blake3.NET?tab=License-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = "CommunityToolkit.Mvvm",
                License = "MIT License",
                Link = "https://github.com/CommunityToolkit/dotnet?tab=License-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = "Microsoft.Extensions.DependencyInjection",
                License = "MIT License",
                Link = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = "Microsoft.Extensions.Hosting",
                License = "MIT License",
                Link = "https://github.com/dotnet/runtime?tab=MIT-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = "Microsoft.Toolkit.Uwp.Notifications",
                License = "MIT License",
                Link = "https://github.com/CommunityToolkit/WindowsCommunityToolkit?tab=License-1-ov-file#",
            },
            new ExternalLicense
            {
                Name = "Microsoft.Windows.SDK.BuildTools",
                License = "MICROSOFT SOFTWARE LICENSE TERMS",
                Link = "https://aka.ms/WinSDKLicenseURL",
            },
            new ExternalLicense
            {
                Name = "Microsoft.WindowsAppSDK",
                License = "MICROSOFT SOFTWARE LICENSE TERMS",
                Link = "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.7.250606001/License",
            },
            new ExternalLicense
            {
                Name = "System.Drawing.Common",
                License = "MIT License",
                Link = "https://github.com/dotnet/winforms?tab=MIT-1-ov-file#",
            },
        ];
    }
}
