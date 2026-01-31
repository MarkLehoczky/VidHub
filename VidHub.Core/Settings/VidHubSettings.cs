using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.RegularExpressions;
using VidHub.Core.Notifications;
using VidHub.Core.Streams;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Settings
{
    public class VidHubSettings : IVidHubSettings
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public static IVidHubSettings Instance => VidHubContext.Host.GetService<IVidHubSettings>();

        public GeneralSettings General { get; set; } = new GeneralSettings();
        public DisplaySettings Display { get; set; } = new DisplaySettings();
        public NotificationSettings Notifications { get; set; } = new NotificationSettings();
        public HealthSettings Health { get; set; } = new HealthSettings();
        public PerformanceSettings Performance { get; set; } = new PerformanceSettings();
        public SidePanelSettings SidePanel { get; set; } = new SidePanelSettings();
        public DialogSettings Dialogs { get; set; } = new DialogSettings();


        public bool DisplayNotification(BaseNotification notification)
        {
            logger.LogTrace("Enter DisplayNotification(BaseNotification) with Severity={Severity}", notification.Severity);
            bool result = notification.Severity switch
            {
                NotificationSeverity.INFORMATIONAL => Notifications.DisplayInformationalSystemNotification || Notifications.DisplayInformationalBarNotification,
                NotificationSeverity.SUCCESS => Notifications.DisplaySuccessSystemNotification || Notifications.DisplaySuccessBarNotification,
                NotificationSeverity.WARNING => Notifications.DisplayWarningSystemNotification || Notifications.DisplayWarningBarNotification,
                NotificationSeverity.ERROR => Notifications.DisplayErrorSystemNotification || Notifications.DisplayErrorBarNotification,
                _ => false,
            };
            logger.LogDebug("DisplayNotification(BaseNotification) returning {Result} for Severity={Severity}", result, notification.Severity);
            return result;
        }
        public bool DisplayNotification(SystemNotification notification)
        {
            logger.LogTrace("Enter DisplayNotification(SystemNotification) with Severity={Severity}", notification.Severity);
            bool result = notification.Severity switch
            {
                NotificationSeverity.INFORMATIONAL => Notifications.DisplayInformationalSystemNotification,
                NotificationSeverity.SUCCESS => Notifications.DisplaySuccessSystemNotification,
                NotificationSeverity.WARNING => Notifications.DisplayWarningSystemNotification,
                NotificationSeverity.ERROR => Notifications.DisplayErrorSystemNotification,
                _ => false,
            };
            logger.LogDebug("DisplayNotification(SystemNotification) returning {Result} for Severity={Severity}", result, notification.Severity);
            return result;
        }
        public bool DisplayNotification(BarNotification notification)
        {
            logger.LogTrace("Enter DisplayNotification(BarNotification) with Severity={Severity}", notification.Severity);
            bool result = notification.Severity switch
            {
                NotificationSeverity.INFORMATIONAL => Notifications.DisplayInformationalBarNotification,
                NotificationSeverity.SUCCESS => Notifications.DisplaySuccessBarNotification,
                NotificationSeverity.WARNING => Notifications.DisplayWarningBarNotification,
                NotificationSeverity.ERROR => Notifications.DisplayErrorBarNotification,
                _ => false,
            };
            logger.LogDebug("DisplayNotification(BarNotification) returning {Result} for Severity={Severity}", result, notification.Severity);
            return result;
        }

        public StringComparison SearchComparison()
        {
            logger.LogTrace("Enter SearchComparison. UseCaseSensitiveSearch={UseCaseSensitive}", SidePanel.UseCaseSensitiveSearch);
            StringComparison comparison = SidePanel.UseCaseSensitiveSearch
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
            logger.LogDebug("SearchComparison selected {Comparison}", comparison);
            return comparison;
        }
        public bool ValidVideo(Video video)
        {
            logger.LogTrace("Enter ValidVideo for Video Path={Path}", video.FilePath);

            if (!string.IsNullOrEmpty(SidePanel.SearchText))
            {
                logger.LogDebug("Applying search text filter: '{SearchText}'", SidePanel.SearchText);
                if (!video.Title.Contains(SidePanel.SearchText, SearchComparison()))
                {
                    logger.LogDebug("Video '{Title}' does not contain search text '{SearchText}'", video.Title, SidePanel.SearchText);
                    return false;
                }
                else
                {
                    logger.LogTrace("Video '{Title}' matched search text '{SearchText}'", video.Title, SidePanel.SearchText);
                }
            }
            else
            {
                logger.LogTrace("No search text filter applied");
            }

            if (SidePanel.FilterDate)
            {
                logger.LogDebug("Applying date filter: Start={StartDate} End={EndDate}", SidePanel.StartDate, SidePanel.EndDate);
                if (SidePanel.StartDate.HasValue && video.Date < SidePanel.StartDate.Value)
                {
                    logger.LogDebug("Video date {VideoDate} is before StartDate {StartDate}", video.Date, SidePanel.StartDate);
                    return false;
                }
                if (SidePanel.EndDate.HasValue && video.Date > SidePanel.EndDate.Value)
                {
                    logger.LogDebug("Video date {VideoDate} is after EndDate {EndDate}", video.Date, SidePanel.EndDate);
                    return false;
                }
            }
            else
            {
                logger.LogTrace("No date filter applied");
            }

            if (SidePanel.FilterDuration)
            {
                logger.LogDebug("Applying duration filter: Min={MinDuration} Max={MaxDuration}", SidePanel.MinDuration, SidePanel.MaxDuration);
                if (SidePanel.MinDuration.HasValue && video.Duration < SidePanel.MinDuration.Value)
                {
                    logger.LogDebug("Video duration {VideoDuration} is less than MinDuration {MinDuration}", video.Duration, SidePanel.MinDuration);
                    return false;
                }
                if (SidePanel.MaxDuration.HasValue && video.Duration > SidePanel.MaxDuration.Value)
                {
                    logger.LogDebug("Video duration {VideoDuration} is greater than MaxDuration {MaxDuration}", video.Duration, SidePanel.MaxDuration);
                    return false;
                }
            }
            else
            {
                logger.LogTrace("No duration filter applied");
            }

            bool filterResolution = SidePanel.DisplayMaximumResolutionVideos
                || SidePanel.DisplayLargeResolutionVideos
                || SidePanel.DisplayMediumResolutionVideos
                || SidePanel.DisplayLowResolutionVideos
                || SidePanel.DisplayMinimumResolutionVideos;
            if (filterResolution)
            {
                logger.LogDebug("Applying resolution filter. Flags: Max={Max} Large={Large} Medium={Medium} Low={Low} Min={Min}",
                    SidePanel.DisplayMaximumResolutionVideos, SidePanel.DisplayLargeResolutionVideos, SidePanel.DisplayMediumResolutionVideos, SidePanel.DisplayLowResolutionVideos, SidePanel.DisplayMinimumResolutionVideos);

                if (video.Metadata.DefaultVideoStream == null)
                {
                    logger.LogDebug("Video '{Title}' has no DefaultVideoStream, excluded by resolution filter", video.Title);
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
                logger.LogTrace("Resolution match result={Match} for Video '{Title}' with Definition={Definition}", matchingResolution, video.Title, video.Metadata.DefaultVideoStream?.Resolution.Definition);
                if (!matchingResolution)
                {
                    logger.LogDebug("Video '{Title}' did not match resolution filters", video.Title);
                    return false;
                }
            }
            else
            {
                logger.LogTrace("No resolution filter applied");
            }

            bool filterFramerate = SidePanel.DisplayMaximumFramerateVideos
                || SidePanel.DisplayLargeFramerateVideos
                || SidePanel.DisplayMediumFramerateVideos
                || SidePanel.DisplayLowFramerateVideos
                || SidePanel.DisplayMinimumFramerateVideos;
            if (filterFramerate)
            {
                logger.LogDebug("Applying framerate filter. Flags: Max={Max} Large={Large} Medium={Medium} Low={Low} Min={Min}",
                    SidePanel.DisplayMaximumFramerateVideos, SidePanel.DisplayLargeFramerateVideos, SidePanel.DisplayMediumFramerateVideos, SidePanel.DisplayLowFramerateVideos, SidePanel.DisplayMinimumFramerateVideos);

                if (video.Metadata.DefaultVideoStream == null)
                {
                    logger.LogDebug("Video '{Title}' has no DefaultVideoStream, excluded by framerate filter", video.Title);
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
                logger.LogTrace("Framerate match result={Match} for Video '{Title}' with Definition={Definition}", matchingFramerate, video.Title, video.Metadata.DefaultVideoStream?.Framerate.Definition);
                if (!matchingFramerate)
                {
                    logger.LogDebug("Video '{Title}' did not match framerate filters", video.Title);
                    return false;
                }
            }
            else
            {
                logger.LogTrace("No framerate filter applied");
            }

            logger.LogDebug("Video '{Title}' passed all filters", video.Title);
            return true;
        }

        public TimeSpan GetPreviewImageTime(Video video)
        {
            logger.LogTrace("Enter GetPreviewImageTime(Video) for {Path}", video?.FilePath);
            bool useDefaultStream = video.Metadata.DefaultVideoStream != null && video.Metadata.DefaultVideoStream.Duration > TimeSpan.Zero;
            logger.LogDebug("DefaultVideoStream available={Available}", useDefaultStream);
            TimeSpan result = useDefaultStream
                ? GetPreviewImageTime(video.Metadata.DefaultVideoStream.Duration)
                : GetPreviewImageTime(video.Duration);
            logger.LogDebug("GetPreviewImageTime(Video) returning {Result}", result);
            return result;
        }
        public TimeSpan GetPreviewImageTime(TimeSpan duration)
        {
            logger.LogTrace("Enter GetPreviewImageTime(TimeSpan) Duration={Duration}", duration);

            if (Dialogs?.PreviewImageFormat == null)
            {
                logger.LogWarning("Dialogs.PreviewImageFormat is null, returning TimeSpan.Zero");
                return TimeSpan.Zero;
            }

            if (Dialogs.PreviewImageFormat.RelativePosition)
            {
                TimeSpan relative = duration * Dialogs.PreviewImageFormat.RelativeTime;
                logger.LogDebug("Using relative preview position. Duration={Duration}, RelativeTime={RelativeTime}, Result={Result}", duration, Dialogs.PreviewImageFormat.RelativeTime, relative);
                return relative;
            }
            else
            {
                TimeSpan selected = duration > Dialogs.PreviewImageFormat.FixedTime ? Dialogs.PreviewImageFormat.FixedTime : duration;
                logger.LogDebug("Using fixed preview time. FixedTime={FixedTime}, Selected={Selected}", Dialogs.PreviewImageFormat.FixedTime, selected);
                return selected;
            }
        }

        public void CustomizeVideoTitle(ref Video video)
        {
            logger.LogTrace("Enter CustomizeVideoTitle for {Path}", video?.FilePath);
            video.Title = GetCustomizedVideoTitle(video);
            logger.LogDebug("Customized title set to '{Title}' for {Path}", video.Title, video.FilePath);
        }
        public string GetCustomizedVideoTitle(Video video)
        {
            logger.LogTrace("Enter GetCustomizedVideoTitle for {Path}", video?.FilePath);
            return GetCustomizedVideoTitle(video, Dialogs.TitleFormat.UseRegex);
        }
        public string GetCustomizedVideoTitle(Video video, bool useRegex)
        {
            logger.LogTrace("Enter GetCustomizedVideoTitle(Video, useRegex={UseRegex}) for {Path}", useRegex, video?.FilePath);

            string newTitle = "";
            if (Dialogs.TitleFormat.IncludePath)
            {
                string path = Path.GetDirectoryName(video.FilePath) + Path.DirectorySeparatorChar;
                newTitle += path;
                logger.LogDebug("Included path in title: {Path}", path);
            }
            else
            {
                logger.LogTrace("Path not included in title");
            }
            if (Dialogs.TitleFormat.IncludeDate)
            {
                string date = video.Date.ToString("yyyy-MM-dd");
                newTitle += date;
                logger.LogDebug("Included date in title: {Date}", date);
            }
            else
            {
                logger.LogTrace("Date not included in title");
            }
            if (Dialogs.TitleFormat.IncludeFilename)
            {
                string filename = Path.GetFileNameWithoutExtension(video.FilePath);
                newTitle += Dialogs.TitleFormat.IncludeDate ? $"_{filename}" : filename;
                logger.LogDebug("Included filename in title: {Filename}", filename);
            }
            else
            {
                logger.LogTrace("Filename not included in title");
            }
            if (Dialogs.TitleFormat.IncludeMetadata && video.Metadata.DefaultVideoStream is not null)
            {
                string codec = video.Metadata.DefaultVideoStream.Codec;
                string size = $"{video.Metadata.DefaultVideoStream.Width}x{video.Metadata.DefaultVideoStream.Height}";
                double fps = Math.Round(video.Metadata.DefaultVideoStream.Framerate.Value);
                string channel = video.Metadata.DefaultAudioStream?.ChannelLayout ?? "silent";
                string metadata = $"({codec})_[{size}_{fps}fps_{channel}]";
                newTitle += Dialogs.TitleFormat.IncludeDate || Dialogs.TitleFormat.IncludeFilename ? $"_{metadata}" : metadata;
                logger.LogDebug("Included metadata in title: {Metadata}", metadata);
            }
            else
            {
                logger.LogTrace("Metadata not included in title or DefaultVideoStream is null");
            }
            if (Dialogs.TitleFormat.IncludeExtension)
            {
                string extension = Path.GetExtension(video.FilePath);
                newTitle += extension;
                logger.LogDebug("Included extension in title: {Extension}", extension);
            }
            else
            {
                logger.LogTrace("Extension not included in title");
            }
            if (useRegex)
            {
                try
                {
                    Regex regex = new(Dialogs.TitleFormat.RegexPattern);
                    newTitle = regex.Replace(newTitle, Dialogs.TitleFormat.RegexReplacement);
                    logger.LogDebug("Applied regex replacement. Pattern={Pattern} Replacement={Replacement}", Dialogs.TitleFormat.RegexPattern, Dialogs.TitleFormat.RegexReplacement);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Regex replacement failed in GetCustomizedVideoTitle for {Path}", video?.FilePath);
                }
            }
            else
            {
                logger.LogTrace("Regex not applied to title");
            }
            logger.LogInformation("GetCustomizedVideoTitle returning '{Title}' for {Path}", newTitle, video?.FilePath);
            return newTitle;
        }

        public void Load()
        {
            logger.LogTrace("Enter Load settings");
            try
            {
                string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
                string appDataSettings = Path.Combine(appDataDirectory, "settings.json");
                logger.LogDebug("Settings path: {SettingsPath}", appDataSettings);

                if (File.Exists(appDataSettings))
                {
                    logger.LogInformation("Settings file found at {SettingsPath}, attempting to read", appDataSettings);
                    string json = File.ReadAllText(appDataSettings);
                    VidHubSettings? settings = JsonSerializer.Deserialize<VidHubSettings>(json);

                    if (settings != null)
                    {
                        logger.LogDebug("Settings deserialized successfully");
                        General = settings.General;
                        Display = settings.Display;
                        Notifications = settings.Notifications;
                        Health = settings.Health;
                        Performance = settings.Performance;
                        Dialogs = settings.Dialogs;
                        logger.LogTrace("Assigned basic settings sections from deserialized settings");
                        if (General.KeepSidePanelSettings)
                        {
                            SidePanel = settings.SidePanel;
                            logger.LogDebug("SidePanel settings restored from file because KeepSidePanelSettings=true");
                        }
                        else
                        {
                            logger.LogDebug("SidePanel settings not restored because KeepSidePanelSettings=false");
                        }
                    }
                    else
                    {
                        logger.LogWarning("Settings file deserialized to null object");
                    }
                }
                else
                {
                    logger.LogInformation("No settings file found at {SettingsPath}, using defaults", appDataSettings);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while loading settings");
            }
        }
        public void Save()
        {
            logger.LogTrace("Enter Save settings");
            try
            {
                string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
                string appDataSettings = Path.Combine(appDataDirectory, "settings.json");
                _ = Directory.CreateDirectory(appDataDirectory);
                logger.LogDebug("Ensured settings directory exists: {Directory}", appDataDirectory);

                string json = JsonSerializer.Serialize(this);
                File.WriteAllText(appDataSettings, json);
                logger.LogInformation("Settings saved to {SettingsPath}", appDataSettings);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save settings");
            }
        }
    }
}
