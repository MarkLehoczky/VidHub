using VidHub.Core;
using VidHub.Core.Models;

namespace VidHub.Services.Modals.Interfaces
{
    public interface IVideoTitleFormatCustomizationService
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
        IList<VideoTitleTemplate> Videos { get; }
        void LoadFormats();
        void UpdateFormats();
    }
}
