using System.Text.RegularExpressions;
using VidHub.Core.Helpers;
using VidHub.Core.Models;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Modals.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Modals
{
    public class VideoTitleFormatCustomizationService(IVideoService service, ISettingsService settings) : IVideoTitleFormatCustomizationService
    {
        public IList<VideoTitleTemplate> Videos { get; } = [];
        public bool IsTemplateMode { get; set; }

        public bool IncludePath
        {
            get => settings.TitleCustomization.IncludePath;
            set 
            {
                settings.TitleCustomization.IncludePath = value;
                UpdateFormats();
            }
        }
        public bool IncludeDate
        {
            get => settings.TitleCustomization.IncludeDate;
            set
            {
                settings.TitleCustomization.IncludeDate = value;
                UpdateFormats();
            }
        }
        public bool IncludeFilename
        {
            get => settings.TitleCustomization.IncludeFilename;
            set
            {
                settings.TitleCustomization.IncludeFilename = value;
                UpdateFormats();
            }
        }
        public bool IncludeMetadata
        {
            get => settings.TitleCustomization.IncludeMetadata;
            set
            {
                settings.TitleCustomization.IncludeMetadata = value;
                UpdateFormats();
            }
        }
        public bool IncludeExtension
        {
            get => settings.TitleCustomization.IncludeExtension;
            set
            {
                settings.TitleCustomization.IncludeExtension = value;
                UpdateFormats();
            }
        }

        public string RegexPattern
        {
            get => settings.TitleCustomization.RegexPattern;
            set
            {
                settings.TitleCustomization.RegexPattern = value;
                UpdateFormats();
            }
        }
        public string RegexReplacement
        {
            get => settings.TitleCustomization.RegexReplacement;
            set
            {
                settings.TitleCustomization.RegexReplacement = value;
                UpdateFormats();
            }
        }
        public bool InvalidRegex { get; set; } = false;

        public bool EnabledRegex
        {
            get => settings.TitleCustomization.EnabledRegex;
            set
            {
                settings.TitleCustomization.EnabledRegex = value;
                UpdateFormats();
            }
        }
        public bool DontShowTitleCustomizationAgain
        {
            get => settings.TitleCustomization.DontShowTitleCustomizationAgain;
            set => settings.TitleCustomization.DontShowTitleCustomizationAgain = value;
        }


        public void LoadFormats()
        {
            if (IsTemplateMode)
            {
                Videos.Clear();
                foreach (var video in service)
                {
                    Videos.Add(new VideoTitleTemplate(video));
                }
            }
            else
            {
                Videos.Clear();
                foreach (var video in service)
                {
                    Videos.Add(new VideoTitleTemplate(video));
                }
            }
            UpdateFormats();
        }

        public void UpdateFormats()
        {
            try
            {
                var regex = new Regex(RegexPattern);
                InvalidRegex = false;
            }
            catch
            {
                InvalidRegex = true;
            }

            foreach (var video in Videos)
            {
                video.Title = settings.TitleCustomization.CustomizeTitle(video.FilePath, EnabledRegex && !InvalidRegex);
                if (!IsTemplateMode)
                {
                    service.FirstOrDefault(v => v.ID == video.ID)!.Title = video.Title;
                    service.Update(UpdateType.ForceUpdateVideoCollection);
                }
            }
        }
    }
}
