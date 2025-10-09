using System.Text.Json;
using VidHub.Core.Helpers;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    public class SettingsService(IMainService service) : ISettingsService
    {
        private bool openPanel = true;
        private bool systemNotifications = true;
        private bool cacheLoad = true;
        private bool concurrentVideoLoading = false;
        private bool keepFilterStatus = true;
        private bool liveTextFiltering = true;
        private bool caseSensitiveTextFiltering = false;
        private bool textSuggestions = true;
        private bool showTitles = true;
        private bool showDates = true;
        private bool showDurations = true;
        private string? dateFormat = "yyyy. MMMM. dd.";
        private string? durationDayFormat = "d\\d\\ hh\\h\\ mm\\m\\ ss\\s";
        private string? durationHourFormat = "h\\:mm\\:ss";
        private string? durationMinuteFormat = "m\\:ss";
        private string? durationSecondFormat = "s\\.fff";
        private double fieldWidth = 480;
        private double fieldHeight = 270;

        private bool includePath = false;
        private bool includeDate = false;
        private bool includeFilename = true;
        private bool includeMetadata = false;
        private bool includeExtension = false;

        string pattern = "";
        string replacement = "";

        bool isRegexEnabled = false;

        private bool dontShowTitleCustomizationAgain = false;

        public void Load()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            Directory.CreateDirectory(appDataDirectory);

            if (File.Exists(appDataSettings))
            {
                string json = File.ReadAllText(appDataSettings);
                var settings = JsonSerializer.Deserialize<SettingsLoader>(json);
                Set(settings);
            }
        }

        public void Save()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            Directory.CreateDirectory(appDataDirectory);

            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(appDataSettings, json);
        }

        public void Set(ISettingsService service)
        {
            openPanel = service.OpenPanel;
            systemNotifications = service.SystemNotifications;
            cacheLoad = service.CacheLoad;
            concurrentVideoLoading = service.ConcurrentVideoLoading;
            keepFilterStatus = service.KeepFilterStatus;
            liveTextFiltering = service.LiveTextFiltering;
            caseSensitiveTextFiltering = service.CaseSensitiveTextFiltering;
            textSuggestions = service.TextSuggestions;
            showTitles = service.ShowTitles;
            showDates = service.ShowDates;
            showDurations = service.ShowDurations;
            dateFormat = service.DateFormat;
            durationDayFormat = service.DurationDayFormat;
            durationHourFormat = service.DurationHourFormat;
            durationMinuteFormat = service.DurationMinuteFormat;
            durationSecondFormat = service.DurationSecondFormat;
            fieldWidth = service.FieldWidth;
            fieldHeight = service.FieldHeight;
            includePath = service.IncludePath;
            includeDate = service.IncludeDate;
            includeFilename = service.IncludeFilename;
            includeMetadata = service.IncludeMetadata;
            includeExtension = service.IncludeExtension;
            pattern = service.Pattern;
            replacement = service.Replacement;
            isRegexEnabled = service.IsRegexEnabled;
            dontShowTitleCustomizationAgain = service.DontShowTitleCustomizationAgain;
        }


        public bool OpenPanel
        {
            get => openPanel;
            set
            {
                if (openPanel == value) return;
                openPanel = value;
                service.Update(UpdateType.UpdateSidepanel);
            }
        }


        public bool SystemNotifications
        {
            get => systemNotifications;
            set => systemNotifications = value;
        }

        public bool CacheLoad
        {
            get => cacheLoad;
            set => cacheLoad = value;
        }

        public bool ConcurrentVideoLoading
        {
            get => concurrentVideoLoading;
            set => concurrentVideoLoading = value;
        }

        public bool KeepFilterStatus
        {
            get => keepFilterStatus;
            set => keepFilterStatus = value;
        }

        public bool LiveTextFiltering
        {
            get => liveTextFiltering;
            set
            {
                if (liveTextFiltering == value) return;
                liveTextFiltering = value;
                service.Update(UpdateType.UpdateAll);
            }
        }

        public bool CaseSensitiveTextFiltering
        {
            get => caseSensitiveTextFiltering;
            set
            {
                if (caseSensitiveTextFiltering == value) return;
                caseSensitiveTextFiltering = value;
                service.Update(UpdateType.UpdateVideoCollection);
            }
        }

        public bool TextSuggestions
        {
            get => textSuggestions;
            set => textSuggestions = value;
        }

        public bool ShowTitles
        {
            get => showTitles;
            set
            {
                if (showTitles == value) return;
                showTitles = value;
                service.Update(UpdateType.UpdateVideoCollection);
            }
        }

        public bool ShowDates
        {
            get => showDates;
            set
            {
                if (showDates == value) return;
                showDates = value;
                service.Update(UpdateType.UpdateVideoCollection);
            }
        }

        public bool ShowDurations
        {
            get => showDurations;
            set
            {
                if (showDurations == value) return;
                showDurations = value;
                service.Update(UpdateType.UpdateVideoCollection);
            }
        }

        public string? DateFormat
        {
            get => dateFormat;
            set
            {
                if (dateFormat == value) return;
                dateFormat = value;
                service.Update(UpdateType.ResetVideoCollection);
            }
        }

        public string? DurationDayFormat
        {
            get => durationDayFormat;
            set
            {
                if (durationDayFormat == value) return;
                durationDayFormat = value;
                service.Update(UpdateType.ResetVideoCollection);
            }
        }

        public string? DurationHourFormat
        {
            get => durationHourFormat;
            set
            {
                if (durationHourFormat == value) return;
                durationHourFormat = value;
                service.Update(UpdateType.ResetVideoCollection);
            }
        }

        public string? DurationMinuteFormat
        {
            get => durationMinuteFormat;
            set
            {
                if (durationMinuteFormat == value) return;
                durationMinuteFormat = value;
                service.Update(UpdateType.ResetVideoCollection);
            }
        }

        public string? DurationSecondFormat
        {
            get => durationSecondFormat;
            set
            {
                if (durationSecondFormat == value) return;
                durationSecondFormat = value;
                service.Update(UpdateType.ResetVideoCollection);
            }
        }

        public double FieldWidth
        {
            get => fieldWidth;
            set
            {
                if (fieldWidth == value) return;
                fieldWidth = value;
                service.Update(UpdateType.UpdateVideoCollection);
            }
        }

        public double FieldHeight
        {
            get => fieldHeight;
            set
            {
                if (fieldHeight == value) return;
                fieldHeight = value;
                service.Update(UpdateType.UpdateVideoCollection);
            }
        }

        public bool IncludePath
        {
            get => includePath;
            set => includePath = value;
        }
        public bool IncludeDate
        {
            get => includeDate;
            set => includeDate = value;
        }
        public bool IncludeFilename
        {
            get => includeFilename;
            set => includeFilename = value;
        }
        public bool IncludeMetadata
        {
            get => includeMetadata;
            set => includeMetadata = value;
        }
        public bool IncludeExtension
        {
            get => includeExtension;
            set => includeExtension = value;
        }

        public string Pattern
        {
            get => pattern;
            set => pattern = value;
        }
        public string Replacement
        {
            get => replacement;
            set => replacement = value;
        }
        public bool InvalidRegex { get; set; } = false;

        public bool IsRegexEnabled
        {
            get => isRegexEnabled;
            set => isRegexEnabled = value;
        }

        public bool DontShowTitleCustomizationAgain
        {
            get => dontShowTitleCustomizationAgain;
            set => dontShowTitleCustomizationAgain = value;
        }
    }
}
