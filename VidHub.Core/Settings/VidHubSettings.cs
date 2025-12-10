using System.Text.Json;
using System.Text.RegularExpressions;
using VidHub.Core.Notifications;
using VidHub.Core.Settings.Models;
using VidHub.Platform;

namespace VidHub.Core.Settings
{
    public class VidHubSettings : IVidHubSettings
    {
        public static IVidHubSettings Instance => Context.Host.GetService<IVidHubSettings>();

        public GeneralSettings General { get; set; } = new GeneralSettings();
        public DisplaySettings Display { get; set; } = new DisplaySettings();
        public NotificationSettings Notifications { get; set; } = new NotificationSettings();
        public VideoHealthSettings VideoHealth { get; set; } = new VideoHealthSettings();
        public PerformanceSettings Performance { get; set; } = new PerformanceSettings();
        public SidePanelSettings SidePanel { get; set; } = new SidePanelSettings();
        public ModalSettings Modals { get; set; } = new ModalSettings();


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
            StringComparison searchComparison = SidePanel.UseCaseSensitiveSearch
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            if (!string.IsNullOrEmpty(SidePanel.SearchText))
            {
                if (!video.Title.Contains(SidePanel.SearchText, searchComparison))
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
            }

            if (SidePanel.EndDate.HasValue && video.Date > SidePanel.EndDate.Value)
            {
                return false;
            }

            if (SidePanel.FilterDuration)
            {
                if (SidePanel.MinDuration.HasValue && video.Duration < SidePanel.MinDuration.Value)
                {
                    return false;
                }
            }

            return !SidePanel.MaxDuration.HasValue || video.Duration <= SidePanel.MaxDuration.Value;
        }

        public TimeSpan GetPreviewImageTime(Video video)
        {
            return video.DefaultVideoStream != null && video.DefaultVideoStream.Duration > TimeSpan.Zero
                ? GetPreviewImageTime(video.DefaultVideoStream.Duration)
                : GetPreviewImageTime(video.Duration);
        }
        public TimeSpan GetPreviewImageTime(TimeSpan duration)
        {
            return Modals.PreviewImageFormat.RelativePosition
                ? duration * Modals.PreviewImageFormat.RelativeTime
                : duration > Modals.PreviewImageFormat.FixedTime ? Modals.PreviewImageFormat.FixedTime : duration;
        }

        public void CustomizeVideoTitle(ref Video video)
        {
            video.Title = GetCustomizedVideoTitle(video);
        }
        public string GetCustomizedVideoTitle(Video video)
        {
            return GetCustomizedVideoTitle(video, Modals.TitleFormat.UseRegex);
        }
        public string GetCustomizedVideoTitle(Video video, bool useRegex)
        {
            string path = Path.GetDirectoryName(video.FilePath) + Path.DirectorySeparatorChar;
            string date = video.Date.ToString("yyyy-MM-dd");
            string filename = Path.GetFileNameWithoutExtension(video.FilePath);
            string metadata = $"({video.DefaultVideoStream?.Codec})_[{video.DefaultVideoStream?.Width}x{video.DefaultVideoStream?.Height}_{video.DefaultVideoStream?.Framerate.Item1 / video.DefaultVideoStream?.Framerate.Item2}fps_{video.DefaultVideoStream?.Bitrate / 1048576}Mbps_{video.DefaultAudioStream?.ChannelLayout}]";
            string extension = Path.GetExtension(video.FilePath);
            string newTitle = "";
            if (Modals.TitleFormat.IncludePath)
            {
                newTitle += path;
            }
            if (Modals.TitleFormat.IncludeDate)
            {
                newTitle += date;
            }
            if (Modals.TitleFormat.IncludeFilename)
            {
                newTitle += Modals.TitleFormat.IncludeDate ? $"_{filename}" : filename;
            }
            if (Modals.TitleFormat.IncludeMetadata)
            {
                newTitle += Modals.TitleFormat.IncludeDate || Modals.TitleFormat.IncludeFilename ? $"_{metadata}" : metadata;
            }
            if (Modals.TitleFormat.IncludeExtension)
            {
                newTitle += extension;
            }
            if (useRegex)
            {
                try
                {
                    Regex regex = new(Modals.TitleFormat.RegexPattern);
                    newTitle = regex.Replace(newTitle, Modals.TitleFormat.RegexReplacement);
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
                    VideoHealth = settings.VideoHealth;
                    Performance = settings.Performance;
                    Modals = settings.Modals;
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
