using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Data;
using VidHub.Core.Models;

namespace VidHub.ViewModels.Modals
{
    public partial class VersionsViewModel : ObservableRecipient
    {
        public IList<VersionInformation> Versions =>
        [
            VersionData.Version_0_5_0,
            VersionData.Version_0_4_0,
            VersionData.Version_0_3_0,
            VersionData.Version_0_2_1,
            VersionData.Version_0_2_0,
            VersionData.Version_0_1_0
        ];
    }
}
