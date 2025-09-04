using System.Collections.ObjectModel;
using VidHub.Core;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoCollectionService
    {
        ObservableCollection<Video> DisplayedVideos { get; }
    }
}
