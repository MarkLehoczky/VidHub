using System.Diagnostics;
using VidHub.Core.Notifications;
using VidHub.Core.Utilities.Helper;

namespace VidHub.Core.Data
{
    public class NotificationData
    {
        public static BarNotification FFmpegNotInstalledNotification()
        {
            return new BarNotification
            {
                Title = "FFmpeg Not Detected",
                Details = "FFmpeg is required for generating video thumbnails and rendering previews. It was not found in the system PATH.",
                Severity = NotificationSeverity.ERROR,
                IsClosable = false,
                DisplayCondition = () =>
                {
                    using Process process = new();
                    process.StartInfo.FileName = "where";
                    process.StartInfo.Arguments = "ffmpeg";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    _ = process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return string.IsNullOrEmpty(output);
                },
                Button = new CustomActionNotificationButton
                {
                    Label = "Install FFmpeg",
                    Details = "Installs FFmpeg using Winget. Application restart is required to start using FFmpeg.",
                    CustomAction = async () =>
                    {
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
                            }
                            catch { }
                        });
                    }
                }
            };
        }

        public static BarNotification HealthyVideosNotification(IList<Video> videos)
        {
            return new BarNotification()
            {
                Title = "All Videos Passed Health Check",
                Details = "All loaded videos are healthy based on the set health level.",
                Severity = NotificationSeverity.SUCCESS,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    lock (new object())
                    {
                        return videos.Count > 0 && videos.All(video => video.HealthState.State is VideoHealth.HEALTHY or VideoHealth.INPROGRESS);
                    }
                }
            };
        }

        public static BarNotification LargeCacheSizeNotification()
        {
            return new BarNotification
            {
                Title = "Large Cache Size",
                Details = "The application's cache data has reached more than 10 GB",
                Severity = NotificationSeverity.WARNING,
                IsClosable = true,
                DisplayCondition = () => new DirectoryInfo(Path.Combine(Path.GetTempPath(), "VidHub")).EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length) > Math.Pow(1024, 3) * 10,
                Button = new CustomActionNotificationButton
                {
                    Label = "Clear Cache",
                    Details = "Clears cached files to recover disk space. With cache loading enabled, all previously cached videos has to be extracted again.",
                    CustomAction = async () =>
                    {
                        await Task.Run(() =>
                        {
                            foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                            {
                                File.Delete(item);
                            }
                        });
                    }
                }
            };
        }

        public static BarNotification MediumCacheSizeNotification()
        {
            return new BarNotification
            {
                Title = "Large Cache Size",
                Details = "The application's cache data has reached more than 1 GB",
                Severity = NotificationSeverity.INFORMATIONAL,
                IsClosable = true,
                DisplayCondition = () => new DirectoryInfo(Path.Combine(Path.GetTempPath(), "VidHub")).EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length) > Math.Pow(1024, 3),
                Button = new CustomActionNotificationButton
                {
                    Label = "Clear Cache",
                    Details = "Clears cached files to recover disk space. With cache loading enabled, all previously cached videos has to be extracted again.",
                    CustomAction = async () =>
                    {
                        await Task.Run(() =>
                        {
                            foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                            {
                                File.Delete(item);
                            }
                        });
                    }
                }
            };
        }

        public static BarNotification NotCheckedVideosNotification(IList<Video> videos)
        {
            return new BarNotification()
            {
                Title = "Not Checked Videos Found",
                Details = "Some loaded videos' health have not been checked yet.",
                Severity = NotificationSeverity.INFORMATIONAL,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    lock (new object())
                    {
                        return videos.Count > 0 && videos.Any(video => video.HealthState.State == VideoHealth.NOTCHECKED);
                    }
                }
            };
        }

        public static BarNotification UnhealthyVideosNotification(IList<Video> videos)
        {
            return new BarNotification()
            {
                Title = "Some Videos Failed Health Check",
                Details = "Some loaded videos are not healthy based on the set health level.",
                Severity = NotificationSeverity.WARNING,
                IsClosable = true,
                DisplayCondition = () =>
                {
                    lock (new object())
                    {
                        return videos.Count > 0 && videos.Any(video =>
                            video.HealthState.State is VideoHealth.FILENOTFOUND
                            or VideoHealth.SERIOUSCORRUPTION
                            or VideoHealth.CRITICALCORRUPTION
                            or VideoHealth.UNKNOWNERROR);
                    }
                }
            };
        }
    }
}
