using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.Services.Connectors.Base
{
    public class SidePanelConnector(IVideoService vs, IVidHubSettings settings, IVideoLoadService load, IVideoOrganizerService organize) : ISidePanelConnector
    {
        public string? CurrentSortOption { get => organize.CurrentSortOption; set => organize.CurrentSortOption = value; }
        public bool EnableLiveSearch => settings.SidePanel.UseRealTimeSearch;
        public DateTimeOffset? EndDate { get => organize.EndDate; set => organize.EndDate = value; }
        public string SearchText { get => organize.SearchText; set => organize.SearchText = value; }
        public bool FilterDate { get => organize.FilterDate; set => organize.FilterDate = value; }
        public bool FilterDuration { get => organize.FilterDuration; set => organize.FilterDuration = value; }
        public bool HasActiveTransfer => load.HasActiveTransfer;
        public int LoadedFileCount => load.LoadedFileCount;
        public TimeSpan? MaxDuration { get => organize.MaxDuration; set => organize.MaxDuration = value; }
        public TimeSpan? MinDuration { get => organize.MinDuration; set => organize.MinDuration = value; }
        public bool OpenedSidePanel => settings.General.OpenedSidePanel;
        public DateTimeOffset? StartDate { get => organize.StartDate; set => organize.StartDate = value; }
        public int TotalFileCount => load.TotalFileCount;
        public string TransferDescription => load.TransferDescription;

        public IEnumerable<string> GetSortOptions()
        {
            return organize.GetSortOptions();
        }

        public void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public IEnumerable<string> Suggestions()
        {
            return organize.Suggestions();
        }

        public void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(IEnumerable<UpdateSection> sections)
        {
            vs.Update(sections);
        }

        public void Update(params UpdateSection[] sections)
        {
            vs.Update(sections);
        }

        public void UpdateSearchText()
        {
            organize.UpdateSearchText();
        }
    }
}
