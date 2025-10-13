using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Models;
using VidHub.Platform;
using VidHub.Services.Modals.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoTitleFormatCustomizationViewModel(IVideoTitleFormatCustomizationService service) : ObservableRecipient
    {
        public IList<VideoTitleTemplate> TitleCollection => service.Videos;

        public bool IncludePath
        {
            get => service.IncludePath;
            set => service.IncludePath = value;
        }
        public bool IncludeDate
        {
            get => service.IncludeDate;
            set => service.IncludeDate = value;
        }
        public bool IncludeFilename
        {
            get => service.IncludeFilename;
            set => service.IncludeFilename = value;
        }
        public bool IncludeMetadata
        {
            get => service.IncludeMetadata;
            set => service.IncludeMetadata = value;
        }
        public bool IncludeExtension
        {
            get => service.IncludeExtension;
            set => service.IncludeExtension = value;
        }

        public string RegexPattern
        {
            get => service.RegexPattern;
            set
            {
                service.RegexPattern = value;
                OnPropertyChanged(nameof(InvalidRegex));
            }
        }
        public string RegexReplacement
        {
            get => service.RegexReplacement;
            set
            {
                service.RegexReplacement = value;
            }
        }
        public bool InvalidRegex => service.InvalidRegex;

        public bool EnabledRegex
        {
            get => service.EnabledRegex;
            set => service.EnabledRegex = value;
        }
        public bool DontShowTitleCustomizationAgain
        {
            get => service.DontShowTitleCustomizationAgain;
            set => service.DontShowTitleCustomizationAgain = value;
        }


        public VideoTitleFormatCustomizationViewModel() : this(Context.Host.GetService<IVideoTitleFormatCustomizationService>())
        {
            Context.Host.GetService<IVideoTitleFormatCustomizationService>().LoadFormats();
        }
    }
}
