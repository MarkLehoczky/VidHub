using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Core.Models;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoTitleFormatCustomizationConnector(IVideoService vs, ISettingsService settings) : IVideoTitleFormatCustomizationConnector
    {
        public ObservableCollection<VideoTitleTemplate> Videos { get; } = [];
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


        public void ChangeVideos(IEnumerable<int> ids)
        {
            ChangeVideos(vs.Where(v => ids.Contains(v.ID)));
        }
        public void ChangeVideos(IEnumerable<Video> videos)
        {
            Videos.Clear();

            foreach (Video video in videos)
            {
                Videos.Add(new VideoTitleTemplate(video));
            }

            UpdateFormats();
        }

        public void UpdateFormats()
        {
            try
            {
                Regex regex = new(RegexPattern);
                InvalidRegex = false;
            }
            catch
            {
                InvalidRegex = true;
            }

            foreach (VideoTitleTemplate video in Videos)
            {
                video.Title = settings.TitleCustomization.CustomizeTitle(video.Instance, EnabledRegex && !InvalidRegex);
                if (!IsTemplateMode)
                {
                    video.Instance.Title = video.Title;
                }
            }
        }

        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(UpdateType type)
        {
            vs.Update(type);
        }
    }
}
