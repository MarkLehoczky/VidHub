using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.Services.Logics;

namespace VidHub.Services.Connectors.Base
{
    public class SidePanelConnector(IVideoService vs, IVidHubSettings settings, IVideoLoadService load, IVideoOrganizerService organize) : ConnectorTemplate(vs), ISidePanelConnector
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public bool OpenedSidePanel => settings.General.OpenedSidePanel;
        public string? SortBy { get => organize.SortBy; set { organize.SortBy = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("SortBy set to {SortBy}", value); } }
        public string Orientation { get => organize.Orientation; set { organize.Orientation = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("Orientation set to {Orientation}", value); } }
        public string SearchText { get => organize.SearchText; set { organize.SearchText = value; vs.Update(UpdateSections.ALL); logger.LogTrace("SearchText updated"); } }
        public bool UseRealTimeSearch => settings.SidePanel.UseRealTimeSearch;

        public bool FilterDate { get => organize.FilterDate; set { organize.FilterDate = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("FilterDate set to {Value}", value); } }
        public DateTimeOffset? StartDate { get => organize.StartDate; set { organize.StartDate = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("StartDate set to {Date}", value); } }
        public DateTimeOffset? EndDate { get => organize.EndDate; set { organize.EndDate = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("EndDate set to {Date}", value); } }

        public bool FilterDuration { get => organize.FilterDuration; set { organize.FilterDuration = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("FilterDuration set to {Value}", value); } }
        public TimeSpan? MaxDuration { get => organize.MaxDuration; set { organize.MaxDuration = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("MaxDuration set to {Value}", value); } }
        public TimeSpan? MinDuration { get => organize.MinDuration; set { organize.MinDuration = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("MinDuration set to {Value}", value); } }

        public bool FilterResolution { get => settings.SidePanel.FilterResolution; set { settings.SidePanel.FilterResolution = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("FilterFramerate set to {Value}", value); } }
        public bool DisplayMaximumResolutionVideos { get => settings.SidePanel.DisplayMaximumResolutionVideos; set { settings.SidePanel.DisplayMaximumResolutionVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayMaximumResolutionVideos set to {Value}", value); } }
        public bool DisplayLargeResolutionVideos { get => settings.SidePanel.DisplayLargeResolutionVideos; set { settings.SidePanel.DisplayLargeResolutionVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayLargeResolutionVideos set to {Value}", value); } }
        public bool DisplayMediumResolutionVideos { get => settings.SidePanel.DisplayMediumResolutionVideos; set { settings.SidePanel.DisplayMediumResolutionVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayMediumResolutionVideos set to {Value}", value); } }
        public bool DisplayLowResolutionVideos { get => settings.SidePanel.DisplayLowResolutionVideos; set { settings.SidePanel.DisplayLowResolutionVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayLowResolutionVideos set to {Value}", value); } }
        public bool DisplayMinimumResolutionVideos { get => settings.SidePanel.DisplayMinimumResolutionVideos; set { settings.SidePanel.DisplayMinimumResolutionVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayMinimumResolutionVideos set to {Value}", value); } }
        
        public bool FilterFramerate { get => settings.SidePanel.FilterFramerate; set { settings.SidePanel.FilterFramerate = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("FilterFramerate set to {Value}", value); } }
        public bool DisplayMaximumFramerateVideos { get => settings.SidePanel.DisplayMaximumFramerateVideos; set { settings.SidePanel.DisplayMaximumFramerateVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayMaximumFramerateVideos set to {Value}", value); } }
        public bool DisplayLargeFramerateVideos { get => settings.SidePanel.DisplayLargeFramerateVideos; set { settings.SidePanel.DisplayLargeFramerateVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayLargeFramerateVideos set to {Value}", value); } }
        public bool DisplayMediumFramerateVideos { get => settings.SidePanel.DisplayMediumFramerateVideos; set { settings.SidePanel.DisplayMediumFramerateVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayMediumFramerateVideos set to {Value}", value); } }
        public bool DisplayLowFramerateVideos { get => settings.SidePanel.DisplayLowFramerateVideos; set { settings.SidePanel.DisplayLowFramerateVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayLowFramerateVideos set to {Value}", value); } }
        public bool DisplayMinimumFramerateVideos { get => settings.SidePanel.DisplayMinimumFramerateVideos; set { settings.SidePanel.DisplayMinimumFramerateVideos = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("DisplayMinimumFramerateVideos set to {Value}", value); } }
       
        public bool FilterTags { get => settings.SidePanel.FilterTags; set { settings.SidePanel.FilterTags = value; vs.Update(UpdateSection.VIDEOCOLLECTION); logger.LogDebug("FilterFramerate set to {Value}", value); } }
        public ObservableCollection<Tag> Tags => settings.General.Tags;

        public bool HasActiveTransfer => load.HasActiveTransfer;
        public int LoadedFileCount => load.LoadedFileCount;
        public int TotalFileCount => load.TotalFileCount;
        public string TransferDescription => load.TransferDescription;


        public void ChangeOrientation()
        {
            organize.Orientation = organize.Orientation == "ASC" ? "DESC" : "ASC";
            vs.Update(UpdateSection.VIDEOCOLLECTION);
            logger.LogDebug("Orientation changed to {Orientation}", organize.Orientation);
        }

        public IEnumerable<string> GetSortOptions()
        {
            logger.LogTrace("GetSortOptions called");
            return organize.GetSortOptions();
        }

        public IEnumerable<string> GetSearchSuggestions()
        {
            logger.LogTrace("GetSearchSuggestions called");
            return organize.GetSearchSuggestions();
        }

        public void UpdateSearchText()
        {
            logger.LogTrace("UpdateSearchText called");
            organize.UpdateSearchText();
            vs.Update(UpdateSection.VIDEOCOLLECTION);
        }
    }
}
