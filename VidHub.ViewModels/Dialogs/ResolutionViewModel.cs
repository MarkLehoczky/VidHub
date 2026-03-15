using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core.Streams;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Connectors.Dialogs;

namespace VidHub.ViewModels.Dialogs
{
    public partial class ResolutionViewModel(IResolutionConnector connector) : ViewModelTemplate(connector)
    {
        public ObservableCollection<FixedResolution> Resolutions => connector.Resolutions;


        public ResolutionViewModel() : this(VidHubContext.Host.GetService<IResolutionConnector>()) { }


        [RelayCommand]
        private void AddResolution()
        {
            connector.AddResolution();
        }

        [RelayCommand]
        private void RemoveResolution(FixedResolution resolution)
        {
            connector.RemoveResolution(resolution);
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
