using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.ViewModels
{
    public class VideoCollectionViewModel(IVideoCollectionService service) : ObservableRecipient
    {
        public ObservableCollection<Video> Videos => service.DisplayedVideos;


        public VideoCollectionViewModel() : this(Context.MainHost.GetService<IVideoCollectionService>()) { }
    }
}
