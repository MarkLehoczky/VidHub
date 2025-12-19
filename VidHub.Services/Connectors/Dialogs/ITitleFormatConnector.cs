using System.Collections.ObjectModel;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public interface ITitleFormatConnector : IUpdateService
    {
        bool HideTitleCustomization { get; set; }
        bool UseRegex { get; set; }
        bool IncludeDate { get; set; }
        bool IncludeExtension { get; set; }
        bool IncludeFilename { get; set; }
        bool IncludeMetadata { get; set; }
        bool IncludePath { get; set; }
        bool InvalidRegex { get; }
        string RegexPattern { get; set; }
        string RegexReplacement { get; set; }
        ObservableCollection<string> Titles { get; }
        void UpdateTitles();
    }
}
