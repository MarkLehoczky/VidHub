using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using VidHub.Core.Models;

namespace VidHub.Core.Settings
{
    public class GeneralSettings
    {
        public bool OpenedSidePanel { get; set; } = true;
        public bool KeepSidePanelSettings { get; set; } = true;
        public bool UseFileContentHash { get; set; } = false;
        public ObservableCollection<Tag> Tags { get; set; } = [];

        [JsonIgnore] public bool ClosedSidePanel => !OpenedSidePanel;
        [JsonIgnore] public bool UseFileNameHash => !UseFileContentHash;
    }
}
