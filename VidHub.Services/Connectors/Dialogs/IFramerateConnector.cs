using System.Collections.ObjectModel;
using VidHub.Core.Streams;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public interface IFramerateConnector : IUpdateService
    {
        ObservableCollection<FixedFramerate> Framerates { get; }

        void AddFramerate();
        void RemoveFramerate(FixedFramerate framerate);
    }
}
