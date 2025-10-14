using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Models;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Modals.Interfaces
{
    public interface IVideoTitleFormatCustomizationConnector : IUpdateService
    {
        bool DontShowTitleCustomizationAgain { get; set; }
        bool EnabledRegex { get; set; }
        bool IncludeDate { get; set; }
        bool IncludeExtension { get; set; }
        bool IncludeFilename { get; set; }
        bool IncludeMetadata { get; set; }
        bool IncludePath { get; set; }
        bool InvalidRegex { get; }
        bool IsTemplateMode { get; set; }
        string RegexPattern { get; set; }
        string RegexReplacement { get; set; }
        ObservableCollection<VideoTitleTemplate> Videos { get; }
        void ChangeVideos(IEnumerable<int> ids);
        void ChangeVideos(IEnumerable<Video> videos);
    }
}
