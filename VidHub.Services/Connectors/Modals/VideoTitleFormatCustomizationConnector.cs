using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoTitleFormatCustomizationConnector(IVideoService vs, IVidHubSettings settings) : ServiceTemplate(vs), IVideoTitleFormatCustomizationConnector
    {
        public ObservableCollection<string> Titles { get; } = [];

        public bool IncludePath
        {
            get => settings.Modals.TitleFormat.IncludePath;
            set
            {
                settings.Modals.TitleFormat.IncludePath = value;
                UpdateTitles();
            }
        }
        public bool IncludeDate
        {
            get => settings.Modals.TitleFormat.IncludeDate;
            set
            {
                settings.Modals.TitleFormat.IncludeDate = value;
                UpdateTitles();
            }
        }
        public bool IncludeFilename
        {
            get => settings.Modals.TitleFormat.IncludeFilename;
            set
            {
                settings.Modals.TitleFormat.IncludeFilename = value;
                UpdateTitles();
            }
        }
        public bool IncludeMetadata
        {
            get => settings.Modals.TitleFormat.IncludeMetadata;
            set
            {
                settings.Modals.TitleFormat.IncludeMetadata = value;
                UpdateTitles();
            }
        }
        public bool IncludeExtension
        {
            get => settings.Modals.TitleFormat.IncludeExtension;
            set
            {
                settings.Modals.TitleFormat.IncludeExtension = value;
                UpdateTitles();
            }
        }

        public string RegexPattern
        {
            get => settings.Modals.TitleFormat.RegexPattern;
            set
            {
                settings.Modals.TitleFormat.RegexPattern = value;
                UpdateTitles();
            }
        }
        public string RegexReplacement
        {
            get => settings.Modals.TitleFormat.RegexReplacement;
            set
            {
                settings.Modals.TitleFormat.RegexReplacement = value;
                UpdateTitles();
            }
        }
        public bool InvalidRegex { get; set; } = false;

        public bool UseRegex
        {
            get => settings.Modals.TitleFormat.UseRegex;
            set
            {
                settings.Modals.TitleFormat.UseRegex = value;
                UpdateTitles();
                Update(UpdateSections.ALL);
            }
        }

        public bool HideTitleCustomization
        {
            get => settings.Modals.TitleFormat.HideTitleCustomization;
            set => settings.Modals.TitleFormat.HideTitleCustomization = value;
        }


        public virtual void UpdateTitles()
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

            IList<Video> videos = vs.GetAllVideos();
            for (int i = 0; i < Math.Min(Titles.Count, videos.Count); i++)
            {
                if (!Equals(Titles[i], videos[i]))
                {
                    Titles[i] = settings.GetCustomizedVideoTitle(videos[i]);
                }
            }
            while (Titles.Count > videos.Count)
            {
                Titles.RemoveAt(Titles.Count - 1);
            }
            for (int i = Titles.Count; i < videos.Count; i++)
            {
                Titles.Add(settings.GetCustomizedVideoTitle(videos[i]));
            }
        }
    }
}
