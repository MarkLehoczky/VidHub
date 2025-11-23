using System.Text.RegularExpressions;

namespace VidHub.Core.Settings
{
    public class TitleCustomizationSettings
    {
        public bool IncludePath { get; set; } = false;
        public bool IncludeDate { get; set; } = false;
        public bool IncludeFilename { get; set; } = true;
        public bool IncludeMetadata { get; set; } = false;
        public bool IncludeExtension { get; set; } = false;
        public string RegexPattern { get; set; } = string.Empty;
        public string RegexReplacement { get; set; } = string.Empty;
        public bool EnabledRegex { get; set; } = false;
        public bool DontShowTitleCustomizationAgain { get; set; } = false;


        public void ChangeTitle(Video video)
        {
            string newTitle = CustomizeTitle(video);
            video.Title = newTitle;
        }

        public string CustomizeTitle(Video video)
        {
            return CustomizeTitle(video, EnabledRegex);
        }
        public string CustomizeTitle(Video video, bool useRegex)
        {
            string path = Path.GetDirectoryName(video.FilePath) + Path.DirectorySeparatorChar;
            string date = video.Date.ToString("yyyy-MM-dd");
            string filename = Path.GetFileNameWithoutExtension(video.FilePath);
            string metadata = $"({video.DefaultVideoStream?.Codec})_[{video.DefaultVideoStream?.Width}x{video.DefaultVideoStream?.Height}_{video.DefaultVideoStream?.Framerate.Item1 / video.DefaultVideoStream?.Framerate.Item2}fps_{video.DefaultVideoStream?.Bitrate / 1048576}Mbps_{video.DefaultAudioStream?.ChannelLayout}]";
            string extension = Path.GetExtension(video.FilePath);
            string newTitle = "";
            if (IncludePath)
            {
                newTitle += path;
            }

            if (IncludeDate)
            {
                newTitle += date;
            }

            if (IncludeFilename)
            {
                newTitle += IncludeDate ? $"_{filename}" : filename;
            }

            if (IncludeMetadata)
            {
                newTitle += IncludeDate || IncludeFilename ? $"_{metadata}" : metadata;
            }

            if (IncludeExtension)
            {
                newTitle += extension;
            }

            if (useRegex)
            {
                try
                {
                    Regex regex = new(RegexPattern);
                    newTitle = regex.Replace(newTitle, RegexReplacement);
                }
                catch { }
            }

            return newTitle;
        }
    }
}
