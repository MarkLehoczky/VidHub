using VidHub.Core.Settings;
using VidHub.Services.Base;
using VidHub.Services.Logics;

namespace VidHub.Services.Connectors.Base
{
    public class SidePanelConnector(IVideoService vs, IVidHubSettings settings, IVideoLoadService load, IVideoOrganizerService organize) : ConnectorTemplate(vs), ISidePanelConnector
    {
        public bool OpenedSidePanel => settings.General.OpenedSidePanel;
        public string? SortBy { get => organize.SortBy; set => organize.SortBy = value; }
        public string Orientation { get => organize.Orientation; set => organize.Orientation = value; }
        public string SearchText { get => organize.SearchText; set => organize.SearchText = value; }
        public bool UseRealTimeSearch => settings.SidePanel.UseRealTimeSearch;

        public bool FilterDate { get => organize.FilterDate; set => organize.FilterDate = value; }
        public DateTimeOffset? StartDate { get => organize.StartDate; set => organize.StartDate = value; }
        public DateTimeOffset? EndDate { get => organize.EndDate; set => organize.EndDate = value; }

        public bool FilterDuration { get => organize.FilterDuration; set => organize.FilterDuration = value; }
        public TimeSpan? MaxDuration { get => organize.MaxDuration; set => organize.MaxDuration = value; }
        public TimeSpan? MinDuration { get => organize.MinDuration; set => organize.MinDuration = value; }

        public bool HasActiveTransfer => load.HasActiveTransfer;
        public int LoadedFileCount => load.LoadedFileCount;
        public int TotalFileCount => load.TotalFileCount;
        public string TransferDescription => load.TransferDescription;


        public void ChangeOrientation()
        {
            organize.Orientation = organize.Orientation == "ASC" ? "DESC" : "ASC";
        }

        public IEnumerable<string> GetSortOptions()
        {
            return organize.GetSortOptions();
        }

        public IEnumerable<string> GetSearchSuggestions()
        {
            return organize.GetSearchSuggestions();
        }

        public void UpdateSearchText()
        {
            organize.UpdateSearchText();
        }
    }
}
