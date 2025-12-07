namespace VidHub.Core.Models
{
    public class VersionInformation
    {
        public string Version { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; } = null;
        public IList<string> NewFeatures { get; set; } = [];
        public IList<string> BugFixes { get; set; } = [];
        public IList<string> InternalChanges { get; set; } = [];
        public bool HasNewFeatures => NewFeatures.Any();
        public bool HasBugFixes => BugFixes.Any();
        public bool HasInternalChanges => InternalChanges.Any();
    }
}
