using VidHub.Core.Utilities;
using VidHub.Services.Connectors.Dialogs;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.ViewModels.Dialogs
{
    public partial class TitleFormatViewModel(ITitleFormatConnector connector) : ViewModelTemplate(connector)
    {
        private readonly ILogger logger = VidHubContext.Logger;
        public ITitleFormatConnector Connector { get; set; } = connector;

        public TitleFormatViewModel() : this(VidHubContext.Host.GetService<ITitleFormatConnector>())
        {
            Connector = VidHubContext.Host.GetService<ITitleFormatConnector>();
            logger.LogTrace("TitleFormatViewModel initialized");
        }


        public IList<string> TitleCollection => Connector.Titles;

        public bool IncludePath
        {
            get => Connector.IncludePath;
            set { Connector.IncludePath = value; logger.LogDebug("IncludePath set to {Value}", value); }
        }
        public bool IncludeDate
        {
            get => Connector.IncludeDate;
            set { Connector.IncludeDate = value; logger.LogDebug("IncludeDate set to {Value}", value); }
        }
        public bool IncludeFilename
        {
            get => Connector.IncludeFilename;
            set { Connector.IncludeFilename = value; logger.LogDebug("IncludeFilename set to {Value}", value); }
        }
        public bool IncludeMetadata
        {
            get => Connector.IncludeMetadata;
            set { Connector.IncludeMetadata = value; logger.LogDebug("IncludeMetadata set to {Value}", value); }
        }
        public bool IncludeExtension
        {
            get => Connector.IncludeExtension;
            set { Connector.IncludeExtension = value; logger.LogDebug("IncludeExtension set to {Value}", value); }
        }

        public string RegexPattern
        {
            get => Connector.RegexPattern;
            set
            {
                Connector.RegexPattern = value;
                OnPropertyChanged(nameof(InvalidRegex));
                logger.LogDebug("RegexPattern set");
            }
        }
        public string RegexReplacement
        {
            get => Connector.RegexReplacement; set { Connector.RegexReplacement = value; logger.LogDebug("RegexReplacement set"); }
        }
        public bool InvalidRegex => Connector.InvalidRegex;

        public bool EnabledRegex
        {
            get => Connector.UseRegex;
            set { Connector.UseRegex = value; logger.LogDebug("UseRegex set to {Value}", value); }
        }
        public bool DontShowTitleCustomizationAgain
        {
            get => Connector.HideTitleCustomization;
            set { Connector.HideTitleCustomization = value; logger.LogDebug("HideTitleCustomization set to {Value}", value); }
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            logger.LogTrace("TitleFormatViewModel.Update invoked");
            OnPropertyChanged(nameof(EnabledRegex));
        }
    }
}
