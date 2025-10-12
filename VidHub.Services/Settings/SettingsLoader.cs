using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    internal class SettingsLoader : ISettingsService
    {
        public bool OpenPanel { get; set; }
        public bool SystemNotifications { get; set; }
        public bool CacheLoad { get; set; }
        public bool ConcurrentVideoLoading { get; set; }
        public bool KeepFilterStatus { get; set; }
        public bool CaseSensitiveTextFiltering { get; set; }
        public bool LiveTextFiltering { get; set; }
        public bool TextSuggestions { get; set; }
        public bool ShowTitles { get; set; }
        public bool ShowDates { get; set; }
        public bool ShowDurations { get; set; }
        public string? DateFormat { get; set; }
        public string? DurationDayFormat { get; set; }
        public string? DurationHourFormat { get; set; }
        public string? DurationMinuteFormat { get; set; }
        public string? DurationSecondFormat { get; set; }
        public double FieldWidth { get; set; }
        public double FieldHeight { get; set; }
        public bool DontShowTitleCustomizationAgain { get; set; }
        public bool IncludePath { get; set; }
        public bool IncludeDate { get; set; }
        public bool IncludeFilename { get; set; }
        public bool IncludeMetadata { get; set; }
        public bool IncludeExtension { get; set; }
        public string Pattern { get; set; } = string.Empty;
        public string Replacement { get; set; } = string.Empty;
        public bool IsRegexEnabled { get; set; }
        public bool RelativePosition { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Milliseconds { get; set; }
        public int FramePercentage { get; set; }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Set(ISettingsService service)
        {
            throw new NotImplementedException();
        }
    }
}
