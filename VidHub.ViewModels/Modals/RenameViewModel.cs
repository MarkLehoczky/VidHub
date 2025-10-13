using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core;

namespace VidHub.ViewModels.Modals
{
    public partial class RenameViewModel : ObservableRecipient
    {
        public Video Video { get; set; } = new();
    }
}
