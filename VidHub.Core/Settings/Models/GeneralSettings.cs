using System.Text.Json.Serialization;

namespace VidHub.Core.Settings.Models
{
    public class GeneralSettings
    {
        public bool OpenedSidePanel { get; set; } = true;
        public bool KeepSidePanelSettings { get; set; } = true;
        public bool UseFileContentHash { get; set; } = false;

        [JsonIgnore] public bool ClosedSidePanel => !OpenedSidePanel;
        [JsonIgnore] public bool UseFileNameHash => !UseFileContentHash;
    }
}
