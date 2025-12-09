using System.Text.Json.Serialization;

namespace VidHub.Core.Settings.Models
{
    public class ModalSettings
    {
        public DisplayFormatSettings DisplayFormat { get; set; } = new();
        public PreviewImageFormatSettings PreviewImageFormat { get; set; } = new();
        public TitleFormatSettings TitleFormat { get; set; } = new();
    }

    public class DisplayFormatSettings
    {
        public string DateFormat { get; set; } = "yyyy. MMMM. dd.";
        public string DurationDayFormat { get; set; } = "d\"day\" hh\"hour\" mm\"minute\" ss\"second\"";
        public string DurationHourFormat { get; set; } = "h\":\"mm\":\"ss";
        public string DurationMinuteFormat { get; set; } = "m\":\"ss";
        public string DurationSecondFormat { get; set; } = "s\".\"fff";
        public double PreviewImageWidth { get; set; } = 480;
        public double PreviewImageHeight { get; set; } = 270;
    }

    public class PreviewImageFormatSettings
    {
        public int FixedHours { get; set; } = 0;
        public int FixedMinutes { get; set; } = 1;
        public int FixedSeconds { get; set; } = 30;
        public int FixedMilliseconds { get; set; } = 0;
        public int RelativePercentage { get; set; } = 50;
        public bool RelativePosition { get; set; } = true;
        public bool ExtractEmbeddedImage { get; set; } = true;

        [JsonIgnore] public TimeSpan FixedTime => new(0, FixedHours, FixedMinutes, FixedSeconds, FixedMilliseconds);
        [JsonIgnore] public double RelativeTime => RelativePercentage / 100.0;
        [JsonIgnore] public bool FixedPosition => !RelativePosition;
    }

    public class TitleFormatSettings
    {
        public bool IncludePath { get; set; } = false;
        public bool IncludeDate { get; set; } = false;
        public bool IncludeFilename { get; set; } = true;
        public bool IncludeMetadata { get; set; } = false;
        public bool IncludeExtension { get; set; } = false;
        public string RegexPattern { get; set; } = string.Empty;
        public string RegexReplacement { get; set; } = string.Empty;
        public bool UseRegex { get; set; } = false;
        public bool HideTitleCustomization { get; set; } = false;
    }
}
