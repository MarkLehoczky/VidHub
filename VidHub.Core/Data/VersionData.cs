using VidHub.Core.Models;

namespace VidHub.Core.Data
{
    public class VersionData
    {
        public static string CurrentVersion => "VidHub 0.5.0";


        public static VersionInformation Version_0_1_0 => new()
        {
            Version = "0.1.0",
            ReleaseDate = new DateTime(2025, 9, 4),
            NewFeatures = [
                "Video display",
                "Video loading",
            ],
            InternalChanges = [
                "Updated license with real information",
            ],
        };

        public static VersionInformation Version_0_2_0 => new()
        {
            Version = "0.2.0",
            ReleaseDate = new DateTime(2025, 9, 7),
            NewFeatures = [
                "Basic sorting",
                "Basic filtering with case sensitivity and live filtering",
                "Basic transfer displaying",
                "Taskbar progress state display",
                "Cache loading",
                "System notifications",
                "Concurrent video loading",
               "Settings and filter configuration saving",
            ],
            InternalChanges = [
                "Basic CI workflow",
                "README implementation",
            ],
        };

        public static VersionInformation Version_0_2_1 => new()
        {
            Version = "0.2.1",
            ReleaseDate = new DateTime(2025, 9, 9),
            BugFixes = [
                "Fix custom color issues for light theme",
            ],
        };

        public static VersionInformation Version_0_3_0 => new()
        {
            Version = "0.3.0",
            ReleaseDate = new DateTime(2025, 9, 13),
            NewFeatures = [
                "Implement title suggestions during text search",
                "Make title suggestions optional",
                "Implement hideable title, date and duration",
            ],
            BugFixes = [
                "Remove unnecessary second default sort option",
                "Reorganize sort options and change orientation icons",
                "Hide search button during live text search",
                "Fix extending sized text input field",
                "Make title, date and duration highlightable text",
                "Do not display tooltip when the text is not truncated",
                "Showcase separator bar between side panel and main display",
            ],
        };

        public static VersionInformation Version_0_4_0 => new()
        {
            Version = "0.4.0",
            ReleaseDate = new DateTime(2025, 10, 12),
            NewFeatures = [
                "Date format customization",
                "Duration format customization",
                "Thumbnail size customization",
                "Title during loading customization",
                "Thumbnail frame customization",
                "Open context menu option",
                "Rename context menu option",
                "Copy file context menu option",
                "Copy file path context menu option",
                "Copy thumbnail image context menu option",
                "Remove context menu option",
            ]
        };

        public static VersionInformation Version_0_5_0 => new()
        {
            Version = "0.5.0",
            ReleaseDate = new DateTime(2025, 11, 23),
            NewFeatures = [
                "Notification when cache size is over 1GB (w/ cache clearing)",
                "Notification when FFmpeg not found (w/ FFmpeg installer)",
                "Activate metadata option for titles",
                "Activate embedded image extraction option for preview images",
            ],
            InternalChanges = [
                "Improve loading and collecting flow",
                "Simultaneous loading and collecting from same batch",
                "Dynamic video counter during load",
                "Extraction, grouping and storing metadata of files",
                "Switched hash algorithm from MD5 to Blake3",
                "Implement simple hash collision handling",
                "Support file content-based hash generation",
            ],
        };
    }
}
