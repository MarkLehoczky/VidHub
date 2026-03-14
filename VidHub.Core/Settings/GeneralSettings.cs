using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using VidHub.Core.Models;
using VidHub.Core.Streams;
using Windows.UI;

namespace VidHub.Core.Settings
{
    public class GeneralSettings
    {
        public bool OpenedSidePanel { get; set; } = true;
        public bool KeepSidePanelSettings { get; set; } = true;
        public bool UseFileContentHash { get; set; } = false;
        public ObservableCollection<FixedResolution> Resolutions { get; set; } = [
            new FixedResolution { Name = "4320p", Width = 7680, Height = 4320 },
            new FixedResolution { Name = "2160p", Width = 3840, Height = 2160 },
            new FixedResolution { Name = "1440p", Width = 2560, Height = 1440 },
            new FixedResolution { Name = "1080p", Width = 1920, Height = 1080 },
            new FixedResolution { Name = "720p", Width = 1280, Height = 720 },
            new FixedResolution { Name = "480p", Width = 720, Height = 480 },
            new FixedResolution { Name = "Low", Width = 0, Height = 0 },
        ];
        public ObservableCollection<FixedFramerate> Framerates { get; set; } = [
            new FixedFramerate { Name = "240 fps", Framerate = 240 },
            new FixedFramerate { Name = "120 fps", Framerate = 120 },
            new FixedFramerate { Name = "60 fps", Framerate = 60 },
            new FixedFramerate { Name = "30 fps", Framerate = 30 },
            new FixedFramerate { Name = "24 fps", Framerate = 24 },
            new FixedFramerate { Name = "12 fps", Framerate = 12 },
            new FixedFramerate { Name = "Low", Framerate = 0 },
        ];
        public ObservableCollection<Tag> Tags { get; set; } = [
            new Tag { Name = "⭐", Color = Color.FromArgb(150, 0, 0, 0), ID = 0 },
            new Tag { Name = "Watched", Color = Color.FromArgb(255, 255, 50, 50), ID = 1 },
            new Tag { Name = "Watch later", Color = Color.FromArgb(255, 50, 255, 50), ID = 2 },
        ];

        [JsonIgnore] public bool ClosedSidePanel => !OpenedSidePanel;
        [JsonIgnore] public bool UseFileNameHash => !UseFileContentHash;
    }
}
