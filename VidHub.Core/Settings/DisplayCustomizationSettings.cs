namespace VidHub.Core.Settings
{
    public class DisplayCustomizationSettings
    {
        public bool DisplayTitles { get; set; } = true;
        public bool DisplayDates { get; set; } = true;
        public bool DisplayDurations { get; set; } = true;
        public string DateFormat { get; set; } = "yyyy. MMMM. dd.";
        public string DurationDayFormat { get; set; } = "d\\d\\ hh\\h\\ mm\\m\\ ss\\s";
        public string DurationHourFormat { get; set; } = "h\\:mm\\:ss";
        public string DurationMinuteFormat { get; set; } = "m\\:ss";
        public string DurationSecondFormat { get; set; } = "s\\.fff";
        public double PreviewImageWidth { get; set; } = 480;
        public double PreviewImageHeight { get; set; } = 270;
    }
}
