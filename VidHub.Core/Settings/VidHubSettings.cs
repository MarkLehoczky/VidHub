using System.Text.Json;
using System.Text.RegularExpressions;
using VidHub.Core.Notifications;
using VidHub.Platform.Environment;
using static VidHub.Core.Streams.Framerate;
using static VidHub.Core.Streams.Resolution;

namespace VidHub.Core.Settings
{
    public class VidHubSettings : IVidHubSettings
    {
        public static IVidHubSettings Instance => Context.Host.GetService<IVidHubSettings>();

        public GeneralSettings General { get; set; } = new GeneralSettings();
        public DisplaySettings Display { get; set; } = new DisplaySettings();
        public NotificationSettings Notifications { get; set; } = new NotificationSettings();
        public HealthSettings Health { get; set; } = new HealthSettings();
        public PerformanceSettings Performance { get; set; } = new PerformanceSettings();
        public SidePanelSettings SidePanel { get; set; } = new SidePanelSettings();
        public DialogSettings Dialogs { get; set; } = new DialogSettings();


        public bool DisplayNotification(BaseNotification notification)
        {
            return notification.Severity switch
            {
                NotificationSeverity.INFORMATIONAL => Notifications.DisplayInformationalSystemNotification || Notifications.DisplayInformationalBarNotification,
                NotificationSeverity.SUCCESS => Notifications.DisplaySuccessSystemNotification || Notifications.DisplaySuccessBarNotification,
                NotificationSeverity.WARNING => Notifications.DisplayWarningSystemNotification || Notifications.DisplayWarningBarNotification,
                NotificationSeverity.ERROR => Notifications.DisplayErrorSystemNotification || Notifications.DisplayErrorBarNotification,
                _ => false,
            };
        }
        public bool DisplayNotification(SystemNotification notification)
        {
            return notification.Severity switch
            {
                NotificationSeverity.INFORMATIONAL => Notifications.DisplayInformationalSystemNotification,
                NotificationSeverity.SUCCESS => Notifications.DisplaySuccessSystemNotification,
                NotificationSeverity.WARNING => Notifications.DisplayWarningSystemNotification,
                NotificationSeverity.ERROR => Notifications.DisplayErrorSystemNotification,
                _ => false,
            };
        }
        public bool DisplayNotification(BarNotification notification)
        {
            return notification.Severity switch
            {
                NotificationSeverity.INFORMATIONAL => Notifications.DisplayInformationalBarNotification,
                NotificationSeverity.SUCCESS => Notifications.DisplaySuccessBarNotification,
                NotificationSeverity.WARNING => Notifications.DisplayWarningBarNotification,
                NotificationSeverity.ERROR => Notifications.DisplayErrorBarNotification,
                _ => false,
            };
        }

        public StringComparison SearchComparison()
        {
            return SidePanel.UseCaseSensitiveSearch
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
        }
        public bool ValidVideo(Video video)
        {
            if (!string.IsNullOrEmpty(SidePanel.SearchText))
            {
                if (!video.Title.Contains(SidePanel.SearchText, SearchComparison()))
                {
                    return false;
                }
            }

            if (SidePanel.FilterDate)
            {
                if (SidePanel.StartDate.HasValue && video.Date < SidePanel.StartDate.Value)
                {
                    return false;
                }
                if (SidePanel.EndDate.HasValue && video.Date > SidePanel.EndDate.Value)
                {
                    return false;
                }
            }

            if (SidePanel.FilterDuration)
            {
                if (SidePanel.MinDuration.HasValue && video.Duration < SidePanel.MinDuration.Value)
                {
                    return false;
                }
                if (SidePanel.MaxDuration.HasValue && video.Duration > SidePanel.MaxDuration.Value)
                {
                    return false;
                }
            }

            bool filterResolution = SidePanel.DisplayMaximumResolutionVideos
                || SidePanel.DisplayLargeResolutionVideos
                || SidePanel.DisplayMediumResolutionVideos
                || SidePanel.DisplayLowResolutionVideos
                || SidePanel.DisplayMinimumResolutionVideos;
            if (filterResolution)
            {
                if (video.Metadata.DefaultVideoStream == null)
                {
                    return false;
                }
                bool matchingResolution = video.Metadata.DefaultVideoStream.Resolution.Definition switch
                {
                    DefinedResolution.UHD8K => SidePanel.DisplayMaximumResolutionVideos,
                    DefinedResolution.UHD4K => SidePanel.DisplayMaximumResolutionVideos,
                    DefinedResolution.QHD => SidePanel.DisplayLargeResolutionVideos,
                    DefinedResolution.FHD => SidePanel.DisplayMediumResolutionVideos,
                    DefinedResolution.HD => SidePanel.DisplayLowResolutionVideos,
                    DefinedResolution.SD => SidePanel.DisplayMinimumResolutionVideos,
                    DefinedResolution.LOW => SidePanel.DisplayMinimumResolutionVideos,
                    _ => false,
                };
                if (!matchingResolution)
                {
                    return false;
                }
            }

            bool filterFramerate = SidePanel.DisplayMaximumFramerateVideos
                || SidePanel.DisplayLargeFramerateVideos
                || SidePanel.DisplayMediumFramerateVideos
                || SidePanel.DisplayLowFramerateVideos
                || SidePanel.DisplayMinimumFramerateVideos;
            if (filterFramerate)
            {
                if (video.Metadata.DefaultVideoStream == null)
                {
                    return false;
                }
                bool matchingFramerate = video.Metadata.DefaultVideoStream.Framerate.Definition switch
                {
                    DefinedFramerate.FPS240 => SidePanel.DisplayMaximumFramerateVideos,
                    DefinedFramerate.FPS120 => SidePanel.DisplayMaximumFramerateVideos,
                    DefinedFramerate.FPS90 => SidePanel.DisplayLargeFramerateVideos,
                    DefinedFramerate.FPS60 => SidePanel.DisplayLargeFramerateVideos,
                    DefinedFramerate.FPS30 => SidePanel.DisplayMediumFramerateVideos,
                    DefinedFramerate.FPS24 => SidePanel.DisplayLowFramerateVideos,
                    DefinedFramerate.FPS20 => SidePanel.DisplayLowFramerateVideos,
                    DefinedFramerate.FPS12 => SidePanel.DisplayMinimumFramerateVideos,
                    DefinedFramerate.LOW => SidePanel.DisplayMinimumFramerateVideos,
                    _ => false,
                };
                if (!matchingFramerate)
                {
                    return false;
                }
            }

            return true;
        }

        public TimeSpan GetPreviewImageTime(Video video)
        {
            return video.Metadata.DefaultVideoStream != null && video.Metadata.DefaultVideoStream.Duration > TimeSpan.Zero
                ? GetPreviewImageTime(video.Metadata.DefaultVideoStream.Duration)
                : GetPreviewImageTime(video.Duration);
        }
        public TimeSpan GetPreviewImageTime(TimeSpan duration)
        {
            return Dialogs.PreviewImageFormat.RelativePosition
                ? duration * Dialogs.PreviewImageFormat.RelativeTime
                : duration > Dialogs.PreviewImageFormat.FixedTime ? Dialogs.PreviewImageFormat.FixedTime : duration;
        }

        public void CustomizeVideoTitle(ref Video video)
        {
            video.Title = GetCustomizedVideoTitle(video);
        }
        public string GetCustomizedVideoTitle(Video video)
        {
            return GetCustomizedVideoTitle(video, Dialogs.TitleFormat.UseRegex);
        }
        public string GetCustomizedVideoTitle(Video video, bool useRegex)
        {
            string newTitle = "";
            if (Dialogs.TitleFormat.IncludePath)
            {
                string path = Path.GetDirectoryName(video.FilePath) + Path.DirectorySeparatorChar;
                newTitle += path;
            }
            if (Dialogs.TitleFormat.IncludeDate)
            {
                string date = video.Date.ToString("yyyy-MM-dd");
                newTitle += date;
            }
            if (Dialogs.TitleFormat.IncludeFilename)
            {
                string filename = Path.GetFileNameWithoutExtension(video.FilePath);
                newTitle += Dialogs.TitleFormat.IncludeDate ? $"_{filename}" : filename;
            }
            if (Dialogs.TitleFormat.IncludeMetadata && video.Metadata.DefaultVideoStream is not null)
            {
                string codec = video.Metadata.DefaultVideoStream.Codec;
                string size = $"{video.Metadata.DefaultVideoStream.Width}x{video.Metadata.DefaultVideoStream.Height}";
                double fps = Math.Round(video.Metadata.DefaultVideoStream.Framerate.Value);
                string channel = video.Metadata.DefaultAudioStream?.ChannelLayout ?? "silent";
                string metadata = $"({codec})_[{size}_{fps}fps_{channel}]";
                newTitle += Dialogs.TitleFormat.IncludeDate || Dialogs.TitleFormat.IncludeFilename ? $"_{metadata}" : metadata;
            }
            if (Dialogs.TitleFormat.IncludeExtension)
            {
                string extension = Path.GetExtension(video.FilePath);
                newTitle += extension;
            }
            if (useRegex)
            {
                try
                {
                    Regex regex = new(Dialogs.TitleFormat.RegexPattern);
                    newTitle = regex.Replace(newTitle, Dialogs.TitleFormat.RegexReplacement);
                }
                catch { }
            }
            return newTitle;
        }

        public void Load()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            string appDataSettings = Path.Combine(appDataDirectory, "settings.json");

            if (File.Exists(appDataSettings))
            {
                string json = File.ReadAllText(appDataSettings);
                VidHubSettings? settings = JsonSerializer.Deserialize<VidHubSettings>(json);

                if (settings != null)
                {
                    General = settings.General;
                    Display = settings.Display;
                    Notifications = settings.Notifications;
                    Health = settings.Health;
                    Performance = settings.Performance;
                    Dialogs = settings.Dialogs;
                    if (General.KeepSidePanelSettings)
                    {
                        SidePanel = settings.SidePanel;
                    }
                }
            }
        }
        public void Save()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            string appDataSettings = Path.Combine(appDataDirectory, "settings.json");
            _ = Directory.CreateDirectory(appDataDirectory);

            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(appDataSettings, json);
        }
    }
}
