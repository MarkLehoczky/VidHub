using System.Text.Json;
using VidHub.Platform;

namespace VidHub.Core.Settings
{
    public class VidHubSettings : IVidHubSettings
    {
        public static IVidHubSettings Instance => Context.Host.GetService<IVidHubSettings>();


        public DisplayCustomizationSettings DisplayCustomization { get; set; } = new DisplayCustomizationSettings();
        public OrganizerSettings Organizer { get; set; } = new OrganizerSettings();
        public PreviewImageCustomizationSettings PreviewImageCustomization { get; set; } = new PreviewImageCustomizationSettings();
        public TitleCustomizationSettings TitleCustomization { get; set; } = new TitleCustomizationSettings();


        public void Load()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            string appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            _ = Directory.CreateDirectory(appDataDirectory);

            if (File.Exists(appDataSettings))
            {
                string json = File.ReadAllText(appDataSettings);
                VidHubSettings? settings = JsonSerializer.Deserialize<VidHubSettings>(json);

                if (settings != null)
                {
                    DisplayCustomization = settings.DisplayCustomization;
                    Organizer = settings.Organizer;
                    PreviewImageCustomization = settings.PreviewImageCustomization;
                    TitleCustomization = settings.TitleCustomization;

                    if (Organizer.Global.SaveOrganizerSettings == false)
                    {
                        Organizer.Display = new DisplayOrganizerSettings();
                    }
                }
            }
        }

        public void Save()
        {
            string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            string appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            _ = Directory.CreateDirectory(appDataDirectory);

            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(appDataSettings, json);
        }
    }
}
