using VidHub.Core.Enums;
using VidHub.Core.Settings;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.Services.Connectors.Base
{
    public class SidePanelConnector(IVideoService vs, ISettingsService settings, IVideoLoadService load, IVideoOrganizerService organize) : ISidePanelConnector
    {
        public string? CurrentSortOption { get => organize.CurrentSortOption; set => organize.CurrentSortOption = value; }
        public bool EnableLiveSearch => settings.Organizer.Global.EnableLiveSearch;
        public DateTimeOffset? EndDate { get => organize.EndDate; set => organize.EndDate = value; }
        public string SearchText { get => organize.SearchText; set => organize.SearchText = value; }
        public bool FilterDate { get => organize.FilterDate; set => organize.FilterDate = value; }
        public bool FilterDuration { get => organize.FilterDuration; set => organize.FilterDuration = value; }
        public bool HasActiveTransfer => load.HasActiveTransfer;
        public int LoadedFileCount => load.LoadedFileCount;
        public TimeSpan? MaxDuration { get => organize.MaxDuration; set => organize.MaxDuration = value; }
        public TimeSpan? MinDuration { get => organize.MinDuration; set => organize.MinDuration = value; }
        public bool OpenedSidePanel => settings.Organizer.Global.OpenedSidePanel;
        public DateTimeOffset? StartDate { get => organize.StartDate; set => organize.StartDate = value; }
        public int TotalFileCount => load.TotalFileCount;
        public string TransferDescription => load.TransferDescription;

        public IEnumerable<string> GetSortOptions()
        {
            return organize.GetSortOptions();
        }

        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public IEnumerable<string> Suggestions()
        {
            return organize.Suggestions();
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(UpdateType type)
        {
            vs.Update(type);
        }

        public void UpdateSearchText()
        {
            organize.UpdateSearchText();
        }
    }
}
