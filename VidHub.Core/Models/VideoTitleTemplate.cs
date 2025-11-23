using CommunityToolkit.Mvvm.ComponentModel;

namespace VidHub.Core.Models
{
    public class VideoTitleTemplate(Video video) : ObservableObject
    {
        private string title = string.Empty;

        public string FilePath { get; } = video.FilePath;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        public int ID { get; } = video.ID;

        public Video Instance => video;
    }
}
