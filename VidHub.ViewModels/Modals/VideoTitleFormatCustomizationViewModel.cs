using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.ViewModels.Base;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoTitleFormatCustomizationViewModel(IVideoTitleFormatCustomizationConnector connector) : ViewModelTemplate(connector)
    {
        public IVideoTitleFormatCustomizationConnector Connector { get; set; }

        public VideoTitleFormatCustomizationViewModel() : this(Context.Host.GetService<IVideoTitleFormatCustomizationConnector>())
        {
            Connector = Context.Host.GetService<IVideoTitleFormatCustomizationConnector>();
        }


        public IList<string> TitleCollection => Connector.Titles;

        public bool IncludePath
        {
            get => Connector.IncludePath;
            set => Connector.IncludePath = value;
        }
        public bool IncludeDate
        {
            get => Connector.IncludeDate;
            set => Connector.IncludeDate = value;
        }
        public bool IncludeFilename
        {
            get => Connector.IncludeFilename;
            set => Connector.IncludeFilename = value;
        }
        public bool IncludeMetadata
        {
            get => Connector.IncludeMetadata;
            set => Connector.IncludeMetadata = value;
        }
        public bool IncludeExtension
        {
            get => Connector.IncludeExtension;
            set => Connector.IncludeExtension = value;
        }

        public string RegexPattern
        {
            get => Connector.RegexPattern;
            set
            {
                Connector.RegexPattern = value;
                OnPropertyChanged(nameof(InvalidRegex));
            }
        }
        public string RegexReplacement
        {
            get => Connector.RegexReplacement; set => Connector.RegexReplacement = value;
        }
        public bool InvalidRegex => Connector.InvalidRegex;

        public bool EnabledRegex
        {
            get => Connector.UseRegex;
            set => Connector.UseRegex = value;
        }
        public bool DontShowTitleCustomizationAgain
        {
            get => Connector.HideTitleCustomization;
            set => Connector.HideTitleCustomization = value;
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            OnPropertyChanged(nameof(EnabledRegex));
        }
    }
}
