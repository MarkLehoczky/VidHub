using System.Collections.ObjectModel;
using VidHub.Core.Settings;
using VidHub.Core.Streams;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public class FramerateConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), IFramerateConnector
    {
        public ObservableCollection<FixedFramerate> Framerates => settings.General.Framerates;


        public void AddFramerate()
        {
            Framerates.Insert(0, new FixedFramerate());
        }

        public void RemoveFramerate(FixedFramerate framerate)
        {
            Framerates.Remove(framerate);
        }
    }
}
