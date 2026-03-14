using System.Collections.ObjectModel;
using VidHub.Core.Streams;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public interface IResolutionConnector : IUpdateService
    {
        ObservableCollection<FixedResolution> Resolutions { get; }

        void AddResolution();
        void RemoveResolution(FixedResolution resolution);
    }
}
