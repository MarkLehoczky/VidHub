using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class RenameViewModel(IMainService service) : ObservableRecipient
    {
        public Video Video { get; set; }
        public string OriginalTitle { get; set; }
        public string Title
        {
            get => Video.Title;
            set
            {
                Video.Title = value;
                service.Update(UpdateType.ResetVideoCollection);
            }
        }

        public RenameViewModel() : this(Context.MainHost.GetService<IMainService>()) { }
    }
}
