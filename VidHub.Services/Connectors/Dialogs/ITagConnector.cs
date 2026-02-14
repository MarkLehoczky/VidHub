using System.Collections.ObjectModel;
using VidHub.Core.Models;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public interface ITagConnector : IUpdateService
    {
        ObservableCollection<Tag> Tags { get; }

        void AddTag();
        void RemoveTag(Tag tag);
         void ChangeColorPickerState(Tag tag);
    }
}
