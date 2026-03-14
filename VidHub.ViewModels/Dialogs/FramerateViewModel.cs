using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core.Streams;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Connectors.Dialogs;

namespace VidHub.ViewModels.Dialogs
{
    public partial class FramerateViewModel(IFramerateConnector connector) : ViewModelTemplate(connector)
    {
        public ObservableCollection<FixedFramerate> Framerates => connector.Framerates;


        public FramerateViewModel() : this(VidHubContext.Host.GetService<IFramerateConnector>()) { }


        [RelayCommand]
        private void AddFramerate()
        {
            connector.AddFramerate();
        }

        [RelayCommand]
        private void RemoveFramerate(FixedFramerate framerate)
        {
            connector.RemoveFramerate(framerate);
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
