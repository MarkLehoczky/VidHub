using Microsoft.Extensions.Logging;
using System.Diagnostics;
using VidHub.Core.Models;
using VidHub.Core.Notifications;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Data
{
    public class NotificationData
    {
        private static readonly ILogger logger = VidHubContext.Logger;

        public static BarNotification FFmpegNotInstalledNotification()
        {
            logger.LogDebug("Creating FFmpegNotInstalledNotification");
            return new BarNotification
            {
                Title = "FFmpeg Not Detected",
                Details = "FFmpeg is required for generating video thumbnails and rendering previews. It was not found in the system PATH.",
                Severity = NotificationSeverity.ERROR,
                IsClosable = false,
                DisplayCondition = () =>
                {
                    logger.LogTrace("Checking FFmpeg installation status");
                    using Process process = new();
                    process.StartInfo.FileName = "where";
                    process.StartInfo.Arguments = "ffmpeg";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    _ = process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    bool isNotInstalled = string.IsNullOrEmpty(output);
                    logger.LogDebug("FFmpeg installation check result: {IsNotInstalled}", isNotInstalled);
                    return isNotInstalled;
                },
                Button = new CustomActionNotificationButton
                {
                    Label = "Install FFmpeg",
                    Details = "Installs FFmpeg using Winget. Application restart is required to start using FFmpeg.",
                    CustomAction = async () =>
                    {
                        logger.LogInformation("FFmpeg installation initiated");
                        await Task.Run(() =>
                        {
                            try
                            {
                                using Process process = new();
                                process.StartInfo.FileName = "winget";
                                process.StartInfo.Arguments = "install ffmpeg";
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.CreateNoWindow = false;
                                process.WaitForExit();
                                logger.LogInformation("FFmpeg installation completed");
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "FFmpeg installation failed");
                            }
                        });
                    }
                }
            };
        }

        public static BarNotification MediumCacheSizeNotification()
        {
            logger.LogDebug("Creating MediumCacheSizeNotification");
            return new BarNotification
            {
                Title = "Large Cache Size",
                Details = "The application's cache data has reached more than 1 GB",
                Severity = NotificationSeverity.INFORMATIONAL,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    logger.LogTrace("Checking cache size (medium threshold: 1GB)");
                    long cacheSize = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "VidHub")).EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
                    bool exceedsThreshold = cacheSize > Math.Pow(1024, 3);
                    logger.LogDebug("Cache size check (medium): {CacheSize} bytes, exceeds threshold: {ExceedsThreshold}", cacheSize, exceedsThreshold);
                    return exceedsThreshold;
                },
                Button = new CustomActionNotificationButton
                {
                    Label = "Clear Cache",
                    Details = "Clears cached files to recover disk space. With cache loading enabled, all previously cached videos has to be extracted again.",
                    CustomAction = async () =>
                    {
                        logger.LogInformation("Cache clearing initiated (medium)");
                        await Task.Run(() =>
                        {
                            try
                            {
                                foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(item);
                                }
                                logger.LogInformation("Cache cleared successfully (medium)");
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Error clearing cache (medium)");
                            }
                        });
                    }
                }
            };
        }

        public static BarNotification LargeCacheSizeNotification()
        {
            logger.LogDebug("Creating LargeCacheSizeNotification");
            return new BarNotification
            {
                Title = "Large Cache Size",
                Details = "The application's cache data has reached more than 10 GB",
                Severity = NotificationSeverity.WARNING,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    logger.LogTrace("Checking cache size (large threshold: 10GB)");
                    long cacheSize = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "VidHub")).EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
                    bool exceedsThreshold = cacheSize > Math.Pow(1024, 3) * 10;
                    logger.LogDebug("Cache size check (large): {CacheSize} bytes, exceeds threshold: {ExceedsThreshold}", cacheSize, exceedsThreshold);
                    return exceedsThreshold;
                },
                Button = new CustomActionNotificationButton
                {
                    Label = "Clear Cache",
                    Details = "Clears cached files to recover disk space. With cache loading enabled, all previously cached videos has to be extracted again.",
                    CustomAction = async () =>
                    {
                        logger.LogInformation("Cache clearing initiated (large)");
                        await Task.Run(() =>
                        {
                            try
                            {
                                foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(item);
                                }
                                logger.LogInformation("Cache cleared successfully (large)");
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Error clearing cache (large)");
                            }
                        });
                    }
                }
            };
        }

        public static BarNotification NotCheckedVideosNotification(Func<IList<Video>> recieveVideos)
        {
            logger.LogDebug("Creating NotCheckedVideosNotification");
            return new BarNotification()
            {
                Title = "Not Checked Videos Found",
                Details = "Some loaded videos' health have not been checked yet.",
                Severity = NotificationSeverity.INFORMATIONAL,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    logger.LogTrace("Checking for not checked videos");
                    IList<Video> videos = recieveVideos?.Invoke() ?? [];
                    bool hasNotChecked = videos.Count > 0 && videos.Any(video => video.Health.State == HealthState.NOTCHECKED);
                    logger.LogDebug("Not checked videos check: count={Count}, has unchecked={HasUnchecked}", videos.Count, hasNotChecked);
                    return hasNotChecked;
                }
            };
        }

        public static BarNotification HealthyVideosNotification(IList<Video> videos)
        {
            logger.LogDebug("Creating HealthyVideosNotification");
            return new BarNotification()
            {
                Title = "All Videos Passed Health Check",
                Details = "All loaded videos are healthy based on the set health level.",
                Severity = NotificationSeverity.SUCCESS,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    logger.LogTrace("Checking for healthy videos");
                    lock (new object())
                    {
                        bool allHealthy = videos.Count > 0 && videos.All(video => video.Health.State is HealthState.HEALTHY or HealthState.INPROGRESS);
                        logger.LogDebug("Healthy videos check: count={Count}, all healthy={AllHealthy}", videos.Count, allHealthy);
                        return allHealthy;
                    }
                }
            };
        }

        public static BarNotification UnhealthyVideosNotification(Func<IList<Video>> recieveVideos)
        {
            logger.LogDebug("Creating UnhealthyVideosNotification");
            return new BarNotification()
            {
                Title = "Some Videos Failed Health Check",
                Details = "Some loaded videos are not healthy based on the set health level.",
                Severity = NotificationSeverity.WARNING,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    logger.LogTrace("Checking for unhealthy videos");
                    IList<Video> videos = recieveVideos?.Invoke() ?? [];
                    bool hasUnhealthy = videos.Count > 0 && videos.Any(video =>
                            video.Health.State is HealthState.FILENOTFOUND
                            or HealthState.SERIOUSCORRUPTION
                            or HealthState.CRITICALCORRUPTION
                            or HealthState.UNKNOWNERROR);
                    logger.LogDebug("Unhealthy videos check: count={Count}, has unhealthy={HasUnhealthy}", videos.Count, hasUnhealthy);
                    return hasUnhealthy;
                }
            };
        }
    }
}
