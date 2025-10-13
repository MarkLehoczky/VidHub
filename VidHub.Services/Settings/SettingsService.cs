using System.Text.Json;
using VidHub.Core.Settings;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public DisplayCustomizationSettings DisplayCustomization { get; set; } = new DisplayCustomizationSettings();
        public OrganizerSettings Organizer { get; set; } = new OrganizerSettings();
        public PreviewImageCustomizationSettings PreviewImageCustomization { get; set; } = new PreviewImageCustomizationSettings();
        public TitleCustomizationSettings TitleCustomization { get; set; } = new TitleCustomizationSettings();


        public void Load()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            Directory.CreateDirectory(appDataDirectory);

            if (File.Exists(appDataSettings))
            {
                string json = File.ReadAllText(appDataSettings);
                var settings = JsonSerializer.Deserialize<SettingsService>(json);

                if (settings != null)
                {
                    DisplayCustomization = settings.DisplayCustomization;
                    Organizer = settings.Organizer;
                    PreviewImageCustomization = settings.PreviewImageCustomization;
                    TitleCustomization = settings.TitleCustomization;

                    if (Organizer.Global.SaveOrganizerSettings == false)
                        Organizer.Display = new DisplayOrganizerSettings();
                }
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

        /*
        public bool OpenPanel
        {
            service.Update(UpdateType.UpdateSidePanel);
        }


        public bool LiveTextFiltering
        {
            service.Update(UpdateType.UpdateAll);
        }

        public bool CaseSensitiveTextFiltering
        {
            service.Update(UpdateType.UpdateVideoCollection);
        }

        public bool ShowTitles
        {
            service.Update(UpdateType.UpdateVideoCollection);
        }

        public bool ShowDates
        {
            service.Update(UpdateType.UpdateVideoCollection);
        }

        public bool ShowDurations
        {
            service.Update(UpdateType.UpdateVideoCollection);
        }

        public string? DateFormat
        {
            service.Update(UpdateType.ForceUpdateVideoCollection);
        }

        public string? DurationDayFormat
        {
            service.Update(UpdateType.ForceUpdateVideoCollection);
        }

        public string? DurationHourFormat
        {
            service.Update(UpdateType.ForceUpdateVideoCollection);
        }

        public string? DurationMinuteFormat
        {
            service.Update(UpdateType.ForceUpdateVideoCollection);
        }

        public string? DurationSecondFormat
        {
            service.Update(UpdateType.ForceUpdateVideoCollection);
        }

        public double FieldWidth
        {
            service.Update(UpdateType.UpdateVideoCollection);
        }

        public double FieldHeight
        {
            service.Update(UpdateType.UpdateVideoCollection);
        }
        */
    }
}
