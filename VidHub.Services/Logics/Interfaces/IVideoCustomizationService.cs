using System.Collections.ObjectModel;
using VidHub.Core;
using static VidHub.Services.Logics.VideoCustomizationService;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoCustomizationService
    {
        ObservableCollection<FormattedVideo> Videos { get; }
        bool IsTemplateMode { get; set; }

        bool IncludePath { get; set; }
        bool IncludeDate { get; set; }
        bool IncludeFilename { get; set; }
        bool IncludeMetadata { get; set; }
        bool IncludeExtension { get; set; }

        string Pattern { get; set; }
        string Replacement { get; set; }
        bool InvalidRegex { get; }

        bool IsRegexEnabled { get; set; }
        bool DontShowAgain { get; set; }

        void CustomizeTitle(Video video);
        void LoadFormats();
        void UpdateFormats();
    }
}
