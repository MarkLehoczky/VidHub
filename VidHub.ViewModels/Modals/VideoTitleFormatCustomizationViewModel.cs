using VidHub.Core;
using VidHub.Core.Models;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.ViewModels.Base;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoTitleFormatCustomizationViewModel(IVideoTitleFormatCustomizationConnector connector) : ViewModelTemplate(connector)
    {
        public VideoTitleFormatCustomizationViewModel() : this(Context.Host.GetService<IVideoTitleFormatCustomizationConnector>()) { }


        public IList<VideoTitleTemplate> TitleCollection => connector.Videos;

        public bool IncludePath
        {
            get => connector.IncludePath;
            set => connector.IncludePath = value;
        }
        public bool IncludeDate
        {
            get => connector.IncludeDate;
            set => connector.IncludeDate = value;
        }
        public bool IncludeFilename
        {
            get => connector.IncludeFilename;
            set => connector.IncludeFilename = value;
        }
        public bool IncludeMetadata
        {
            get => connector.IncludeMetadata;
            set => connector.IncludeMetadata = value;
        }
        public bool IncludeExtension
        {
            get => connector.IncludeExtension;
            set => connector.IncludeExtension = value;
        }

        public string RegexPattern
        {
            get => connector.RegexPattern;
            set
            {
                connector.RegexPattern = value;
                OnPropertyChanged(nameof(InvalidRegex));
            }
        }
        public string RegexReplacement
        {
            get => connector.RegexReplacement; set => connector.RegexReplacement = value;
        }
        public bool InvalidRegex => connector.InvalidRegex;

        public bool EnabledRegex
        {
            get => connector.EnabledRegex;
            set => connector.EnabledRegex = value;
        }
        public bool DontShowTitleCustomizationAgain
        {
            get => connector.DontShowTitleCustomizationAgain;
            set => connector.DontShowTitleCustomizationAgain = value;
        }

        public bool TemplateMode
        {
            get => connector.IsTemplateMode;
            set => connector.IsTemplateMode = value;
        }

        public void ChangeVideos(IEnumerable<int> ids)
        {
            connector.ChangeVideos(ids);
        }

        public void ChangeVideos(IEnumerable<Video> videos)
        {
            connector.ChangeVideos(videos);
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
