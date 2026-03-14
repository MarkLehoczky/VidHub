using System.Collections.ObjectModel;
using VidHub.Core.Settings;
using VidHub.Core.Streams;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public class ResolutionConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), IResolutionConnector
    {
        public ObservableCollection<FixedResolution> Resolutions => settings.General.Resolutions;


        public void AddResolution()
        {
            Resolutions.Insert(0, new FixedResolution());
        }

        public void RemoveResolution(FixedResolution resolution)
        {
            Resolutions.Remove(resolution);
        }
    }
}
