namespace VidHub.Services.Settings.Interfaces
{
    public interface ISettingsService
    {
        bool OpenPanel { get; set; }
        bool SystemNotifications { get; set; }
        bool CacheLoad { get; set; }
        bool ConcurrentVideoLoading { get; set; }
        bool KeepFilterStatus { get; set; }
        bool CaseSensitiveTextFiltering { get; set; }
        bool LiveTextFiltering { get; set; }
        bool TextSuggestions { get; set; }
        bool ShowTitles { get; set; }
        bool ShowDates { get; set; }
        bool ShowDurations { get; set; }
        string? DateFormat { get; set; }
        string? DurationDayFormat { get; set; }
        string? DurationHourFormat { get; set; }
        string? DurationMinuteFormat { get; set; }
        string? DurationSecondFormat { get; set; }
        double FieldWidth { get; set; }
        double FieldHeight { get; set; }
        bool IncludePath { get; set; }
        bool IncludeDate { get; set; }
        bool IncludeFilename { get; set; }
        bool IncludeMetadata { get; set; }
        bool IncludeExtension { get; set; }
        string Pattern { get; set; }
        string Replacement { get; set; }
        bool IsRegexEnabled { get; set; }
        bool DontShowTitleCustomizationAgain { get; set; }
        void Load();
        void Save();
        void Set(ISettingsService service);
    }
}
