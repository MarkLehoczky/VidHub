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
            var newTitle = CustomizeTitle(video.Title);
            video.Title = newTitle;
        }

        public string CustomizeTitle(string filePath)
        {
            var newTitle = "";
            if (IncludePath)
                newTitle += Path.GetFullPath(filePath)[..^Path.GetFileName(filePath).Length];

            if (IncludeDate)
                newTitle += File.GetCreationTime(filePath).ToString("yyyy-MM-dd");

            if (IncludeFilename)
                newTitle += IncludeDate ? $"_{Path.GetFileNameWithoutExtension(filePath)}" : Path.GetFileNameWithoutExtension(filePath);

            if (IncludeMetadata)
                newTitle += "[Metadata]";

            if (IncludeExtension)
                newTitle += Path.GetExtension(filePath);

            if (EnabledRegex)
                try
                {
                    var regex = new Regex(RegexPattern);
                    newTitle = regex.Replace(newTitle, RegexReplacement);
                }
                catch { }

            return newTitle;
        }
        public string CustomizeTitle(string filePath, bool useRegex)
        {
            var newTitle = "";
            if (IncludePath)
                newTitle += Path.GetFullPath(filePath)[..^Path.GetFileName(filePath).Length];

            if (IncludeDate)
                newTitle += File.GetCreationTime(filePath).ToString("yyyy-MM-dd");

            if (IncludeFilename)
                newTitle += IncludeDate ? $"_{Path.GetFileNameWithoutExtension(filePath)}" : Path.GetFileNameWithoutExtension(filePath);

            if (IncludeMetadata)
                newTitle += "[Metadata]";

            if (IncludeExtension)
                newTitle += Path.GetExtension(filePath);

            if (useRegex)
                try
                {
                    var regex = new Regex(RegexPattern);
                    newTitle = regex.Replace(newTitle, RegexReplacement);
                }
                catch { }

            return newTitle;
        }
        public string CustomizeTitle(Video Video)
        {
            var newTitle = "";
            if (IncludePath)
                newTitle += Path.GetFullPath(Video.FilePath)[..^Path.GetFileName(Video.FilePath).Length];

            if (IncludeDate)
                newTitle += File.GetCreationTime(Video.FilePath).ToString("yyyy-MM-dd");

            if (IncludeFilename)
                newTitle += IncludeDate ? $"_{Path.GetFileNameWithoutExtension(Video.FilePath)}" : Path.GetFileNameWithoutExtension(Video.FilePath);

            if (IncludeMetadata)
                newTitle += "[Metadata]";

            if (IncludeExtension)
                newTitle += Path.GetExtension(Video.FilePath);

            if (EnabledRegex)
                try
                {
                    var regex = new Regex(RegexPattern);
                    newTitle = regex.Replace(newTitle, RegexReplacement);
                }
                catch { }

            return newTitle;
        }
    }
}
