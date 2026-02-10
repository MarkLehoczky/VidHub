using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core.Models;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;

namespace VidHub.ViewModels.Dialogs
{
    public partial class TagViewModel(IVideoService vs) : ViewModelTemplate(vs)
    {
        public ObservableCollection<Tag> Tags { get; } = [];


        public TagViewModel() : this(VidHubContext.Host.GetService<IVideoService>()) { }


        [RelayCommand]
        private void AddTag()
        {
            Tags.Insert(0, new Tag());
        }

        [RelayCommand]
        private void RemoveTag(Tag tag)
        {
            Tags.Remove(tag);
        }

        [RelayCommand]
        private void ChangeColorPickerState(Tag tag)
        {
            tag.ColorPickerOpen = !tag.ColorPickerOpen;
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
