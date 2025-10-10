using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;
using static VidHub.Services.Logics.VideoCustomizationService;

namespace VidHub.ViewModels.Modals
{
    public partial class TitleCustomizationViewModel(IVideoCustomizationService service) : ObservableRecipient
    {
        public ObservableCollection<FormattedVideo> TitleCollection => service.Videos;

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

        public string Pattern
        {
            get => service.Pattern;
            set
            {
                service.Pattern = value;
                OnPropertyChanged(nameof(InvalidRegex));
            }
        }
        public string Replacement
        {
            get => service.Replacement;
            set
            {
                service.Replacement = value;
            }
        }
        public bool InvalidRegex => service.InvalidRegex;

        public bool IsRegexEnabled
        {
            get => service.IsRegexEnabled;
            set => service.IsRegexEnabled = value;
        }
        public bool DontShowAgain
        {
            get => service.DontShowAgain;
            set => service.DontShowAgain = value;
        }


        public TitleCustomizationViewModel() : this(Context.MainHost.GetService<IVideoCustomizationService>())
        {
            Context.MainHost.GetService<IVideoCustomizationService>().LoadFormats();
        }
    }
}
