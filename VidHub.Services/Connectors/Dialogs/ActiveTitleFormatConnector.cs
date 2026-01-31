using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Services.Base;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Services.Connectors.Dialogs
{
    public class ActiveTitleFormatConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), ITitleFormatConnector
    {
        private readonly ILogger logger = VidHubContext.Logger;
        public ObservableCollection<string> Titles { get; } = [];

        public bool IncludePath
        {
            get => settings.Dialogs.TitleFormat.IncludePath;
            set
            {
                settings.Dialogs.TitleFormat.IncludePath = value;
                logger.LogDebug("IncludePath set to {Value}", value);
                UpdateTitles();
            }
        }
        public bool IncludeDate
        {
            get => settings.Dialogs.TitleFormat.IncludeDate;
            set
            {
                settings.Dialogs.TitleFormat.IncludeDate = value;
                logger.LogDebug("IncludeDate set to {Value}", value);
                UpdateTitles();
            }
        }
        public bool IncludeFilename
        {
            get => settings.Dialogs.TitleFormat.IncludeFilename;
            set
            {
                settings.Dialogs.TitleFormat.IncludeFilename = value;
                logger.LogDebug("IncludeFilename set to {Value}", value);
                UpdateTitles();
            }
        }
        public bool IncludeMetadata
        {
            get => settings.Dialogs.TitleFormat.IncludeMetadata;
            set
            {
                settings.Dialogs.TitleFormat.IncludeMetadata = value;
                logger.LogDebug("IncludeMetadata set to {Value}", value);
                UpdateTitles();
            }
        }
        public bool IncludeExtension
        {
            get => settings.Dialogs.TitleFormat.IncludeExtension;
            set
            {
                settings.Dialogs.TitleFormat.IncludeExtension = value;
                logger.LogDebug("IncludeExtension set to {Value}", value);
                UpdateTitles();
            }
        }

        public string RegexPattern
        {
            get => settings.Dialogs.TitleFormat.RegexPattern;
            set
            {
                settings.Dialogs.TitleFormat.RegexPattern = value;
                logger.LogDebug("RegexPattern set");
                UpdateTitles();
            }
        }
        public string RegexReplacement
        {
            get => settings.Dialogs.TitleFormat.RegexReplacement;
            set
            {
                settings.Dialogs.TitleFormat.RegexReplacement = value;
                logger.LogDebug("RegexReplacement set");
                UpdateTitles();
            }
        }
        public bool InvalidRegex { get; set; } = false;

        public bool UseRegex
        {
            get => settings.Dialogs.TitleFormat.UseRegex;
            set
            {
                settings.Dialogs.TitleFormat.UseRegex = value;
                logger.LogDebug("UseRegex set to {Value}", value);
                UpdateTitles();
                Update(UpdateSections.ALL);
            }
        }

        public bool HideTitleCustomization
        {
            get => settings.Dialogs.TitleFormat.HideTitleFormatDialog;
            set => settings.Dialogs.TitleFormat.HideTitleFormatDialog = value;
        }


        public void UpdateTitles()
        {
            logger.LogTrace("UpdateTitles invoked");
            try
            {
                Regex regex = new(RegexPattern);
                InvalidRegex = false;
                logger.LogDebug("Regex pattern validated");
            }
            catch (Exception ex)
            {
                InvalidRegex = true;
                logger.LogWarning(ex, "Invalid regex pattern in ActiveTitleFormatConnector");
            }

            IList<Video> videos = [.. vs.GetAllVideos().Where(v => !v.LoadingFinished)];
            for (int i = 0; i < Math.Min(Titles.Count, videos.Count); i++)
            {
                if (!Equals(Titles[i], videos[i]))
                {
                    Titles[i] = settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex);
                    videos[i].Title = settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex);
                }
            }
            while (Titles.Count > videos.Count)
            {
                Titles.RemoveAt(Titles.Count - 1);
            }
            for (int i = Titles.Count; i < videos.Count; i++)
            {
                Titles.Add(settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex));
                videos[i].Title = settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex);
            }
            foreach (Video video in vs.GetAllVideos().Where(v => !v.LoadingFinished))
            {
                video.Title = settings.GetCustomizedVideoTitle(video, UseRegex && !InvalidRegex);
            }
            logger.LogDebug("UpdateTitles completed for {Count} videos", videos.Count);
        }
    }
}
