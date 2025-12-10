using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VidHub.Core;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoTitleFormatCustomizationConnector(IVideoService vs, IVidHubSettings settings) : IVideoTitleFormatCustomizationConnector
    {
        public ObservableCollection<VideoTitleTemplate> Videos { get; } = [];
        public bool IsTemplateMode { get; set; }

        public bool IncludePath
        {
            get => settings.Modals.TitleFormat.IncludePath;
            set
            {
                settings.Modals.TitleFormat.IncludePath = value;
                UpdateFormats();
            }
        }
        public bool IncludeDate
        {
            get => settings.Modals.TitleFormat.IncludeDate;
            set
            {
                settings.Modals.TitleFormat.IncludeDate = value;
                UpdateFormats();
            }
        }
        public bool IncludeFilename
        {
            get => settings.Modals.TitleFormat.IncludeFilename;
            set
            {
                settings.Modals.TitleFormat.IncludeFilename = value;
                UpdateFormats();
            }
        }
        public bool IncludeMetadata
        {
            get => settings.Modals.TitleFormat.IncludeMetadata;
            set
            {
                settings.Modals.TitleFormat.IncludeMetadata = value;
                UpdateFormats();
            }
        }
        public bool IncludeExtension
        {
            get => settings.Modals.TitleFormat.IncludeExtension;
            set
            {
                settings.Modals.TitleFormat.IncludeExtension = value;
                UpdateFormats();
            }
        }

        public string RegexPattern
        {
            get => settings.Modals.TitleFormat.RegexPattern;
            set
            {
                settings.Modals.TitleFormat.RegexPattern = value;
                UpdateFormats();
            }
        }
        public string RegexReplacement
        {
            get => settings.Modals.TitleFormat.RegexReplacement;
            set
            {
                settings.Modals.TitleFormat.RegexReplacement = value;
                UpdateFormats();
            }
        }
        public bool InvalidRegex { get; set; } = false;

        public bool EnabledRegex
        {
            get => settings.Modals.TitleFormat.UseRegex;
            set
            {
                settings.Modals.TitleFormat.UseRegex = value;
                UpdateFormats();
            }
        }
        public bool DontShowTitleCustomizationAgain
        {
            get => settings.Modals.TitleFormat.HideTitleCustomization;
            set => settings.Modals.TitleFormat.HideTitleCustomization = value;
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
                video.Title = settings.GetCustomizedVideoTitle(video.Instance, EnabledRegex && !InvalidRegex);
                if (!IsTemplateMode)
                {
                    video.Instance.Title = video.Title;
                }
            }
        }

        public void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(IEnumerable<UpdateSection> sections)
        {
            vs.Update(sections);
        }
        public void Update(params UpdateSection[] sections)
        {
            vs.Update(sections);
        }
    }
}
