using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Data;
using VidHub.Core.Models;

namespace VidHub.ViewModels.Modals
{
    public partial class LicensesViewModel : ObservableRecipient
    {
        public string License => ApplicationLicenseData.Type;
        public string Copyright => ApplicationLicenseData.Copyright;
        public string LicenseText => ApplicationLicenseData.Text;

        public IList<ExternalLicense> ExternalLicenses =>
        [
            ExternalLicenseData.WinUI,
            ExternalLicenseData.DotNet,
            ExternalLicenseData.Blake3,
            ExternalLicenseData.CommunityToolkit_Mvvm,
            ExternalLicenseData.Microsoft_Extensions_DependencyInjection,
            ExternalLicenseData.Microsoft_Extensions_Hosting,
            ExternalLicenseData.Microsoft_Toolkit_Uwp_Notifications,
            ExternalLicenseData.Microsoft_Windows_SDK_BuildTools,
            ExternalLicenseData.Microsoft_WindowsAppSDK,
            ExternalLicenseData.System_Drawing_Common
        ];
    }
}
