using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core.Models;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Connectors.Dialogs;

namespace VidHub.ViewModels.Dialogs
{
    public partial class TagViewModel(ITagConnector connector) : ViewModelTemplate(connector)
    {
        public ObservableCollection<Tag> Tags => connector.Tags;


        public TagViewModel() : this(VidHubContext.Host.GetService<ITagConnector>()) { }


        [RelayCommand]
        private void AddTag()
        {
            connector.AddTag();
        }

        [RelayCommand]
        private void RemoveTag(Tag tag)
        {
            connector.RemoveTag(tag);
        }

        [RelayCommand]
        private void ChangeColorPickerState(Tag tag)
        {
            connector.ChangeColorPickerState(tag);
            OnPropertyChanged(nameof(Tags));
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
